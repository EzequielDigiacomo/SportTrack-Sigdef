# Mapeo de Clase: Distancia

> **Descripción:** Distancias de las regatas y pruebas.

## Entidad Dominio: `Distancia`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `Id` | `int` | Sí (FK) |
| `DistanciaRegata` | `DistanciaRegataEnum` | - |
| `GapSugerido` | `int` | - |
| `Pruebas` | `ICollection<Prueba>` | Sí (Navegación) |

## DTO: `DistanciaDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Id` | `int` | `Id` | **Coincide Exacto** | - |
| `DistanciaRegata` | `int` | `DistanciaRegata` | **Coincide Exacto** | Cambio de tipo: DistanciaRegataEnum -> int |
| `Metros` | `int` | `-` | *No coincide* | - |
| `Descripcion` | `string` | `-` | *No coincide* | - |
| `GapSugerido` | `int` | `GapSugerido` | **Coincide Exacto** | - |

## DTO: `DistanciaCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `DistanciaRegata` | `DistanciaRegataEnum` | `DistanciaRegata` | **Coincide Exacto** | - |
| `GapSugerido` | `int` | `GapSugerido` | **Coincide Exacto** | - |

## DTO: `DistanciaUpdateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `DistanciaRegata` | `DistanciaRegataEnum` | `DistanciaRegata` | **Coincide Exacto** | - |
| `GapSugerido` | `int` | `GapSugerido` | **Coincide Exacto** | - |

## DTO: `DistanciaOptionDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdDistanciaEnum` | `int` | `-` | *No coincide* | - |
| `CodigoDistanca` | `string` | `-` | *No coincide* | - |
| `NombreDistancias` | `string` | `-` | *No coincide* | - |
| `Metros` | `decimal` | `-` | *No coincide* | - |
| `TipoDistancia` | `string` | `-` | *No coincide* | - |