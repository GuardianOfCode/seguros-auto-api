
# 🚗 Seguros Auto API

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/C%23-Backend-239120?style=for-the-badge&logo=csharp&logoColor=white" />
  <img src="https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/SQL_Server-Database-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" />
  <img src="https://img.shields.io/badge/Entity_Framework_Core-ORM-68217A?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Swagger-API_Documentation-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" />
</p>

<p align="center">
  **API REST para la gestión de seguros de automóviles**
  <br />
  Desarrollada con ASP.NET Core 8, Entity Framework Core y SQL Server.
</p>

---

## 📌 Descripción del proyecto

**Seguros Auto API** es una interfaz de programación de aplicaciones (API REST) desarrollada en ASP.NET Core 8, orientada a la gestión de un sistema de seguros de automóviles.

El proyecto permite administrar clientes, vehículos, coberturas y pólizas mediante endpoints HTTP, utilizando SQL Server como sistema gestor de bases de datos y Entity Framework Core como herramienta de acceso a datos.

La API cuenta con una arquitectura organizada por responsabilidades, facilitando el mantenimiento, la escalabilidad y la separación de la lógica de negocio.

---

## 🛠️ Tecnologías utilizadas

| Tecnología | Descripción |
|---|---|
| C# | Lenguaje de programación |
| .NET 8 | Plataforma de desarrollo |
| ASP.NET Core Web API | Desarrollo de servicios REST |
| Entity Framework Core | ORM para acceso a datos |
| SQL Server | Sistema gestor de bases de datos |
| Swagger | Documentación y pruebas de endpoints |
| Visual Studio | Entorno de desarrollo |

---

## 📂 Estructura del proyecto

```text
SegurosAuto/
│
├── Script de base de datos.sql
│
└── SegurosAuto.Api/
    │
    ├── Controllers/
    │   ├── ClientesController.cs
    │   ├── VehiculosController.cs
    │   ├── CoberturasController.cs
    │   └── PolizasController.cs
    │
    ├── Data/
    │   └── ApplicationDbContext.cs
    │
    ├── Models/
    │
    ├── DTOs/
    │
    ├── Services/
    │
    ├── Repositories/
    │
    ├── appsettings.json
    ├── Program.cs
    └── SegurosAuto.Api.csproj
```

---

## 🗄️ Base de datos

El proyecto utiliza **SQL Server** y una base de datos denominada:

```sql
SegurosAutoDb
```

La estructura de la base de datos se encuentra incluida en el repositorio mediante el archivo:

`Script de base de datos.sql`

### 📥 Creación de la base de datos

Para preparar el entorno de ejecución, es necesario crear la base de datos antes de iniciar la API.

1. Descargar o clonar el repositorio.
2. Localizar el archivo `Script de base de datos.sql` en la raíz del proyecto.
3. Abrir **SQL Server Management Studio (SSMS)**.
4. Conectarse a la instancia de SQL Server que se utilizará.
5. Abrir el archivo SQL desde SSMS.
6. Ejecutar el script mediante el botón **Execute** o la tecla `F5`.

Al finalizar, se creará la base de datos `SegurosAutoDb` con las tablas y relaciones necesarias para el funcionamiento de la API.

---

## ⚙️ Configuración del proyecto

Antes de ejecutar la API, es necesario configurar la conexión con SQL Server.

### 1. Requisitos previos

