# Fase 3 — Hardening (sin Mercado Pago)

Fecha: 2026-07-14  
**Mercado Pago / webhook:** pospuesto a propósito (aún no se usa). No se implementó validación de firma.

---

## Qué se implementó

| Ítem | Detalle |
|------|---------|
| **Rate limit auth** | Política `auth`: 20 req/min por IP en `POST /api/Auth/login` y `register`. Respuesta **429** si se excede. |
| **Uploads** | `FileUploadRules`: máx 6 MB; extensiones `.jpg/.jpeg/.png/.webp/.gif/.pdf`; MIME alineados. |
| **Headers** | `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`, `Permissions-Policy`. |

---

## Render

Sin variables nuevas (más allá de Fase 1: `TokenKey` + `AllowedOrigins`).

Si alguien fuerza login/register en bucle desde la misma IP → verá **429** un minuto.

---

## Smoke

| Prueba | Esperado |
|--------|----------|
| Login normal | OK |
| ~25 logins fallidos en 1 min misma IP | Alguno 429 |
| Upload PDF/JPG logueado | OK |
| Upload `.exe` / MIME raro | 400 |
| Live + jueces | Sin cambio |

---

## Fase 4 (verificación)

Checklist operativo post-deploy: ver [Fase-4-Verificacion.md](./Fase-4-Verificacion.md).
