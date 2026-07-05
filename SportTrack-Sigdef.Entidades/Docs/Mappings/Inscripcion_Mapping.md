# Mapeo de Clase: Inscripcion

> **Descripción:** Registro de la inscripción de atletas o botes en una prueba de un evento.

## Entidad Dominio: `Inscripcion`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `IdInscripcion` | `int` | - |
| `IdEventoPrueba` | `int` | Sí (FK) |
| `IdParticipante` | `int?` | Sí (FK) |
| `FechaInscripcion` | `DateTime` | - |
| `NumeroCompetidor` | `string` | - |
| `EsCabezaDeSerie` | `bool` | - |
| `Estado` | `Enums.EstadoInscripcionEnum` | - |
| `Pagado` | `bool` | - |
| `EventoPrueba` | `EventoPrueba` | Sí (Navegación) |
| `Participante` | `Participante?` | - |
| `Tripulantes` | `ICollection<InscripcionTripulante>` | Sí (Navegación) |
| `Resultados` | `ICollection<Resultado>` | Sí (Navegación) |

## DTO: `InscripcionDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Id` | `int` | `IdInscripcion` | Mapeado (Id) | Clave primaria renombrada |
| `EventoPruebaId` | `int` | `EventoPrueba.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `ParticipanteId` | `int?` | `Participante.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `ParticipanteNombreCompleto` | `string?` | `Participante.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `ClubNombre` | `string?` | `-` | *No coincide* | - |
| `ClubSigla` | `string?` | `-` | *No coincide* | - |
| `FechaInscripcion` | `DateTime` | `FechaInscripcion` | **Coincide Exacto** | - |
| `NumeroCompetidor` | `string` | `NumeroCompetidor` | **Coincide Exacto** | - |
| `EsCabezaDeSerie` | `bool` | `EsCabezaDeSerie` | **Coincide Exacto** | - |
| `Estado` | `string` | `Estado` | **Coincide Exacto** | Cambio de tipo: Enums.EstadoInscripcionEnum -> string |
| `Pagado` | `bool` | `Pagado` | **Coincide Exacto** | - |
| `ClubId` | `int?` | `-` | *No coincide* | - |
| `ParticipanteClubId` | `int?` | `Participante.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `EventoNombre` | `string?` | `-` | *No coincide* | - |
| `PruebaNombre` | `string?` | `-` | *No coincide* | - |
| `Tripulantes` | `ICollection<InscripcionTripulanteDto>` | `Tripulantes` | **Coincide Exacto** | Cambio de tipo: ICollection<InscripcionTripulante> -> ICollection<InscripcionTripulanteDto> |

## DTO: `InscripcionCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `EventoPruebaId` | `int` | `EventoPrueba.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `ParticipanteId` | `int?` | `Participante.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `NumeroCompetidor` | `string` | `NumeroCompetidor` | **Coincide Exacto** | - |
| `Pagado` | `bool` | `Pagado` | **Coincide Exacto** | - |
| `Tripulantes` | `ICollection<InscripcionTripulanteCreateDto>` | `Tripulantes` | **Coincide Exacto** | Cambio de tipo: ICollection<InscripcionTripulante> -> ICollection<InscripcionTripulanteCreateDto> |

## DTO: `InscripcionUpdateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `EventoPruebaId` | `int?` | `EventoPrueba.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `Estado` | `string?` | `Estado` | **Coincide Exacto** | Cambio de tipo: Enums.EstadoInscripcionEnum -> string? |
| `NumeroCompetidor` | `string?` | `NumeroCompetidor` | **Coincide Exacto** | Cambio de tipo: string -> string? |
| `Pagado` | `bool?` | `Pagado` | **Coincide Exacto** | Cambio de tipo: bool -> bool? |

## DTO: `InscripcionDetailDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdInscripcion` | `int` | `IdInscripcion` | **Coincide Exacto** | - |
| `ParticipanteId` | `int` | `Participante.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `IdEvento` | `int` | `-` | *No coincide* | - |
| `IdEventoPrueba` | `int` | `IdEventoPrueba` | **Coincide Exacto** | - |
| `FechaInscripcion` | `DateTime` | `FechaInscripcion` | **Coincide Exacto** | - |
| `AtletaFederacion` | `AtletaDto?` | `-` | *No coincide* | - |
| `Evento` | `EventoDto?` | `-` | *No coincide* | - |