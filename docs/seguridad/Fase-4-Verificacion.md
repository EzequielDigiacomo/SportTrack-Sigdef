# Fase 4 — Verificación post-deploy

Fecha smoke API (remoto Render): **2026-07-14**

## Resultado automático (API `sporttrack-sigdef.onrender.com`)

| Prueba | Resultado |
|--------|-----------|
| `GET /api/Health` | 200 OK |
| `GET /api/Eventos/proximos` (anon) | 200 OK |
| `POST /api/Auth/login` user inexistente | 401 (esperado) |
| `POST /api/Auth/register` sin token | **401** (Fase 1 OK en prod) |
| `GET /api/Atleta` sin token | **401** (Fase 2 OK en prod) |
| `POST /api/Fases/1/Iniciar` sin token | **401** (Fase 1 OK en prod) |
| Mercado Pago | No aplica |

La API en Render ya está rechazando escrituras/CRUD anónimos. Falta confirmar **UI** (Live + jueces + login admin).

---

## Checklist manual (vos)

### Público / Live
- [ ] Abrir `https://sporttrack.pro/resultados/{idEvento}` sin login → carga
- [ ] F12 Network: `.../hubs/timing/negotiate` → 200 (no `hubs/results`)
- [ ] Con otra ventana: largar / cortar → Live se actualiza

### Auth / Admin
- [ ] Login con un usuario que funcione (si `admin` falla, usar otro Admin o reset SQL Fase 4 nota abajo)
- [ ] Panel Super / eventos / listados con sesión OK
- [ ] FrontSigdef (si aplica) listados con sesión OK

### Jueces (carrera corta)
- [ ] Largador: largar serie
- [ ] Mesa de llegada: cortar 1–2 carriles
- [ ] Mesa de control: sync 3 roles + tiempos + timer

### Negativos opcionales en browser
- [ ] Origen raro (p.ej. `https://example.com` en fetch) → CORS bloqueado
- [ ] Opcional: spamear login → 429

### Ops
- [x] API arranca / Health 200
- [ ] `TokenKey` + `AllowedOrigins` revisados en Render
- [x] Mercado Pago: no aplica

---

## Si `admin` / `admin123` no entra

Usuario = **`admin`** (no “SuperAdmin”). Si la cuenta quedó bloqueada:

```sql
UPDATE seguridad."Usuarios"
SET "PasswordHash" = '$2a$12$6ET.51wRwWnd/mscg3c8l.DcgbMBbQmVSqJ/pHpUcpNAPe4mzxoOS',
    "IntentosFallidos" = 0,
    "EstaActivo" = true
WHERE LOWER("Username") = 'admin';
```

---

## Criterio de cierre Fase 4

Live + un flujo de jueces + un login admin en UI = **Fase 4 cerrada**.  
API negativos de arriba ya están OK en Render.
