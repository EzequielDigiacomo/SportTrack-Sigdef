# EventosController.cs

## 1. Qué es

Controller rico de **eventos y pruebas de evento**. Mezcla listados autenticados (scope por club/federación), endpoints Live anónimos, y CRUD. Usa claims + `IAuthService.GetMeAsync` para resolver alcance.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Claims (`Name`, `Role`, custom) | Scope multi-tenant |
| Fallback try/catch a claims | Si falla GetMe |
| `[AllowAnonymous]` + rate `live` | Detalle, pruebas, fases |
| Query params | `clubId` opcional |
| Comentarios de Authorize deshabilitados | Pruebas assign sin Roles estrictos |
| Varios services | Evento + Fase + Auth |

## 3. Namespace / usings

- `SportTrack_Sigdef.Controllers.Eventos`
- Authorization, Mvc, RateLimiting, Evento + Dtos, Claims, Fase, etc.

## 4. Detalle de métodos

| Método | Ruta | Auth | Resumen |
|--------|------|------|---------|
| `GetEventos` | GET | Auth | Lista según rol/fed/club |
| `DebugEvents` | GET `debug` | Auth | Debug de claims + count |
| `GetFases` | GET `{id}/fases` | Anónimo live | Fases del evento |
| `GetProximosEventos` | GET `proximos` | Anónimo (refina si auth) | Próximos |
| `GetEvento` | GET `{id}` | Anónimo live | Detalle |
| `CreateEvento` | POST | Auth | Rellena Fed/Club del usuario |
| `UpdateEvento` / `DeleteEvento` | PUT/DELETE `{id}` | Auth | Scope club para Club/Admin |
| `GetPruebas` | GET `{id}/pruebas` | Anónimo live | Pruebas asignadas |
| `AssignPrueba` | POST `{id}/pruebas` | Auth | Asigna prueba |
| `UpdatePrueba` | PUT `pruebas/{id}` | Auth | Update EventoPrueba |
| `UnassignPrueba` | DELETE `pruebas/{id}` | Auth | Quita prueba |

## 5. Notas de estudio

1. Archivo largo: estudiá primero el bloque de resolución de `targetId`/`isFederacion` — es el corazón multi-tenant.
2. Live anónimo explica paneles públicos de competencia.
3. Comentarios `// [Authorize(Roles = "Admin")]` muestran deuda técnica consciente.
4. Relacioná con `FasesController` y sync de estado en `Program.cs`.
