-- Agregar columna Color a Agenda_Estatus (ejecutar en la base de datos existente)
USE [db_abe7ed_agenda]
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[Agenda_Estatus]') AND name = 'Color'
)
BEGIN
    ALTER TABLE [dbo].[Agenda_Estatus]
    ADD [Color] NVARCHAR(7) NOT NULL CONSTRAINT DF_Agenda_Estatus_Color DEFAULT '#667eea';

    UPDATE [dbo].[Agenda_Estatus] SET [Color] = '#f39c12' WHERE [NombreEstatus] = N'Pendiente';
    UPDATE [dbo].[Agenda_Estatus] SET [Color] = '#3788d8' WHERE [NombreEstatus] = N'En Proceso';
    UPDATE [dbo].[Agenda_Estatus] SET [Color] = '#2ecc71' WHERE [NombreEstatus] = N'Completado';
    UPDATE [dbo].[Agenda_Estatus] SET [Color] = '#e74c3c' WHERE [NombreEstatus] = N'Cancelado';
    UPDATE [dbo].[Agenda_Estatus] SET [Color] = '#9b59b6' WHERE [NombreEstatus] = N'En Espera';

    PRINT 'Columna Color agregada a Agenda_Estatus.'
END
ELSE
BEGIN
    PRINT 'La columna Color ya existe en Agenda_Estatus.'
END
GO
