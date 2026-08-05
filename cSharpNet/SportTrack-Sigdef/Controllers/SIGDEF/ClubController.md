# ClubController.cs (SIGDEF)

## 1. Qué es

CRUD SIGDEF de clubes (`api/Club`) + búsqueda por término. Paralelo a `api/Clubes` de SportTrack.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Dual API | Mismo dominio, distinto cliente |
| Search en ruta | `search/{term}` |
| Thin controller | `IClubServices` (SIGDEF) vs `IClubService` (SportTrack) |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, DTOs Club, Federaciones

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetClubes` | GET | Listado |
| `GetClub` | GET `{id}` | Detalle |
| `SearchClubes` | GET `search/{term}` | Búsqueda |
| `PostClub` | POST | Alta |
| `PutClub` | PUT `{id}` | Update |
| `DeleteClub` | DELETE `{id}` | Baja |

## 5. Notas de estudio

- Ojo a nombres de interfaces: `IClubServices` (plural Services) vs `IClubService`.
- Tenant/filtrado suele vivir en el service SIGDEF.
- Tabla única `catalogos.Clubes` en EF.
