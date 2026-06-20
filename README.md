# CRUD Agenda_Estatus - ASP.NET MVC

## Descripción
Sistema CRUD completo para gestionar los estados de la tabla `Agenda_Estatus` en ASP.NET MVC con Bootstrap 5 y un sidebar moderno.

## Características
? Crear, Leer, Actualizar y Eliminar estados
? Interfaz moderna con Bootstrap 5
? Sidebar responsive
? Validación de datos
? Interfaz amigable y responsive
? Entity Framework para acceso a datos
? Auditoría con fechas de creación y modificación

## Requisitos Previos
- .NET Framework 4.6
- SQL Server (WEB-SERCOMTEC)
- Base de datos: `crud_agenda`

## Instalación

### 1. Crear la tabla en la base de datos
Ejecutar el script SQL ubicado en: `Scripts/CreateAgendaEstatusTable.sql`

```sql
-- En SQL Server Management Studio
USE [crud_agenda]
-- Ejecutar el contenido del archivo CreateAgendaEstatusTable.sql
```

### 2. Verificar la conexión a la base de datos
El archivo `Web.config` ya contiene la cadena de conexión:
```xml
<add name="AgendaEntities" connectionString="Data Source=WEB-SERCOMTEC;Initial Catalog=crud_agenda;User ID=sa;Password=estrelladedios" providerName="System.Data.SqlClient" />
```

### 3. Compilar el proyecto
```
Build ? Build Solution (Ctrl + Shift + B)
```

## Estructura del Proyecto

```
Crud_agenda/
??? Controllers/
?   ??? AgendaEstatusController.cs       # Controlador principal
??? Models/
?   ??? AgendaEstatus.cs                 # Modelo de datos
?   ??? AgendaDbContext.cs               # Contexto de base de datos
??? Views/
?   ??? AgendaEstatus/
?       ??? Index.cshtml                 # Listado de estados
?       ??? Create.cshtml                # Crear nuevo estado
?       ??? Edit.cshtml                  # Editar estado
?       ??? Details.cshtml               # Ver detalles
?       ??? Delete.cshtml                # Confirmar eliminación
??? Scripts/
    ??? CreateAgendaEstatusTable.sql     # Script de creación de tabla
```

## Uso

### Acceder al CRUD
1. Inicia la aplicación
2. En el sidebar, haz clic en "Estados" o ve a: `http://localhost:puerto/AgendaEstatus`

### Operaciones Disponibles

#### Ver Listado (Index)
- Muestra todos los estados registrados
- Filtro por estado activo/inactivo
- Acciones rápidas para cada estado

#### Crear Nuevo Estado
- Haz clic en "Nuevo Estado"
- Completa el formulario
- Los campos requeridos están marcados con *
- El estado se marca como activo por defecto

#### Editar Estado
- Haz clic en "Editar" en la fila del estado
- Modifica los campos necesarios
- Se registra la fecha de última modificación

#### Ver Detalles
- Haz clic en "Ver" para ver la información completa
- Desde aquí puedes editar o eliminar

#### Eliminar Estado
- Haz clic en "Eliminar"
- Confirma la acción en la pantalla de confirmación
- ?? Esta acción no puede revertirse

## Campos de la Tabla

| Campo | Tipo | Descripción |
|-------|------|-------------|
| IdEstatus | INT (PK) | Identificador único del estado |
| NombreEstatus | NVARCHAR(100) | Nombre del estado (requerido) |
| Descripcion | NVARCHAR(500) | Descripción del estado (opcional) |
| Activo | BIT | Estado activo/inactivo (por defecto: 1) |
| FechaCreacion | DATETIME | Fecha de creación (automática) |
| FechaModificacion | DATETIME | Fecha de última actualización |

## Datos de Ejemplo Incluidos

La tabla se crea con estos estados predeterminados:
- ?? Pendiente
- ?? En Proceso
- ? Completado
- ? Cancelado
- ?? En Espera

## Rutas disponibles

```
GET     /AgendaEstatus              # Listado de estados
GET     /AgendaEstatus/Create       # Formulario de creación
POST    /AgendaEstatus/Create       # Guardar nuevo estado
GET     /AgendaEstatus/Edit/{id}    # Formulario de edición
POST    /AgendaEstatus/Edit/{id}    # Guardar cambios
GET     /AgendaEstatus/Details/{id} # Ver detalles
GET     /AgendaEstatus/Delete/{id}  # Confirmación de eliminación
POST    /AgendaEstatus/Delete/{id}  # Confirmar eliminación
```

## Notas Técnicas

### Entity Framework
- Versión: 6.0
- Mapping: Convention-based (Convenciones sobre configuración)

### Validaciones
- Campo "NombreEstatus" es requerido
- Máximo 100 caracteres en nombre
- Máximo 500 caracteres en descripción

### Seguridad
- Protección CSRF en todos los formularios
- Validación de estado 404 si no existe
- Validación de modelo en servidor

## Solución de Problemas

### Error: "Cannot insert explicit value for identity column in table 'Agenda_Estatus' when IDENTITY_INSERT is set to OFF"
Este error ocurre si intentas insertar valores explícitos en la columna `IdEstatus` (que es auto-generada).

**Solución:**
- ? El código de la aplicación es correcto y no insertar valores explícitos
- ? Si recibiste este error en SQL Server, asegúrate de:
  - NO especificar `IdEstatus` en los INSERT (ya se genera automáticamente)
  - Usar el script: `Scripts/ResetAgendaEstatusTable.sql` para limpiar y reiniciar

**Ejemplo correcto:**
```sql
-- ? CORRECTO - No especificar IdEstatus
INSERT INTO [dbo].[Agenda_Estatus] ([NombreEstatus], [Descripcion], [Activo]) 
VALUES (N'Pendiente', N'Descripción', 1)

-- ? INCORRECTO - No hagas esto
INSERT INTO [dbo].[Agenda_Estatus] ([IdEstatus], [NombreEstatus]) 
VALUES (1, 'Pendiente')
```

### Error de conexión a base de datos
- Verifica que el servidor SQL esté corriendo en `WEB-SERCOMTEC`
- Confirma que la base de datos `crud_agenda` existe
- Verifica las credenciales (usuario: sa)

### Las vistas no se muestran correctamente
- Asegúrate de tener Bootstrap 5 conectado (se carga desde CDN)
- Verifica la conexión a internet para cargar estilos de CDN

### Tabla no encontrada
- Ejecuta el script `Scripts/CreateAgendaEstatusTable.sql`
- Verifica que estés usando la conexión correcta

## Scripts SQL Disponibles

### 1. **CreateAgendaEstatusTable.sql** - CREAR LA TABLA
- Crea la tabla `Agenda_Estatus` si no existe
- Crea índices para optimizar búsquedas
- Inserta datos de ejemplo iniciales
- **Úsalo cuando sea la primera vez**

```bash
# En SQL Server Management Studio:
-- Abre el archivo y ejecuta (F5)
```

### 2. **ResetAgendaEstatusTable.sql** - LIMPIAR Y REINICIAR
- Elimina todos los registros
- Reinicia el contador de identidad
- Reinsertan datos de ejemplo
- **Úsalo si necesitas empezar de cero sin perder la estructura**

```bash
# En SQL Server Management Studio:
-- Abre el archivo y ejecuta (F5)
```

## Contribuciones
Este CRUD fue generado siguiendo las mejores prácticas de ASP.NET MVC.

## Licencia
Este proyecto es de uso interno.

---

**Desarrollado con:** ASP.NET MVC | Bootstrap 5 | Entity Framework 6 | SQL Server
