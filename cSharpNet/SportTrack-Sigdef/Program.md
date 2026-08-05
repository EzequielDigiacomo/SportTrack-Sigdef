# Program.cs

## 1. Qué es

Punto de entrada de la Web API (*top-level statements*). Configura:

- PostgreSQL / EF Core
- Cache, SignalR, CORS, JWT, rate limiting
- DI de todos los services/repositories
- Swagger
- Pipeline HTTP
- Migraciones automáticas al arranque
- Hub SignalR `/hubs/timing`

No hay clase `Startup` clásica: todo vive en este archivo (estilo .NET 6+).

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso en el archivo |
|----------|-------------------|
| `WebApplication.CreateBuilder` | Host + configuración |
| `IServiceCollection` / DI | `AddScoped`, `AddSingleton`, `AddHostedService` |
| `AddDbContext` + `UseNpgsql` | EF Core + PostgreSQL |
| JWT Bearer | `AddAuthentication` / `AddJwtBearer` |
| Authorization | `FallbackPolicy` = autenticado |
| Rate limiting | Políticas `auth` y `live` |
| CORS | Policy nombrada `CorsPolicy` |
| Middleware | `UseMiddleware<T>`, `UseAuthentication`, etc. |
| SignalR | `AddSignalR` / `MapHub` |
| Swagger | `AddSwaggerGen` + UI en Development |
| Forwarded headers | Proxies (Render) |
| Top-level local function | `ResolveConnectionString` |
| `IHostedService` | `EventoEstadoBackgroundService` |
| AutoMapper | `AddAutoMapper` |
| `AddApplicationPart` | Controllers del ensamblado Controladores |

## 3. Namespace / usings

Archivo con **top-level statements** (sin namespace propio para el cuerpo principal).

Usings destacados: JWT, RateLimiting, EF Core, OpenApi, ForwardedHeaders, namespaces de `Controladores.*`, `Middleware`, `SportTrack_Sigdef`.

## 4. Detalle por bloques

### 4.1 Connection string

```csharp
var connectionString = ResolveConnectionString(builder.Configuration);
```

Función local al final:

1. `GetConnectionString("DefaultConnection")`
2. Si vacío → env `DATABASE_URL`
3. Normaliza `postgres://` → `postgresql://` (Npgsql 8)

### 4.2 DbContext

```csharp
builder.Services.AddDbContext<SportTrackDbContext>(options =>
    options.UseNpgsql(connectionString, npgsql => {
        npgsql.EnableRetryOnFailure(maxRetryCount: 3);
        npgsql.CommandTimeout(30);
    }));
```

Reintentos ante fallos transitorios de red/BD.

### 4.3 Cache y SignalR

- `AddMemoryCache` (límite de tamaño) + `ILiveCacheService` singleton.
- `AddSignalR` para timing en vivo.

### 4.4 CORS

Une orígenes de config `AllowedOrigins` (CSV) con lista hardcodeada (localhost, Capacitor, dominios producción). Registra `CorsAllowedOrigins` como singleton y policy con `AllowCredentials`.

### 4.5 TokenKey y autorización

```csharp
var tokenKey = TokenKeyResolver.Resolve(...);
options.FallbackPolicy = RequireAuthenticatedUser();
```

Fuera de Development, sin `TokenKey` la app no arranca.

### 4.6 Rate limiter

- `auth`: 20/min por IP (login/register).
- `live`: 120/min por IP (endpoints públicos Live).
- Status 429 al rechazar.

### 4.7 JWT Bearer

Valida firma simétrica; no valida issuer/audience. `OnMessageReceived`:

1. Header `Authorization: Bearer ...`
2. Query `access_token`
3. Cookie `X-Access-Token`

### 4.8 Registro DI

Docenas de `AddScoped` para SportTrack (botes, eventos, auth…) y SIGDEF (atletas, traspasos, tenant…). Cloudinary vía `Configure<CloudinarySettings>`. MercadoPago extension. AutoMapper profile.

### 4.9 Controllers + JSON

```csharp
AddControllers()
  .AddApplicationPart(...PagoTransaccionController...)
  .AddJsonOptions(ReferenceHandler.IgnoreCycles);
```

Evita ciclos de navegación EF al serializar.

### 4.10 Swagger + JWT Authorize button

Define esquema Bearer para probar desde Swagger UI.

### 4.11 Pipeline post-Build

1. Forwarded headers (limpia KnownNetworks/Proxies).
2. Scope: `CanConnect` → `MigrateAsync` → sync estados de eventos.
3. `ExceptionMiddleware` → `SecurityHeadersMiddleware`.
4. HSTS/HTTPS si no Development.
5. Swagger solo Development.
6. CORS → RateLimiter → Authentication → Authorization.
7. `MapControllers` + `MapHub<TimingHub>("/hubs/timing").AllowAnonymous()`.
8. `app.Run()`.

## 5. Notas de estudio

1. Dibujá el pipeline en orden: un middleware mal ubicado explica muchos bugs de CORS/auth.
2. `FallbackPolicy` + `[AllowAnonymous]` es el modelo de seguridad “cerrado por defecto”.
3. Migrar al arranque es cómodo en PaaS; en equipos grandes a veces se prefiere CI/`dotnet ef database update`.
4. Compará lifetimes: DbContext Scoped vs cache Singleton.
5. Guía: [02-aspnet-core-y-api.md](../Fundamentos/02-aspnet-core-y-api.md).
