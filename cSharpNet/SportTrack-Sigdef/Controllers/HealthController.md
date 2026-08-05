# HealthController.cs

> Fuente: `SportTrack-Sigdef/Controllers/HealthController.cs`

## Que es este archivo

Archivo C# del proyecto **SportTrack-Sigdef**. Define: `HealthController` (tipo principal: **class**).
Sirve como material de estudio del codigo real de SportTrack-Sigdef.

## Conceptos C# / .NET que aparecen

- **class**: tipo referencia; define datos (propiedades) y comportamiento (metodos).
- **async/await + Task**: programacion asincrona para I/O (DB, HTTP) sin bloquear hilos.
- **Atributos HTTP**: mapean metodos a rutas REST (GET/POST/PUT/DELETE).
- **Controller ASP.NET Core**: expone endpoints; hereda de ControllerBase.
- **Autorizacion**: [Authorize] exige token/rol; [AllowAnonymous] permite acceso libre.
- **EF Core**: DbContext / DbSet<T> conectan el modelo C# con tablas SQL.
- **Nullable**: string? / int? indican que el valor puede ser null.

## Namespace

```
SportTrack_Sigdef.Controllers
```

## Usings (importaciones)

- `Microsoft.AspNetCore.Authorization`
- `Microsoft.AspNetCore.Mvc`
- `Microsoft.EntityFrameworkCore`
- `SportTrack_Sigdef.AccesoDatos`

## Atributos detectados

- `[ApiController]` - Convenciones de API (validacion automatica del modelo).
- `[controller]` - Atributo de metadatos aplicado al tipo o miembro.
- `[AllowAnonymous]` - Permite acceso sin autenticacion.
- `[HttpGet]` - Endpoint HTTP GET (lectura).
- `[HttpGet("db")]` - Endpoint HTTP GET (lectura).

## Propiedades

_Sin propiedades auto-implementadas detectadas (o son de otro estilo)._

## Metodos

| Metodo | Retorno | Parametros | Async |
|--------|---------|------------|-------|
| `Get` | `IActionResult` | `-` | No |
| `Database` | `Task<IActionResult>` | `-` | Si |

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