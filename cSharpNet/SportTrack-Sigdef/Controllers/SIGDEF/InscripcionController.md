# InscripcionController.cs (SIGDEF)

## 1. Qué es

API SIGDEF de **inscripciones** (`api/Inscripcion`): listado, detalle, por evento, alta y baja. Paralela a `api/Inscripciones` de SportTrack.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Dual API | Misma idea, services distintos |
| Filtro por evento | GET `evento/{idEvento}` |
| Sin PUT en este controller | Solo POST/DELETE además de GETs |
| Thin controller | `IInscripcionServices` |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, DTOs Inscripcion, Federaciones

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetInscripciones` | GET | Listado |
| `GetInscripcion` | GET `{id}` | Detalle |
| `GetInscripcionesPorEvento` | GET `evento/{idEvento}` | Por evento |
| `PostInscripcion` | POST | Alta |
| `DeleteInscripcion` | DELETE `{id}` | Baja |

## 5. Notas de estudio

1. Compará verbs con `InscripcionesController` (SportTrack tiene PUT, toggle seeding, registro).
2. Tabla subyacente puede ser la misma familia `regatas.Inscripciones` según el service.
3. Naming singular/plural en rutas ayuda a no mezclar clientes.
