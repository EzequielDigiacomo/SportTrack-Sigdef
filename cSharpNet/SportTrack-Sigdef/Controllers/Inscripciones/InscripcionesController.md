# InscripcionesController.cs

## 1. Qué es

API SportTrack de **inscripciones** a pruebas de evento: registro filtrado, CRUD, consulta por evento/club, toggle cabeza de serie. Scope distinto para rol Club vs Admin.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Scope por rol | Club → `clubScope`; Admin → `federacionScope` |
| Query params múltiples | `registro` |
| `[HttpPatch]` | Toggle seeding |
| Flag `allowWhenClosed` | Admin/SuperAdmin pueden borrar con evento cerrado |
| Comparación ordinal ignore case | Roles |

## 3. Namespace / usings

- `SportTrack_Sigdef.Controllers.Inscripciones`
- Authorization, Mvc, Inscripcion + Dtos, Claims, etc.

## 4. Detalle

| Método | Ruta | Notas |
|--------|------|-------|
| `GetRegistro` | GET `registro` | Vista de registro con scopes |
| `GetInscripciones` | GET | Todas (service) |
| `GetInscripcion` | GET `{id}` | |
| `CreateInscripcion` | POST | Comentario sobre validar club |
| `UpdateInscripcion` | PUT `{id}` | |
| `DeleteInscripcion` | DELETE `{id}` | `allowWhenClosed` según rol |
| `GetByEventoPrueba` | GET `evento-prueba/{id}` | |
| `GetByEventoAndClub` | GET `evento/{eventoId}/club/{clubId}` | |
| `ToggleSeeding` | PATCH `{id}/toggle-seeding` | Cabeza de serie |

## 5. Notas de estudio

1. No confundir con `SIGDEF/.../InscripcionController` (otra superficie).
2. `GetRegistro` muestra autorización por **reducción de filtros** (Club no puede pedir otro clubId).
3. Tripulación multi-persona está modelada en EF (`InscripcionTripulante`).
