# Módulo de mensajería privada — Plan (backend)

> Copia de referencia.  
> **Versión canónica:** `SportTrack-Front/Documentacion/Plan_Modulo_Mensajeria.md`  
> Este repo: backend API (`SportTrack-Sigdef`).

---

## Estado

**MVP (fases 0–4) cerrado** — jul 2026.  
Fases **5–10** = mejoras futuras (ver plan canónico).

---

## Cómo retomar (chat de Cursor)

1. `Ejecutá Fase 5 del plan de mensajería — búsqueda`
2. `Ejecutá Fase 6 — reporte de acuse`
3. `Ejecutá Fase 7 — adjuntos`
4. `Ejecutá Fase 8 — copia SMTP`
5. `Ejecutá Fase 9 — UX avanzada`
6. `Ejecutá Fase 10 — gobernanza y retención`

---

## Fases MVP ✅

| Fase | Backend | Frontend |
|------|---------|----------|
| **0** | Hilo, Mensaje, API, SuperAdmin↔Admin | — |
| **1** | — | Bandeja SuperAdmin |
| **2** | Permisos Admin↔Club | Bandejas Admin + Club |
| **3** | CampanaEnvio, masivo | UI masivo + campañas |
| **4** | `GET /no-leidos/count` | Puntito rojo + polling |

---

## Fases futuras (resumen)

| Fase | Tema | Backend típico |
|------|------|----------------|
| **5** | Búsqueda / filtros | Query params en `/hilos` |
| **6** | Reporte de acuse | `GET /campanas/{id}/acuse` + export |
| **7** | Adjuntos | Tabla `Adjuntos` + storage + upload |
| **8** | Email SMTP | `IEmailService` + config proveedor |
| **9** | UX / SignalR badge | `MessagingHub` opcional |
| **10** | Retención / auditoría | Jobs + políticas |

---

## Archivos clave ya creados

- `Entidades/Hilo.cs`, `Mensaje.cs`, `CampanaEnvio.cs`
- `Controladores/Mensajes/` (Service, Repository, DTOs)
- `Controllers/MensajesController.cs`
- Migraciones: `AddMensajeria`, `AddCampanasEnvio`
- DI en `Program.cs` + `SportTrackDbContext` (esquema `comunicacion`)
