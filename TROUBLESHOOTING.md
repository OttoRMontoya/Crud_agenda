# ?? TROUBLESHOOTING - Solución de Problemas

## Error: "Cannot insert explicit value for identity column"

### Síntomas
- Mensaje de error al crear un estado
- La tabla tiene un error de identidad
- Error dice algo sobre IDENTITY_INSERT

### Causas Posibles
1. ? Intentaste insertar `IdEstatus` manualmente en SQL
2. ? El script SQL original incluía `IdEstatus` en INSERT
3. ? Hay transacción pendiente de IDENTITY_INSERT

### Soluciones

#### Opción 1: Ejecutar Script de Reset (RECOMENDADO)
```sql
-- Ejecuta el archivo: Scripts/ResetAgendaEstatusTable.sql

-- Limpia todo
DELETE FROM [dbo].[Agenda_Estatus]
GO

-- Reinicia el contador
DBCC CHECKIDENT ('[dbo].[Agenda_Estatus]', RESEED, 0)
GO

-- Inserta datos de ejemplo nuevamente
INSERT INTO [dbo].[Agenda_Estatus] ([NombreEstatus], [Descripcion], [Activo]) 
VALUES
(N'Pendiente', N'Estado inicial, esperando atención', 1),
(N'En Proceso', N'Se está trabajando en la tarea', 1),
(N'Completado', N'Tarea finalizada exitosamente', 1),
(N'Cancelado', N'La tarea fue cancelada', 1),
(N'En Espera', N'Esperando información o recursos adicionales', 1)
GO
```

#### Opción 2: Eliminar y Recrear Tabla
```sql
-- En caso de que Opción 1 no funcione
DROP TABLE [dbo].[Agenda_Estatus]
GO

-- Ejecuta: Scripts/CreateAgendaEstatusTable.sql
```

#### Opción 3: Habilitar IDENTITY_INSERT (Temporal)
```sql
-- SOLO si necesitas insertar datos específicos
SET IDENTITY_INSERT [dbo].[Agenda_Estatus] ON
GO

-- Tu INSERT aquí
INSERT INTO [dbo].[Agenda_Estatus] ([IdEstatus], [NombreEstatus], [Descripcion], [Activo]) 
VALUES (1, N'Pendiente', N'Descripción', 1)
GO

-- IMPORTANTE: Deshabilitarlo después
SET IDENTITY_INSERT [dbo].[Agenda_Estatus] OFF
GO
```

---

## Error: "Cannot connect to server WEB-SERCOMTEC"

### Síntomas
- Página carga pero sin datos
- Error en Application Insights o Event Viewer
- Timeout al cargar `/AgendaEstatus`

### Causas Posibles
1. ? SQL Server no está corriendo
2. ? El nombre del servidor es incorrecto
3. ? Las credenciales son incorrectas
4. ? No hay acceso de red al servidor
5. ? Firewall bloqueando puerto 1433

### Soluciones

#### Paso 1: Verificar SQL Server
```powershell
# Abre PowerShell como Administrador
Get-Service -Name MSSQL* | Select-Object Status, Name
# Resultado esperado: Status debe ser "Running"

# Si está parado:
Start-Service -Name MSSQLSERVER
```

#### Paso 2: Verificar Conectividad
```powershell
# Prueba conexión al servidor
Test-NetConnection -ComputerName WEB-SERCOMTEC -Port 1433
# Resultado esperado: TcpTestSucceeded = True
```

#### Paso 3: Verificar Connection String
```xml
<!-- En Web.config, verifica que sea así: -->
<add name="AgendaEntities" 
     connectionString="Data Source=WEB-SERCOMTEC;Initial Catalog=crud_agenda;User ID=sa;Password=estrelladedios" 
     providerName="System.Data.SqlClient" />
```

#### Paso 4: Usar SQL Server Management Studio
```
1. Abre SSMS
2. Server name: WEB-SERCOMTEC
3. Authentication: SQL Server Authentication
4. Login: sa
5. Password: estrelladedios
6. Intenta conectar
```

