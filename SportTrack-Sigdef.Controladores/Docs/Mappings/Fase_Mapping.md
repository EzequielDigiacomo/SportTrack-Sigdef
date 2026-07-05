# Mapeo de Clase: Fase

> **Descripción:** Fases o mangas de competición (Series, Semifinales, Finales).

## Entidad Dominio: `Fase`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `Id` | `int` | Sí (FK) |
| `EtapaId` | `int` | - |
| `NombreFase` | `string` | - |
| `NumeroFase` | `int` | - |
| `FechaHoraProgramada` | `DateTime?` | - |
| `Estado` | `string` | - |
| `FechaHoraInicioReal` | `DateTime?` | - |
| `FechaHoraFinReal` | `DateTime?` | - |
| `Etapa` | `Etapa` | Sí (Navegación) |
| `Resultados` | `ICollection<Resultado>` | Sí (Navegación) |

## DTO: `FaseDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Id` | `int` | `Id` | **Coincide Exacto** | - |
| `EtapaId` | `int` | `EtapaId` | **Coincide Exacto** | - |
| `EtapaNombre` | `string` | `Etapa.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `EventoPruebaId` | `int` | `Id.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `NombreFase` | `string` | `NombreFase` | **Coincide Exacto** | - |
| `NumeroFase` | `int` | `NumeroFase` | **Coincide Exacto** | - |
| `EtapaOrden` | `int` | `-` | *No coincide* | - |
| `FechaHoraProgramada` | `DateTime?` | `FechaHoraProgramada` | **Coincide Exacto** | - |
| `Estado` | `string` | `Estado` | **Coincide Exacto** | - |
| `Prueba` | `SportTrack_Sigdef.Controladores.Evento.Dtos.EventoPruebaDto?` | `-` | *No coincide* | - |
| `Resultados` | `List<ResultadoFaseDto>` | `Resultados` | **Coincide Exacto** | Cambio de tipo: ICollection<Resultado> -> List<ResultadoFaseDto> |