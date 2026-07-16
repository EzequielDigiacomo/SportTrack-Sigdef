# Seguridad

| Documento | Contenido |
|-----------|-----------|
| [Fase-0-Inventario-Baseline.md](./Fase-0-Inventario-Baseline.md) | Inventario público/abierto + smoke baseline (2026-07-14) |
| [Fase-1-Escrituras-Auth.md](./Fase-1-Escrituras-Auth.md) | Hub/Fases/Resultados/Auth/CORS/TokenKey |
| [Fase-2-Auth-Default.md](./Fase-2-Auth-Default.md) | FallbackPolicy + Authorize CRUD + HTTPS/HSTS |
| [Fase-3-Hardening.md](./Fase-3-Hardening.md) | Rate limit auth, uploads, headers (**sin** Mercado Pago) |
| [Fase-4-Verificacion.md](./Fase-4-Verificacion.md) | Checklist smoke post-deploy |
| [Diseno-Enforcement-Planes.md](./Diseno-Enforcement-Planes.md) | Enforcement planes SaaS (matriz 200/400, logins, Live vs jueces) |
| [Plan-Endurecimiento-Seguridad.md](./Plan-Endurecimiento-Seguridad.md) | Plan fases Auth / CORS / SignalR / JWT |

**Estado:** Fases 0–3 en código. MP webhook pospuesto. Deploy: `TokenKey` + `AllowedOrigins`. Luego checklist Fase 4.
