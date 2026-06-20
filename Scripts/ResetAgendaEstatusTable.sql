-- Script para reiniciar/limpiar la tabla Agenda_Estatus
-- Úsalo si necesitas empezar de cero

USE [crud_agenda]
GO

-- Opción 1: Solo eliminar todos los registros
DELETE FROM [dbo].[Agenda_Estatus]
GO

-- Opción 2: Reiniciar el contador de identidad a 0
DBCC CHECKIDENT ('[dbo].[Agenda_Estatus]', RESEED, 0)
GO

-- Opción 3: Insertar datos de ejemplo nuevamente
INSERT INTO [dbo].[Agenda_Estatus] ([NombreEstatus], [Descripcion], [Activo]) 
VALUES
(N'Pendiente', N'Estado inicial, esperando atención', 1),
(N'En Proceso', N'Se está trabajando en la tarea', 1),
(N'Completado', N'Tarea finalizada exitosamente', 1),
(N'Cancelado', N'La tarea fue cancelada', 1),
(N'En Espera', N'Esperando información o recursos adicionales', 1)
GO

PRINT 'Tabla limpiada e datos de ejemplo insertados correctamente.'
GO

-- Verificar los datos
SELECT * FROM [dbo].[Agenda_Estatus]
GO
