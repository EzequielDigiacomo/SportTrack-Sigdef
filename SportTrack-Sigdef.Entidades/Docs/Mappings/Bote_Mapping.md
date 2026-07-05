# Mapeo de Clase: Bote

> **Descripción:** Representa los botes/embarcaciones utilizados en regatas.

## Entidad Dominio: `Bote`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `Id` | `int` | Sí (FK) |
| `Tipo` | `string` | - |
| `Pruebas` | `ICollection<Prueba>` | Sí (Navegación) |

## DTO: `BoteDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Id` | `int` | `Id` | **Coincide Exacto** | - |
| `Tipo` | `string` | `Tipo` | **Coincide Exacto** | - |

## DTO: `BoteCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Tipo` | `string` | `Tipo` | **Coincide Exacto** | - |

## DTO: `BoteUpdateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Tipo` | `string` | `Tipo` | **Coincide Exacto** | - |