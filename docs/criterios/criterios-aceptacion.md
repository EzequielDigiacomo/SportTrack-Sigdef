# Criterios de aceptación — SportTrack-Sigdef

**Fecha:** 2026-07-11

## Auth / usuarios

- [ ] `PUT /Auth/usuarios/{id}/password` actualiza hash BCrypt y permite login.
- [ ] Id de ruta = `IdUsuario`.
- [ ] No depender de `reset-password` no expuesto en controller para el front Admin.

## Atletas / club / tutores

- [ ] Listado de atletas por club incluye `FechaNacimiento` (y documento si aplica).
- [ ] Tras deploy, FrontSigdef columna Tutor deja de mostrar todo “—” por falta de fecha.
- [ ] `AtletaTutor` acepta `ParticipanteId` + `IdTutor`.

## Seguridad (plan)

- [ ] Avanzar fases del plan en [../seguridad/Plan-Endurecimiento-Seguridad.md](../seguridad/Plan-Endurecimiento-Seguridad.md) sin romper Live/SignalR de lectura pública.

## Mensajería

- [ ] Requests de `/api/mensajes/*` requieren `X-Client-App` válido (`sporttrack` \| `sigdef`).
- [ ] Create/list/unread/campañas respetan `SistemaOrigen`.
- [ ] Hilos/campañas de un origen no son visibles con el otro header.
- [ ] Backfill: datos pre-migración quedan en `sporttrack`.

Detalle: [../guias/mensajeria-aislamiento.md](../guias/mensajeria-aislamiento.md). Matriz QA en FrontSigdef `docs/criterios/mensajeria-aislamiento.md`.

## DoD

1. Cambio en código API guardado.  
2. Documentado en `docs/cambios/`.  
3. Si afecta front, FrontSigdef `docs/` actualizado.  
4. Deploy en el entorno compartido cuando el fix sea de proyección/endpoint.
