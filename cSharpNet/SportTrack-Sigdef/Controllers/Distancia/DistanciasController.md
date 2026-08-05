# DistanciasController.cs

## 1. Qué es

CRUD de **Distancias** de regata y listado de tipos de distancia (`regata-tipos`). Autenticado. Sin namespace de archivo.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Enum en dominio | `DistanciaRegata` (en entidad/EF como int) |
| Service CRUD | `IDistanciaService` |
| REST estándar | GET/POST/PUT/DELETE |
| Endpoint auxiliar | `regata-tipos` |

## 3. Namespace / usings

- Sin namespace
- Authorization, Mvc, Distancia + Dtos

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetDistancias` | GET | Todas |
| `GetDistancia` | GET `{id}` | Por id |
| `CreateDistancia` | POST | Alta |
| `UpdateDistancia` | PUT `{id}` | Update |
| `DeleteDistancia` | DELETE `{id}` | Baja |
| `GetDistanciasRegata` | GET `regata-tipos` | Metadatos/tipos de distancia |

## 5. Notas de estudio

1. En EF, `Metros` y `Descripcion` están con `Ignore` (calculados).
2. Seed incluye gaps sugeridos por distancia.
3. Buen caso para estudiar enum + conversión en DbContext.
