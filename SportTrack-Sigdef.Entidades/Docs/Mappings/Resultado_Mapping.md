# Mapeo de Clase: Resultado

> **Descripción:** Resultados de los participantes en las diferentes fases de una prueba.

## Entidad Dominio: `Resultado`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `Id` | `int` | Sí (FK) |
| `FaseId` | `int` | - |
| `InscripcionId` | `int` | - |
| `Carril` | `int?` | - |
| `EsCabezaDeSerie` | `bool` | - |
| `TiempoOficial` | `TimeSpan?` | - |
| `Posicion` | `int?` | - |
| `Puntos` | `decimal?` | - |
| `VelocidadMedia` | `decimal?` | - |
| `Estado` | `Enums.EstadoResultadoEnum` | - |
| `Observaciones` | `string?` | - |
| `FaseOrigenId` | `int?` | - |
| `ReglaClasificacionAplicada` | `string?` | - |
| `FechaRegistro` | `DateTime` | - |
| `FechaActualizacion` | `DateTime?` | - |
| `UsuarioRegistro` | `string?` | - |
| `UsuarioActualizacion` | `string?` | - |
| `Fase` | `Fase` | Sí (Navegación) |
| `Inscripcion` | `Inscripcion` | Sí (Navegación) |
| `Penalizaciones` | `ICollection<Penalizacion>` | Sí (Navegación) |

## DTO: `ResultadoFaseDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Id` | `int` | `Id` | **Coincide Exacto** | - |
| `FaseId` | `int` | `FaseId` | **Coincide Exacto** | - |
| `InscripcionId` | `int` | `InscripcionId` | **Coincide Exacto** | - |
| `ParticipanteId` | `int?` | `Id.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `NumeroCompetidor` | `string?` | `-` | *No coincide* | - |
| `ParticipanteNombre` | `string?` | `-` | *No coincide* | - |
| `ClubNombre` | `string?` | `-` | *No coincide* | - |
| `ClubSigla` | `string?` | `-` | *No coincide* | - |
| `Carril` | `int?` | `Carril` | **Coincide Exacto** | - |
| `EsCabezaDeSerie` | `bool` | `EsCabezaDeSerie` | **Coincide Exacto** | - |
| `Tripulantes` | `List<SportTrack_Sigdef.Controladores.Inscripcion.Dtos.InscripcionTripulanteDto>` | `-` | *No coincide* | - |
| `TiempoOficial` | `TimeSpan?` | `TiempoOficial` | **Coincide Exacto** | - |
| `Posicion` | `int?` | `Posicion` | **Coincide Exacto** | - |
| `Estado` | `string` | `Estado` | **Coincide Exacto** | Cambio de tipo: Enums.EstadoResultadoEnum -> string |