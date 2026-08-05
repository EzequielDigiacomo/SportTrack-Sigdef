# SportTrack-Sigdef — API ASP.NET Core

Proyecto de entrada HTTP: **Controllers**, **Middleware**, **Program.cs**, seguridad.

## Estructura documentada

| Ruta | Contenido |
|------|-----------|
| `Program.md` | Arranque: DI, CORS, JWT, Swagger, pipeline |
| `Controllers/` | Endpoints REST |
| `Controllers/SIGDEF/` | Endpoints del módulo federaciones |
| `Middleware/` | Excepciones y security headers |
| `Security/` | Resolución de claves JWT |
| `CorsAllowedOrigins.md` | Lista blanca CORS |

## Flujo de un request

```
HTTP → Middleware → Authentication → Controller → Service → DbContext → PostgreSQL
                         ↓
                      JWT / Roles
```

## Orden de lectura sugerido

1. `Program.md`
2. `Controllers/HealthController.md`
3. `Middleware/ExceptionMiddleware.md`
4. `Controllers/Auth/AuthController.md`
5. `Controllers/Eventos/EventosController.md`
6. Un controller SIGDEF: `Controllers/SIGDEF/AtletaController.md`

Continúa en: [`../Fundamentos/02-aspnet-core-y-api.md`](../Fundamentos/02-aspnet-core-y-api.md)
