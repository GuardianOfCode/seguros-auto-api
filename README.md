
# 🚗 Seguros Auto API

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" />
  <img src="https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" />
  <img src="https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=for-the-badge&logo=nuget&logoColor=white" />
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" />
</p>

<p align="center">
  <strong>API REST para la gestión y emisión de pólizas de seguros de automóviles</strong>
</p>

---

## 📑 Contenido

1. [Descripción](#descripción)
2. [Tecnologías](#tecnologías)
3. [Estructura del proyecto](#estructura-del-proyecto)
4. [Requisitos](#requisitos)
5. [Configuración](#configuración)
6. [Base de datos](#base-de-datos)
7. [Ejecución del proyecto](#ejecución-del-proyecto)
8. [Endpoints](#endpoints)
9. [Ejemplo de emisión de póliza](#ejemplo-de-emisión-de-póliza)
10. [Reglas de negocio](#reglas-de-negocio)
11. [Arquitectura y modelo de datos](#arquitectura-y-modelo-de-datos)
12. [Solución de problemas](#solución-de-problemas)
13. [Autor](#autor)

---

## 🚀 Descripción

**Seguros Auto API** es un servicio REST desarrollado para gestionar la emisión de pólizas de seguros de automóviles.

El sistema permite registrar clientes, administrar coberturas, registrar vehículos y emitir pólizas relacionando la información del cliente, el vehículo y las coberturas seleccionadas.

La aplicación está organizada mediante una arquitectura por capas, separando las responsabilidades de los controladores, servicios y repositorios para facilitar el mantenimiento y la evolución del sistema.

---

## 🧰 Tecnologías

| Tecnología | Descripción |
|---|---|
| C# | Lenguaje de programación. |
| ASP.NET Core .NET 8 | Framework para desarrollar la API REST. |
| Entity Framework Core | ORM para el acceso y gestión de datos. |
| SQL Server | Sistema gestor de base de datos relacional. |
| Swagger / OpenAPI | Documentación y pruebas de endpoints. |
| Visual Studio 2022 | Entorno de desarrollo. |

---

## 📁 Estructura del proyecto

```text
SegurosAuto/
├── SegurosAuto.sln
├── README.md
├── database/
│   └── 01_crear_base_de_datos.sql
└── SegurosAuto.Api/
    ├── Controllers/
    ├── Data/
    ├── Dtos/
    ├── Exceptions/
    ├── Middleware/
    ├── Migrations/
    ├── Models/
    ├── Repositories/
    ├── Services/
    ├── Properties/
    ├── appsettings.json
    └── Program.cs
```

### 🗂️ Descripción de las carpetas

| Carpeta | Responsabilidad |
|---|---|
| `Controllers/` | Recibe las solicitudes HTTP y devuelve las respuestas de la API. |
| `Services/` | Contiene la lógica de negocio y las validaciones del sistema. |
| `Repositories/` | Gestiona el acceso a la base de datos mediante Entity Framework Core. |
| `Models/` | Contiene las entidades que representan las tablas de la base de datos. |
| `Data/` | Contiene `AppDbContext`, la configuración de las entidades y los datos iniciales. |
| `Dtos/` | Define los objetos de transferencia de datos para las solicitudes y respuestas JSON. |
| `Exceptions/` | Contiene las excepciones personalizadas de la aplicación. |
| `Middleware/` | Centraliza el manejo de errores y las respuestas HTTP. |
| `Migrations/` | Contiene las migraciones de Entity Framework Core. |

---

## ⚙️ Requisitos

Para ejecutar el proyecto se necesita:

- .NET 8 SDK.
- SQL Server.
- Visual Studio 2022.
- SQL Server Management Studio (SSMS).
- Entity Framework Core Tools, versión `8.0.10`.

### 🔎 Verificar .NET

```powershell
dotnet --version
```

La versión instalada debe comenzar con `8.`.

### 📦 Instalar Entity Framework Core Tools

```powershell
dotnet tool install --global dotnet-ef --version 8.0.10
```

Verificar la instalación:

```powershell
dotnet ef --version
```

---

## 🔌 Configuración

La conexión a SQL Server está definida en:

`SegurosAuto.Api/appsettings.json`

Configuración utilizada:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-CAKRKLJ;Database=SegurosAutoDb;Integrated Security=True;Encrypt=False;TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

La conexión utiliza:

- **Servidor:** `DESKTOP-CAKRKLJ`
- **Base de datos:** `SegurosAutoDb`
- **Autenticación:** Windows Authentication.
- **Cifrado:** desactivado mediante `Encrypt=False`.
- **Certificado:** `TrustServerCertificate=True`.

---

## 🗄️ Base de datos

El proyecto utiliza **Entity Framework Core Code First**, mediante migraciones para crear y actualizar la estructura de la base de datos.

Desde PowerShell, ubicado en la carpeta principal del proyecto, ejecuta:

```powershell
dotnet ef database update --project SegurosAuto.Api
```

Este comando aplica las migraciones y crea o actualiza la base de datos `SegurosAutoDb`.

### 🔍 Comprobar la base de datos

En SQL Server Management Studio, conéctate al servidor `DESKTOP-CAKRKLJ`, selecciona la base de datos `SegurosAutoDb` y ejecuta:

```sql
SELECT * FROM Clientes;
SELECT * FROM Vehiculos;
SELECT * FROM Coberturas;
SELECT * FROM Polizas;
SELECT * FROM PolizaCoberturas;
```

---

## ▶️ Ejecución del proyecto

Desde PowerShell, en la carpeta principal del proyecto, ejecuta:

```powershell
dotnet run --project SegurosAuto.Api --launch-profile https
```

Cuando la API inicie correctamente, la consola mostrará las direcciones locales de ejecución.

Abre la dirección HTTPS y agrega `/swagger`.

Ejemplo:

```text
https://localhost:7110/swagger
```

El puerto corresponde al configurado para la ejecución del proyecto.

### 💻 Ejecución desde Visual Studio 2022

1. Abre `SegurosAuto.sln`.
2. Selecciona `SegurosAuto.Api` como proyecto de inicio.
3. Selecciona el perfil `https`.
4. Presiona **F5** para iniciar la aplicación.

---

## 🌐 Endpoints

La API dispone de endpoints para administrar clientes, coberturas y pólizas.

### 👤 Clientes

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/clientes` | Obtiene todos los clientes. |
| GET | `/api/clientes/{id}` | Obtiene un cliente por su identificador. |
| POST | `/api/clientes` | Registra un nuevo cliente. |
| PUT | `/api/clientes/{id}` | Actualiza la información de un cliente. |
| DELETE | `/api/clientes/{id}` | Elimina un cliente que no tenga pólizas asociadas. |

### 🛡️ Coberturas

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/coberturas` | Obtiene todas las coberturas. |
| GET | `/api/coberturas/{id}` | Obtiene una cobertura por su identificador. |
| POST | `/api/coberturas` | Registra una nueva cobertura. |
| PUT | `/api/coberturas/{id}` | Actualiza una cobertura. |
| DELETE | `/api/coberturas/{id}` | Elimina una cobertura que no esté asociada a pólizas. |

### 📄 Pólizas

| Método | Endpoint | Descripción |
|---|---|---|
| POST | `/api/polizas/emitir` | Emite una nueva póliza de automóvil. |
| GET | `/api/polizas/{id}` | Consulta el detalle de una póliza. |
| GET | `/api/polizas` | Obtiene el historial de pólizas emitidas. |

---

## 🧾 Ejemplo de emisión de póliza

### 📤 Solicitud HTTP

```http
POST /api/polizas/emitir
Content-Type: application/json
```

### 📨 Cuerpo JSON

```json
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

### 📥 Respuesta esperada

```json
{
  "id": 1,
  "numeroPoliza": "POL-2026-000001",
  "fechaEmision": "2026-10-30T15:30:00Z",
  "sumaAsegurada": 18500.00,
  "primaTotal": 6000.00,
  "cliente": {
    "id": 1,
    "nombre": "Juan Pérez",
    "identificacion": "001-010190-0001A",
    "correo": "juan.perez@correo.com"
  },
  "vehiculo": {
    "id": 1,
    "placa": "M123456",
    "marca": "Toyota",
    "modelo": "Corolla",
    "anio": 2022,
    "valorComercial": 18500.00
  },
  "coberturas": [
    {
      "id": 1,
      "nombre": "Responsabilidad Civil",
      "montoCobertura": 1500.00
    },
    {
      "id": 2,
      "nombre": "Choque",
      "montoCobertura": 2500.00
    },
    {
      "id": 3,
      "nombre": "Robo",
      "montoCobertura": 2000.00
    }
  ]
}
```

---

## 📌 Reglas de negocio

- La prima total se calcula en el servidor sumando los montos de las coberturas seleccionadas.
- La suma asegurada corresponde al valor comercial del vehículo.
- El número de póliza se genera automáticamente con el formato `POL-AAAA-000001`.
- Si la placa del vehículo ya existe, se reutiliza el registro y se actualizan sus datos; si no existe, se registra un vehículo nuevo.
- Los identificadores de coberturas repetidos se consideran una sola vez.
- El cliente y las coberturas seleccionadas deben existir.
- El año del vehículo no puede ser posterior al año actual.
- La emisión guarda la póliza, el vehículo y sus coberturas dentro de una misma transacción.
- El monto de cada cobertura queda registrado en `PolizaCoberturas` al momento de emitir la póliza.
- Los cambios posteriores en el catálogo de coberturas no modifican las pólizas emitidas.
- No se permite eliminar clientes que tengan pólizas asociadas.
- No se permite eliminar coberturas que estén asociadas a pólizas emitidas.

### 📊 Códigos de respuesta HTTP

| Código | Descripción |
|---|---|
| `200 OK` | Solicitud procesada correctamente. |
| `201 Created` | Recurso creado correctamente. |
| `204 No Content` | Operación completada sin contenido de respuesta. |
| `400 Bad Request` | Error de validación o regla de negocio. |
| `404 Not Found` | Recurso no encontrado. |
| `500 Internal Server Error` | Error inesperado en el servidor. |

Los errores utilizan el formato estándar `ProblemDetails`.

Ejemplo:

```json
{
  "title": "Recurso no encontrado",
  "status": 404,
  "detail": "El cliente con Id 999 no existe."
}
```

---

## 🏗️ Arquitectura y modelo de datos

### 🔄 Arquitectura por capas

```text
             Cliente HTTP
                  |
                  v
             Controller
                  |
                  v
               Service
                  |
                  v
             Repository
                  |
                  v
              AppDbContext
                  |
                  v
              SQL Server
```

Cada capa tiene una responsabilidad específica:

- **Controller:** recibe las solicitudes HTTP y coordina las respuestas.
- **Service:** ejecuta las reglas de negocio y validaciones.
- **Repository:** realiza las operaciones de acceso a datos.
- **AppDbContext:** administra las entidades y su relación con la base de datos.
- **SQL Server:** almacena la información del sistema.

### 🔗 Relaciones del modelo de datos

```text
Clientes
   |
   | 1:N
   |
Polizas
   |
   | N:N
   |
PolizaCoberturas
   |
   | N:1
   |
Coberturas


Vehiculos
   |
   | 1:N
   |
Polizas
```

### 🗃️ Descripción de las tablas

| Tabla | Campos principales | Descripción |
|---|---|---|
| `Clientes` | `Id`, `Nombre`, `Identificacion`, `Correo` | Información de los clientes. |
| `Vehiculos` | `Id`, `Placa`, `Marca`, `Modelo`, `Anio`, `ValorComercial` | Información de los vehículos asegurados. |
| `Coberturas` | `Id`, `Nombre`, `MontoCobertura` | Catálogo de coberturas disponibles. |
| `Polizas` | `Id`, `NumeroPoliza`, `ClienteId`, `VehiculoId`, `FechaEmision`, `SumaAsegurada`, `PrimaTotal` | Información de las pólizas emitidas. |
| `PolizaCoberturas` | `PolizaId`, `CoberturaId`, `MontoCobertura` | Relación entre pólizas y coberturas. |

La tabla `PolizaCoberturas` implementa la relación muchos a muchos entre `Polizas` y `Coberturas`. Su clave primaria compuesta por `PolizaId` y `CoberturaId` evita registrar dos veces la misma cobertura dentro de una póliza.

---

## 🧩 Solución de problemas

| Problema | Solución |
|---|---|
| `dotnet` no se reconoce como comando. | Verifica que .NET 8 SDK esté instalado y abre una nueva terminal. |
| `dotnet ef` no se reconoce. | Instala Entity Framework Core Tools versión `8.0.10`. |
| Error de conexión con SQL Server. | Comprueba que el servidor `DESKTOP-CAKRKLJ` esté iniciado y que la cadena de conexión sea correcta. |
| `Login failed for user`. | Verifica la autenticación de Windows y los permisos del usuario. |
| No se puede abrir `SegurosAutoDb`. | Ejecuta `dotnet ef database update --project SegurosAuto.Api`. |
| `Invalid object name`. | Aplica las migraciones para crear las tablas. |
| Swagger no muestra la documentación. | Compila el proyecto con `dotnet build SegurosAuto.Api` y vuelve a ejecutarlo. |
| El puerto ya está en uso. | Detén la otra instancia de la API o revisa `Properties/launchSettings.json`. |
| Advertencia del certificado HTTPS. | Ejecuta `dotnet dev-certs https --trust`. |

---

## ✍️ Autor

**Byron Salvador Picado Bermúdez**  
📧 `picadobyron495@gmail.com`
