# Fase 0 — Inventario y smoke baseline

Fecha: 2026-07-14  
Repos: `SportTrack-Sigdef` (API) + impacto `SportTrack-Front` / `FrontSigdef`  
Estado: **documentación lista** — sin cambios de código ni de Render en esta fase.

---

## Premisa técnica actual

| Ítem | Valor |
|------|--------|
| `FallbackPolicy` / auth por defecto | **No.** Sin `[Authorize]` ⇒ anónimo |
| Hub | `MapHub<TimingHub>("/hubs/timing")` — sin auth |
| CORS | Lee `AllowedOrigins` pero aplica `SetIsOriginAllowed(_ => true)` + credentials |
| JWT `TokenKey` | Env opcional; fallback hardcodeado en `Program.cs` y `TokenService.cs` |
| Front Live | `/resultados/:id` público; `timingSignalRService.connect()` sin usuario |
| Front jueces | `ProtectedRoute` + roles + `requiereControlesLive` |

**Render en Fase 0:** no hace falta tocar nada.  
**Aviso Render:** recién al implementar Fase 1 (CORS estricto + exigir `TokenKey`). Ver sección final.

---

## 1. Lista blanca objetivo (lo que DEBE seguir público)

Esto es lo que las Fases 1–2 deben preservar. Todo lo demás pasa a JWT + roles.

### 1.1 Auth / ops

| Método | Ruta | Motivo |
|--------|------|--------|
| POST | `/api/Auth/login` | Login |
| POST | `/api/Auth/logout` | Cerrar cookie |
| GET | `/api/Health`, `/api/Health/db` | Monitoring |
| POST | `/api/Support/frontend-error` | Telemetría (ya AllowAnonymous) |
| POST | `/api/Notificacion/webhook` | Mercado Pago (público **con firma** en Fase 3; hoy sin firma) |

### 1.2 Live — lectura HTTP

| Método | Ruta | Controller |
|--------|------|------------|
| GET | `/api/Eventos/proximos` | EventosController `[AllowAnonymous]` |
| GET | `/api/Eventos/{id}` | EventosController `[AllowAnonymous]` |
| GET | `/api/Eventos/{id}/fases` | EventosController `[AllowAnonymous]` |
| GET | `/api/Eventos/{id}/pruebas` | EventosController `[AllowAnonymous]` |
| GET | `/api/Fases/all-by-evento/{eventoId}` | Hoy abierto; **candidato** a seguir público para Live si lo usa el front |
| GET | `/api/Fases/EventoPrueba/{eventoPruebaId}` | Lectura; decidir en Fase 1 si Live lo necesita anónimo |
| GET | `/api/Resultados/Fase/{faseId}` | Lectura de grilla; Live/polling |

> Antes de Fase 1: verificar en red del browser Live qué GET exactos pegar; marcar solo esos como anónimos.

### 1.3 SignalR — espectador

| Acción | Path / método | Público |
|--------|---------------|---------|
| Negociar conexión | `GET/POST /hubs/timing` | Sí |
| `JoinRaceGroup` | hub | Sí |
| `JoinEventGroup` | hub | Sí |
| `LeaveRaceGroup` | hub | Sí |
| `OnDisconnectedAsync` | hub | Sí (runtime) |
| `GetServerTime` | hub | Sí |
| Recibir broadcasts | `RaceStarted`, `TimeReceived`, `globalTimeReceived`, etc. | Sí (server → client) |

### 1.4 SignalR — NO públicos (cerrar en Fase 1)

| Método hub | Impacto si queda abierto |
|------------|--------------------------|
| `RequestStartRace` | Larga / cambia estado en DB |
| `RequestResetRace` | Resetea fase |
| `SendTime` | Falsifica tiempos en vivo |
| `FinishRace` | Broadcast fin |
| `RecordLap` | Broadcast vueltas |
| `UpdateResultStatus` | DNS/DNF/DSQ en DB |
| `RequestPaymentStatusChange` | Spam broadcast pagos |

