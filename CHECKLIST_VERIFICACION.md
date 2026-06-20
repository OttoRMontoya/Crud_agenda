# ? CHECKLIST DE VERIFICACIÓN

## ?? Pre-Ejecución

- [ ] Visual Studio abierto
- [ ] Proyecto cargado: `Crud_agenda.csproj`
- [ ] SQL Server ejecutándose
- [ ] Base de datos `crud_agenda` existe
- [ ] Conexión de red hacia `WEB-SERCOMTEC` disponible

## ??? Base de Datos

- [ ] Ejecutar script: `Scripts/CreateAgendaEstatusTable.sql`
- [ ] Verificar tabla creada en SQL Server
- [ ] Verificar 5 registros de ejemplo insertados
- [ ] Verificar índices creados (IX_Agenda_Estatus_Activo, IX_Agenda_Estatus_NombreEstatus)

```sql
-- Verificación en SQL Server:
SELECT COUNT(*) FROM [crud_agenda].[dbo].[Agenda_Estatus]
-- Resultado esperado: 5
```

## ??? Compilación

- [ ] Abrir `Crud_agenda.csproj` en Visual Studio
- [ ] Compilar: `Ctrl + Shift + B`
- [ ] Resultado esperado:
  ```
  ? Compilación correcta.
  0 Advertencia(s)
  0 Errores
  ```

## ?? Archivos Verificados

### Modelos
- [ ] `Models/AgendaEstatus.cs` - Tiene `[DatabaseGenerated]`
- [ ] `Models/AgendaDbContext.cs` - Tiene configuración en `OnModelCreating`

### Controlador
- [ ] `Controllers/AgendaEstatusController.cs` - Bind sin `IdEstatus` en Create
- [ ] Tiene método Index
- [ ] Tiene método Create (GET y POST)
- [ ] Tiene método Edit (GET y POST)
- [ ] Tiene método Delete (GET y POST)
- [ ] Tiene método Details

### Vistas
- [ ] `Views/AgendaEstatus/Index.cshtml` - Listado con tabla
- [ ] `Views/AgendaEstatus/Create.cshtml` - Formulario crear
- [ ] `Views/AgendaEstatus/Edit.cshtml` - Formulario editar
- [ ] `Views/AgendaEstatus/Details.cshtml` - Detalles
- [ ] `Views/AgendaEstatus/Delete.cshtml` - Confirmación eliminar
- [ ] `Views/Shared/_Layout.cshtml` - Sidebar Bootstrap 5 actualizado

### Configuración
- [ ] `Web.config` - Conexión `AgendaEntities` agregada
- [ ] `connectionString` apunta a `WEB-SERCOMTEC`
- [ ] Usuario: `sa`
- [ ] Base de datos: `crud_agenda`

### Documentación
- [ ] `README.md` - Documentación completa
- [ ] `GUIA_RAPIDA.md` - Guía de inicio rápido
- [ ] `CAMBIOS_REALIZADOS.md` - Resumen de cambios

### Scripts SQL
- [ ] `Scripts/CreateAgendaEstatusTable.sql` - Crear tabla
- [ ] `Scripts/ResetAgendaEstatusTable.sql` - Limpiar tabla

## ?? Ejecución

- [ ] Iniciar aplicación: `F5` o `Ctrl + F5`
- [ ] Esperar a que cargue completamente
- [ ] Verificar que NO hay errores en la consola

## ?? Navegación

- [ ] Acceso a home: `http://localhost:puerto/`
- [ ] Sidebar visible en la izquierda
- [ ] Menú "Estados" visible en sidebar
- [ ] Clic en "Estados" redirige a `/AgendaEstatus`

## ?? Funcionalidades CRUD

### Listar (Index)
- [ ] Acceder a `/AgendaEstatus`
- [ ] Se muestra tabla con datos
- [ ] Aparecen los 5 datos de ejemplo
- [ ] Botones: Ver, Editar, Eliminar visibles
- [ ] Botón "Nuevo Estado" visible

### Crear (Create)
- [ ] Clic en "Nuevo Estado" funciona
- [ ] Formulario muestra campos:
  - [ ] Nombre del Estado (requerido)
  - [ ] Descripción (opcional)
  - [ ] Checkbox Activo (marcado por defecto)
- [ ] Rellenar campos y guardar
- [ ] Redirecciona a Index
- [ ] Nuevo estado aparece en la tabla
- [ ] Mensaje de éxito mostrado: "Estado creado exitosamente"

### Editar (Edit)
- [ ] Clic en "Editar" en una fila funciona
- [ ] Formulario pre-rellena datos correctamente
- [ ] Mostrar ID, Fecha de Creación y Última Modificación
- [ ] Modificar un campo
- [ ] Guardar cambios
- [ ] Redirecciona a Index
- [ ] Cambios se ven en la tabla
- [ ] Mensaje de éxito mostrado: "Estado actualizado exitosamente"
- [ ] Fecha de modificación se actualiza

### Ver Detalles (Details)
- [ ] Clic en "Ver" muestra detalles completos
- [ ] Botones para Editar, Eliminar, Volver visible
- [ ] Todos los campos mostrados correctamente

### Eliminar (Delete)
- [ ] Clic en "Eliminar" muestra confirmación
- [ ] Advertencia de "No se puede revertir" visible
- [ ] Botones "Sí, Eliminar" y "Cancelar" visibles
- [ ] Confirmar eliminación
- [ ] Redirecciona a Index
- [ ] Estado no aparece más en la tabla
- [ ] Mensaje de éxito mostrado: "Estado eliminado exitosamente"

## ?? Interfaz Visual

- [ ] Sidebar morado/gradiente visible
- [ ] Bootstrap 5 estilos aplicados
- [ ] Responsive en desktop
- [ ] Responsive en tablet
- [ ] Responsive en móvil
- [ ] Botones con iconos de Bootstrap Icons
- [ ] Tablas con estilos modernos
- [ ] Alertas mostradas correctamente
- [ ] Mensajes de error/éxito visibles

## ?? Validaciones

- [ ] Campo "Nombre del Estado" requerido (intenta guardar vacío)
- [ ] Máximo 100 caracteres en nombre
- [ ] Máximo 500 caracteres en descripción
- [ ] Estados 404 si ID no existe
- [ ] Protección CSRF en formularios

## ?? Manejo de Errores

- [ ] Si ocurre error de conexión BD, mostrar mensaje
- [ ] Redirecciones a 404 si no existe registro
- [ ] Validaciones muestran en rojo

## ? Posibles Problemas y Soluciones

### Problema: "Cannot insert explicit value for identity column"
- [ ] Ejecutar `Scripts/ResetAgendaEstatusTable.sql`
- [ ] Borrar datos y reintentar

### Problema: Tabla no encontrada
- [ ] Ejecutar `Scripts/CreateAgendaEstatusTable.sql`
- [ ] Verificar base de datos correcta

### Problema: No se conecta a BD
- [ ] Verificar SQL Server ejecutándose
- [ ] Verificar credenciales en `Web.config`
- [ ] Verificar acceso a servidor `WEB-SERCOMTEC`

### Problema: Estilos no cargan
- [ ] Verificar conexión a internet (CDN Bootstrap)
- [ ] Abrir consola del navegador (F12) para ver errores

## ?? Resumen

Total de verificaciones: 80+

Cuando todas estén ?:
- **PROYECTO LISTO PARA PRODUCCIÓN**

---

**Documento de verificación final**
**Fecha de creación:** 2024
**Estado:** Proyecto completamente configurado
