# Módulo de mensajería privada — Plan definitivo

> Copia de referencia del plan de implementación.  
> **Versión canónica:** `SportTrack-Front/Documentacion/Plan_Modulo_Mensajeria.md`  
> Este repo: backend API (`SportTrack-Sigdef`).

---

## Cómo ejecutar cada fase (chat de Cursor)

1. `Ejecutá Fase 0 del plan de mensajería — solo backend 1:1 SuperAdmin↔Admin`
2. `Ejecutá Fase 1 — UI SuperAdmin`
3. `Ejecutá Fase 2 — Admin y Club`
4. `Ejecutá Fase 3 — envío masivo`
5. `Ejecutá Fase 4 — badge polling`

---

## Fases — resumen

| Fase | Backend (este repo) | Frontend (SportTrack-Front) |
|------|---------------------|----------------------------|
| **0** | Hilo, Mensaje, migración, MensajesController, permisos SuperAdmin↔Admin | — |
| **1** | — | Bandeja SuperAdmin |
| **2** | Permisos Admin↔Club | Bandejas Admin + Club |
| **3** | CampanaEnvio, POST /hilos/masivo | UI masivo + campañas |
| **4** | GET /no-leidos/count | Badge polling |

Ver plan completo en: `../reposFront/SportTrack-Front/Documentacion/Plan_Modulo_Mensajeria.md`

---

## Archivos a crear en este repo (por fase)

### Fase 0

- `SportTrack-Sigdef.Entidades/Entidades/Hilo.cs`
- `SportTrack-Sigdef.Entidades/Entidades/Mensaje.cs`
- `SportTrack-Sigdef.AccesoDatos/Migrations/..._AddMensajeria.cs`
- `SportTrack-Sigdef.Controladores/Mensajes/` (Service, Repository, DTOs)
- `SportTrack-Sigdef/Controllers/MensajesController.cs`
- Modificar: `SportTrackDbContext.cs`, `Program.cs`

### Fase 2

- Extender `MensajeService` — permisos Admin↔Club

### Fase 3

- `CampanaEnvio.cs` + migración
- Endpoints masivo y campañas

### Fase 4

- Endpoint `no-leidos/count`
