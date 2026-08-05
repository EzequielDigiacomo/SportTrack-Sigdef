# ParticipantesController.cs

## 1. Qué es

CRUD de **participantes/atletas de competencia** (SportTrack) con filtrado por claims de club, federación y rol.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Helper estático privado | `ParseClaimId` |
| Claims custom | `ClubId`, `FederacionId` |
| `ClaimTypes.Role` | Rol para el service |
| CRUD REST | GET/POST/PUT/DELETE |
| `CreatedAtAction` | Alta |

## 3. Namespace / usings

- `SportTrack_Sigdef.Controllers.Participantes`
- Authorization, Mvc, Participante + Dtos, Claims, Tasks

## 4. Detalle

| Método | Ruta | Notas |
|--------|------|-------|
| `GetParticipantes` | GET | Pasa clubId, role, federacionId al service |
| `GetParticipante` | GET `{id}` | |
| `GetByClub` | GET `club/{clubId}` | Filtro por club |
| `CreateParticipante` | POST | |
| `UpdateParticipante` | PUT `{id}` | Usa CreateDto también en update |
| `DeleteParticipante` | DELETE `{id}` | NoContent |

### `ParseClaimId`

Devuelve `int?` solo si parsea y `> 0`.

## 5. Notas de estudio

1. En BD, `Documento` y `Email` tienen índices únicos filtrados (DbContext).
2. SIGDEF tiene flujo paralelo (`AtletaController`, `PersonaController`).
3. El filtrado real de seguridad debe validarse en el service (no solo en listados).
