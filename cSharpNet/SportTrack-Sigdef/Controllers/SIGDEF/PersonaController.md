# PersonaController.cs (SIGDEF)

## 1. Qué es

CRUD SIGDEF de **personas** (base demográfica compartida). Busca también por documento. Namespace histórico `SIGDEF.API.Controllers`.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Thin controller | Delega a `IPersonaServices` |
| `[Authorize]` a nivel clase | Todo autenticado |
| Ruta por documento | Segmento literal + param |
| DTOs de Entidades | Create/Detail/Dto |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, DTOs Participante, Federaciones services

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetPersonas` | GET | Listado |
| `GetPersona` | GET `{id}` | Detalle |
| `GetPersonaByDocumento` | GET `documento/{documento}` | Lookup |
| `PostPersona` | POST | Alta |
| `PutPersona` | PUT `{id}` | Update |
| `DeletePersona` | DELETE `{id}` | Baja |

## 5. Notas de estudio

- “Persona” en SIGDEF ≈ datos civiles; “Atleta” agrega federación/club.
- Índice único filtrado de Documento está en DbContext.
- Namespace `SIGDEF.API` convive con `SportTrack_Sigdef` en el mismo host.
