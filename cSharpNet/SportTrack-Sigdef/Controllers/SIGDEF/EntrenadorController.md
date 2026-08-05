# EntrenadorController.cs (SIGDEF)

## 1. Qué es

CRUD de **entrenadores federados**, más un listado especial de selección nacional/selección.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Endpoint semántico | GET `seleccion` |
| Thin controller | `IEntrenadorServices` |
| Orden de rutas | `seleccion` vs `{id}` |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, DTOs EntrenadorFederacion, Federaciones

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetEntrenadores` | GET | Listado |
| `GetEntrenador` | GET `{id}` | Detalle |
| `GetEntrenadoresSeleccion` | GET `seleccion` | Subconjunto selección |
| `PostEntrenador` | POST | Alta |
| `PutEntrenador` | PUT `{id}` | Update |
| `DeleteEntrenador` | DELETE `{id}` | Baja |

## 5. Notas de estudio

- Migración `AddEntrenadorLicencia` sugiere campos de licencia en el modelo.
- FK a Federacion con `SetNull` en DbContext.
- Si `seleccion` se interpretara como `{id}`, fallaría el parse; por eso el segmento literal es importante.
