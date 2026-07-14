# Fase 1 — Escrituras con JWT (implementada)

Fecha: 2026-07-14  
Código en `SportTrack-Sigdef` (build OK).

---

## Qué cambió

| Área | Cambio |
|------|--------|
| **TimingHub** | Join/Leave/`GetServerTime` anónimos. Start/reset/`SendTime`/finish/lap/status con `[Authorize(Roles=CompetitionOperators)]`. Payments: Admin/SuperAdmin/Club/soporte. |
| **FasesController** | GET Live anónimos. Mutaciones (Iniciar, Reiniciar, Generar, Promover, BatchUpdate, Delete, …) con roles de competencia. |
| **ResultadosController** | GET anónimo. `PUT BatchUpdate` autenticado + roles. |
| **Auth register** | Solo `Admin` / `SuperAdmin` / `soporte_tecnico`. Whitelist de roles; **no** se puede crear SuperAdmin por API. |
| **Password / perfil** | Solo self o admin (cierra IDOR). |
| **SaaS debug-me** | Ya no anónimo; SuperAdmin/soporte. |
| **CORS** | Lista blanca (`WithOrigins` + mismos orígenes en ExceptionMiddleware). |
| **TokenKey** | Obligatorio fuera de Development; fallback solo en Dev. |

Roles competencia: `Admin,SuperAdmin,JuezControl,Largador,Cronometrista,soporte_tecnico`.

---

## Qué sigue igual (si hay sesión / Live correctos)

- Live sin login: Connect + Join + escuchar + GET eventos/fases/resultados.
- Largador / llegada / control / admin logueados (ya mandan Bearer).
- Alta de usuarios desde GestionLogins (admin logueado).

---

## Antes de deploy a Render — HACER ESTO

1. **Environment** del Web Service `sporttrack-sigdef`:

```text
TokenKey=<secreto-largo-aleatorio-64+chars>
AllowedOrigins=https://sporttrack-fec.vercel.app,https://sigdef.vercel.app,https://oficialsporttrack.vercel.app
```

(Localhost ya va hardcodeado en `Program.cs` para Dev.)

2. **Redeploy** del API en Render **después** de guardar esas vars (o al menos `TokenKey` antes del primer arranque con este código).

3. Si `TokenKey` es **nuevo** (distinto al fallback viejo): todos deben **volver a loguearse**.

4. Smoke post-deploy: Live sin login; largar; cortar tiempos; control; login FrontSigdef; alta usuario desde panel.

### Si olvidás TokenKey en prod

La API **no arranca** (InvalidOperationException al resolver la key).

### Si falta un origen Vercel en la lista

Browser: CORS blocked en login/API/SignalR desde ese dominio. Agregalo a `AllowedOrigins` y redeploy.

---

## Smoke post-Fase 1 (objetivo)

| Prueba | Esperado |
|--------|----------|
| Live `/resultados/{id}` sin login | OK |
| Hub negotiate `/hubs/timing` anónimo | OK |
| Largar / tiempos / control con login | OK |
| `POST /api/Fases/{id}/Iniciar` sin token | **401** |
| `POST /api/Auth/register` sin token | **401** |
| Register con rol SuperAdmin (admin token) | **400** |
| Password de otro usuario (user A sobre id de B, no admin) | **403** |
