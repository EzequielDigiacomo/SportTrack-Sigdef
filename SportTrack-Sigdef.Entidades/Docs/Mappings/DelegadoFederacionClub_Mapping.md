# Mapeo de Clase: DelegadoFederacionClub

> **Descripción:** Representantes delegados de un club ante la federación.

## Entidad Dominio: `DelegadoFederacionClub`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `IdParticipante` | `int?` | Sí (FK) |
| `Participante` | `Participante` | Sí (Navegación) |
| `IdRol` | `int` | Sí (FK) |
| `RolFederacion` | `RolFederacion` | Sí (Navegación) |
| `IdFederacion` | `int?` | Sí (FK) |
| `Federacion` | `Federacion` | Sí (Navegación) |
| `ClubIdClub` | `int?` | - |
| `Club` | `Club?` | - |

## DTO: `DelegadoClubDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int?` | `Participante.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `IdRol` | `int` | `IdRol` | **Coincide Exacto** | - |
| `IdFederacion` | `int?` | `IdFederacion` | **Coincide Exacto** | - |
| `IdClub` | `int?` | `-` | *No coincide* | - |
| `NombrePersona` | `string?` | `-` | *No coincide* | - |
| `TipoRol` | `string?` | `-` | *No coincide* | - |
| `NombreFederacion` | `string?` | `-` | *No coincide* | - |
| `NombreClub` | `string?` | `-` | *No coincide* | - |
| `Documento` | `string?` | `-` | *No coincide* | - |
| `Email` | `string?` | `-` | *No coincide* | - |
| `Telefono` | `string?` | `-` | *No coincide* | - |

## DTO: `DelegadoClubCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int?` | `Participante.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `IdRol` | `int` | `IdRol` | **Coincide Exacto** | - |
| `IdFederacion` | `int?` | `IdFederacion` | **Coincide Exacto** | - |
| `IdClub` | `int?` | `-` | *No coincide* | - |