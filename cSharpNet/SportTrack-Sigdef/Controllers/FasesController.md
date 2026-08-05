# FasesController.cs

> Fuente: `SportTrack-Sigdef/Controllers/FasesController.cs`

## Que es este archivo

Archivo C# del proyecto **SportTrack-Sigdef**. Define: `FasesController` (tipo principal: **class**).
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
- `Microsoft.AspNetCore.RateLimiting`
- `SportTrack_Sigdef.Controladores.Auth`
- `SportTrack_Sigdef.Controladores.Fase`
- `SportTrack_Sigdef.Controladores.Fase.Dtos`
- `System`
- `System.Collections.Generic`
- `System.Threading.Tasks`

## Atributos detectados

- `[ApiController]` - Convenciones de API (validacion automatica del modelo).
- `[controller]` - Atributo de metadatos aplicado al tipo o miembro.
- `[HttpGet("EventoPrueba/{eventoPruebaId}")]` - Endpoint HTTP GET (lectura).
- `[AllowAnonymous]` - Permite acceso sin autenticacion.
- `[EnableRateLimiting("live")]` - Atributo de metadatos aplicado al tipo o miembro.
- `[HttpGet("all-by-evento/{eventoId}")]` - Endpoint HTTP GET (lectura).
- `[HttpGet("ProgresionAudit/{eventoPruebaId}")]` - Endpoint HTTP GET (lectura).
- `[Authorize(Roles = AuthRolePolicies.CompetitionOperators)]` - Requiere autenticacion (y opcionalmente roles/policies).
- `[HttpPost("BatchUpdate")]` - Endpoint HTTP POST (creacion).
- `[FromBody]` - Parametro desde el body JSON.
- `[HttpPost("Generar/{eventoPruebaId}")]` - Endpoint HTTP POST (creacion).
- `[HttpPost("GenerarManual/{eventoPruebaId}")]` - Endpoint HTTP POST (creacion).
- `[HttpPost("Promover/{eventoPruebaId}")]` - Endpoint HTTP POST (creacion).
- `[HttpDelete("{id}")]` - Endpoint HTTP DELETE (baja).
- `[HttpPost("{id}/Iniciar")]` - Endpoint HTTP POST (creacion).
- `[FromQuery]` - Parametro desde la query string.
- `[HttpPost("{id}/Finalizar")]` - Endpoint HTTP POST (creacion).
- `[HttpPost("{id}/Reiniciar")]` - Endpoint HTTP POST (creacion).
- `[HttpPost("{id}/EnviarARevision")]` - Endpoint HTTP POST (creacion).

## Propiedades

_Sin propiedades auto-implementadas detectadas (o son de otro estilo)._

## Metodos

| Metodo | Retorno | Parametros | Async |
|--------|---------|------------|-------|
| `GetFasesPorEventoPrueba` | `Task<ActionResult<IEnumerable<FaseDto>>>` | `int eventoPruebaId` | Si |
| `GetFasesPorEvento` | `Task<ActionResult<IEnumerable<FaseDto>>>` | `int eventoId` | Si |
| `GetProgresionAudit` | `Task<ActionResult<IEnumerable<ProgressionAuditDto>>>` | `int eventoPruebaId` | Si |
| `BatchUpdate` | `Task<ActionResult>` | `[FromBody] List<FaseBatchUpdateDto> dto` | Si |
| `GenerarFases` | `Task<ActionResult<IEnumerable<FaseDto>>>` | `int eventoPruebaId` | Si |
| `GenerarFasesManual` | `Task<ActionResult<IEnumerable<FaseDto>>>` | `int eventoPruebaId, [FromBody] List<ManualPlacementDto> placements` | Si |
| `Promover` | `Task<ActionResult<IEnumerable<FaseDto>>>` | `int eventoPruebaId` | Si |
| `Delete` | `Task<ActionResult>` | `int id` | Si |
| `Iniciar` | `Task<ActionResult<FaseDto>>` | `int id, [FromQuery] DateTime? startTime = null` | Si |
| `Finalizar` | `Task<ActionResult<FaseDto>>` | `int id` | Si |
| `Reiniciar` | `Task<ActionResult<FaseDto>>` | `int id` | Si |
| `EnviarARevision` | `Task<ActionResult<FaseDto>>` | `int id` | Si |

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