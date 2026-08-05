# AtletaController.cs (SIGDEF)

## 1. Qué es

CRUD y consultas de **atletas federados**, incluyendo alta completa (`full`) y listado **paginado**. Orden de rutas: `club/{id}` y `paged` antes/además de `{id:int}` para evitar ambigüedad.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Route constraints `{id:int}` | Evita capturar “paged”/strings |
| Paginación | `PaginationParamsDto` → `PagedResponseDto` |
| Dos POSTs | Simple vs `full` |
| Using `SIGDEF.DTOs` | Posible DTO legacy |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, DTOs AtletaFederacion/Base, Federaciones

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetAtletas` | GET | Todos (detalle) |
| `GetAtletasByClub` | GET `club/{clubId}` | Por club |
| `GetAtleta` | GET `{id}` | Uno |
| `GetAtletasPaginados` | GET `paged` | Paginado + filtros query |
| `PostAtleta` | POST | Alta estándar |
| `PostAtletaFull` | POST `full` | Alta enriquecida |
| `PutAtleta` / `DeleteAtleta` | PUT/DELETE `{id}` | Update/baja |

## 5. Notas de estudio

1. Orden/constraints de rutas es lección clásica de ASP.NET routing.
2. Compará con `ParticipantesController` (SportTrack).
3. Límites SaaS (`MaxAtletas`) se aplican en servicios, no aquí.
