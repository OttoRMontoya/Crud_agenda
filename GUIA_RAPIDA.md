# Guía de Inicio Rápido - CRUD Agenda_Estatus

## ?? Pasos para Empezar

### 1?? Crear la Tabla en SQL Server
```sql
-- Abre SQL Server Management Studio
-- Copia y ejecuta el contenido de: Scripts/CreateAgendaEstatusTable.sql
```

**Resultado esperado:**
```
Tabla Agenda_Estatus creada exitosamente con datos de ejemplo.
```

### 2?? Verificar la Conexión
- El archivo `Web.config` ya tiene configurada la conexión:
  ```xml
  <add name="AgendaEntities" 
       connectionString="Data Source=WEB-SERCOMTEC;Initial Catalog=crud_agenda;User ID=sa;Password=estrelladedios" 
       providerName="System.Data.SqlClient" />
  ```

### 3?? Ejecutar la Aplicación
```
Visual Studio ? F5 (Debug) o Ctrl + F5 (Sin Debug)
```

### 4?? Acceder al CRUD
Opción A - Por el Sidebar:
- Haz clic en **"Estados"** en el menú lateral izquierdo

Opción B - Por URL directa:
```
http://localhost:puerto/AgendaEstatus
```

---

## ?? Operaciones Básicas

### ? Crear un Estado
1. Haz clic en el botón **"Nuevo Estado"**
2. Rellena los campos:
   - **Nombre del Estado** * (requerido)
   - **Descripción** (opcional)
   - **Activo** (checkbox, por defecto marcado)
3. Haz clic en **"Guardar"**

### ?? Ver Listado
- Ve a `/AgendaEstatus`
- Se muestra una tabla con:
  - ID del estado
  - Nombre
  - Descripción
  - Estado (Activo/Inactivo)
  - Fecha de creación
  - Acciones (Ver, Editar, Eliminar)

### ??? Editar un Estado
1. En la tabla, haz clic en **"Editar"** en la fila del estado
2. Modifica los campos necesarios
3. Haz clic en **"Actualizar"**
4. Se registra automáticamente la **"Fecha de Modificación"**

### ??? Ver Detalles
1. En la tabla, haz clic en **"Ver"**
2. Se muestra toda la información del estado
3. Desde aquí puedes:
   - Editar
   - Eliminar
   - Volver al listado

### ??? Eliminar un Estado
1. En la tabla, haz clic en **"Eliminar"**
2. Confirma que deseas eliminar (?? No se puede revertir)
3. Haz clic en **"Sí, Eliminar"**

---

## ?? Problemas Comunes

### ? Error: "Cannot insert explicit value for identity column"
**Causa:** Intentaste insertar valores explícitos en `IdEstatus`

**Solución:**
- La aplicación está correcta
- Si ocurre en SQL, ejecuta: `Scripts/ResetAgendaEstatusTable.sql`

### ? Error: "Cannot connect to server"
**Causa:** SQL Server no está accesible

**Solución:**
- Verifica que el servidor `WEB-SERCOMTEC` esté activo
- Verifica credenciales en `Web.config`

### ? Las vistas se ven sin estilos
**Causa:** Bootstrap 5 no se cargó (CDN)

**Solución:**
- Verifica conexión a internet
- Los estilos se cargan desde: `https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/`

---

## ?? Estructura de Archivos Importantes

```
Crud_agenda/
??? Controllers/
?   ??? AgendaEstatusController.cs    ? Lógica principal
??? Models/
?   ??? AgendaEstatus.cs             ? Modelo de datos
?   ??? AgendaDbContext.cs           ? Conexión a BD
??? Views/
?   ??? AgendaEstatus/
?   ?   ??? Index.cshtml             ? Listado
?   ?   ??? Create.cshtml            ? Crear
?   ?   ??? Edit.cshtml              ? Editar
?   ?   ??? Details.cshtml           ? Detalles
?   ?   ??? Delete.cshtml            ? Eliminar
?   ??? Shared/
?       ??? _Layout.cshtml           ? Layout con sidebar
??? Scripts/
?   ??? CreateAgendaEstatusTable.sql ? Crear tabla
?   ??? ResetAgendaEstatusTable.sql  ? Limpiar
??? Web.config                        ? Configuración

```

---

## ?? Datos de la Tabla

| Campo | Tipo | Descripción |
|-------|------|-------------|
| **IdEstatus** | INT (PK, Auto) | ID único (generado automáticamente) |
| **NombreEstatus** | NVARCHAR(100) | Nombre del estado (REQUERIDO) |
| **Descripcion** | NVARCHAR(500) | Descripción del estado |
| **Activo** | BIT | 1=Activo, 0=Inactivo |
| **FechaCreacion** | DATETIME | Se asigna automáticamente |
| **FechaModificacion** | DATETIME | Se actualiza al editar |

---

## ?? Datos de Ejemplo Incluidos

La tabla se crea con estos estados:

1. ?? **Pendiente** - Estado inicial, esperando atención
2. ?? **En Proceso** - Se está trabajando en la tarea
3. ? **Completado** - Tarea finalizada exitosamente
4. ? **Cancelado** - La tarea fue cancelada
5. ?? **En Espera** - Esperando información o recursos adicionales

---

## ?? Rutas Disponibles

```
GET     /AgendaEstatus              # Listado de estados
GET     /AgendaEstatus/Create       # Formulario crear
POST    /AgendaEstatus/Create       # Guardar nuevo
GET     /AgendaEstatus/Edit/{id}    # Formulario editar
POST    /AgendaEstatus/Edit/{id}    # Guardar cambios
GET     /AgendaEstatus/Details/{id} # Ver detalles
GET     /AgendaEstatus/Delete/{id}  # Confirmación eliminar
POST    /AgendaEstatus/Delete/{id}  # Confirmar eliminación
```

---

## ?? Consejos

? **Siempre haz backup** antes de ejecutar scripts de limpieza
? **Compila antes de ejecutar** (Ctrl + Shift + B)
? **Verifica la conexión** antes de reportar errores
? **Los datos de ejemplo** se pueden editar o eliminar

---

**¿Necesitas ayuda?** Consulta `README.md` para documentación completa.
