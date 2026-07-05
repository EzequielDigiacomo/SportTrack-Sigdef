# Mapeo de Clase: InscripcionTripulante

> **Descripción:** Tripulantes asociados a una inscripción en botes de equipo (K2, K4, etc.).

## Entidad Dominio: `InscripcionTripulante`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `Id` | `int` | Sí (FK) |
| `InscripcionId` | `int` | - |
| `ParticipanteId` | `int` | - |
| `PosicionEnBote` | `int?` | - |
| `Inscripcion` | `Inscripcion` | Sí (Navegación) |
| `Participante` | `Participante` | Sí (Navegación) |

## DTO: `InscripcionTripulanteDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Id` | `int` | `Id` | **Coincide Exacto** | - |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `ParticipanteNombreCompleto` | `string?` | `Participante.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `PosicionEnBote` | `int?` | `PosicionEnBote` | **Coincide Exacto** | - |

## DTO: `InscripcionTripulanteCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `PosicionEnBote` | `int?` | `PosicionEnBote` | **Coincide Exacto** | - |