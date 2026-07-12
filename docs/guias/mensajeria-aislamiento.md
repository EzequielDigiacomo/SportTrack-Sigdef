# Mensajería — aislamiento por producto

Fecha: 2026-07-12

## Problema

SportTrack-Front y FrontSigdef comparten la misma API y BD. Sin aislamiento, los chats de un producto aparecerían en el otro.

## Solución

1. Header obligatorio en cada request autenticado a mensajería: **`X-Client-App`** = `sporttrack` \| `sigdef`.
2. Campo **`SistemaOrigen`** en `comunicacion.Hilos` y `comunicacion.CampanasEnvio`.
3. Al crear hilo/campaña se persiste el origen normalizado desde el header.
4. Listados, detalle, unread y campañas **filtran** por ese origen.
5. Migración `20260712190000_AddSistemaOrigenMensajeria`: datos existentes → `sporttrack`.

Código de referencia:

- `MensajeriaSistemaOrigen.cs`
- `MensajeService` / `MensajeRepository` / `MensajesController`

## Endpoints (prefijo `/api/mensajes`)

| Método | Ruta | Notas |
|--------|------|-------|
| GET | `/hilos` | Filtrado por origen + usuario participante |
| GET | `/hilos/{id}` | 404 si otro origen |
| POST | `/hilos` | Setea `SistemaOrigen` |
| POST | `/hilos/masivo` | Campaña + N hilos del mismo origen |
| POST | `/hilos/{id}/responder` | |
| PATCH | `/hilos/{id}/leer` | |
| GET | `/no-leidos/count` | Solo origen del header |
| GET | `/campanas` | Solo campañas del remitente + origen |
| GET | `/campanas/{id}` | |

## Fronts

| App | Constante |
|-----|-----------|
| FrontSigdef `api.js` | `CLIENT_APP = 'sigdef'` |
| SportTrack-Front `api.js` | `CLIENT_APP = 'sporttrack'` |

## QA

Matriz completa en FrontSigdef → `docs/criterios/mensajeria-aislamiento.md`.