Roles previstos para escrituras: `Largador`, `Cronometrista`, `JuezControl`, `Admin`, `SuperAdmin` (ajustar por método si hace falta más fino).

---

## 2. Inventario actual — superficie abierta (riesgo)

### 2.1 TimingHub — todos los métodos invocables sin JWT

Ver [`TimingHub.cs`](../../SportTrack-Sigdef.Controladores/Hubs/TimingHub.cs).

### 2.2 Competencia REST — controllers sin `[Authorize]` de clase

| Controller | Base | Escritura abierta (crítica) |
|------------|------|-----------------------------|
| **FasesController** | `api/Fases` | POST `BatchUpdate`, `Generar/*`, `GenerarManual/*`, `Promover/*`, `{id}/Iniciar`, `{id}/Finalizar`, `{id}/Reiniciar`, `{id}/EnviarARevision` · DELETE `{id}` |
| **ResultadosController** | `api/Resultados` | PUT `BatchUpdate` |
| **AuthController** | `api/Auth` | POST `register` (rol arbitrario); login/logout OK anónimos |
| **NotificacionController** | `api/Notificacion` | POST `webhook` sin firma |
| SIGDEF CRUD | `api/Atleta`, `Persona`, `Usuario`, `Club`, `Tutor`, … | GET/POST/PUT/DELETE anónimos |
| Catálogos | `api/Botes`, `Categorias`, `Distancias` | CRUD anónimo |
| **PagoTransaccionController** | `api/PagoTransaccion` | Preferencias / estados sin auth |
| **EventoPruebaController** | `api/legacy/eventos/...` | CRUD legacy abierto |

Lecturas GET de `Clubes`, `Participantes`, `Inscripciones`, `Federaciones` también anónimas (mutaciones de Clubes/Participantes/Inscripciones/Federaciones sí tienen `[Authorize]` en métodos).

### 2.3 Controllers con `[Authorize]` + excepciones anónimas

| Controller | Anónimos explícitos |
|------------|---------------------|
| EventosController | `proximos`, `{id}`, `{id}/fases`, `{id}/pruebas` |
| SaaSController | GET `debug-me` (**quitar o proteger en Fase 1/2**) |
| SupportController | POST `frontend-error` |
| HealthController | clase `[AllowAnonymous]` |

### 2.4 Auth — detalle

| Ruta | Hoy |
|------|-----|
| POST `login` / `logout` | Anónimo (OK) |
| POST `register` | Anónimo (**cerrar Fase 1**) |
| GET `usuarios`, `me` | `[Authorize]` |
| PUT `usuarios/{id}/password`, `perfil` | `[Authorize]` pero **sin check de ownership** (IDOR → Fase 1) |
| PATCH `toggle-activo` | Admin, SuperAdmin |

### 2.5 Front que ya manda JWT (no se rompe si se protege escritura)

| App | Evidencia |
|-----|-----------|
| SportTrack-Front jueces | ProtectedRoute + `accessTokenFactory` en TimingSignalR |
| SportTrack-Front admin / control | Bearer vía interceptor |
| FrontSigdef | Token en `localStorage` + `VITE_API_URL` a Render |

---

## 3. Orígenes / env conocidos (referencia; no tocar aún)

| Variable | Dónde | Valores conocidos |
|----------|-------|-------------------|
| API | Render | `https://sporttrack-sigdef.onrender.com` |
| SportTrack-Front | Vercel | `https://sporttrack-fec.vercel.app` |
| FrontSigdef | Vercel | `https://sigdef.vercel.app` |
| Landing ops | Vercel | `https://oficialsporttrack.vercel.app` (verificar si llama API) |
| Dev | local | `http://localhost:5173`, `http://localhost:3000` |
| `AllowedOrigins` | Render (opcional hoy) | Leído pero **ignorado** por CORS permisivo |
| `TokenKey` | Render (opcional hoy) | Si falta → fallback hardcodeado |

---

## 4. Smoke baseline (foto “funciona hoy”)

Marcá cada ítem **antes** de Fase 1. Entornos: prod Vercel + API Render (o staging equivalente).

### 4.1 Público / Live

