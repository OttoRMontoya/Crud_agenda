USE [crud_agenda]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agenda]') AND type in (N'U'))
BEGIN
    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON

    CREATE TABLE [dbo].[Agenda](
        [Id_Agenda_Cita] [int] IDENTITY(1,1) NOT NULL,
        [Id_Calendario] [int] NOT NULL,
        [Empresa] [int] NOT NULL,
        [Codigo_Doctor] [int] NOT NULL,
        [Codigo] [int] NOT NULL,
        [Clinica] [int] NOT NULL,
        [No_Ficha_Ingreso] [int] NOT NULL,
        [Contacto] [nvarchar](100) NOT NULL,
        [Inicio] [datetime] NOT NULL,
        [Fin] [datetime] NOT NULL,
        [Asunto] [nvarchar](50) NOT NULL,
        [Descripcion] [nvarchar](250) NOT NULL,
        [Lugar] [nvarchar](50) NOT NULL,
        [Etiqueta] [int] NOT NULL,
        [Estatus] [int] NOT NULL,
        [TodoelDia] [bit] NOT NULL,
        [Telefono1] [nvarchar](12) NOT NULL,
        [Telefono2] [nvarchar](12) NOT NULL,
        [Celular] [nvarchar](12) NOT NULL,
        [EMail] [nvarchar](100) NOT NULL,
        [Envia_Recordatorio] [bit] NOT NULL,
        [c_Usuario_Crea] [int] NOT NULL,
        [Fecha_Crea] [datetime] NOT NULL,
        [c_Usuario_Modifica] [int] NOT NULL,
        [Fecha_Modifica] [datetime] NOT NULL,
        CONSTRAINT [PK_Agenda] PRIMARY KEY CLUSTERED ([Id_Agenda_Cita] ASC)
    )

    ALTER TABLE [dbo].[Agenda] ADD CONSTRAINT [DF_Agenda_Fecha_Crea] DEFAULT (getdate()) FOR [Fecha_Crea]
    ALTER TABLE [dbo].[Agenda] ADD CONSTRAINT [DF_Agenda_Fecha_Modifica] DEFAULT (getdate()) FOR [Fecha_Modifica]

    PRINT 'Tabla Agenda creada correctamente.'
END
ELSE
BEGIN
    PRINT 'La tabla Agenda ya existe.'
END
GO
