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

## DoD

1. Cambio en código API guardado.  
2. Documentado en `docs/cambios/`.  
3. Si afecta front, FrontSigdef `docs/` actualizado.  
4. Deploy en el entorno compartido cuando el fix sea de proyección/endpoint.
