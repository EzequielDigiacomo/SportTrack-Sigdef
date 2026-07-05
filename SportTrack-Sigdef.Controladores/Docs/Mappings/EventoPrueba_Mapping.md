# Mapeo de Clase: EventoPrueba

> **Descripción:** Asociación de una Prueba específica a un Evento determinado.

## Entidad Dominio: `EventoPrueba`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `IdEventoPrueba` | `int` | - |
| `IdEvento` | `int` | Sí (FK) |
| `IdPrueba` | `int` | Sí (FK) |
| `FechaHora` | `DateTime` | - |
| `MaxParticipantes` | `int` | - |
| `Pista` | `string?` | - |
| `Estado` | `EstadoEventoEnum` | - |
| `PlanProgresionAsignado` | `string?` | - |
| `PrecioCategoria` | `decimal?` | - |
| `Evento` | `Evento` | Sí (Navegación) |
| `Prueba` | `Prueba` | Sí (Navegación) |
| `Inscripciones` | `ICollection<Inscripcion>` | Sí (Navegación) |
| `Etapas` | `ICollection<Etapa>` | Sí (Navegación) |
| `ReglasProgresion` | `ICollection<ReglaProgresion>` | Sí (Navegación) |

## DTO: `EventoPruebaDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdEventoPrueba` | `int` | `IdEventoPrueba` | **Coincide Exacto** | - |
| `IdEvento` | `int` | `IdEvento` | **Coincide Exacto** | - |
| `IdPrueba` | `int` | `IdPrueba` | **Coincide Exacto** | - |
| `PrecioCategoria` | `decimal?` | `PrecioCategoria` | **Coincide Exacto** | - |
| `Prueba` | `SportTrack_Sigdef.Entidades.DTOs.Prueba.PruebaDto` | `Prueba` | **Coincide Exacto** | Cambio de tipo: Prueba -> SportTrack_Sigdef.Entidades.DTOs.Prueba.PruebaDto |

## DTO: `EventoPruebaCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdEvento` | `int` | `IdEvento` | **Coincide Exacto** | - |
| `IdPrueba` | `int` | `IdPrueba` | **Coincide Exacto** | - |
| `PrecioCategoria` | `decimal?` | `PrecioCategoria` | **Coincide Exacto** | - |

## DTO: `EventoPruebaUpdateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdEventoPrueba` | `int` | `IdEventoPrueba` | **Coincide Exacto** | - |

## DTO: `EventoPruebaResponseDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdEventoPrueba` | `int` | `IdEventoPrueba` | **Coincide Exacto** | - |
| `DistanciaId` | `int` | `-` | *No coincide* | - |
| `DistanciaCodigo` | `string` | `-` | *No coincide* | - |
| `DistanciaNombre` | `string` | `-` | *No coincide* | - |
| `Metros` | `decimal` | `-` | *No coincide* | - |
| `CategoriaEdad` | `int` | `-` | *No coincide* | - |
| `PrecioCategoria` | `decimal?` | `PrecioCategoria` | **Coincide Exacto** | - |
| `DistanciaRegata` | `int` | `-` | *No coincide* | - |
| `TipoBote` | `int` | `-` | *No coincide* | - |
| `TipoBoteNombre` | `string` | `-` | *No coincide* | - |
| `SexoCompetencia` | `int` | `-` | *No coincide* | - |