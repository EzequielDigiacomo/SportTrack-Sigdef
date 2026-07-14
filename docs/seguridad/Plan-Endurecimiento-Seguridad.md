# Plan de endurecimiento de seguridad (SIGDEF / SportTrack API)

Fecha: 2026-07-11  
Alcance: `SportTrack-Sigdef` (API) + impacto en FrontSigdef / SportTrack-Front  
Objetivo: cerrar superficie de ataque **sin romper Live público ni SignalR de espectadores**.

---

## Principio

Separar:

- **Lectura pública** → público Live (sin login)
- **Escritura crítica** → jueces / admin / federación / club (con JWT + roles)

Hoy el hub `/hubs/timing` está abierto y expone métodos sensibles (`RequestStartRace`, `RequestResetRace`, `SendTime`, etc.) sin auth. Eso se endurece; **escuchar** la carrera no.

---

## Estado actual (resumen)

| Área | Estado |
|------|--------|
| SQL injection | Bien mitigado (EF Core + LINQ, sin SQL crudo relevante) |
| Auth JWT | Existe, pero no hay política global de “autenticado por defecto” |
| Controllers | Varios CRUD / SIGDEF sin `[Authorize]` |
| CORS | Muy permisivo (`SetIsOriginAllowed(_ => true)` + credentials) |
| JWT secret | Fallback hardcodeado si falta config |
| Live / Eventos | Varios `GET` con `[AllowAnonymous]` (correcto para público) |
| SignalR `TimingHub` | Conexión + Join públicos; también acciones de juez sin auth (riesgo) |

---

## Lista blanca pública (no romper)

Mantener anónimo de forma **explícita**:

1. **Live / lectura de eventos**
   - `GET` eventos / fases / próximos ya marcados `[AllowAnonymous]`
2. **SignalR espectador**
   - Conectar a `/hubs/timing`
   - `JoinEventGroup` / `JoinRaceGroup` / `Leave*`
   - `GetServerTime`
   - Recibir eventos del servidor (broadcast)
3. **Health / planes públicos** (si aplica al producto)
4. Otros endpoints de marketing/soporte que hoy sean intencionalmente públicos

Todo lo demás: autenticado.

---

## Fases

### Fase 0 — Inventario (1–2 días) — HECHO (doc 2026-07-14)

Detalle ejecutable: [Fase-0-Inventario-Baseline.md](./Fase-0-Inventario-Baseline.md)

- Documentar endpoints públicos vs privados
- Confirmar pantallas Live y consolas juez en SportTrack-Front
- Checklist de smoke: Live sin login / juez con login / admin SIGDEF con login
- Pendiente operativo: ejecutar checklist L/J/A y anotar resultados antes de Fase 1

### Fase 1 — Escrituras críticas — HECHO (ver Fase-1-Escrituras-Auth.md)

Hub escrituras, Fases/Resultados, register/IDOR, CORS lista, TokenKey sin fallback en prod.

### Fase 2 — Auth por defecto + HTTPS — HECHO (ver Fase-2-Auth-Default.md)

1. `FallbackPolicy` RequireAuthenticatedUser
2. `[Authorize]` en CRUD SIGDEF / catálogos / legacy
3. HTTPS redirect + HSTS en prod
4. Hub: `MapHub.AllowAnonymous` + Join públicos; escrituras JWT (Fase 1)

### Fase 3 — Hardening — HECHO parcial (ver Fase-3-Hardening.md)

- Rate limit login/register
- Uploads MIME/extensión/tamaño
- Headers de seguridad HTTP
- **Webhook Mercado Pago: pospuesto** (no se usa aún)

### Fase 4 — Verificación (checklist)

Ver [Fase-4-Verificacion.md](./Fase-4-Verificacion.md).

### Fase 5 — Verificación

- [ ] Live sin login abre y recibe updates
- [ ] Espectador no puede invocar start/reset/sendTime
- [ ] Juez con token sí puede operar
- [ ] SIGDEF/admin sin token → 401 en mutaciones
- [ ] CORS OK desde dominios reales; bloqueado desde origen arbitrario
- [ ] Register / password change no permiten escalada

---

## ¿Afecta Live / SignalR?

| Escenario | ¿Se rompe? |
|-----------|------------|
| Público viendo Live | No, si Join + reads quedan anónimos |
| Updates server → cliente | No |
| Consolas juez (start / times) | Solo si hoy operan sin login; el fix es exigir JWT ahí |
| FrontSigdef admin | Debe usar token (ya lo hace en la práctica) |

---

## Orden de implementación recomendado

1. Lista blanca Live/SignalR  
2. Auth global + `[AllowAnonymous]` en esa lista  
3. Proteger métodos del hub de jueces  
4. Cerrar CRUDs y register  
5. CORS / JWT secret / HTTPS  
6. Rate limit, uploads, webhooks  

**Siguiente paso concreto sugerido:** Fase 1 + Fase 2 (auth global + hub espectador/juez), sin tocar aún todo el CRUD.

---

## Archivos clave

- `SportTrack-Sigdef/Program.cs` — auth, CORS, JWT, `MapHub<TimingHub>`
- `SportTrack-Sigdef.Controladores/Hubs/TimingHub.cs` — Join* vs acciones críticas
- `SportTrack-Sigdef/Controllers/Eventos/EventosController.cs` — `[AllowAnonymous]` Live
- Front: `SportTrack-Front/src/services/TimingSignalRService.js`
- Front admin: `FrontSigdef/src/services/api.js` (Bearer token)

---

## Notas

- SQLi no es el problema principal; el riesgo real es **autorización inconsistente**.
- No aplicar auth ciega al hub completo: rompería Live.
- Preferir `[Authorize]` en métodos críticos del hub, no solo en controllers HTTP.