#### Paso 5: Si falla, prueba alternativas
```xml
<!-- Intenta con dirección IP si el nombre no funciona -->
<add name="AgendaEntities" 
     connectionString="Data Source=192.168.x.x;Initial Catalog=crud_agenda;User ID=sa;Password=estrelladedios" 
     providerName="System.Data.SqlClient" />

<!-- O intenta con puerto explícito -->
<add name="AgendaEntities" 
     connectionString="Data Source=WEB-SERCOMTEC,1433;Initial Catalog=crud_agenda;User ID=sa;Password=estrelladedios" 
     providerName="System.Data.SqlClient" />
```

---

## Error: "The name 'media' does not exist in the current context"

### Síntomas
- Error de compilación CS0103
- Ocurre en `_Layout.cshtml`
- Mencionan la palabra "media"

### Causa
- `@media` en CSS fue interpretado como código Razor

### Solución (YA APLICADA ?)
```html
<!-- ? Incorrecto -->
<style>
    @media (max-width: 768px) {
        /* estilos */
    }
</style>

<!-- ? Correcto -->
<style>
    @@media (max-width: 768px) {
        /* estilos */
    }
</style>
```

---

## Error: "Unexpected 'if' keyword after '@' character"

### Síntomas
- Error en vistas Razor (.cshtml)
- Ocurre en Create.cshtml o Edit.cshtml
- Menciona "if" o "foreach"

### Causa
- `@if` o `@foreach` dentro de bloques `@using`

### Solución (YA APLICADA ?)
```razor
<!-- ? Incorrecto -->
@using (Html.BeginForm(...))
{
    @if (ModelState.IsValid)  <!-- @ INNECESARIO aquí -->
    {
        @foreach (var item in list) <!-- @ INNECESARIO aquí -->
        {
        }
    }
}

<!-- ? Correcto -->
@using (Html.BeginForm(...))
{
    if (ModelState.IsValid)  <!-- SIN @ -->
    {
        foreach (var item in list) <!-- SIN @ -->
        {
        }
    }
}
```

---

## Las vistas se ven sin estilos (sin Bootstrap)

### Síntomas
- Página carga pero sin colores
- Tablas sin bordes
- Botones sin estilos
- Solo HTML plano

### Causas Posibles
1. ? Bootstrap 5 no se descargó del CDN
2. ? No hay conexión a internet
3. ? El CDN está bloqueado por firewall/proxy

### Soluciones

#### Opción 1: Verificar CDN Online
```
https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css
```

#### Opción 2: Abrir Consola del Navegador (F12)
```
1. Presiona F12
2. Ve a la pestaña "Network"
3. Recarga la página (F5)
4. Busca recursos de bootstrap
5. Si están en rojo = no se descargaron
```

#### Opción 3: Usar Bootstrap Local
```html
<!-- Descarga Bootstrap 5 manualmente -->
<!-- 1. Ve a: https://getbootstrap.com/docs/5.3/getting-started/download/ -->
<!-- 2. Descarga los archivos -->
<!-- 3. Coloca en: ~/Content/bootstrap.min.css -->
<!-- 4. Cambia en _Layout.cshtml: -->

<!-- Antes (CDN): -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">

<!-- Después (Local): -->
<link href="@Url.Content("~/Content/bootstrap.min.css")" rel="stylesheet">
```

#### Opción 4: Verificar Proxy/Firewall
```
1. Abre Configuración de Red
2. Verifica proxy
3. Si hay proxy, configúralo en Visual Studio:
   - Tools ? Options ? Internet & Network ? Web Browser ? Proxy
```

---

## Error 404 en rutas AgendaEstatus

### Síntomas
- `/AgendaEstatus` devuelve 404
- El menú funciona pero la página no existe
- "Controller or action not found"

### Causas Posibles
1. ? Controlador no compiló correctamente
2. ? Falta la carpeta `Views/AgendaEstatus`
3. ? Las vistas no existen

