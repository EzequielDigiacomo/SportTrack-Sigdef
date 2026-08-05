# FederacionesController.cs

## 1. Qué es

CRUD de **federaciones** vía `IFederacionServices`. Create/Delete solo SuperAdmin; Update SuperAdmin/Admin.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Thin controller | Delega 100% al service |
| Roles por verbo | POST/PUT/DELETE más estrictos |
| `ActionResult<T>` | Tipado de DTOs |
| Namespace anidado | `Controllers.Federaciones` |

## 3. Namespace / usings

- `SportTrack_Sigdef.Controllers.Federaciones`
- Authorization, Mvc, Federaciones services, DTOs Federacion

## 4. Detalle

| Método | HTTP | Roles extra | Service |
|--------|------|-------------|---------|
| `GetFederaciones` | GET | Auth | `GetFederaciones` |
| `GetFederacion` | GET `{id}` | Auth | `GetFederacion` |
| `CreateFederacion` | POST | SuperAdmin | `PostFederacion` |
| `UpdateFederacion` | PUT `{id}` | SuperAdmin,Admin | `PutFederacion` |
| `DeleteFederacion` | DELETE `{id}` | SuperAdmin | `DeleteFederacion` |

## 5. Notas de estudio

- Federación es el tenant raíz de SIGDEF.
- Alta “con admin” también existe en `SaaSController.CreateFederacion`.
- Buen ejemplo mínimo de CRUD REST.
