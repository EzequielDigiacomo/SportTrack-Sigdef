# AtletaTutorController.cs (SIGDEF)

## 1. Qué es

Gestiona la relación **atleta ↔ tutor** (tabla puente con PK compuesta). Altas y bajas por par de IDs; consultas por atleta o por tutor.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Ruta con dos IDs | `{participanteId}/{idTutor}` |
| Relación muchos a muchos | Expuesta como recurso propio |
| Sin PUT | Solo create/delete de vínculo |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, DTOs AtletaFederacionTutor, Federaciones

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetAtletaTutores` | GET | Todos los vínculos |
| `GetAtletaTutor` | GET `{participanteId}/{idTutor}` | Uno |
| `GetTutoresPorAtleta` | GET `atleta/{participanteId}` | Tutores del atleta |
| `GetAtletasPorTutor` | GET `tutor/{idTutor}` | Atletas del tutor |
| `PostAtletaTutor` | POST | Crear vínculo |
| `DeleteAtletaTutor` | DELETE `{participanteId}/{idTutor}` | Quitar vínculo |

## 5. Notas de estudio

1. En Fluent API: `HasKey(at => new { at.IdAtleta, at.IdTutor })`.
2. Diseño REST de asociaciones: a veces se anidan bajo `/Atleta/{id}/tutores`; aquí el recurso es de primer nivel.
3. Útil para entender PKs compuestas end-to-end (EF → API).
