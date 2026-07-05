# Mapeo de Clase: AtletaFederacionTutor

> **Descripción:** Relación entre un atleta menor de edad y su tutor legal.

## Entidad Dominio: `AtletaFederacionTutor`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `IdAtleta` | `int` | Sí (FK) |
| `IdTutor` | `int` | Sí (FK) |
| `AtletaFederacion` | `AtletaFederacion` | Sí (Navegación) |
| `TutorFederacion` | `TutorFederacion` | Sí (Navegación) |
| `Parentesco` | `Parentesco` | - |

## DTO: `AtletaTutorDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `-` | *No coincide* | - |
| `IdTutor` | `int` | `IdTutor` | **Coincide Exacto** | - |
| `Parentesco` | `Parentesco` | `Parentesco` | **Coincide Exacto** | - |
| `NombreAtleta` | `string?` | `-` | *No coincide* | - |
| `NombreTutor` | `string?` | `-` | *No coincide* | - |
| `NombreClub` | `string?` | `-` | *No coincide* | - |