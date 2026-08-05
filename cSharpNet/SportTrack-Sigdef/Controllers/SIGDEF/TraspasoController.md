# TraspasoController.cs (SIGDEF)

## 1. Qué es

API completa del flujo de **traspasos de atletas entre clubes**: períodos, solicitudes, validaciones, aprobaciones/rechazos (origen y federación), cancelación, auditoría y export CSV.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Workflow HTTP | Varios POST de transición de estado |
| `[FromQuery]` filtros | estado, periodoId, limit, term, forzar |
| `File(...)` | Descarga CSV (`FileContentResult`) |
| `CreatedAtAction` | Alta de período/solicitud |
| Nullable DTO return | `PeriodoTraspasoDto?` |
| Query `forzar` | Aprobación forzada por federación |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, Federaciones (`ITraspasoService`), DTOs Traspaso, etc.

## 4. Detalle de métodos

### Períodos

| Método | Ruta | Acción |
|--------|------|--------|
| `GetPeriodos` | GET `periodos` | Lista |
| `GetPeriodoActivo` | GET `periodo-activo` | Actual |
| `CreatePeriodo` | POST `periodos` | Alta |
| `UpdatePeriodo` | PUT `periodos/{id}` | Update |

### Solicitudes y apoyo

| Método | Ruta | Acción |
|--------|------|--------|
| `GetSolicitudes` | GET | Filtro `estado` |
| `GetAuditoria` | GET `auditoria` | Trail |
| `ExportCsv` | GET `export/csv` | Archivo CSV |
| `BuscarAtletas` | GET `buscar-atletas?term=` | Typeahead |
| `GetSolicitud` | GET `{id}` | Detalle |
| `GetValidaciones` | GET `{id}/validaciones` | Reglas de negocio |
| `CrearSolicitud` | POST | Nueva solicitud |

### Transiciones

| Método | Ruta | Actor típico |
|--------|------|----------------|
| `AceptarOrigen` | POST `{id}/aceptar-origen` | Club origen |
| `RechazarOrigen` | POST `{id}/rechazar-origen` | Club origen + motivo |
| `Aprobar` | POST `{id}/aprobar?forzar=` | Federación |
| `Rechazar` | POST `{id}/rechazar` | Federación + motivo |
| `Cancelar` | POST `{id}/cancelar` | Cancelación |

## 5. Notas de estudio

1. Modelo EF: `PeriodosTraspaso` + `SolicitudesTraspaso` con enum converter de estado e índices.
2. Es un excelente ejemplo de **máquina de estados** expuesta como endpoints.
3. `File(bytes, contentType, fileName)` enseña descargas en APIs.
4. Orden de rutas: literales (`periodos`, `export/csv`) antes de `{id}` evitan colisiones.
