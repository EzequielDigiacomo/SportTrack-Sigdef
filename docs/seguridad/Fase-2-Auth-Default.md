# Fase 2 — Autenticado por defecto + HTTPS

Fecha: 2026-07-14  
Build: OK.

---

## Qué cambió

| Área | Cambio |
|------|--------|
| **FallbackPolicy** | `RequireAuthenticatedUser` en `Program.cs`. Sin atributo ⇒ exige JWT. |
| **CRUD SIGDEF / catálogos / pagos / legacy** | `[Authorize]` a nivel clase (Atleta, Persona, Usuario, Club, Tutor, Botes, Categorias, Distancias, PagoTransaccion, EventoPrueba, etc.). |
| **Webhook MP** | Clase Authorize + `[AllowAnonymous]` solo en `POST api/Notificacion/webhook`. |
| **Hub** | `MapHub(...).AllowAnonymous()` para connect Live; Join/`GetServerTime` AllowAnonymous; escrituras Authorize (Fase 1). |
| **HTTPS** | En no-Development: `UseHsts` + `UseHttpsRedirection`; ForwardedHeaders limpia KnownNetworks/Proxies (Render). |

### Sigue público (AllowAnonymous explícito)

- `POST /api/Auth/login`, `logout`
- `GET /api/Health`, `/db`
- `POST /api/Support/frontend-error`
- Live: `GET Eventos/proximos|{id}|fases|pruebas`, `GET Fases/...`, `GET Resultados/Fase/{id}`
- `POST /api/Notificacion/webhook`
- SignalR `/hubs/timing` connect + Join* + GetServerTime

### Ahora exige login (antes GETs abiertos)

- `GET /api/Clubes`, Participantes, Inscripciones, Federaciones, Atleta, etc.  
  Fronts logueados (SportTrack / FrontSigdef) ya mandan Bearer → sin cambio de UX.

---

## Render

**No hace falta una variable nueva** si Fase 1 ya tiene `TokenKey` + `AllowedOrigins`.

HSTS/HTTPS usan `X-Forwarded-Proto` de Render (ForwardedHeaders ya configurado).

Tras deploy: smoke bi-front + Live.

---

## Smoke post-Fase 2

| # | Prueba | Esperado |
|---|--------|----------|
| 1 | Live sin login | Carga + hub timing OK |
| 2 | `GET /api/Atleta` sin token | **401** |
| 3 | FrontSigdef logueado — listar atletas/clubes | OK |
| 4 | SportTrack admin — eventos/fases | OK |
| 5 | Largador / llegada / control | OK |
| 6 | Login público | OK |

---

## Próximo: Fase 3

Rate limit login, firma webhook MP, uploads MIME, headers seguridad, cookie-first gradual.
