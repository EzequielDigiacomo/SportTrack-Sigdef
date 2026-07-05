# Mapeo de Clase: Participante

> **Descripción:** Representación unificada de atletas federados o personas que participan en regatas.

## Entidad Dominio: `Participante`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `ParticipanteId` | `int` | - |
| `Nombre` | `string` | - |
| `Apellido` | `string` | - |
| `FechaNacimiento` | `DateTime` | - |
| `SexoId` | `int` | - |
| `CategoriaId` | `int?` | - |
| `Pais` | `string?` | - |
| `IdClub` | `int?` | Sí (FK) |
| `Club` | `Club?` | - |
| `Documento` | `string?` | - |
| `Email` | `string?` | - |
| `Telefono` | `string?` | - |
| `Direccion` | `string?` | - |
| `PagoAfiliacionAlDia` | `bool` | - |
| `Sexo` | `Sexo` | Sí (Navegación) |
| `Categoria` | `Categoria?` | - |
| `Inscripciones` | `ICollection<Inscripcion>` | Sí (Navegación) |
| `DelegadoFederacionClub` | `DelegadoFederacionClub?` | - |
| `EntrenadorFederacion` | `EntrenadorFederacion?` | - |
| `TutorFederacion` | `TutorFederacion?` | - |
| `AtletaFederacion` | `AtletaFederacion?` | - |
| `Documentacion` | `ICollection<DocumentacionFederacionPersona>` | Sí (Navegación) |
| `Usuario` | `Usuario?` | - |
| `Pagos` | `ICollection<PagoFederacionTransaccion>` | Sí (Navegación) |

## DTO: `ParticipanteDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Id` | `int` | `-` | *No coincide* | - |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Apellido` | `string` | `Apellido` | **Coincide Exacto** | - |
| `FechaNacimiento` | `DateTime` | `FechaNacimiento` | **Coincide Exacto** | - |
| `SexoId` | `int` | `SexoId` | **Coincide Exacto** | - |
| `SexoNombre` | `string` | `Nombre.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `CategoriaId` | `int?` | `CategoriaId` | **Coincide Exacto** | - |
| `CategoriaNombre` | `string?` | `Nombre.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `ClubId` | `int?` | `Club.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `ClubNombre` | `string?` | `Nombre.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `Pais` | `string?` | `Pais` | **Coincide Exacto** | - |
| `Dni` | `string?` | `-` | *No coincide* | - |
| `Email` | `string?` | `Email` | **Coincide Exacto** | - |
| `Edad` | `int` | `-` | *No coincide* | - |
| `PagoAfiliacionAlDia` | `bool` | `PagoAfiliacionAlDia` | **Coincide Exacto** | - |

## DTO: `ParticipanteCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Apellido` | `string` | `Apellido` | **Coincide Exacto** | - |
| `FechaNacimiento` | `DateTime` | `FechaNacimiento` | **Coincide Exacto** | - |
| `SexoId` | `int` | `SexoId` | **Coincide Exacto** | - |
| `CategoriaId` | `int?` | `CategoriaId` | **Coincide Exacto** | - |
| `ClubId` | `int?` | `Club.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `FederacionId` | `int?` | `-` | *No coincide* | - |
| `Pais` | `string?` | `Pais` | **Coincide Exacto** | - |
| `Dni` | `string?` | `-` | *No coincide* | - |
| `Email` | `string?` | `Email` | **Coincide Exacto** | - |
| `PagoAfiliacionAlDia` | `bool` | `PagoAfiliacionAlDia` | **Coincide Exacto** | - |