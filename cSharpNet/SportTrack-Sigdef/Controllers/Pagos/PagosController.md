# PagosController.cs

## 1. Qué es

Gestión de **pagos/recibos** SportTrack: historial con scope, registro, toggles de estado de pago (club/atleta/inscripción), eliminación simple y bulk.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| Scope por rol + claims | SuperAdmin puede query `idFederacion` |
| Reescritura local de `role` | SuperAdmin + filtro → trata como Admin |
| `[FromBody] bool` | Toggles |
| Bulk delete | `List<int>` en body |
| Username desde claims | Auditoría de quién registró/eliminó |

## 3. Namespace / usings

- `SportTrack_Sigdef.Controllers.Pagos`
- Authorization, Mvc, Pago + Dtos, Claims, Tasks

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetHistorial` | GET `historial` | Historial scoped |
| `RegistrarPago` | POST `registrar` | Alta con username |
| `ToggleClubPago` | PUT `clubes/{id}/toggle` | Al día / no |
| `SolicitarPago` | PUT `clubes/{id}/solicitar-pago` | Pendiente |
| `ToggleAtletaPago` | PUT `atletas/{id}/toggle` | |
| `ToggleInscripcionPago` | PUT `inscripciones/{id}/toggle` | |
| `EliminarPago` | DELETE `{id}` | |
| `EliminarPagos` | DELETE `bulk` | Masivo |

## 5. Notas de estudio

1. Distinguí pagos de regata (`Pagos` en esquema `regatas`) de transacciones SIGDEF/MercadoPago (otro controller en ensamblado Controladores).
2. Body `bool` es simple pero poco autoexplicativo en Swagger; DTOs nombrados suelen ser más claros.
3. Claims `FederacionId`/`ClubId` definen el universo visible.
