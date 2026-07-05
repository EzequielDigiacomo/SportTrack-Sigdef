# Mapeo de Clase: Club

> **Descripción:** Representa los clubes deportivos dentro de la federación. Mapea campos del SaaS y afiliación.

## Entidad Dominio: `Club`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `IdClub` | `int` | - |
| `Nombre` | `string` | - |
| `Siglas` | `string?` | - |
| `Email` | `string?` | - |
| `Telefono` | `string?` | - |
| `Direccion` | `string?` | - |
| `Ubicacion` | `string?` | - |
| `Activo` | `bool` | - |
| `IdFederacion` | `int?` | Sí (FK) |
| `Federacion` | `Federacion?` | - |
| `PlanSaaSId` | `int?` | - |
| `PlanSaaS` | `PlanSaaS?` | - |
| `FrecuenciaPago` | `string?` | - |
| `FechaAltaPlan` | `DateTime?` | - |
| `FechaVencimientoPlan` | `DateTime?` | - |
| `BloqueadoPorFaltaDePago` | `bool` | - |
| `PagoAfiliacionAlDia` | `bool` | - |
| `SolicitudPagoPendiente` | `bool` | - |
| `EstadoMatricula` | `EstadoPago` | - |
| `Participantes` | `ICollection<Participante>` | Sí (Navegación) |
| `Usuarios` | `ICollection<Usuario>` | Sí (Navegación) |
| `AtletasFederados` | `ICollection<AtletaFederacion>` | Sí (Navegación) |
| `Entrenadores` | `ICollection<EntrenadorFederacion>` | Sí (Navegación) |
| `Representantes` | `ICollection<DelegadoFederacionClub>` | Sí (Navegación) |
| `Pagos` | `ICollection<PagoFederacionTransaccion>` | Sí (Navegación) |

## DTO: `ClubDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdClub` | `int` | `IdClub` | **Coincide Exacto** | - |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Direccion` | `string` | `Direccion` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `Telefono` | `string` | `Telefono` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `Siglas` | `string` | `Siglas` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `IdFederacion` | `int?` | `IdFederacion` | **Coincide Exacto** | - |
| `EstadoMatricula` | `SportTrack_Sigdef.Entidades.Enums.EstadoPago` | `EstadoMatricula` | **Coincide Exacto** | Cambio de tipo: EstadoPago -> SportTrack_Sigdef.Entidades.Enums.EstadoPago |
| `TieneDelegado` | `bool` | `-` | *No coincide* | - |
| `CantidadAtletas` | `int` | `-` | *No coincide* | - |
| `CantidadEntrenadores` | `int` | `-` | *No coincide* | - |
| `CantidadRepresentantes` | `int` | `-` | *No coincide* | - |

## DTO: `ClubCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Direccion` | `string` | `Direccion` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `Telefono` | `string` | `Telefono` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `Siglas` | `string` | `Siglas` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `Email` | `string` | `Email` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `IdFederacion` | `int?` | `IdFederacion` | **Coincide Exacto** | - |
| `EstadoMatricula` | `Enums.EstadoPago` | `EstadoMatricula` | **Coincide Exacto** | Cambio de tipo: EstadoPago -> Enums.EstadoPago |

## DTO: `ClubUpdateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |

## DTO: `ClubDetailDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdClub` | `int` | `IdClub` | **Coincide Exacto** | - |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Direccion` | `string` | `Direccion` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `Telefono` | `string` | `Telefono` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `Siglas` | `string` | `Siglas` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `Email` | `string` | `Email` | **Coincide Exacto** | Cambio de tipo: string? -> string |
| `IdFederacion` | `int?` | `IdFederacion` | **Coincide Exacto** | - |
| `EstadoMatricula` | `Enums.EstadoPago` | `EstadoMatricula` | **Coincide Exacto** | Cambio de tipo: EstadoPago -> Enums.EstadoPago |
| `AtletasFederados` | `List<AtletaDto>?` | `AtletasFederados` | **Coincide Exacto** | Cambio de tipo: ICollection<AtletaFederacion> -> List<AtletaDto>? |
| `Entrenadores` | `List<EntrenadorDto>?` | `Entrenadores` | **Coincide Exacto** | Cambio de tipo: ICollection<EntrenadorFederacion> -> List<EntrenadorDto>? |
| `Representantes` | `List<DelegadoClubDto>?` | `Representantes` | **Coincide Exacto** | Cambio de tipo: ICollection<DelegadoFederacionClub> -> List<DelegadoClubDto>? |
| `Pagos` | `List<PagoTransaccionDto>?` | `Pagos` | **Coincide Exacto** | Cambio de tipo: ICollection<PagoFederacionTransaccion> -> List<PagoTransaccionDto>? |