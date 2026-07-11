# Changelog — 2026-07 — Atletas club, contratos front, seguridad

## 1. Atletas por club — FechaNacimiento

- En `ClubServices` (proyección de atletas del club) se incluyen campos de nacimiento/documento para que el front calcule edad y muestre estado de tutor.
- **Acción pendiente de entorno:** redeploy de la API en Render (u host) si aún no está publicado.

## 2. Contratos alineados con FrontSigdef

Documentados en [../casos-de-uso/contratos-front-sigdef.md](../casos-de-uso/contratos-front-sigdef.md):

- Password admin → Auth PUT + `IdUsuario`
- AtletaTutor → `ParticipanteId`
- Detalles con `participante`

## 3. Seguridad

- Plan de endurecimiento centralizado en [../seguridad/Plan-Endurecimiento-Seguridad.md](../seguridad/Plan-Endurecimiento-Seguridad.md) (antes en la raíz de `docs/`).
- Fases 1–2 (auth global + SignalR jueces) **pendientes de implementar**; el plan está escrito.

## 4. Documentación

- Carpeta `docs/` reorganizada (guías, casos, criterios, cambios, seguridad, referencia).
