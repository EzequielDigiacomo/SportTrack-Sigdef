# RolController.cs (SIGDEF)

## 1. Qué es

CRUD de **roles federación** (catálogo de roles SIGDEF en tabla `federacion.Roles`). Distinto del claim JWT `RolFederacion` del usuario, aunque conceptualmente relacionados.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| CRUD de catálogo | Roles como entidad |
| Thin controller | `IRolServices` |
| DTOs Create/Detail | |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, DTOs RolFederacion, Federaciones

## 4. Detalle

| Método | HTTP | Service |
|--------|------|---------|
| `GetRoles` | GET | `GetRoles` |
| `GetRol` | GET `{id}` | `GetRol` |
| `PostRol` | POST | `PostRol` |
| `PutRol` | PUT `{id}` | `PutRol` |
| `DeleteRol` | DELETE `{id}` | `DeleteRol` |

## 5. Notas de estudio

- No confundir con `[Authorize(Roles = "Admin")]` (roles de ASP.NET Identity/JWT).
- Este recurso es datos de dominio SIGDEF; la autorización JWT usa strings en el token.
- Buen punto para discutir “autorización de app” vs “roles de negocio en BD”.
