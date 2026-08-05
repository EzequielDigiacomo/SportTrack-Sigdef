# ClubesController.cs

## 1. Qué es

API de **clubes para SportTrack** (`api/Clubes`). SIGDEF usa otro controller (`api/Club`) sobre el mismo modelo. Filtra por federación del tenant cuando aplica.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Multi-API / mismo dominio | SportTrack vs SIGDEF |
| `ITenantProvider` | `GetFederacionId()` |
| Filtrado en memoria post-query | `Where(c => c.FederacionId == fedId)` |
| Completar DTO en create | Asigna `FederacionId` del tenant |
| XML doc comment | Explica dualidad de APIs |

## 3. Namespace / usings

- `SportTrack_Sigdef.Controllers.Clubes`
- Authorization, Mvc, Club + Dtos, Federaciones (tenant), Linq, Tasks

## 4. Detalle

| Método | Ruta | Notas |
|--------|------|-------|
| `GetClubes` | GET | Filtra por federación si hay tenant |
| `GetClub` | GET `{id}` | Sin filtro extra en controller |
| `CreateClub` | POST | Inyecta FederacionId del tenant si falta |
| `UpdateClub` | PUT `{id}` | |
| `DeleteClub` | DELETE `{id}` | NoContent |

## 5. Notas de estudio

1. Compará con `Controllers/SIGDEF/ClubController.md`.
2. Filtrar en memoria vs en SQL: aquí el service trae todo y luego filtra; en producción a veces se prefiere filtrar en repositorio.
3. Tenant provider es el patrón multi-tenant de este backend.
