# Mapeo de Clase: Evento

> **Descripción:** Representa los eventos o campeonatos organizados por la federación o clubes.

## Entidad Dominio: `Evento`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `IdEvento` | `int` | - |
| `Nombre` | `string` | - |
| `Fecha` | `DateTime` | - |
| `FechaFin` | `DateTime?` | - |
| `Ubicacion` | `string?` | - |
| `Estado` | `EstadoEventoEnum` | - |
| `FechaCreacion` | `DateTime` | - |
| `FechaFinInscripciones` | `DateTime?` | - |
| `EstaActivo` | `bool` | - |
| `Descripcion` | `string?` | - |
| `TipoEvento` | `string` | - |
| `FechaInicioInscripciones` | `DateTime?` | - |
| `Ciudad` | `string?` | - |
| `Provincia` | `string?` | - |
| `PrecioBase` | `decimal` | - |
| `CupoMaximo` | `int` | - |
| `TieneCronometraje` | `bool` | - |
| `RequiereCertificadoMedico` | `bool` | - |
| `Observaciones` | `string?` | - |
| `IdClub` | `int?` | Sí (FK) |
| `Club` | `Club?` | - |
| `IdFederacion` | `int?` | Sí (FK) |
| `Federacion` | `Federacion?` | - |
| `InscripcionesHabilitadas` | `bool` | - |
| `RestringirSoloCategoriaPropia` | `bool` | - |
| `PermitirSub23EnSenior` | `bool` | - |
| `PermitirMasterBajarASenior` | `bool` | - |
| `PermitirCompletarK4` | `bool` | - |
| `LimitacionBotesAB` | `bool` | - |
| `HoraInicioEvento` | `TimeSpan` | - |
| `CarrilesDisponibles` | `int` | - |
| `PerfilTiempo` | `PerfilTiempoEnum` | - |
| `HoraInicioReceso` | `TimeSpan` | - |
| `HoraFinReceso` | `TimeSpan` | - |
| `SinReceso` | `bool` | - |
| `GapEntrePruebas` | `int` | - |
| `PermitirCombinadas` | `bool` | - |
| `UsarGapVariable` | `bool` | - |
| `TimeZoneId` | `string` | - |
| `CategoriasHabilitadas` | `string?` | - |
| `BotesHabilitados` | `string?` | - |
| `DistanciasHabilitadas` | `string?` | - |
| `FechaActualizacion` | `DateTime?` | - |
| `EventoPruebas` | `ICollection<EventoPrueba>` | Sí (Navegación) |

## DTO: `EventoDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdEvento` | `int` | `IdEvento` | **Coincide Exacto** | - |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Descripcion` | `string` | `Descripcion` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `TipoEvento` | `string` | `TipoEvento` | **Coincide Exacto** | - |
| `Ubicacion` | `string` | `Ubicacion` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `IdClub` | `int` | `IdClub` | **Coincide Exacto** | Cambio de tipo: int? -> int |
| `FechaInicio` | `DateTime` | `-` | *No coincide* | - |
| `FechaFin` | `DateTime` | `FechaFin` | **Coincide Exacto** | Cambio de tipo: DateTime? -> DateTime |
| `CantidadInscripciones` | `int` | `-` | *No coincide* | - |
| `TotalAtletas` | `int` | `-` | *No coincide* | - |
| `TotalClubes` | `int` | `-` | *No coincide* | - |
| `Estado` | `string` | `Estado` | **Coincide Exacto** | Cambio de tipo: EstadoEventoEnum -> string |
| `Pruebas` | `List<EventoPruebaDto>` | `-` | *No coincide* | - |