### Soluciones

#### Paso 1: Verificar Compilación
```
Visual Studio ? Build ? Clean Solution
Visual Studio ? Build ? Rebuild Solution
```

#### Paso 2: Verificar Estructura
```
? Archivo debe existir: Controllers/AgendaEstatusController.cs
? Carpeta debe existir: Views/AgendaEstatus/
? Archivos deben existir:
   - Views/AgendaEstatus/Index.cshtml
   - Views/AgendaEstatus/Create.cshtml
   - Views/AgendaEstatus/Edit.cshtml
   - Views/AgendaEstatus/Details.cshtml
   - Views/AgendaEstatus/Delete.cshtml
```

#### Paso 3: Verificar Nombre del Controlador
```csharp
// En Controllers/AgendaEstatusController.cs, debe ser:
public class AgendaEstatusController : Controller
{
    // ...
}

// ? NO:
public class AgendaEstatusesController : Controller { }
// ? NO:
public class EstatusController : Controller { }
```

---

## Error: "Object reference not set to an instance of an object" en vistas

### Síntomas
- "NullReferenceException" en la consola
- La vista carga pero falta algo
- Error al renderizar un campo

### Causas Posibles
1. ? El modelo es null
2. ? Una propiedad del modelo es null
3. ? Se intenta acceder a una propiedad que no existe

### Soluciones

#### Opción 1: Verificar el Controlador
```csharp
// En el controlador, asegúrate de devolver un modelo válido
public ActionResult Index()
{
    var agendaEstatus = db.AgendaEstatus.OrderByDescending(x => x.FechaCreacion).ToList();

    // ? Esto nunca es null, pero puede estar vacío
    return View(agendaEstatus ?? new List<AgendaEstatus>());
}
```

#### Opción 2: Verificar la Vista
```razor
<!-- ? Siempre verifica antes de usar -->
@if (Model != null && Model.Count > 0)
{
    <p>@Model.First().NombreEstatus</p>
}
else
{
    <p>Sin datos</p>
}

<!-- ? NO hagas esto sin verificar -->
<p>@Model.First().NombreEstatus</p>  <!-- Puede lanzar error -->
```

---

## Tabla aparece vacía

### Síntomas
- Se muestra el mensaje "No hay estados registrados"
- La tabla existe pero sin datos
- Los datos de ejemplo no aparecen

### Causas Posibles
1. ? No ejecutaste el script de creación
2. ? El script no insertó datos
3. ? Los datos fueron eliminados
4. ? Estás conectado a base de datos equivocada

### Soluciones

#### Paso 1: Verificar que la tabla existe
```sql
USE [crud_agenda]
GO
SELECT * FROM [dbo].[Agenda_Estatus]
GO
-- Resultado esperado: Mínimo 5 filas
```

#### Paso 2: Si la tabla está vacía
```sql
-- Ejecuta el script para insertar datos:
-- Scripts/ResetAgendaEstatusTable.sql
```

#### Paso 3: Verificar base de datos correcta
```xml
<!-- En Web.config, verifica: -->
<add name="AgendaEntities" 
     connectionString="Data Source=WEB-SERCOMTEC;Initial Catalog=crud_agenda;..." />
     <!-- ^^^^^^^^ Debe ser crud_agenda, NO otra base de datos -->
```

---

## Validaciones no funcionan

### Síntomas
- Puedes guardar campos vacíos
- Límite de caracteres no se respeta
- Validaciones no se muestran

### Causas Posibles
1. ? JavaScript de validación no cargó
2. ? Validación del lado del cliente deshabilitada
3. ? ModelState no se verifica en controlador

### Soluciones

#### Opción 1: Verificar Web.config
```xml
<!-- En Web.config, verifica: -->
<add key="ClientValidationEnabled" value="true" />
<add key="UnobtrusiveJavaScriptEnabled" value="true" />
```

