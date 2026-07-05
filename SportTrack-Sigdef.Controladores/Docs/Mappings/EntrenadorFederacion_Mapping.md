# Mapeo de Clase: EntrenadorFederacion

> **Descripción:** Entrenadores federados asociados a clubes.

## Entidad Dominio: `EntrenadorFederacion`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `ParticipanteId` | `int` | - |
| `Participante` | `Participante` | Sí (Navegación) |
| `IdClub` | `int?` | Sí (FK) |
| `Club` | `Club?` | - |
| `IdFederacion` | `int?` | Sí (FK) |
| `Federacion` | `Federacion?` | - |
| `PerteneceSeleccion` | `bool?` | - |
| `CategoriaSeleccion` | `string?` | - |
| `BecadoEnard` | `bool?` | - |
| `BecadoSdn` | `bool?` | - |
| `MontoBeca` | `decimal?` | - |
| `PresentoAptoMedico` | `bool?` | - |

## DTO: `EntrenadorDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `IdClub` | `int` | `IdClub` | **Coincide Exacto** | Cambio de tipo: int? -> int |
| `Licencia` | `string` | `-` | *No coincide* | - |
| `PerteneceSeleccion` | `bool` | `PerteneceSeleccion` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |
| `CategoriaSeleccion` | `string` | `CategoriaSeleccion` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `BecadoEnard` | `bool` | `BecadoEnard` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |
| `BecadoSdn` | `bool` | `BecadoSdn` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |
| `MontoBeca` | `decimal` | `MontoBeca` | **Coincide Exacto** | Cambio de tipo: decimal? -> decimal |
| `PresentoAptoMedico` | `bool` | `PresentoAptoMedico` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |
| `NombrePersona` | `string?` | `-` | *No coincide* | - |
| `NombreClub` | `string?` | `-` | *No coincide* | - |
| `SiglasClub` | `string?` | `-` | *No coincide* | - |

## DTO: `EntrenadorCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `IdClub` | `int?` | `IdClub` | **Coincide Exacto** | - |
| `IdFederacion` | `int?` | `IdFederacion` | **Coincide Exacto** | - |
| `PerteneceSeleccion` | `bool` | `PerteneceSeleccion` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |
| `CategoriaSeleccion` | `string?` | `CategoriaSeleccion` | **Coincide Exacto** | - |
| `BecadoEnard` | `bool` | `BecadoEnard` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |
| `BecadoSdn` | `bool` | `BecadoSdn` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |
| `MontoBeca` | `decimal` | `MontoBeca` | **Coincide Exacto** | Cambio de tipo: decimal? -> decimal |
| `PresentoAptoMedico` | `bool` | `PresentoAptoMedico` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |

## DTO: `EntrenadorDetailDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `IdClub` | `int?` | `IdClub` | **Coincide Exacto** | - |
| `PerteneceSeleccion` | `bool` | `PerteneceSeleccion` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |
| `CategoriaSeleccion` | `string?` | `CategoriaSeleccion` | **Coincide Exacto** | - |
| `BecadoEnard` | `bool` | `BecadoEnard` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |
| `BecadoSdn` | `bool` | `BecadoSdn` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |
| `MontoBeca` | `decimal` | `MontoBeca` | **Coincide Exacto** | Cambio de tipo: decimal? -> decimal |
| `PresentoAptoMedico` | `bool` | `PresentoAptoMedico` | **Coincide Exacto** | Cambio de tipo: bool? -> bool |
| `Participante` | `PersonaDto?` | `Participante` | **Coincide Exacto** | Cambio de tipo: Participante -> PersonaDto? |
| `Club` | `ClubDto?` | `Club` | **Coincide Exacto** | Cambio de tipo: Club? -> ClubDto? |