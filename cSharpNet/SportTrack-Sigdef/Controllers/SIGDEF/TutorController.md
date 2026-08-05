# TutorController.cs (SIGDEF)

## 1. Qué es

CRUD de **tutores** de atletas menores en el módulo federación.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| CRUD fino | `ITutorServices` |
| DTOs Create/Detail | Separación lectura/escritura |
| `[Authorize]` | Clase completa |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, DTOs TutorFederacion, Federaciones

## 4. Detalle

| Método | HTTP | Service |
|--------|------|---------|
| `GetTutores` | GET | `GetTutores` |
| `GetTutor` | GET `{id}` | `GetTutor` |
| `PostTutor` | POST | `PostTutor` |
| `PutTutor` | PUT `{id}` | `PutTutor` |
| `DeleteTutor` | DELETE `{id}` | `DeleteTutor` |

## 5. Notas de estudio

- Relación N-N con atletas vía `AtletaTutorController` / tabla `AtletasTutores` (clave compuesta).
- Tabla en esquema `federacion`.
