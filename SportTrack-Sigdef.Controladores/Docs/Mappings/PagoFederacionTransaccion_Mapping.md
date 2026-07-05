# Mapeo de Clase: PagoFederacionTransaccion

> **Descripción:** Transacciones de pago de cuotas o afiliaciones en el sistema federativo.

## Entidad Dominio: `PagoFederacionTransaccion`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `IdPago` | `int` | Sí (FK) |
| `Concepto` | `string` | - |
| `Monto` | `decimal` | - |
| `Estado` | `EstadoPagoTransaccion` | - |
| `FechaCreacion` | `DateTime` | - |
| `FechaAprobacion` | `DateTime?` | - |
| `IdParticipante` | `int` | Sí (FK) |
| `Participante` | `Participante` | Sí (Navegación) |
| `IdClub` | `int` | Sí (FK) |
| `Club` | `Club` | Sí (Navegación) |
| `IdMercadoPago` | `string` | Sí (FK) |

## DTO: `PagoTransaccionDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdPago` | `int` | `IdPago` | **Coincide Exacto** | - |
| `Concepto` | `string` | `Concepto` | **Coincide Exacto** | - |
| `Monto` | `decimal` | `Monto` | **Coincide Exacto** | - |
| `Estado` | `EstadoPagoTransaccion` | `Estado` | **Coincide Exacto** | - |
| `FechaCreacion` | `DateTime` | `FechaCreacion` | **Coincide Exacto** | - |
| `FechaAprobacion` | `DateTime?` | `FechaAprobacion` | **Coincide Exacto** | - |
| `ParticipanteId` | `int` | `Participante.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `IdClub` | `int` | `IdClub` | **Coincide Exacto** | - |
| `IdMercadoPago` | `string` | `IdMercadoPago` | **Coincide Exacto** | - |
| `NombrePersona` | `string?` | `-` | *No coincide* | - |
| `NombreClub` | `string?` | `-` | *No coincide* | - |
| `EstadoDescripcion` | `string?` | `-` | *No coincide* | - |

## DTO: `PagoTransaccionCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Concepto` | `string` | `Concepto` | **Coincide Exacto** | - |
| `Monto` | `decimal` | `Monto` | **Coincide Exacto** | - |
| `Estado` | `EstadoPagoTransaccion` | `Estado` | **Coincide Exacto** | - |
| `ParticipanteId` | `int` | `Participante.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `IdClub` | `int` | `IdClub` | **Coincide Exacto** | - |
| `IdMercadoPago` | `string` | `IdMercadoPago` | **Coincide Exacto** | - |

## DTO: `PagoTransaccionDetailDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdPago` | `int` | `IdPago` | **Coincide Exacto** | - |
| `Concepto` | `string` | `Concepto` | **Coincide Exacto** | - |
| `Monto` | `decimal` | `Monto` | **Coincide Exacto** | - |
| `Estado` | `EstadoPagoTransaccion` | `Estado` | **Coincide Exacto** | - |
| `FechaCreacion` | `DateTime` | `FechaCreacion` | **Coincide Exacto** | - |
| `FechaAprobacion` | `DateTime?` | `FechaAprobacion` | **Coincide Exacto** | - |
| `ParticipanteId` | `int` | `Participante.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `IdClub` | `int` | `IdClub` | **Coincide Exacto** | - |
| `IdMercadoPago` | `string` | `IdMercadoPago` | **Coincide Exacto** | - |
| `Participante` | `PersonaDto?` | `Participante` | **Coincide Exacto** | Cambio de tipo: Participante -> PersonaDto? |
| `Club` | `ClubDto?` | `Club` | **Coincide Exacto** | Cambio de tipo: Club -> ClubDto? |