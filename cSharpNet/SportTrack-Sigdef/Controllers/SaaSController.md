# SaaSController.cs

> Fuente: `SportTrack-Sigdef/Controllers/SaaSController.cs`

## Que es este archivo

Archivo C# del proyecto **SportTrack-Sigdef**. Define: `SaaSController` (tipo principal: **class**).
Sirve como material de estudio del codigo real de SportTrack-Sigdef.

## Conceptos C# / .NET que aparecen

- **class**: tipo referencia; define datos (propiedades) y comportamiento (metodos).
- **async/await + Task**: programacion asincrona para I/O (DB, HTTP) sin bloquear hilos.
- **Atributos HTTP**: mapean metodos a rutas REST (GET/POST/PUT/DELETE).
- **Controller ASP.NET Core**: expone endpoints; hereda de ControllerBase.
- **Autorizacion**: [Authorize] exige token/rol; [AllowAnonymous] permite acceso libre.
- **Nullable**: string? / int? indican que el valor puede ser null.

## Namespace

```
SportTrack_Sigdef.Controllers
```

## Usings (importaciones)

- `Microsoft.AspNetCore.Authorization`
- `Microsoft.AspNetCore.Mvc`
- `SportTrack_Sigdef.Controladores.SaaS`
- `System.Threading.Tasks`
- `System.Linq`

## Atributos detectados

- `[controller]` - Atributo de metadatos aplicado al tipo o miembro.
- `[ApiController]` - Convenciones de API (validacion automatica del modelo).
- `[Authorize]` - Requiere autenticacion (y opcionalmente roles/policies).
- `[HttpGet("debug-me")]` - Endpoint HTTP GET (lectura).
- `[Authorize(Roles = "SuperAdmin,soporte_tecnico")]` - Requiere autenticacion (y opcionalmente roles/policies).
- `[HttpGet("planes")]` - Endpoint HTTP GET (lectura).
- `[HttpPost("asignar-plan")]` - Endpoint HTTP POST (creacion).
- `[Authorize(Roles = "SuperAdmin,Admin")]` - Requiere autenticacion (y opcionalmente roles/policies).
- `[HttpGet("clubes-status")]` - Endpoint HTTP GET (lectura).
- `[Authorize(Roles = "SuperAdmin,Admin,soporte_tecnico")]` - Requiere autenticacion (y opcionalmente roles/policies).
- `[HttpPatch("clubes/{id}/toggle-activo")]` - Endpoint HTTP PATCH (actualizacion parcial).
- `[HttpPost("create-federacion")]` - Endpoint HTTP POST (creacion).
- `[Authorize(Roles = "SuperAdmin")]` - Requiere autenticacion (y opcionalmente roles/policies).
- `[FromBody]` - Parametro desde el body JSON.
- `[HttpGet("global-metrics")]` - Endpoint HTTP GET (lectura).

## Propiedades

_Sin propiedades auto-implementadas detectadas (o son de otro estilo)._

## Metodos

| Metodo | Retorno | Parametros | Async |
|--------|---------|------------|-------|
| `DebugMe` | `ActionResult` | `-` | No |
| `GetPlanes` | `Task<IActionResult>` | `-` | Si |
| `AsignarPlan` | `Task<IActionResult>` | `int clubId, int planId` | Si |
| `GetClubesStatus` | `Task<IActionResult>` | `-` | Si |
| `ToggleActivo` | `Task<IActionResult>` | `int id` | Si |
| `CreateFederacion` | `Task<IActionResult>` | `[FromBody] SportTrack_Sigdef.Controladores.SaaS.Dtos.SaaSCreateFederacionDto dto` | Si |
| `GetGlobalMetrics` | `Task<IActionResult>` | `-` | Si |

## Como estudiarlo

1. Abre el `.cs` original en el IDE.
2. Identifica el tipo (class) y su responsabilidad.
3. Lee cada propiedad: tipo, nullabilidad y significado de negocio.
4. Si hay metodos, sigue el flujo (validaciones -> persistencia -> retorno DTO).
5. Busca en este mismo directorio los tipos relacionados (DTOs, interfaces, entidades).

## Notas de estudio

- En C#, casi todo vive dentro de un **tipo** (class / interface / enum / record).
- Los corchetes `[Atributo]` agregan metadatos usados por el runtime, EF Core, ASP.NET, Swagger, etc.
- `Task` / `async` aparecen cuando hay trabajo de I/O (base de datos, HTTP, archivos).
- Las interfaces (`I...`) desacoplan el contrato de la implementacion (util para testing y DI).