## DTO: `EventoCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Fecha` | `DateTime` | `Fecha` | **Coincide Exacto** | - |
| `FechaFin` | `DateTime?` | `FechaFin` | **Coincide Exacto** | - |
| `Ubicacion` | `string?` | `Ubicacion` | **Coincide Exacto** | - |
| `FechaFinInscripciones` | `DateTime?` | `FechaFinInscripciones` | **Coincide Exacto** | - |
| `RestringirSoloCategoriaPropia` | `bool` | `RestringirSoloCategoriaPropia` | **Coincide Exacto** | - |
| `PermitirSub23EnSenior` | `bool` | `PermitirSub23EnSenior` | **Coincide Exacto** | - |
| `PermitirMasterBajarASenior` | `bool` | `PermitirMasterBajarASenior` | **Coincide Exacto** | - |
| `PermitirCompletarK4` | `bool` | `PermitirCompletarK4` | **Coincide Exacto** | - |
| `LimitacionBotesAB` | `bool` | `LimitacionBotesAB` | **Coincide Exacto** | - |
| `ClubId` | `int?` | `Club.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `InscripcionesHabilitadas` | `bool` | `InscripcionesHabilitadas` | **Coincide Exacto** | - |
| `HoraInicioEvento` | `string` | `HoraInicioEvento` | **Coincide Exacto** | Cambio de tipo: TimeSpan -> string |
| `CarrilesDisponibles` | `int` | `CarrilesDisponibles` | **Coincide Exacto** | - |
| `PerfilTiempo` | `string` | `PerfilTiempo` | **Coincide Exacto** | Cambio de tipo: PerfilTiempoEnum -> string |
| `HoraInicioReceso` | `string` | `HoraInicioReceso` | **Coincide Exacto** | Cambio de tipo: TimeSpan -> string |
| `HoraFinReceso` | `string` | `HoraFinReceso` | **Coincide Exacto** | Cambio de tipo: TimeSpan -> string |
| `SinReceso` | `bool` | `SinReceso` | **Coincide Exacto** | - |
| `GapEntrePruebas` | `int` | `GapEntrePruebas` | **Coincide Exacto** | - |
| `PermitirCombinadas` | `bool` | `PermitirCombinadas` | **Coincide Exacto** | - |
| `UsarGapVariable` | `bool` | `UsarGapVariable` | **Coincide Exacto** | - |
| `TimeZoneId` | `string` | `TimeZoneId` | **Coincide Exacto** | - |
| `CategoriasHabilitadas` | `string?` | `CategoriasHabilitadas` | **Coincide Exacto** | - |
| `BotesHabilitados` | `string?` | `BotesHabilitados` | **Coincide Exacto** | - |
| `DistanciasHabilitadas` | `string?` | `DistanciasHabilitadas` | **Coincide Exacto** | - |

## DTO: `EventoUpdateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdEvento` | `int` | `IdEvento` | **Coincide Exacto** | - |
| `EstaActivo` | `bool` | `EstaActivo` | **Coincide Exacto** | - |

## DTO: `EventoDetailDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdEvento` | `int` | `IdEvento` | **Coincide Exacto** | - |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Email` | `string` | `-` | *No coincide* | - |
| `FechaInicio` | `DateTime` | `-` | *No coincide* | - |
| `FechaFin` | `DateTime` | `FechaFin` | **Coincide Exacto** | Cambio de tipo: DateTime? -> DateTime |
| `Inscripciones` | `List<InscripcionDto>?` | `-` | *No coincide* | - |
| `Pruebas` | `List<EventoPrueba.EventoPruebaDto>` | `-` | *No coincide* | - |

## DTO: `EventoResponseDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdEvento` | `int` | `IdEvento` | **Coincide Exacto** | - |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Descripcion` | `string?` | `Descripcion` | **Coincide Exacto** | - |
| `TipoEventoId` | `int` | `TipoEvento.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `TipoEventoNombre` | `string` | `Nombre.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `TipoEventoIcono` | `string` | `-` | *No coincide* | - |
| `TipoEventoColor` | `string` | `-` | *No coincide* | - |
| `FechaInicio` | `DateTime` | `-` | *No coincide* | - |
| `FechaFin` | `DateTime` | `FechaFin` | **Coincide Exacto** | Cambio de tipo: DateTime? -> DateTime |
| `FechaInicioInscripciones` | `DateTime?` | `FechaInicioInscripciones` | **Coincide Exacto** | - |
| `FechaFinInscripciones` | `DateTime?` | `FechaFinInscripciones` | **Coincide Exacto** | - |
| `Ubicacion` | `string?` | `Ubicacion` | **Coincide Exacto** | - |
| `Ciudad` | `string?` | `Ciudad` | **Coincide Exacto** | - |
| `Provincia` | `string?` | `Provincia` | **Coincide Exacto** | - |
| `Pruebas` | `List<EventoPruebaResponseDto>` | `-` | *No coincide* | - |
| `PrecioBase` | `decimal` | `PrecioBase` | **Coincide Exacto** | - |
| `CupoMaximo` | `int` | `CupoMaximo` | **Coincide Exacto** | - |
| `TieneCronometraje` | `bool` | `TieneCronometraje` | **Coincide Exacto** | - |
| `RequiereCertificadoMedico` | `bool` | `RequiereCertificadoMedico` | **Coincide Exacto** | - |
| `EstaActivo` | `bool` | `EstaActivo` | **Coincide Exacto** | - |
| `FechaCreacion` | `DateTime` | `FechaCreacion` | **Coincide Exacto** | - |
| `Observaciones` | `string?` | `Observaciones` | **Coincide Exacto** | - |
| `TotalInscritos` | `int` | `-` | *No coincide* | - |
| `CuposDisponibles` | `int` | `-` | *No coincide* | - |
| `InscripcionesAbiertas` | `bool` | `-` | *No coincide* | - |
| `TieneCupoDisponible` | `bool` | `-` | *No coincide* | - |
| `DiasRestantes` | `int` | `-` | *No coincide* | - |
| `FechasDisplay` | `string` | `-` | *No coincide* | - |
| `PeriodoInscripcionesDisplay` | `string` | `-` | *No coincide* | - |
| `EstadoDisplay` | `string` | `-` | *No coincide* | - |
| `UbicacionCompleta` | `string` | `-` | *No coincide* | - |
| `PrecioDisplay` | `string` | `-` | *No coincide* | - |
| `CupoDisplay` | `string` | `-` | *No coincide* | - |