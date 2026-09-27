# 04 · Manual de Uso de la API

**Última actualización: 2026-09-27**
**Audiencia:** integradores frontend, administradores y personal de soporte.

---

## Índice

1. [Convenciones](#1-convenciones)
2. [Autenticación](#2-autenticación)
3. [Consumo por caso de uso](#3-consumo-por-caso-de-uso)
4. [Ejemplos (curl / PowerShell)](#4-ejemplos-curl--powershell)
5. [Swagger en desarrollo](#5-swagger-en-desarrollo)
6. [Manejo de errores](#6-manejo-de-errores)
7. [Uso para soporte](#7-uso-para-soporte)
8. [FAQ](#8-faq)

---

## 1. Convenciones

| Aspecto | Valor |
|---------|-------|
| **Base URL local** | `http://localhost:5012` / `https://localhost:7232` |
| **Base URL producción** | Dominio del servicio en Render (p. ej. `https://<servicio>.onrender.com`) |
| **Prefijo** | Rutas bajo `api/**` (el hub SignalR es `/hubs/timing`) |
| **Formato** | JSON (`application/json`); uploads `multipart/form-data` |
| **Auth** | `Authorization: Bearer <JWT>` (preferido) o cookie `X-Access-Token` |
| **Cabecera de origen** | `X-Client-App: sporttrack \| sigdef` (aislamiento de mensajería/logins) |
| **Ciclos** | Fechas/horas en UTC; el backend convierte `DateTime` a UTC globalmente |
| **Rate limit** | `auth` = 20 req/min/IP · `live` = 120 req/min/IP |

---

## 2. Autenticación

### 2.1 Login y token

```http
POST /api/Auth/login
Content-Type: application/json
X-Client-App: sigdef

{ "username": "admin", "password": "admin123" }
```

Respuesta `200`: body con el JWT y cabecera `Set-Cookie: X-Access-Token=...`.

### 2.2 Resolución del token (orden real)

1. `Authorization: Bearer <token>`
2. Query string `?access_token=<token>` (para SignalR)
3. Cookie `X-Access-Token`

### 2.3 Logout y reset

```http
POST /api/Auth/logout
```

```http
POST /api/Auth/solicitar-reset-password
{ "username": "..." }
```

> La solicitud de reset devuelve siempre una **respuesta genérica** (no revela si el usuario existe).

### 2.4 Endpoints de identidad

| Método | Ruta | Rol | Descripción |
|--------|------|-----|-------------|
| POST | `api/Auth/login` | Anónimo (rate `auth`) | Emite JWT + cookie |
| POST | `api/Auth/logout` | Anónimo | Limpia cookie |
| POST | `api/Auth/solicitar-reset-password` | Anónimo (rate `auth`) | Reset genérico |
| POST | `api/Auth/register` | Admins (rate `auth`) | Alta de usuario |
| GET | `api/Auth/usuarios` | Autenticado | Lista usuarios del ámbito |
| PUT | `api/Auth/usuarios/{id}/password` | Autenticado | Cambiar contraseña |
| PUT | `api/Auth/usuarios/{id}/perfil` | Autenticado | Actualizar perfil |
| PATCH | `api/Auth/usuarios/{id}/toggle-activo` | Admin, SuperAdmin, soporte | Activar/desactivar |
| DELETE | `api/Auth/usuarios/{id}` | Admin, SuperAdmin, soporte | Baja de login |
| GET | `api/Auth/me` | Autenticado | Perfil propio |

---

## 3. Consumo por caso de uso

### 3.1 Live público (sin token)

| Método | Ruta | Rol | Parámetros |
|--------|------|-----|-----------|
| GET | `api/Eventos/proximos` | Anónimo | — |
| GET | `api/Eventos/{id}` | Anónimo (rate `live`) | `id` |
| GET | `api/Eventos/{id}/fases` | Anónimo (rate `live`) | `id` |
| GET | `api/Eventos/{id}/pruebas` | Anónimo (rate `live`) | `id` |
| GET | `api/Resultados/Fase/{faseId}` | Anónimo (rate `live`, cache 30 s) | `faseId` |
| GET | `api/Fases/EventoPrueba/{eventoPruebaId}` | Anónimo (rate `live`) | `eventoPruebaId` |
| GET | `api/Fases/all-by-evento/{eventoId}` | Anónimo (rate `live`) | `eventoId` |
| GET | `api/Health` · `api/Health/db` | Anónimo | — |
| GET | `api/SaaS/planes` | Anónimo | — |

### 3.2 Eventos y programación

| Método | Ruta | Rol | Parámetros |
|--------|------|-----|-----------|
| GET | `api/Eventos` | Autenticado | filtros |
| GET | `api/Eventos/debug` | Autenticado | — |
| POST | `api/Eventos` | Autenticado | body evento |
| PUT | `api/Eventos/{id}` | Autenticado | `id`, body |
| DELETE | `api/Eventos/{id}` | Autenticado | `id` |
| POST | `api/Eventos/{id}/pruebas` | Autenticado | `id`, body prueba |
| POST | `api/Eventos/{id}/pruebas/largada` | Autenticado | `id`, body largada |
| PUT | `api/Eventos/pruebas/{id}` | Autenticado | `id`, body |
| DELETE | `api/Eventos/pruebas/{id}` | Autenticado | `id` |

### 3.3 Fases y progresión ICF

| Método | Ruta | Rol | Parámetros |
|--------|------|-----|-----------|
| POST | `api/Fases/Generar/{eventoPruebaId}` | CompetitionOperators | `eventoPruebaId` |
| POST | `api/Fases/GenerarManual/{eventoPruebaId}` | CompetitionOperators | `eventoPruebaId`, body |
| POST | `api/Fases/GenerarLargadaMaraton` | CompetitionOperators | body |
| POST | `api/Fases/Promover/{eventoPruebaId}` | CompetitionOperators | `eventoPruebaId` |
| POST | `api/Fases/BatchUpdate` | CompetitionOperators | body (lista) |
| GET | `api/Fases/ProgresionAudit/{eventoPruebaId}` | CompetitionOperators | `eventoPruebaId` |
| POST | `api/Fases/{id}/Iniciar` | CompetitionOperators | `id`, body (hora) |
| POST | `api/Fases/{id}/Finalizar` | CompetitionOperators | `id` |
| POST | `api/Fases/{id}/Reiniciar` | CompetitionOperators | `id`, `motivo` (≥5) |
| POST | `api/Fases/{id}/EnviarARevision` | CompetitionOperators | `id` |
| DELETE | `api/Fases/{id}` | CompetitionOperators | `id` |

### 3.4 Resultados y cronometraje

| Método | Ruta | Rol | Parámetros |
|--------|------|-----|-----------|
| GET | `api/Resultados/Fase/{faseId}` | Anónimo (rate `live`) | `faseId` |
| PUT | `api/Resultados/BatchUpdate` | CompetitionOperators | body (lista de resultados) |
| POST | `api/timing-outbox` | CompetitionOperators | body envío |
| GET | `api/timing-outbox/pending` | CompetitionOperators | — |
| POST | `api/timing-outbox/flush` | CompetitionOperators | — |
| POST | `api/timing-outbox/{faseId}/commit` | CompetitionOperators | `faseId` |
| DELETE | `api/timing-outbox/{faseId}` | CompetitionOperators | `faseId` |

### 3.5 Inscripciones, participantes y catálogos

| Método | Ruta | Rol |
|--------|------|-----|
| GET/POST/PUT/DELETE | `api/Inscripciones`, `api/Inscripciones/{id}`, `api/Inscripciones/registro`, `api/Inscripciones/evento-prueba/{id}`, `api/Inscripciones/evento/{id}/club/{id}` | Autenticado |
| PATCH | `api/Inscripciones/{id}/toggle-seeding` | Autenticado |
| CRUD | `api/Participantes`, `api/Participantes/club/{clubId}` | Autenticado |
| CRUD | `api/Clubes`, `api/Botes`, `api/Categorias`, `api/Distancias` | Autenticado |
| GET/POST/PUT/DELETE | `api/legacy/eventos/{idEvento}/pruebas` | Autenticado |

### 3.6 SIGDEF

| Método | Ruta | Rol | Notas |
|--------|------|-----|-------|
| GET | `api/Atleta`, `api/Atleta/paged`, `api/Atleta/{id}`, `api/Atleta/club/{clubId}` | Autenticado | Listados y filtros |
| POST | `api/Atleta`, `api/Atleta/full` | Autenticado | Alta simple / alta completa |
| POST | `api/Atleta/{id}/liberar` | Autenticado | Pasa a Agente Libre |
| CRUD | `api/Club`, `api/DelegadoClub`, `api/Entrenador`, `api/Tutor`, `api/Persona`, `api/Rol`, `api/Usuario`, `api/AtletaTutor`, `api/Inscripcion` | Autenticado | CRUD estándar |
| GET | `api/Traspaso/periodos`, `api/Traspaso/periodo-activo`, `api/Traspaso`, `api/Traspaso/{id}`, `api/Traspaso/{id}/validaciones`, `api/Traspaso/buscar-atletas`, `api/Traspaso/auditoria`, `api/Traspaso/export/csv` | Autenticado | Consultas |
| POST | `api/Traspaso`, `api/Traspaso/periodos`, `api/Traspaso/{id}/aceptar-origen`, `.../rechazar-origen`, `.../aprobar`, `.../rechazar`, `.../cancelar` | Autenticado | Flujo de traspaso |
| PUT | `api/Traspaso/periodos/{id}` | Autenticado | Editar periodo |
| POST | `api/Documentacion/upload` (multipart, ≤6 MB) | Autenticado | Cloudinary |
| GET | `api/Documentacion/persona/{id}` | Autenticado | — |
| DELETE | `api/Documentacion/{id}` | Autenticado | — |

### 3.7 Pagos y MercadoPago

| Método | Ruta | Rol |
|--------|------|-----|
| GET | `api/Pagos/historial` | Autenticado |
| POST | `api/Pagos/registrar` | Autenticado |
| PUT | `api/Pagos/clubes/{id}/toggle`, `api/Pagos/clubes/{id}/solicitar-pago` | Autenticado |
| PUT | `api/Pagos/atletas/{id}/toggle`, `api/Pagos/inscripciones/{id}/toggle` | Autenticado |
| DELETE | `api/Pagos/{id:int}`, `api/Pagos/bulk` | Autenticado |
| POST | `api/PagoTransaccion/preferencia` + CRUD | Autenticado |
| POST | `api/Notificacion/webhook` | Anónimo ⚠️ sin validación de firma |

### 3.8 Mensajería

| Método | Ruta | Rol |
|--------|------|-----|
| GET | `api/mensajes/hilos`, `api/mensajes/hilos/{id}`, `api/mensajes/no-leidos/count`, `api/mensajes/campanas`, `api/mensajes/campanas/{id}` | SuperAdmin, Admin, Club |
| POST | `api/mensajes/hilos`, `api/mensajes/hilos/{id}/responder` | SuperAdmin, Admin, Club |
| PATCH | `api/mensajes/hilos/{id}/leer` | SuperAdmin, Admin, Club |
| POST | `api/mensajes/hilos/masivo` | SuperAdmin, Admin |

### 3.9 SaaS

| Método | Ruta | Rol |
|--------|------|-----|
| GET | `api/SaaS/planes` | Anónimo |
| GET | `api/SaaS/debug-me` | SuperAdmin, soporte |
| PUT | `api/SaaS/planes/{id}` | SuperAdmin |
| POST | `api/SaaS/asignar-plan` | SuperAdmin, Admin |
| GET | `api/SaaS/clubes-status` | SuperAdmin, Admin, soporte |
| PATCH | `api/SaaS/clubes/{id}/toggle-activo` | SuperAdmin, Admin, soporte |
| POST | `api/SaaS/create-federacion` | SuperAdmin |
| GET | `api/SaaS/global-metrics` | SuperAdmin, soporte |

### 3.10 Auditoría, soporte, backups, audience

| Método | Ruta | Rol |
|--------|------|-----|
| GET | `api/Auditoria`, `api/Auditoria/por-eventos` | Autenticado |
| POST | `api/Auditoria/client-action` | Autenticado |
| DELETE | `api/Auditoria/por-evento/{eventoId}/sin-problemas`, `api/Auditoria/{id}` | Autenticado |
| GET | `api/Diagnostic/check-eventos`, `api/Diagnostic/search/{query}` | Admin, SuperAdmin, soporte |
| GET | `api/Support/por-eventos`, `api/Support/timing-outbox`, `api/Support/logs` | Autenticado |
| POST | `api/Support/client-action`, `api/Support/timing-outbox/{faseId}/commit`, `api/Support/frontend-error` | Autenticado / Anónimo |
| DELETE | `api/Support/timing-outbox/{id}`, `api/Support/logs/clear` | Autenticado / SuperAdmin |
| GET | `api/Backup/download?scope=full\|federacion&idFederacion=N`, `api/Backup/history` | SuperAdmin, soporte |
| GET | `api/Audience/live`, `api/Audience/peaks`, `api/Audience/capacity` | SuperAdmin, soporte |
| PUT | `api/Audience/capacity` | SuperAdmin, soporte |

### 3.11 SignalR (tiempo real)

| Elemento | Valor |
|----------|-------|
| **Hub** | `/hubs/timing` |
| **Grupos** | `race_{faseId}`, `event_{eventoId}`, `operators`, `user_{username}`, `fed_{federacionId}` |
| **Métodos AllowAnonymous** | `JoinRaceGroup`, `JoinEventGroup`, `LeaveRaceGroup`, `GetServerTime` |
| **Métodos CompetitionOperators** | `JoinOperatorsGroup`, `RequestStartRace`, `RequestResetRace`, `RecordLap`, `FinishRace`, `SendTime`, `UpdateResultStatus` |
| **Otros** | `JoinUserNotificationsGroup`, `JoinFederationNotificationsGroup`, `RequestPaymentStatusChange` |
| **Eventos** | `RaceStarted`, `RaceFinished`, `RaceReset`, `RaceInReview`, `globalRaceStarted`, `globalRaceOfficialized`, `globalRaceInReview`, `GlobalResultStatusUpdated`, `TimeReceived`, `globalTimeReceived`, `LapRecorded`, `ResultadoActualizado`, `RacePresenceUpdated`, `EventPresenceUpdated`, `newMessageReceived`, `newEventCreated`, `paymentStatusChangeRequested` |

Conexión con token: `?access_token=<JWT>`.

---

## 4. Ejemplos (curl / PowerShell)

### 4.1 Login (curl)

```bash
curl -X POST http://localhost:5012/api/Auth/login \
  -H "Content-Type: application/json" \
  -H "X-Client-App: sigdef" \
  -d '{"username":"admin","password":"admin123"}' -i
```

### 4.2 Login (PowerShell) y guardado del token

```powershell
$body = @{ username = "admin"; password = "admin123" } | ConvertTo-Json
$resp = Invoke-RestMethod -Method Post -Uri "http://localhost:5012/api/Auth/login" `
  -ContentType "application/json" -Headers @{ "X-Client-App" = "sigdef" } -Body $body
$token = $resp.token
$headers = @{ Authorization = "Bearer $token"; "X-Client-App" = "sigdef" }
```

> El nombre exacto del campo del token depende del DTO de login (`AuthService`); inspeccioná el body de respuesta con Swagger o en el frontend.

### 4.3 Consultar Live (anónimo)

```bash
curl http://localhost:5012/api/Resultados/Fase/123
```

### 4.4 Generar fases

```powershell
Invoke-RestMethod -Method Post -Uri "http://localhost:5012/api/Fases/Generar/45" -Headers $headers
```

### 4.5 Enviar resultados en lote

```bash
curl -X PUT http://localhost:5012/api/Resultados/BatchUpdate \
  -H "Authorization: Bearer $TOKEN" -H "Content-Type: application/json" \
  -d '[{"resultadoId":1,"tiempo":"00:01:32.450","posicion":1,"estado":"Finalizado"}]'
```

### 4.6 Encolar/enviar timing outbox

```bash
curl -X POST http://localhost:5012/api/timing-outbox \
  -H "Authorization: Bearer $TOKEN" -H "Content-Type: application/json" \
  -d '{"faseId":123,"resultadoId":9,"tiempo":"00:02:10.100"}'
```

### 4.7 Descargar backup (PowerShell)

```powershell
Invoke-WebRequest -Uri "https://<servicio>.onrender.com/api/Backup/download?scope=full" `
  -Headers $headers -OutFile "backup.sql"
```

---

## 5. Swagger en desarrollo

- Disponible **solo** con `ASPNETCORE_ENVIRONMENT=Development`.
- URL: `http://localhost:5012/swagger`
- Botón **Authorize** con esquema `Bearer`: pegar el JWT (sin la palabra `Bearer` según la configuración del esquema).
- En producción Swagger **no** se expone.

---

## 6. Manejo de errores

| Código | Significado | Causas típicas | Acción |
|--------|-------------|----------------|--------|
| **400** | Bad Request | Validación, carril ocupado, motivo de reseteo < 5, resultados incompletos | Revisar body/parámetros |
| **401** | Unauthorized | Sin token, token expirado, usuario inactivo/bloqueado, login inválido | Renovar login |
| **403** | Forbidden | Rol sin permiso, plan sin acceso, aislamiento de tenant | Verificar rol y plan |
| **404** | Not Found | Recurso inexistente | Verificar id |
| **429** | Too Many Requests | Rate limit `auth` (20/min) o `live` (120/min) | Reintentar con backoff |
| **500** | Internal Server Error | Excepción no controlada (se audita `ERROR_FATAL`, mensaje sanitizado) | Ver logs `/api/Support/logs` |

`ExceptionMiddleware` traduce `NotFound`/`Unauthorized`/`BadRequest`; las inesperadas se loguean, se auditan como `ERROR_FATAL` y devuelven mensaje sanitizado.

---

## 7. Uso para soporte

### 7.1 Timing outbox

| Acción | Endpoint |
|--------|----------|
| Ver pendientes de una fase/contexto | `GET api/Support/timing-outbox` o `GET api/timing-outbox/pending` |
| Commit manual de una fase | `POST api/Support/timing-outbox/{faseId}/commit` |
| Descartar una entrada | `DELETE api/Support/timing-outbox/{id}` |

### 7.2 Auditoría

| Acción | Endpoint |
|--------|----------|
| Consultar registros | `GET api/Auditoria` |
| Ver agrupado por eventos (últimos 2500 registros) | `GET api/Auditoria/por-eventos` |
| Registrar acción de cliente | `POST api/Auditoria/client-action` |
| Limpiar sin problemas de un evento | `DELETE api/Auditoria/por-evento/{eventoId}/sin-problemas` |

### 7.3 Logs

| Acción | Endpoint |
|--------|----------|
| Ver logs | `GET api/Support/logs` |
| Limpiar logs | `DELETE api/Support/logs/clear` (SuperAdmin) |
| Reportar error de frontend | `POST api/Support/frontend-error` (Anónimo) |

### 7.4 Métricas y audiencia

| Acción | Endpoint |
|--------|----------|
| Métricas globales | `GET api/SaaS/global-metrics` |
| Audiencia live / picos / capacidad | `GET api/Audience/live`, `/peaks`, `/capacity` |
| Ajustar capacidad | `PUT api/Audience/capacity` |
| Diagnóstico de eventos | `GET api/Diagnostic/check-eventos` |
| Búsqueda de diagnóstico | `GET api/Diagnostic/search/{query}` |

### 7.5 Backups

| Acción | Endpoint |
|--------|----------|
| Backup completo | `GET api/Backup/download?scope=full` |
| Backup por federación | `GET api/Backup/download?scope=federacion&idFederacion=N` |
| Historial | `GET api/Backup/history` |

---

## 8. FAQ

**¿Por qué recibo 401?**
Token ausente/expirado, usuario inactivo o bloqueado por pago/vencimiento, o credenciales inválidas. Tras 5 intentos fallidos el usuario queda `EstaActivo = false`. Verificá también la cabecera `X-Client-App`: si el sistema de origen no tiene acceso al plan, el login puede fallar.

**¿Por qué recibo 429?**
Superaste el rate limit. Los endpoints `auth` admiten 20 req/min/IP y los `live` 120 req/min/IP. Esperá ~1 minuto o reducí la frecuencia (usá el Live solo cuando haga falta).

**¿Cómo funciona el reset de contraseña?**
`POST api/Auth/solicitar-reset-password` devuelve siempre una respuesta genérica. El backend notifica automáticamente (mensajería) si el usuario existe.

**¿Cómo descargo un backup?**
Con rol SuperAdmin/soporte llamá a `GET api/Backup/download?scope=full`. Requiere `pg_dump` disponible en el entorno (en Docker viene `postgresql-client-18`).

**¿Cómo se comporta el Live sin login?**
Los endpoints marcados `[AllowAnonymous]` (resultados, fases, próximos) son públicos con rate limit `live` y cache corto. El hub SignalR se conecta anónimo para lectura, pero las escrituras exigen rol `CompetitionOperators`.

**¿Cuál es la diferencia entre cookie y Bearer?**
Ambos funcionan. Se recomienda `Authorization: Bearer` por ser cross-origin seguro; la cookie `X-Access-Token` se mantiene como compatibilidad y para escenarios de navegador.

---

## Pie de página

- ➡️ [05 · Manual Técnico](./05-Manual-Tecnico.md)
- ➡️ [06 · Manual de Instalación y Despliegue](./06-Manual-Instalacion-Despliegue.md)
- ⬅️ [03 · Diagramas de Flujo](./03-Diagramas-de-Flujo.md) · [Volver al índice del Pack](./README.md)