Verificar que se encuentren instalados los siguientes componentes:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads)
- [SQL Server Management Studio](https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms)
- [Visual Studio](https://visualstudio.microsoft.com/) o un editor compatible con .NET.

### 2. Clonar el repositorio

```bash
git clone URL_DEL_REPOSITORIO
```

Ingresar al directorio del proyecto:

```bash
cd SegurosAuto
```

### 3. Configurar la cadena de conexión

Abrir el archivo `appsettings.json`, ubicado dentro del proyecto `SegurosAuto.Api`.

Localizar la sección `ConnectionStrings` y configurar la conexión a SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=SegurosAutoDb;Integrated Security=True;Encrypt=False;TrustServerCertificate=True"
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

**Importante:** Reemplazar `TU_SERVIDOR` por el nombre de la instancia de SQL Server utilizada en el entorno de desarrollo.

Por ejemplo, dependiendo de la instalación, puede ser el nombre de una instancia local, una instancia de SQL Server Express o un servidor remoto.

La cadena de conexión debe corresponder con la instancia donde se ejecutó previamente el script de base de datos.

---

## 📦 Restauración de dependencias

Abrir una terminal en la carpeta del proyecto `SegurosAuto.Api` y ejecutar:

```bash
dotnet restore
```

Este comando descarga y restaura los paquetes NuGet necesarios para compilar y ejecutar la aplicación.

---

## ▶️ Ejecución de la API

Desde la terminal, dentro del directorio `SegurosAuto.Api`, ejecutar:

```bash
dotnet run
```

Una vez iniciada correctamente, la API estará disponible en las direcciones que indique la consola.

Si se utiliza Visual Studio, también es posible ejecutar el proyecto mediante el botón **Start** o `F5`.

---

## 📖 Documentación y pruebas con Swagger

El proyecto incorpora Swagger para documentar y probar los endpoints de la API de manera interactiva.

Una vez iniciada la aplicación, abrir en el navegador la dirección de Swagger indicada por el entorno de ejecución. Generalmente:

```text
https://localhost:PUERTO/swagger
```

Desde esta interfaz es posible:

- Consultar los endpoints disponibles.
- Visualizar los métodos HTTP.
- Revisar los parámetros y modelos de datos.
- Enviar solicitudes directamente a la API.
- Verificar las respuestas y códigos de estado HTTP.

---

## 🔗 Endpoints principales

| Módulo | Descripción |
|---|---|
| Clientes | Administración de clientes |
| Vehículos | Administración de vehículos |
| Coberturas | Administración de coberturas |
| Pólizas | Gestión de pólizas de seguros |

Los endpoints específicos pueden consultarse desde la documentación interactiva de Swagger.

---

## 🧩 Modelo de datos

La base de datos contempla las siguientes entidades principales:

| Tabla | Descripción |
|---|---|
| Clientes | Información de los clientes |
| Vehiculos | Información de los vehículos asegurados |
| Coberturas | Tipos de cobertura disponibles |
| Polizas | Información de las pólizas emitidas |
| PolizaCoberturas | Relación entre pólizas y coberturas |

### Relaciones principales

- Un cliente puede tener varias pólizas.
- Un vehículo puede estar asociado a varias pólizas.
- Una póliza puede incluir varias coberturas.
- Una cobertura puede estar asociada a varias pólizas.

La relación entre pólizas y coberturas se implementa mediante la tabla intermedia `PolizaCoberturas`.

---

## 🚀 Flujo de instalación

Para poner en funcionamiento el proyecto desde cero, seguir este orden:

```text
        DESCARGAR REPOSITORIO
                 │
                 ▼
       EJECUTAR SCRIPT SQL
                 │
                 ▼
       CREAR BASE DE DATOS
          SegurosAutoDb
                 │
                 ▼
      CONFIGURAR APPSETTINGS
       Cadena de conexión
                 │
                 ▼
       RESTAURAR DEPENDENCIAS
           dotnet restore
                 │
                 ▼
          EJECUTAR API
             dotnet run
                 │
                 ▼
        ABRIR DOCUMENTACIÓN
              SWAGGER
                 │
                 ▼
         PROBAR ENDPOINTS
```

---

## 👨‍💻 Autor

**Desarrollado como proyecto de gestión de seguros de automóviles.**

---

<p align="center">
  <strong>Seguros Auto API</strong>
  <br />
  <sub>Desarrollado con .NET 8 · ASP.NET Core · SQL Server</sub>
</p>
