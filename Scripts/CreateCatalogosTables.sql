USE [crud_agenda]
GO

-- Empresa
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agenda_Empresa]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Agenda_Empresa] (
        [IdEmpresa] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [NombreEmpresa] NVARCHAR(100) NOT NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME NULL
    )
    CREATE INDEX IX_Agenda_Empresa_Activo ON [dbo].[Agenda_Empresa]([Activo])
    PRINT 'Tabla Agenda_Empresa creada.'
END
GO

-- Clínica
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agenda_Clinica]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Agenda_Clinica] (
        [IdClinica] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [IdEmpresa] INT NOT NULL,
        [NombreClinica] NVARCHAR(100) NOT NULL,
        [Direccion] NVARCHAR(200) NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME NULL,
        CONSTRAINT FK_Agenda_Clinica_Empresa FOREIGN KEY ([IdEmpresa]) REFERENCES [dbo].[Agenda_Empresa]([IdEmpresa])
    )
    CREATE INDEX IX_Agenda_Clinica_Empresa ON [dbo].[Agenda_Clinica]([IdEmpresa])
    PRINT 'Tabla Agenda_Clinica creada.'
END
GO

-- Doctor
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agenda_Doctor]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Agenda_Doctor] (
        [Codigo_Doctor] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [IdEmpresa] INT NOT NULL,
        [IdClinica] INT NULL,
        [NombreDoctor] NVARCHAR(100) NOT NULL,
        [Especialidad] NVARCHAR(100) NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME NULL,
        CONSTRAINT FK_Agenda_Doctor_Empresa FOREIGN KEY ([IdEmpresa]) REFERENCES [dbo].[Agenda_Empresa]([IdEmpresa]),
        CONSTRAINT FK_Agenda_Doctor_Clinica FOREIGN KEY ([IdClinica]) REFERENCES [dbo].[Agenda_Clinica]([IdClinica])
    )
    CREATE INDEX IX_Agenda_Doctor_Empresa ON [dbo].[Agenda_Doctor]([IdEmpresa])
    CREATE INDEX IX_Agenda_Doctor_Clinica ON [dbo].[Agenda_Doctor]([IdClinica])
    PRINT 'Tabla Agenda_Doctor creada.'
END
GO

-- Datos de ejemplo
IF NOT EXISTS (SELECT 1 FROM [dbo].[Agenda_Empresa])
BEGIN
    INSERT INTO [dbo].[Agenda_Empresa] ([NombreEmpresa], [Activo]) VALUES
    (N'Sercomtec Salud', 1),
    (N'Clínica Integral', 1)

    DECLARE @Emp1 INT = (SELECT IdEmpresa FROM [dbo].[Agenda_Empresa] WHERE NombreEmpresa = N'Sercomtec Salud')
    DECLARE @Emp2 INT = (SELECT IdEmpresa FROM [dbo].[Agenda_Empresa] WHERE NombreEmpresa = N'Clínica Integral')

    INSERT INTO [dbo].[Agenda_Clinica] ([IdEmpresa], [NombreClinica], [Direccion], [Activo]) VALUES
    (@Emp1, N'Sede Central', N'Av. Principal 100', 1),
    (@Emp1, N'Sede Norte', N'Calle Norte 250', 1),
    (@Emp2, N'Consultorios Sur', N'Blvd. Sur 45', 1)

    DECLARE @Cli1 INT = (SELECT IdClinica FROM [dbo].[Agenda_Clinica] WHERE NombreClinica = N'Sede Central')
    DECLARE @Cli2 INT = (SELECT IdClinica FROM [dbo].[Agenda_Clinica] WHERE NombreClinica = N'Sede Norte')

    INSERT INTO [dbo].[Agenda_Doctor] ([IdEmpresa], [IdClinica], [NombreDoctor], [Especialidad], [Activo]) VALUES
    (@Emp1, @Cli1, N'Dr. Carlos García', N'Cardiología', 1),
    (@Emp1, @Cli1, N'Dra. Ana López', N'Pediatría', 1),
    (@Emp1, @Cli2, N'Dr. Miguel Ruiz', N'Medicina General', 1),
    (@Emp2, NULL, N'Dra. Laura Méndez', N'Ginecología', 1)

    PRINT 'Datos de ejemplo insertados en catálogos.'
END
GO
