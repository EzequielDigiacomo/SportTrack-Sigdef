# Mapeo de Clase: Prueba

> **Descripción:** Definición de las pruebas olímpicas o promocionales (Categoría + Bote + Distancia).

## Entidad Dominio: `Prueba`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `IdPrueba` | `int` | - |
| `Nombre` | `string` | - |
| `TipoBote` | `int` | - |
| `CategoriaEdad` | `int` | - |
| `DistanciaId` | `int` | - |
| `SexoCompetencia` | `int` | - |
| `Descripcion` | `string?` | - |
| `Bote` | `Bote` | Sí (Navegación) |
| `Categoria` | `Categoria` | Sí (Navegación) |
| `Distancia` | `Distancia` | Sí (Navegación) |
| `Sexo` | `Sexo` | Sí (Navegación) |
| `EventoPruebas` | `ICollection<EventoPrueba>` | Sí (Navegación) |

## DTO: `PruebaDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdPrueba` | `int` | `IdPrueba` | **Coincide Exacto** | - |
| `Distancia` | `DistanciaRegata` | `Distancia` | **Coincide Exacto** | Cambio de tipo: Distancia -> DistanciaRegata |
| `CategoriaEdad` | `CategoriaEdad` | `CategoriaEdad` | **Coincide Exacto** | Cambio de tipo: int -> CategoriaEdad |
| `SexoCompetencia` | `SexoCompetencia` | `SexoCompetencia` | **Coincide Exacto** | Cambio de tipo: int -> SexoCompetencia |
| `TipoBote` | `TipoBote` | `TipoBote` | **Coincide Exacto** | Cambio de tipo: int -> TipoBote |

## DTO: `PruebaCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Distancia` | `DistanciaRegata` | `Distancia` | **Coincide Exacto** | Cambio de tipo: Distancia -> DistanciaRegata |
| `CategoriaEdad` | `CategoriaEdad` | `CategoriaEdad` | **Coincide Exacto** | Cambio de tipo: int -> CategoriaEdad |
| `SexoCompetencia` | `SexoCompetencia` | `SexoCompetencia` | **Coincide Exacto** | Cambio de tipo: int -> SexoCompetencia |
| `TipoBote` | `TipoBote` | `TipoBote` | **Coincide Exacto** | Cambio de tipo: int -> TipoBote |