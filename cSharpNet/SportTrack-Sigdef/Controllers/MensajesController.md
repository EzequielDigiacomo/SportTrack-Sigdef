# MensajesController.cs

## 1. Qué es

API de **mensajería privada** (hilos, respuestas, campañas masivas, no leídos). Roles `SuperAdmin,Admin,Club`. Distingue sistema origen vía header `X-Client-App`.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| `[Route("api/mensajes")]` | Ruta fija (no `[controller]`) |
| Roles en clase + override | Campañas solo Admin/SuperAdmin |
| `CreatedAtAction` | 201 con Location |
| Header custom | `X-Client-App` |
| Excepción de negocio | `BadRequestException` |
| `MensajeriaSistemaOrigen.Normalizar` | Validar origen |

## 3. Namespace / usings

- `SportTrack_Sigdef.Controllers`
- Authorization, Mvc, Exceptions, Mensajes + Dtos, Entidades

## 4. Detalle de métodos

| Método | Ruta | Notas |
|--------|------|-------|
| `GetHilos` | GET `hilos` | Query opcional `campanaId` |
| `GetHilo` | GET `hilos/{id}` | Detalle |
| `CrearHilo` | POST `hilos` | 201 Created |
| `EnviarMasivo` | POST `hilos/masivo` | Solo Admin/SuperAdmin |
| `ResponderHilo` | POST `hilos/{id}/responder` | |
| `MarcarLeido` | PATCH `hilos/{id}/leer` | |
| `GetNoLeidosCount` | GET `no-leidos/count` | Badge UI |
| `GetCampanas` / `GetCampana` | GET `campanas`… | Solo Admin/SuperAdmin |

### `ResolveSistemaOrigen` (privado)

Lee header; normaliza; si inválido → `BadRequestException` (capturada por ExceptionMiddleware → 400).

## 5. Notas de estudio

1. Multi-producto (SportTrack/SIGDEF) en la misma tabla con `SistemaOrigen`.
2. Constraint de ruta `{id:int}` evita ambigüedades.
3. Modelo EF en esquema `comunicacion` (ver DbContext).