#### Opción 2: Verificar Vista
```html
<!-- Asegúrate de que la vista incluya: -->
@Scripts.Render("~/bundles/jquery")
@Scripts.Render("~/bundles/jqueryval")  <!-- Validación -->
@RenderSection("scripts", required: false)
```

#### Opción 3: Validación del servidor (SIEMPRE hacer esto)
```csharp
// En el controlador, SIEMPRE verifica:
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create([Bind(...)] AgendaEstatus agendaEstatus)
{
    if (ModelState.IsValid)  // ? IMPORTANTE
    {
        // Guardar
    }
    return View(agendaEstatus);
}
```

---

## La aplicación es lenta

### Síntomas
- Las acciones tardan mucho
- Crear/Editar tarda más de 3 segundos
- Listar tarda aunque hay pocos datos

### Causas Posibles
1. ? Query N+1 en Entity Framework
2. ? Conexión lenta a BD
3. ? Servidor SQL sobrecargado

### Soluciones

#### Opción 1: Optimizar la consulta en Index
```csharp
// En AgendaEstatusController.cs
public ActionResult Index()
{
    // ? CORRECTO - Trae todo de una vez
    var agendaEstatus = db.AgendaEstatus
        .AsNoTracking()  // ? No necesita tracking si solo lee
        .OrderByDescending(x => x.FechaCreacion)
        .ToList();

    return View(agendaEstatus);
}
```

#### Opción 2: Verificar índices en BD
```sql
-- Verifica que existan los índices:
SELECT * FROM sys.indexes 
WHERE object_id = OBJECT_ID('Agenda_Estatus')
GO
```

#### Opción 3: Monitorear en SQL Server Management Studio
```sql
-- Ejecuta durante las consultas para ver qué toma tiempo
-- Abre Activity Monitor en SSMS
-- Tools ? Activity Monitor
```

---

## Mensajes de error no se muestran

### Síntomas
- TempData["Message"] no aparece
- No ves el verde de "Estado creado exitosamente"
- Las alertas no se muestran

### Causas Posibles
1. ? TempData no se está usando correctamente
2. ? Vista no tiene el código de alertas
3. ? Redirección pierde TempData

### Soluciones

#### Opción 1: Verificar Controlador
```csharp
// En AgendaEstatusController.cs, USA TempData:
public ActionResult Create(AgendaEstatus agendaEstatus)
{
    if (ModelState.IsValid)
    {
        db.AgendaEstatus.Add(agendaEstatus);
        db.SaveChanges();

        TempData["Message"] = "Estado creado exitosamente.";  // ?
        return RedirectToAction("Index");  // ? Redirección es IMPORTANTE
    }
    return View(agendaEstatus);
}
```

#### Opción 2: Verificar Layout
```html
<!-- En Views/Shared/_Layout.cshtml, debe estar: -->
@if (TempData["Message"] != null)
{
    <div class="alert alert-success alert-dismissible fade show" role="alert">
        <i class="bi bi-check-circle"></i>
        @TempData["Message"]
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
}
```

---

## ? Verificación Final Rápida

```
1. ¿Compiló sin errores?
   msbuild Crud_agenda.csproj /p:Configuration=Debug

2. ¿Tabla existe?
   SELECT COUNT(*) FROM [crud_agenda].[dbo].[Agenda_Estatus]

3. ¿Tiene datos?
   SELECT * FROM [crud_agenda].[dbo].[Agenda_Estatus]

4. ¿Se puede conectar?
   Test-NetConnection -ComputerName WEB-SERCOMTEC -Port 1433

5. ¿La aplicación inicia?
   Visual Studio ? F5

6. ¿Cargan las páginas?
   http://localhost:puerto/AgendaEstatus
```

---

**Si aún tienes problemas:**

1. ? Revisa los logs de Event Viewer en Windows
2. ? Abre la consola del navegador (F12) y busca errores
3. ? Revisa el Output window de Visual Studio
4. ? Consulta Application Insights si está configurado

**¡Documentación completa de troubleshooting lista!**
