# Mapeo de Clase: AtletaFederacion

> **Descripción:** Detalle federativo de un atleta (número de ficha, estado médico, etc.).

## Entidad Dominio: `AtletaFederacion`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `ParticipanteId` | `int` | - |
| `Participante` | `Participante` | Sí (Navegación) |
| `IdClub` | `int?` | Sí (FK) |
| `Club` | `Club?` | - |
| `IdFederacion` | `int?` | Sí (FK) |
| `Federacion` | `Federacion?` | - |
| `EstadoPago` | `EstadoPago` | - |
| `PerteneceSeleccion` | `bool` | - |
| `Categoria` | `CategoriaEdad?` | - |
| `FechaCreacion` | `DateTime` | - |
| `BecadoEnard` | `bool` | - |
| `BecadoSdn` | `bool` | - |
| `MontoBeca` | `decimal` | - |
| `PresentoAptoMedico` | `bool` | - |
| `FechaAptoMedico` | `DateTime?` | - |
| `Inscripciones` | `ICollection<Inscripcion>` | Sí (Navegación) |
| `Tutores` | `ICollection<AtletaFederacionTutor>` | Sí (Navegación) |

## DTO: `AtletaDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `IdClub` | `int?` | `IdClub` | **Coincide Exacto** | - |
| `Documento` | `string` | `-` | *No coincide* | - |
| `FechaNacimiento` | `DateTime` | `-` | *No coincide* | - |
| `EstadoPago` | `EstadoPago` | `EstadoPago` | **Coincide Exacto** | - |
| `PerteneceSeleccion` | `bool` | `PerteneceSeleccion` | **Coincide Exacto** | - |
| `Categoria` | `CategoriaEdad?` | `Categoria` | **Coincide Exacto** | - |
| `BecadoEnard` | `bool` | `BecadoEnard` | **Coincide Exacto** | - |
| `BecadoSdn` | `bool` | `BecadoSdn` | **Coincide Exacto** | - |
| `MontoBeca` | `decimal` | `MontoBeca` | **Coincide Exacto** | - |
| `PresentoAptoMedico` | `bool` | `PresentoAptoMedico` | **Coincide Exacto** | - |
| `FechaAptoMedico` | `DateTime?` | `FechaAptoMedico` | **Coincide Exacto** | - |
| `FechaCreacion` | `DateTime` | `FechaCreacion` | **Coincide Exacto** | - |
| `NombrePersona` | `string?` | `-` | *No coincide* | - |
| `NombreClub` | `string?` | `-` | *No coincide* | - |

## DTO: `AtletaCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `IdClub` | `int?` | `IdClub` | **Coincide Exacto** | - |
| `IdFederacion` | `int?` | `IdFederacion` | **Coincide Exacto** | - |
| `EstadoPago` | `EstadoPago` | `EstadoPago` | **Coincide Exacto** | - |
| `PerteneceSeleccion` | `bool` | `PerteneceSeleccion` | **Coincide Exacto** | - |
| `Categoria` | `CategoriaEdad?` | `Categoria` | **Coincide Exacto** | - |
| `BecadoEnard` | `bool` | `BecadoEnard` | **Coincide Exacto** | - |
| `BecadoSdn` | `bool` | `BecadoSdn` | **Coincide Exacto** | - |
| `MontoBeca` | `decimal` | `MontoBeca` | **Coincide Exacto** | - |
| `PresentoAptoMedico` | `bool` | `PresentoAptoMedico` | **Coincide Exacto** | - |
| `FechaAptoMedico` | `DateTime?` | `FechaAptoMedico` | **Coincide Exacto** | - |
| `FechaCreacion` | `DateTime` | `FechaCreacion` | **Coincide Exacto** | - |
| `NombrePersona` | `string?` | `-` | *No coincide* | - |
| `NombreClub` | `string?` | `-` | *No coincide* | - |

## DTO: `AtletaDetailDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `IdClub` | `int?` | `IdClub` | **Coincide Exacto** | - |
| `EstadoPago` | `EstadoPago` | `EstadoPago` | **Coincide Exacto** | - |
| `PerteneceSeleccion` | `bool` | `PerteneceSeleccion` | **Coincide Exacto** | - |
| `Categoria` | `CategoriaEdad?` | `Categoria` | **Coincide Exacto** | - |
| `BecadoEnard` | `bool` | `BecadoEnard` | **Coincide Exacto** | - |
| `BecadoSdn` | `bool` | `BecadoSdn` | **Coincide Exacto** | - |
| `MontoBeca` | `decimal` | `MontoBeca` | **Coincide Exacto** | - |
| `PresentoAptoMedico` | `bool` | `PresentoAptoMedico` | **Coincide Exacto** | - |
| `FechaAptoMedico` | `DateTime?` | `FechaAptoMedico` | **Coincide Exacto** | - |
| `FechaCreacion` | `DateTime` | `FechaCreacion` | **Coincide Exacto** | - |
| `Participante` | `PersonaDto?` | `Participante` | **Coincide Exacto** | Cambio de tipo: Participante -> PersonaDto? |
| `Club` | `ClubDto?` | `Club` | **Coincide Exacto** | Cambio de tipo: Club? -> ClubDto? |
| `Inscripciones` | `List<InscripcionDto>?` | `Inscripciones` | **Coincide Exacto** | Cambio de tipo: ICollection<Inscripcion> -> List<InscripcionDto>? |
| `Tutores` | `List<AtletaTutorDto>?` | `Tutores` | **Coincide Exacto** | Cambio de tipo: ICollection<AtletaFederacionTutor> -> List<AtletaTutorDto>? |

## DTO: `AtletaListDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int` | `ParticipanteId` | **Coincide Exacto** | - |
| `NombrePersona` | `string` | `-` | *No coincide* | - |
| `Documento` | `string?` | `-` | *No coincide* | - |
| `FechaNacimiento` | `DateTime` | `-` | *No coincide* | - |
| `Edad` | `int?` | `-` | *No coincide* | - |
| `NombreClub` | `string?` | `-` | *No coincide* | - |
| `Categoria` | `CategoriaEdad?` | `Categoria` | **Coincide Exacto** | - |
| `PerteneceSeleccion` | `bool` | `PerteneceSeleccion` | **Coincide Exacto** | - |
| `EstadoPago` | `EstadoPago` | `EstadoPago` | **Coincide Exacto** | - |
| `FechaCreacion` | `DateTime?` | `FechaCreacion` | **Coincide Exacto** | Cambio de tipo: DateTime -> DateTime? |
| `CantidadDocumentos` | `int?` | `-` | *No coincide* | - |
| `TutorInfo` | `TutorListDto?` | `-` | *No coincide* | - |

## DTO: `AtletaFullCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `PersonaAtleta` | `PersonaCreateDto` | `-` | *No coincide* | - |
| `DatosDeportivos` | `AtletaCreateDto` | `-` | *No coincide* | - |
| `EsMenor` | `bool` | `-` | *No coincide* | - |
| `TutorFederacion` | `TutorFullDto?` | `-` | *No coincide* | - |