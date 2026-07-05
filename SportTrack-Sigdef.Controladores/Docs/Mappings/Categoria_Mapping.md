# Mapeo de Clase: Categoria

> **Descripción:** Categorías de edad y nivel en las que compiten los atletas.

## Entidad Dominio: `Categoria`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `Id` | `int` | Sí (FK) |
| `Nombre` | `string` | - |
| `EdadMin` | `int?` | - |
| `EdadMax` | `int?` | - |
| `Pruebas` | `ICollection<Prueba>` | Sí (Navegación) |
| `Participantes` | `ICollection<Participante>` | Sí (Navegación) |

## DTO: `CategoriaDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Id` | `int` | `Id` | **Coincide Exacto** | - |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `EdadMin` | `int?` | `EdadMin` | **Coincide Exacto** | - |
| `EdadMax` | `int?` | `EdadMax` | **Coincide Exacto** | - |

## DTO: `CategoriaCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `EdadMin` | `int?` | `EdadMin` | **Coincide Exacto** | - |
| `EdadMax` | `int?` | `EdadMax` | **Coincide Exacto** | - |

## DTO: `CategoriaUpdateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `EdadMin` | `int?` | `EdadMin` | **Coincide Exacto** | - |
| `EdadMax` | `int?` | `EdadMax` | **Coincide Exacto** | - |

## DTO: `CategoriaEdadDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Id` | `int` | `Id` | **Coincide Exacto** | - |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Valor` | `string` | `-` | *No coincide* | - |