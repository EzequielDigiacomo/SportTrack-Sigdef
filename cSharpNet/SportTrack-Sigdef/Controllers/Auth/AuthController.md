# AuthController.cs

> Fuente: `SportTrack-Sigdef/Controllers/Auth/AuthController.cs`

## Que es este archivo

Archivo C# del proyecto **SportTrack-Sigdef**. Define: `AuthController` (tipo principal: **class**).
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
SportTrack_Sigdef.Controllers.Auth
```

## Usings (importaciones)

- `Microsoft.AspNetCore.Authorization`
- `Microsoft.AspNetCore.Http`
- `Microsoft.AspNetCore.Mvc`
- `Microsoft.AspNetCore.RateLimiting`
- `SportTrack_Sigdef.Controladores.Auth`
- `SportTrack_Sigdef.Controladores.Auth.Dtos`
- `SportTrack_Sigdef.Controladores.Mensajes`
- `System.Linq`
- `System.Security.Claims`
- `System.Threading.Tasks`

## Atributos detectados

- `[ApiController]` - Convenciones de API (validacion automatica del modelo).
- `[controller]` - Atributo de metadatos aplicado al tipo o miembro.
- `[HttpPost("login")]` - Endpoint HTTP POST (creacion).
- `[AllowAnonymous]` - Permite acceso sin autenticacion.
- `[EnableRateLimiting("auth")]` - Atributo de metadatos aplicado al tipo o miembro.
- `[HttpPost("logout")]` - Endpoint HTTP POST (creacion).
- `[HttpPost("solicitar-reset-password")]` - Endpoint HTTP POST (creacion).
- `[FromBody]` - Parametro desde el body JSON.
- `[HttpPost("register")]` - Endpoint HTTP POST (creacion).
- `[Authorize(Roles = AuthRolePolicies.Admins)]` - Requiere autenticacion (y opcionalmente roles/policies).
- `[HttpGet("usuarios")]` - Endpoint HTTP GET (lectura).
- `[Authorize]` - Requiere autenticacion (y opcionalmente roles/policies).
- `[HttpPut("usuarios/{id}/password")]` - Endpoint HTTP PUT (actualizacion).
- `[HttpPut("usuarios/{id}/perfil")]` - Endpoint HTTP PUT (actualizacion).
- `[HttpPatch("usuarios/{id}/toggle-activo")]` - Endpoint HTTP PATCH (actualizacion parcial).
- `[Authorize(Roles = "Admin,SuperAdmin,soporte_tecnico")]` - Requiere autenticacion (y opcionalmente roles/policies).
- `[HttpGet("me")]` - Endpoint HTTP GET (lectura).

## Propiedades

_Sin propiedades auto-implementadas detectadas (o son de otro estilo)._

## Metodos

| Metodo | Retorno | Parametros | Async |
|--------|---------|------------|-------|
| `Login` | `Task<ActionResult<AuthResponseDto>>` | `LoginDto loginDto` | Si |
| `Logout` | `IActionResult` | `-` | No |
| `SolicitarResetPassword` | `Task<ActionResult>` | `[FromBody] SolicitarResetPasswordDto dto` | Si |
| `Register` | `Task<ActionResult>` | `RegisterDto registerDto` | Si |
| `GetUsuarios` | `Task<ActionResult>` | `-` | Si |
| `UpdatePassword` | `Task<ActionResult>` | `int id, [FromBody] string newPassword` | Si |
| `UpdatePerfil` | `Task<ActionResult>` | `int id, [FromBody] UpdatePerfilDto dto` | Si |
| `ToggleActivo` | `Task<ActionResult>` | `int id` | Si |
| `GetMe` | `Task<ActionResult<UsuarioDto>>` | `-` | Si |
| `CanManageUserAsync` | `Task<bool>` | `int targetUserId` | Si |

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