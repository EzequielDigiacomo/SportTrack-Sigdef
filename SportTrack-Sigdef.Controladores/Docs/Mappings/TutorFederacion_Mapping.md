# Mapeo de Clase: TutorFederacion

> **Descripción:** Tutores legales de los atletas menores de edad.

## Entidad Dominio: `TutorFederacion`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `ParticipanteId` | `int` | - |
| `Participante` | `Participante` | Sí (Navegación) |
| `TipoTutor` | `string` | - |
| `AtletasTutores` | `ICollection<AtletaFederacionTutor>` | Sí (Navegación) |

## DTO: `TutorDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `TipoTutor` | `string` | `TipoTutor` | **Coincide Exacto** | - |
| `NombrePersona` | `string?` | `-` | *No coincide* | - |
| `Documento` | `string?` | `-` | *No coincide* | - |
| `Telefono` | `string?` | `-` | *No coincide* | - |
| `Email` | `string?` | `-` | *No coincide* | - |
| `CantidadAtletas` | `int` | `-` | *No coincide* | - |

## DTO: `TutorCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `TipoTutor` | `string` | `TipoTutor` | **Coincide Exacto** | - |

## DTO: `TutorDetailDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `TipoTutor` | `string` | `TipoTutor` | **Coincide Exacto** | - |
| `Participante` | `PersonaDto?` | `Participante` | **Coincide Exacto** | Cambio de tipo: Participante -> PersonaDto? |
| `AtletasTutores` | `List<AtletaTutorDto>?` | `AtletasTutores` | **Coincide Exacto** | Cambio de tipo: ICollection<AtletaFederacionTutor> -> List<AtletaTutorDto>? |