# DelegadoClubController.cs (SIGDEF)

## 1. Qué es

CRUD de **delegados de club** ante la federación, con listado por federación.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Filtro por padre | `federacion/{idFederacion}` |
| CRUD estándar | GET/POST/PUT/DELETE |
| DTOs tipados | Create/Detail |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, DTOs DelegadoFederacionClub, Federaciones

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetDelegadosClub` | GET | Todos |
| `GetDelegadoClub` | GET `{id}` | Uno |
| `GetDelegadosPorFederacion` | GET `federacion/{idFederacion}` | Por fed |
| `PostDelegadoClub` | POST | Alta |
| `PutDelegadoClub` | PUT `{id}` | Update |
| `DeleteDelegadoClub` | DELETE `{id}` | Baja |

## 5. Notas de estudio

- En EF: FK a Federacion con `OnDelete(SetNull)`.
- Rol organizacional distinto de “usuario Club” en JWT, aunque puedan relacionarse en negocio.
