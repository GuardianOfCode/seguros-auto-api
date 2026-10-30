# Seguros Auto API

Prototipo funcional del módulo de **emisión de pólizas de automóviles** para *Seguros LAFISE S.A.* API REST en **C# / .NET 8** con **SQL Server** y **Swagger**.

> **¿Solo quieres evaluarlo rápido?** Ve a la sección [Inicio rápido](#inicio-rápido-5-minutos). Si algo falla, revisa [Solución de problemas](#solución-de-problemas).

## Tabla de contenido

1. [Inicio rápido (5 minutos)](#inicio-rápido-5-minutos)
2. [Requisitos previos](#paso-1--instalar-los-requisitos)
3. [Obtener el proyecto](#paso-2--obtener-el-proyecto)
4. [Configurar la conexión a SQL Server](#paso-3--configurar-la-conexión-a-sql-server)
5. [Crear la base de datos](#paso-4--crear-la-base-de-datos)
6. [Ejecutar la API](#paso-5--ejecutar-la-api)
7. [Verificar que todo funciona](#paso-6--verificar-que-todo-funciona)
8. [Solución de problemas](#solución-de-problemas)
9. [Endpoints](#endpoints)
10. [Ejemplo de emisión de póliza](#ejemplo-de-emisión-de-póliza)
11. [Reglas de negocio](#reglas-de-negocio)
12. [Arquitectura y modelo de datos](#arquitectura-y-modelo-de-datos)
13. [Decisiones de diseño](#decisiones-de-diseño)

---

## Inicio rápido (5 minutos)

Si ya tienes **.NET 8 SDK** y **SQL Server (LocalDB o Express)**, abre una terminal (PowerShell) en la carpeta del proyecto y ejecuta:

```powershell
# 1. Instalar la herramienta de migraciones (solo la primera vez)
dotnet tool install --global dotnet-ef --version 8.0.10

# 2. Crear la base de datos con tablas y datos iniciales
dotnet ef database update --project SegurosAuto.Api

# 3. Ejecutar la API
dotnet run --project SegurosAuto.Api --launch-profile https
```

Luego abre en el navegador la dirección que aparece en la consola, agregando `/swagger`:

```
https://localhost:<puerto>/swagger
```

Si algo no funciona, sigue la guía detallada de abajo.

---

## Paso 1 — Instalar los requisitos

Necesitas **tres cosas**. Marca cada una cuando la tengas.

### ☐ 1.1 .NET 8 SDK

1. Descárgalo desde <https://dotnet.microsoft.com/download/dotnet/8.0> (elige **SDK**, no solo Runtime).
2. Instálalo con las opciones por defecto.
3. **Verifica** abriendo una terminal nueva:

```powershell
dotnet --version
```

Debe mostrar un número que empiece con `8.` (por ejemplo `8.0.403`).

### ☐ 1.2 SQL Server

Necesitas **una** de estas opciones:

| Opción | Cuándo usarla | Cómo obtenerla |
| --- | --- | --- |
| **LocalDB** | Windows con Visual Studio 2022 (ya viene incluida) | Se instala con Visual Studio (carga de trabajo *Desarrollo de ASP.NET y web*). |
| **SQL Server Express** | Windows sin Visual Studio | <https://www.microsoft.com/sql-server/sql-server-downloads> → Express. |
| **Docker** | Mac, Linux, o si prefieres no instalar SQL Server | Ver comando abajo. |

**Verificar LocalDB** (Windows):

```powershell
sqllocaldb info
```

Debe listar `MSSQLLocalDB`. Si no aparece, instala SQL Server Express.

**Opción Docker** (cualquier sistema operativo):

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Clave_Segura_123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

### ☐ 1.3 Herramienta de migraciones de EF Core

```powershell
dotnet tool install --global dotnet-ef --version 8.0.10
```

**Verifica:**

```powershell
dotnet ef --version
```

> Si dice que ya está instalada, perfecto. Si da error de versión, actualiza con `dotnet tool update --global dotnet-ef --version 8.0.10`.

### ☐ 1.4 (Opcional) Herramientas recomendadas

- **Visual Studio 2022** (Community es gratis) o **VS Code**.
- **SQL Server Management Studio (SSMS)** para ver la base de datos: <https://aka.ms/ssmsfullsetup>
- **Git**: <https://git-scm.com/downloads>

---

## Paso 2 — Obtener el proyecto

**Opción A: con Git**

```powershell
git clone https://github.com/GuardianOfCode/seguros-auto-api.git
cd seguros-auto-api
```

**Opción B: sin Git**

1. En GitHub, botón verde **Code** → **Download ZIP**.
2. Descomprime el ZIP.
3. Abre una terminal dentro de la carpeta descomprimida.

**Verifica** que estás en la carpeta correcta: debes ver el archivo `.sln` y la carpeta `SegurosAuto.Api`.

```powershell
dir
```

Estructura esperada:

```
seguros-auto-api/
├── SegurosAuto.sln
├── README.md
├── database/
│   └── 01_crear_base_de_datos.sql
└── SegurosAuto.Api/
    ├── Controllers/
    ├── Services/
    ├── Repositories/
    ├── Models/
    ├── Data/
    ├── Dtos/
    ├── Exceptions/
    ├── Middleware/
    ├── Migrations/
    ├── appsettings.json
    └── Program.cs
```

> Si tu carpeta está organizada distinto (por ejemplo, con una carpeta extra), ajusta las rutas `SegurosAuto.Api` de los comandos de este README.

---

## Paso 3 — Configurar la conexión a SQL Server

Abre el archivo `SegurosAuto.Api/appsettings.json` y revisa esta sección:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SegurosAutoDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

**Por defecto funciona con LocalDB, sin cambiar nada.** Si usas otra opción, reemplaza el valor de `DefaultConnection`:

| Si usas | Cadena de conexión |
| --- | --- |
| **LocalDB** (por defecto) | `Server=(localdb)\\MSSQLLocalDB;Database=SegurosAutoDb;Trusted_Connection=True;TrustServerCertificate=True` |
| **SQL Server Express** | `Server=localhost\\SQLEXPRESS;Database=SegurosAutoDb;Trusted_Connection=True;TrustServerCertificate=True` |
| **SQL Server con usuario y clave** | `Server=localhost;Database=SegurosAutoDb;User Id=sa;Password=TU_CLAVE;TrustServerCertificate=True` |
| **Docker** (comando del paso 1.2) | `Server=localhost,1433;Database=SegurosAutoDb;User Id=sa;Password=Clave_Segura_123;TrustServerCertificate=True` |

> ⚠️ En un archivo JSON, las barras invertidas se escriben **dobles** (`\\`).

---

## Paso 4 — Crear la base de datos

Elige **una** de las dos opciones. La **Opción A** es la más sencilla porque crea la base de datos sola.

### Opción A (recomendada): Migraciones de EF Core

Desde la carpeta donde está el `.sln`:

```powershell
dotnet ef database update --project SegurosAuto.Api
```

Esto **crea la base `SegurosAutoDb`**, las 5 tablas y carga los datos iniciales. Al terminar debe mostrar `Done.`

**Con Visual Studio**, en lugar del comando: menú *Herramientas → Administrador de paquetes NuGet → Consola del Administrador de paquetes*, verifica que **Proyecto predeterminado** sea `SegurosAuto.Api` y ejecuta:

```
Update-Database
```

### Opción B: Script SQL (con SSMS)

El script crea las tablas, pero **la base de datos debe existir antes**.

1. Abre **SQL Server Management Studio** y conéctate a tu servidor (`(localdb)\MSSQLLocalDB` o `localhost\SQLEXPRESS`).
2. Abre una **Nueva consulta**, pega y ejecuta (F5):

```sql
CREATE DATABASE SegurosAutoDb;
GO
```

3. Abre el archivo `database/01_crear_base_de_datos.sql` en SSMS.
4. En el selector de base de datos (arriba a la izquierda), elige **SegurosAutoDb**, o agrega al inicio del script:

```sql
USE SegurosAutoDb;
GO
```

5. Ejecuta el script (F5).

### Verificar que la base quedó creada

En SSMS, abre una nueva consulta sobre `SegurosAutoDb` y ejecuta:

```sql
SELECT COUNT(*) AS Coberturas FROM Coberturas;   -- debe dar 5
SELECT COUNT(*) AS Clientes   FROM Clientes;     -- debe dar 2
```

Deben existir 5 tablas: `Clientes`, `Vehiculos`, `Coberturas`, `Polizas`, `PolizaCoberturas` (más `__EFMigrationsHistory` si usaste migraciones).

---

## Paso 5 — Ejecutar la API

### Opción A: Terminal

```powershell
dotnet run --project SegurosAuto.Api --launch-profile https
```

Espera a ver líneas como estas (el puerto puede variar):

```
Now listening on: https://localhost:7110
Now listening on: http://localhost:5110
Application started. Press Ctrl+C to shut down.
```

Abre en el navegador: **`https://localhost:7110/swagger`** (usa tu puerto).

Para detener la API: `Ctrl + C`.

### Opción B: Visual Studio 2022

1. Abre `SegurosAuto.sln`. Visual Studio restaura los paquetes solo.
2. En el Explorador de soluciones, clic derecho en `SegurosAuto.Api` → **Establecer como proyecto de inicio**.
3. En la barra superior, elige el perfil **https** (no IIS Express).
4. Presiona **F5**. Se abre el navegador en Swagger.

### Si el navegador advierte sobre el certificado HTTPS

Es el certificado de desarrollo de .NET. Confíalo una sola vez con:

```powershell
dotnet dev-certs https --trust
```

(Acepta el cuadro de diálogo que aparece.) Reinicia el navegador.

---

## Paso 6 — Verificar que todo funciona

En Swagger sigue esta secuencia. Cada paso debe dar el resultado indicado:

| # | Acción | Resultado esperado |
| --- | --- | --- |
| 1 | `GET /api/clientes` → *Try it out* → *Execute* | **200**, lista con 2 clientes |
| 2 | `GET /api/coberturas` | **200**, lista con 5 coberturas |
| 3 | `POST /api/polizas/emitir` con el cuerpo de ejemplo (ya viene relleno) | **201**, `numeroPoliza: "POL-2026-000001"`, `primaTotal: 6000` |
| 4 | `GET /api/polizas/1` | **200**, detalle completo de la póliza |
| 5 | `GET /api/polizas` | **200**, historial con la póliza emitida |
| 6 | `POST /api/polizas/emitir` con `"clienteId": 999` | **404** |
| 7 | `POST /api/polizas/emitir` con `"coberturaIds": []` | **400** |
| 8 | `GET /api/polizas/9999` | **404** |

Si los 8 pasos dan lo esperado, la instalación está correcta. ✅

También puedes comprobarlo desde SSMS, después del paso 3:

```sql
SELECT * FROM Polizas;
SELECT * FROM PolizaCoberturas;   -- una fila por cobertura contratada
```

---

## Solución de problemas

| Síntoma | Causa probable | Solución |
| --- | --- | --- |
| `dotnet` no se reconoce como comando | .NET SDK no instalado o terminal abierta antes de instalar | Instala el SDK (paso 1.1) y **abre una terminal nueva**. |
| `dotnet ef` no se reconoce | Herramienta no instalada | `dotnet tool install --global dotnet-ef --version 8.0.10` y abre una terminal nueva. |
| `A network-related or instance-specific error` / `Error 40` / `Error 53` | SQL Server apagado o nombre de servidor incorrecto | Revisa el `Server=` del paso 3. En LocalDB: `sqllocaldb start MSSQLLocalDB`. |
| `Login failed for user` | Usuario o clave incorrectos | Revisa `User Id` y `Password` en la cadena de conexión. |
| `Cannot open database "SegurosAutoDb"` | La base aún no existe | Ejecuta el paso 4. |
| `Invalid object name 'Clientes'` | La base existe pero sin tablas | Ejecuta `dotnet ef database update --project SegurosAuto.Api` o el script SQL (paso 4). |
| `There is already an object named...` al correr el script | Ya se habían creado las tablas | Usa una base vacía, o elimínala: `DROP DATABASE SegurosAutoDb;` y repite el paso 4. |
| `Could not find file '...SegurosAuto.Api.xml'` | Falta generar el archivo de documentación | Compila primero: `dotnet build SegurosAuto.Api`. El proyecto ya trae `GenerateDocumentationFile` activado. |
| Swagger abre pero sin descripciones | El `.xml` no se generó | `dotnet build SegurosAuto.Api`, vuelve a ejecutar y fuerza refresco (`Ctrl + F5`). |
| `Address already in use` / puerto ocupado | Otra instancia corriendo | Cierra la otra ejecución (`Ctrl + C`) o cambia el puerto en `Properties/launchSettings.json`. |
| Advertencia de certificado HTTPS | Certificado de desarrollo sin confiar | `dotnet dev-certs https --trust` |
| Quiero empezar de cero la base de datos | — | `dotnet ef database drop --project SegurosAuto.Api` y luego `dotnet ef database update --project SegurosAuto.Api`. |

---

## Endpoints

### Clientes (CRUD completo)

| Método | Ruta | Descripción | Respuestas |
| --- | --- | --- | --- |
| GET | `/api/clientes` | Lista de clientes | 200 |
| GET | `/api/clientes/{id}` | Detalle de un cliente | 200, 404 |
| POST | `/api/clientes` | Crea un cliente | 201, 400 |
| PUT | `/api/clientes/{id}` | Actualiza un cliente | 200, 400, 404 |
| DELETE | `/api/clientes/{id}` | Elimina un cliente (sin pólizas) | 204, 400, 404 |

### Coberturas (CRUD completo)

| Método | Ruta | Descripción | Respuestas |
| --- | --- | --- | --- |
| GET | `/api/coberturas` | Catálogo de coberturas | 200 |
| GET | `/api/coberturas/{id}` | Detalle de una cobertura | 200, 404 |
| POST | `/api/coberturas` | Crea una cobertura | 201, 400 |
| PUT | `/api/coberturas/{id}` | Actualiza una cobertura | 200, 400, 404 |
| DELETE | `/api/coberturas/{id}` | Elimina una cobertura (sin uso) | 204, 400, 404 |

### Pólizas

| Método | Ruta | Descripción | Respuestas |
| --- | --- | --- | --- |
| POST | `/api/polizas/emitir` | **Emite una póliza** (principal) | 201, 400, 404 |
| GET | `/api/polizas/{id}` | Detalle de una póliza emitida | 200, 404 |
| GET | `/api/polizas` | Historial de todas las emisiones | 200 |

### Probar con Postman (opcional)

Con la API corriendo: **Import** → pega `https://localhost:<puerto>/swagger/v1/swagger.json` → **Import**. Se genera la colección completa.

---

## Ejemplo de emisión de póliza

**Solicitud**

```http
POST /api/polizas/emitir
Content-Type: application/json

{
  "clienteId": 1,
  "vehiculo": {
    "placa": "M123456",
    "marca": "Toyota",
    "modelo": "Corolla",
    "anio": 2022,
    "valorComercial": 18500.00
  },
  "coberturaIds": [1, 2, 3]
}
```

**Respuesta `201 Created`**

```json
{
  "id": 1,
  "numeroPoliza": "POL-2026-000001",
  "fechaEmision": "2026-10-30T15:30:00Z",
  "sumaAsegurada": 18500.00,
  "primaTotal": 6000.00,
  "cliente": { "id": 1, "nombre": "Juan Pérez", "identificacion": "001-010190-0001A", "correo": "juan.perez@correo.com" },
  "vehiculo": { "id": 1, "placa": "M123456", "marca": "Toyota", "modelo": "Corolla", "anio": 2022, "valorComercial": 18500.00 },
  "coberturas": [
    { "id": 1, "nombre": "Responsabilidad Civil", "montoCobertura": 1500.00 },
    { "id": 2, "nombre": "Choque", "montoCobertura": 2500.00 },
    { "id": 3, "nombre": "Robo", "montoCobertura": 2000.00 }
  ]
}
```

### Datos iniciales cargados

| Coberturas | Monto |
| --- | --- |
| 1 · Responsabilidad Civil | 1,500.00 |
| 2 · Choque | 2,500.00 |
| 3 · Robo | 2,000.00 |
| 4 · Incendio | 1,000.00 |
| 5 · Asistencia Vial | 300.00 |

| Clientes | Identificación |
| --- | --- |
| 1 · Juan Pérez | 001-010190-0001A |
| 2 · María López | 001-150285-0002B |

---

## Reglas de negocio

- **PrimaTotal** se calcula en el servidor: suma de los montos de las coberturas seleccionadas (1,500 + 2,500 + 2,000 = **6,000**).
- **SumaAsegurada** es el valor comercial del vehículo.
- **NumeroPoliza** se autogenera con el formato `POL-AAAA-000001`.
- Si la **placa** ya existe, se reutiliza el vehículo y se actualizan sus datos; si no, se crea uno nuevo.
- Los Ids de cobertura repetidos se consideran una sola vez.
- El cliente y todas las coberturas deben existir. El año del vehículo no puede ser futuro.
- Póliza, vehículo y coberturas se guardan en **una sola transacción**.
- Cada póliza guarda el monto de cada cobertura **al momento de emitirse**: si el catálogo cambia después, las pólizas existentes no se alteran.
- No se puede eliminar un **cliente** con pólizas ni una **cobertura** incluida en pólizas emitidas (responde 400).

**Códigos de respuesta:** `201` recurso creado · `400` validación o regla de negocio incumplida · `404` recurso inexistente · `500` error inesperado. Los errores usan el formato estándar `ProblemDetails`:

```json
{ "title": "Recurso no encontrado", "status": 404, "detail": "El cliente con Id 999 no existe." }
```

---

## Arquitectura y modelo de datos

```
Controller  →  Service  →  Repository  →  SQL Server
```

| Carpeta | Responsabilidad |
| --- | --- |
| `Controllers/` | Reciben la petición HTTP y devuelven el código de respuesta. |
| `Services/` | Lógica de negocio: validaciones y cálculo de la prima. |
| `Repositories/` | Único punto de acceso a la base de datos (EF Core). |
| `Models/` | Entidades que se mapean a las tablas. |
| `Data/` | `AppDbContext`: llaves, tipos, índices únicos y datos iniciales. |
| `Dtos/` | Contratos de entrada y salida del JSON, con sus validaciones. |
| `Exceptions/` · `Middleware/` | Convierten excepciones en respuestas HTTP (400, 404, 500). |

| Tabla | Campos principales | Notas |
| --- | --- | --- |
| **Clientes** | `Id`, `Nombre`, `Identificacion`, `Correo` | `Identificacion` única. |
| **Vehiculos** | `Id`, `Placa`, `Marca`, `Modelo`, `Anio`, `ValorComercial` | `Placa` única. |
| **Coberturas** | `Id`, `Nombre`, `MontoCobertura` | Catálogo. |
| **Polizas** | `Id`, `NumeroPoliza`, `ClienteId`, `VehiculoId`, `FechaEmision`, `SumaAsegurada`, `PrimaTotal` | `NumeroPoliza` único. |
| **PolizaCoberturas** | `PolizaId`, `CoberturaId`, `MontoCobertura` | Muchos a muchos; llave compuesta. |

Relaciones: Cliente 1—N Pólizas (`RESTRICT`) · Vehículo 1—N Pólizas (`RESTRICT`) · Póliza N—N Cobertura vía `PolizaCoberturas` (`CASCADE` desde la póliza, `RESTRICT` hacia la cobertura).

---

## Decisiones de diseño

- **Code First con migraciones**, más un script SQL generado a partir de ellas.
- **DTOs separados de las entidades:** la API nunca expone directamente el modelo de base de datos.
- **Montos en `decimal(18,2)`** para evitar errores de redondeo; fechas en **UTC**.
- **Índices únicos** en identificación del cliente, placa del vehículo y número de póliza.
- **Pólizas inmutables:** una póliza emitida es un documento con valor legal y su prima queda congelada, por lo que solo se crea, se consulta y se lista.
- **Manejo de errores centralizado** en un middleware, sin `try/catch` en los controllers.
- **Swagger documentado** con comentarios XML (descripciones, ejemplos y códigos de respuesta por endpoint).

## Autor

Byron Salvador Picado Bermudez-picadobyron495@gmail.com
