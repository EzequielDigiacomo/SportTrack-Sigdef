# TraspasoDtos.cs

## Qué es este archivo

Conjunto de **DTOs** (Data Transfer Objects) para el módulo de traspasos: lectura/alta/edición de periodos, solicitudes, validaciones, búsqueda de atletas y auditoría. No son entidades EF; son contratos de API.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Varias clases en un archivo | Organización por feature. |
| DTO vs entidad | Sin navegaciones EF; datos planos. |
| Patrón Create/Update/Read | Clases distintas por operación. |
| `List<T>` inicializada | `Items = new()`. |
| Props nullable en Update | Patch parcial (`DateTime?`, `bool?`). |
| Namespace de DTOs | `...DTOs.Traspaso`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.Traspaso`
- `SportTrack_Sigdef.Entidades.Enums` (importado; los DTOs exponen estado como `string` en lectura).
- `System.Collections.Generic` — listas.

## Clases y miembros

### PeriodoTraspasoDto
Lectura: `Id`, `IdFederacion`, fechas, `Activo`, `Observaciones`, **`EsVigente`** (calculado en servicio, no vive en la entidad).

### PeriodoTraspasoCreateDto
Alta: fechas, `Activo` (default true), observaciones. Sin Id.

### PeriodoTraspasoUpdateDto
Edición parcial: todas las props nullable excepto la intención de “enviar solo lo que cambia”.

### SolicitudTraspasoDto
Vista enriquecida: ids + **nombres** de participante/clubes, `Estado` como `string`, motivos y timestamps del workflow.

### SolicitudTraspasoCreateDto
Input mínimo: `ParticipanteId`, `IdClubDestino`, `MotivoSolicitud?`.

### TraspasoMotivoDto
Wrapper de un motivo (`string?`) para endpoints de rechazo/cancelación.

### TraspasoValidacionItemDto / TraspasoValidacionDto
Resultado de reglas de negocio: códigos, descripción, `Ok`, `Bloqueante`, y agregación `PuedeAprobar` + lista de ítems.

### AtletaTraspasoBusquedaDto
Resultado de búsqueda para elegir atleta a trasladar.

### TraspasoAuditoriaDto
Proyección liviana de auditoría relacionada al traspaso.

## Relaciones

Espejan `PeriodoTraspaso`, `SolicitudTraspaso`, `Participante`, `Club`, `Auditoria` **sin** referenciar navigation properties.

## Notas de estudio

1. Un DTO por caso de uso evita over-posting y filtrado manual de campos.
2. `EsVigente` y nombres denormalizados son trabajo de la capa de aplicación/mapping (AutoMapper, manual, etc.).
3. Update con nullables = semántica “null significa no cambiar” (hay que documentarlo en la API).
4. Validación como DTO enseña a devolver **por qué** no se puede aprobar, no solo un bool.
