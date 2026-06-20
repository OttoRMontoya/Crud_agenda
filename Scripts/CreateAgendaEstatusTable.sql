-- Script para crear la tabla Agenda_Estatus en la base de datos crud_agenda
-- Ejecutar este script en SQL Server

USE [crud_agenda]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agenda_Estatus]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Agenda_Estatus] (
        [IdEstatus] INT PRIMARY KEY IDENTITY(1,1),
        [NombreEstatus] NVARCHAR(100) NOT NULL,
        [Descripcion] NVARCHAR(500) NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [FechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME NULL
    )

    -- Crear índice para mejorar búsquedas
    CREATE INDEX IX_Agenda_Estatus_Activo ON [dbo].[Agenda_Estatus]([Activo])
    CREATE INDEX IX_Agenda_Estatus_NombreEstatus ON [dbo].[Agenda_Estatus]([NombreEstatus])

    -- Insertar algunos datos de ejemplo (SIN especificar IdEstatus, se genera automáticamente)
    INSERT INTO [dbo].[Agenda_Estatus] ([NombreEstatus], [Descripcion], [Activo]) 
    VALUES
    (N'Pendiente', N'Estado inicial, esperando atención', 1),
    (N'En Proceso', N'Se está trabajando en la tarea', 1),
    (N'Completado', N'Tarea finalizada exitosamente', 1),
    (N'Cancelado', N'La tarea fue cancelada', 1),
    (N'En Espera', N'Esperando información o recursos adicionales', 1)

    PRINT 'Tabla Agenda_Estatus creada exitosamente con datos de ejemplo.'
END
ELSE
BEGIN
    PRINT 'La tabla Agenda_Estatus ya existe.'

    -- Si necesitas reiniciar los datos de ejemplo, descomenta lo siguiente:
    -- DELETE FROM [dbo].[Agenda_Estatus]
    -- DBCC CHECKIDENT ('[dbo].[Agenda_Estatus]', RESEED, 0)
    -- INSERT INTO [dbo].[Agenda_Estatus] ([NombreEstatus], [Descripcion], [Activo]) 
    -- VALUES
    -- (N'Pendiente', N'Estado inicial, esperando atención', 1),
    -- (N'En Proceso', N'Se está trabajando en la tarea', 1),
    -- (N'Completado', N'Tarea finalizada exitosamente', 1),
    -- (N'Cancelado', N'La tarea fue cancelada', 1),
    -- (N'En Espera', N'Esperando información o recursos adicionales', 1)
END
GO
