# ResultadosController.cs

> Fuente: `SportTrack-Sigdef/Controllers/ResultadosController.cs`

## Que es este archivo

Archivo C# del proyecto **SportTrack-Sigdef**. Define: `ResultadosController`, `ResultadoUpdateDto` (tipo principal: **class**).
Sirve como material de estudio del codigo real de SportTrack-Sigdef.

## Conceptos C# / .NET que aparecen

- **class**: tipo referencia; define datos (propiedades) y comportamiento (metodos).
- **enum**: conjunto de constantes con nombre (estados, tipos, roles).
- **async/await + Task**: programacion asincrona para I/O (DB, HTTP) sin bloquear hilos.
- **Atributos HTTP**: mapean metodos a rutas REST (GET/POST/PUT/DELETE).
- **Controller ASP.NET Core**: expone endpoints; hereda de ControllerBase.
- **Autorizacion**: [Authorize] exige token/rol; [AllowAnonymous] permite acceso libre.
- **Propiedades auto-implementadas**: el compilador crea el campo privado (get; set;).
- **Nullable**: string? / int? indican que el valor puede ser null.
- **SignalR Hub**: comunicacion en tiempo real (WebSockets).
- **AutoMapper**: mapea entidades a DTOs.

## Namespace

```
SportTrack_Sigdef.Controllers
```

## Usings (importaciones)

- `AutoMapper`
- `Microsoft.AspNetCore.Authorization`
- `Microsoft.AspNetCore.Mvc`
- `Microsoft.AspNetCore.RateLimiting`
- `Microsoft.AspNetCore.SignalR`
- `SportTrack_Sigdef.Controladores.Auth`
- `SportTrack_Sigdef.Controladores.Caching`
- `SportTrack_Sigdef.Controladores.Fase.Dtos`
- `SportTrack_Sigdef.Controladores.Hubs`
- `SportTrack_Sigdef.Controladores.Resultado`
- `System`
- `System.Collections.Generic`
- `System.Linq`
- `System.Threading.Tasks`

## Atributos detectados

- `[ApiController]` - Convenciones de API (validacion automatica del modelo).
- `[controller]` - Atributo de metadatos aplicado al tipo o miembro.
- `[HttpGet("Fase/{faseId}")]` - Endpoint HTTP GET (lectura).
- `[AllowAnonymous]` - Permite acceso sin autenticacion.
- `[EnableRateLimiting("live")]` - Atributo de metadatos aplicado al tipo o miembro.
- `[HttpPut("BatchUpdate")]` - Endpoint HTTP PUT (actualizacion).
- `[Authorize(Roles = AuthRolePolicies.CompetitionOperators)]` - Requiere autenticacion (y opcionalmente roles/policies).

## Propiedades

| Propiedad | Tipo | Notas |
|-----------|------|-------|
| `Id` | `int` | No-null (segun anotaciones) |
| `TiempoOficial` | `TimeSpan?` | Puede ser null |
| `Posicion` | `int?` | Puede ser null |
| `Estado` | `string?` | Puede ser null |
| `Carril` | `int?` | Puede ser null |
| `ParticipanteNombre` | `string?` | Puede ser null |
| `ClubSigla` | `string?` | Puede ser null |

## Metodos

| Metodo | Retorno | Parametros | Async |
|--------|---------|------------|-------|
| `GetResultadosPorFase` | `Task<ActionResult<IEnumerable<ResultadoFaseDto>>>` | `int faseId` | Si |
| `BatchUpdate` | `Task<ActionResult<IEnumerable<ResultadoFaseDto>>>` | `List<ResultadoUpdateDto> dto` | Si |

## Miembros del enum

_No se pudo extraer el cuerpo del enum._

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