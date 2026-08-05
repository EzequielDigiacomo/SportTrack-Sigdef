# 02 — ASP.NET Core y la API

Basado en `SportTrack-Sigdef` (`Program.cs`, Controllers, Middleware).

## Qué es ASP.NET Core

Framework web de .NET para construir APIs HTTP. En este repo, la API recibe JSON, aplica reglas de negocio y responde JSON.

## `Program.cs`: el corazón del arranque

Orden mental:

1. **Crear el builder** → `WebApplication.CreateBuilder(args)`
2. **Registrar servicios** → `builder.Services.Add...` (DI)
3. **Construir la app** → `builder.Build()`
4. **Configurar el pipeline** → middleware (`Use...`, `Map...`)
5. **Correr** → `app.Run()`

### Inyección de dependencias (DI)

```csharp
builder.Services.AddScoped<IAuthService, AuthService>();
```

| Lifetime | Significado |
|----------|-------------|
| `AddSingleton` | Una instancia para toda la app |
| `AddScoped` | Una instancia por request HTTP |
| `AddTransient` | Nueva instancia cada vez que se pide |

Luego el Controller pide la dependencia en el constructor:

```csharp
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;
}
```

## Controllers

```csharp
[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<...>> GetAll() { ... }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EventoCreateDto dto) { ... }
}
```

| Concepto | Rol |
|----------|-----|
| `[ApiController]` | Validación automática del modelo + convenciones API |
| `[Route(...)]` | Base URL del controller |
| `[HttpGet/Post/...]` | Verbo HTTP |
| `ActionResult` / `IActionResult` | Respuesta HTTP tipada (`Ok`, `NotFound`, `BadRequest`) |
| `[FromBody]` | Lee JSON del body |
| `[Authorize]` | Exige JWT / política de roles |

## Middleware

El pipeline es una cadena:

```
Request → ExceptionMiddleware → SecurityHeaders → Authentication → Controllers → Response
```

Cada middleware puede:

- Ejecutar lógica antes
- Llamar al siguiente (`await next()`)
- Ejecutar lógica después
- Cortar la cadena (devolver error)

## CORS, JWT, Swagger (en este proyecto)

- **CORS**: lista blanca de orígenes del frontend
- **JWT Bearer**: autenticación por token
- **Swagger/OpenAPI**: documentación interactiva de endpoints
- **SignalR**: tiempo real (hubs)
- **Rate limiting**: límite de requests

## Qué estudiar ahora

1. `SportTrack-Sigdef/Program.md`
2. Un controller simple: `Controllers/HealthController.md`
3. Un controller de dominio: `Controllers/Eventos/EventosController.md`
4. `Middleware/ExceptionMiddleware.md`
