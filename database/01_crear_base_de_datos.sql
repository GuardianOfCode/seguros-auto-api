
 =====================================================================
   SISTEMA DE EMISION DE SEGUROS DE AUTOMOVIL
   Base de datos SegurosAutoDb
   Motor SQL Server 2019+
   ===================================================================== 

IF DB_ID(N'SegurosAutoDb') IS NULL
BEGIN
    CREATE DATABASE SegurosAutoDb;
END
GO

USE SegurosAutoDb;
GO

 =====================================================================
   1. TABLA CLIENTES
   ===================================================================== 

CREATE TABLE dbo.Clientes (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(150) NOT NULL,
    Identificacion NVARCHAR(30) NOT NULL,
    Correo NVARCHAR(150) NOT NULL,

    CONSTRAINT PK_Clientes PRIMARY KEY (Id),
    CONSTRAINT UQ_Clientes_Identificacion UNIQUE (Identificacion)
);
GO

 =====================================================================
   2. TABLA VEHICULOS
   ===================================================================== 

CREATE TABLE dbo.Vehiculos (
    Id INT IDENTITY(1,1) NOT NULL,
    Placa NVARCHAR(15) NOT NULL,
    Marca NVARCHAR(50) NOT NULL,
    Modelo NVARCHAR(50) NOT NULL,
    Anio INT NOT NULL,
    ValorComercial DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_Vehiculos PRIMARY KEY (Id),
    CONSTRAINT UQ_Vehiculos_Placa UNIQUE (Placa),
    CONSTRAINT CK_Vehiculos_ValorComercial
        CHECK (ValorComercial  0),
    CONSTRAINT CK_Vehiculos_Anio
        CHECK (Anio BETWEEN 1900 AND 2100)
);
GO

 =====================================================================
   3. TABLA COBERTURAS
   ===================================================================== 

CREATE TABLE dbo.Coberturas (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    MontoCobertura DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_Coberturas PRIMARY KEY (Id),
    CONSTRAINT CK_Coberturas_Monto
        CHECK (MontoCobertura = 0)
);
GO

 =====================================================================
   4. TABLA POLIZAS
   ===================================================================== 

CREATE TABLE dbo.Polizas (
    Id INT IDENTITY(1,1) NOT NULL,
    NumeroPoliza NVARCHAR(30) NOT NULL,
    ClienteId INT NOT NULL,
    VehiculoId INT NOT NULL,
    FechaEmision DATETIME2 NOT NULL,
    SumaAsegurada DECIMAL(18,2) NOT NULL,
    PrimaTotal DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_Polizas PRIMARY KEY (Id),

    CONSTRAINT UQ_Polizas_NumeroPoliza
        UNIQUE (NumeroPoliza),

    CONSTRAINT FK_Polizas_Clientes
        FOREIGN KEY (ClienteId)
        REFERENCES dbo.Clientes(Id),

    CONSTRAINT FK_Polizas_Vehiculos
        FOREIGN KEY (VehiculoId)
        REFERENCES dbo.Vehiculos(Id),

    CONSTRAINT CK_Polizas_SumaAsegurada
        CHECK (SumaAsegurada  0),

    CONSTRAINT CK_Polizas_PrimaTotal
        CHECK (PrimaTotal = 0)
);
GO

 =====================================================================
   5. TABLA POLIZACOBERTURAS
   RELACION MUCHOS A MUCHOS
   ===================================================================== 