| # | Prueba | OK? | Notas |
|---|--------|-----|-------|
| L1 | Abrir `/resultados/{idEvento}` **sin login** | ☐ | Carga evento / fases |
| L2 | Tras largar (otra sesión), Live recibe actualización (EN VIVO / estado) | ☐ | SignalR o polling |
| L3 | Consola Network: negotiate a `/hubs/timing` (NO `/hubs/results`) sin 401/404 | ☐ | Vite proxea `/hubs` → Render. 404 en `/hubs/results` = cliente legacy (corregido en front) |
| L4 | Login SportTrack-Front OK | ☐ | |

### 4.2 Jueces (sesiones logueadas)

| # | Prueba | Rol | OK? | Notas |
|---|--------|-----|-----|-------|
| J1 | `/jueces/largador` — largar serie | Largador/Admin | ☐ | |
| J2 | `/jueces/llegada` — cortar tiempos; grilla actualiza | Cronometrista/Admin | ☐ | |
| J3 | `/juez-control` — ver sync, seguir timer, ver tiempos en vivo | JuezControl/Admin | ☐ | |
| J4 | Reiniciar fase desde UI autorizada | Admin/Largador | ☐ | |
| J5 | `/jueces/carga-manual` — guardar tiempos | Admin | ☐ | |

### 4.3 Admin / SIGDEF

| # | Prueba | App | OK? | Notas |
|---|--------|-----|-----|-------|
| A1 | Login FrontSigdef | FrontSigdef | ☐ | |
| A2 | Alta usuario desde panel (GestionLogins) | SportTrack o SIGDEF | ☐ | Usa register autenticado en práctica |
| A3 | Flujo típico eventos / inscripciones con sesión | — | ☐ | |

### 4.4 Negativo (hoy DEBERÍA pasar = vulnerabilidad; post Fase 1 DEBE fallar)

| # | Prueba | Hoy (esperado baseline) | Post Fase 1 (objetivo) |
|---|--------|-------------------------|------------------------|
| N1 | Script/Postman SignalR `RequestStartRace` sin token | ☐ Funciona (malo) | Debe fallar |
| N2 | `POST /api/Fases/{id}/Iniciar` sin token | ☐ 200 (malo) | 401 |
| N3 | `PUT /api/Resultados/BatchUpdate` sin token | ☐ 200 (malo) | 401 |
| N4 | `POST /api/Auth/register` anónimo creando Admin | ☐ Posible (malo) | Bloqueado |

Completar L/J/A como “verde actual”. Los N son evidencia de riesgo; no “arreglar” en Fase 0.

---

## 5. Criterio de cierre Fase 0

- [x] Lista blanca objetivo documentada  
- [x] Inventario hub + Fases/Resultados/Auth/SIGDEF  
- [x] Smoke checklist listo para ejecutar  
- [ ] Smoke L1–L4, J1–J5, A1–A3 ejecutado y anotado (dueño producto / QA)  
- [ ] Confirmado en Network del Live qué GET usa (ajustar §1.2 si hace falta)

---

## 6. Aviso Render — cuándo actuar

**Ahora (Fase 0):** no cambiar variables.

**Antes del deploy de Fase 1 (cuando digamos “hora de Render”):**

1. Setear `TokenKey` (secreto largo, único) en el Web Service → redeploy → re-login de usuarios.  
2. Setear `AllowedOrigins` con dominios reales (comma-separated), ej.:

```text
https://sporttrack-fec.vercel.app,https://sigdef.vercel.app,https://oficialsporttrack.vercel.app
```

3. Redeploy API.  
4. Recién ahí merge/deploy del código que quita fallback JWT y CORS permisivo.

Sin ese orden: riesgo de API que no arranca o fronts bloqueados por CORS.

---

## 7. Siguiente paso

**Fase 1** (código): auth en métodos de escritura del hub + mutaciones `Fases`/`Resultados` + cerrar register/IDOR + (tras Render) CORS + TokenKey sin fallback.

Ver [Plan-Endurecimiento-Seguridad.md](./Plan-Endurecimiento-Seguridad.md).