CREATE TABLE dbo.PolizaCoberturas (
    PolizaId INT NOT NULL,
    CoberturaId INT NOT NULL,
    MontoCobertura DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_PolizaCoberturas
        PRIMARY KEY (PolizaId, CoberturaId),

    CONSTRAINT FK_PolizaCoberturas_Polizas
        FOREIGN KEY (PolizaId)
        REFERENCES dbo.Polizas(Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_PolizaCoberturas_Coberturas
        FOREIGN KEY (CoberturaId)
        REFERENCES dbo.Coberturas(Id),

    CONSTRAINT CK_PolizaCoberturas_Monto
        CHECK (MontoCobertura = 0)
);
GO

 =====================================================================
   6. INDICES
   ===================================================================== 

CREATE INDEX IX_Polizas_ClienteId
ON dbo.Polizas(ClienteId);

CREATE INDEX IX_Polizas_VehiculoId
ON dbo.Polizas(VehiculoId);

CREATE INDEX IX_PolizaCoberturas_CoberturaId
ON dbo.PolizaCoberturas(CoberturaId);
GO

 =====================================================================
   7. DATOS INICIALES COBERTURAS
   ===================================================================== 

INSERT INTO dbo.Coberturas
(Nombre, MontoCobertura)
VALUES
(N'Responsabilidad Civil', 1500.00),
(N'Choque', 2500.00),
(N'Robo', 2000.00),
(N'Incendio', 1000.00),
(N'Asistencia Vial', 300.00);
GO

 =====================================================================
   8. DATOS INICIALES CLIENTES
   ===================================================================== 

INSERT INTO dbo.Clientes
(Nombre, Identificacion, Correo)
VALUES
(N'Juan Pérez', N'001-010190-0001A',
 N'juan.perez@correo.com'),

(N'María López', N'001-150285-0002B',
 N'maria.lopez@correo.com');
GO

 =====================================================================
   9. DATOS INICIALES VEHICULOS
   ===================================================================== 

INSERT INTO dbo.Vehiculos
(Placa, Marca, Modelo, Anio, ValorComercial)
VALUES
(N'M123456', N'Toyota', N'Corolla', 2022, 18000),
(N'M654321', N'Hyundai', N'H100', 2023, 22000),
(N'M987654', N'Nissan', N'Frontier', 2021, 25000),
(N'M456789', N'Toyota', N'Hilux', 2024, 35000),
(N'M789123', N'Kia', N'Sportage', 2022, 21000);
GO

 =====================================================================
   10. DATOS INICIALES POLIZAS
   ===================================================================== 

INSERT INTO dbo.Polizas
(NumeroPoliza, ClienteId, VehiculoId,
 FechaEmision, SumaAsegurada, PrimaTotal)
VALUES
(N'POL-001', 1, 1, '2026-01-10', 18000, 1200),
(N'POL-002', 2, 2, '2026-01-15', 22000, 1500),
(N'POL-003', 1, 3, '2026-02-01', 25000, 1800),
(N'POL-004', 2, 4, '2026-02-10', 35000, 2200),
(N'POL-005', 1, 5, '2026-02-15', 21000, 1600);
GO

 =====================================================================
   11. RELACIONAR POLIZAS CON SUS COBERTURAS
   ===================================================================== 

INSERT INTO dbo.PolizaCoberturas
(PolizaId, CoberturaId, MontoCobertura)
VALUES
(1, 1, 1500),
(1, 2, 2500),
(1, 3, 2000),

(2, 1, 1500),
(2, 2, 2500),
(2, 4, 1000),

(3, 1, 1500),
(3, 3, 2000),
(3, 5, 300),

(4, 1, 1500),
(4, 2, 2500),
(4, 3, 2000),
(4, 4, 1000),

(5, 1, 1500),
(5, 2, 2500),
(5, 5, 300);
GO

 =====================================================================
   12. CONSULTA GENERAL DE POLIZAS
   ===================================================================== 

SELECT
    P.Id,
    P.NumeroPoliza,
    C.Nombre AS Cliente,
    C.Identificacion,
    V.Placa,
    V.Marca,
    V.Modelo,
    V.Anio,
    P.FechaEmision,
    P.SumaAsegurada,
    P.PrimaTotal
FROM dbo.Polizas P
INNER JOIN dbo.Clientes C
    ON P.ClienteId = C.Id
INNER JOIN dbo.Vehiculos V
    ON P.VehiculoId = V.Id
ORDER BY P.Id;
GO

 =====================================================================
   13. CONSULTA DE COBERTURAS POR POLIZA
   ===================================================================== 

SELECT
    P.NumeroPoliza,
    C.Nombre AS Cliente,
    CO.Nombre AS Cobertura,
    PC.MontoCobertura
FROM dbo.PolizaCoberturas PC
INNER JOIN dbo.Polizas P
    ON PC.PolizaId = P.Id
INNER JOIN dbo.Clientes C
    ON P.ClienteId = C.Id
INNER JOIN dbo.Coberturas CO
    ON PC.CoberturaId = CO.Id
ORDER BY P.NumeroPoliza, CO.Nombre;
GO

 =====================================================================
   FIN DEL SCRIPT
   ===================================================================== 
