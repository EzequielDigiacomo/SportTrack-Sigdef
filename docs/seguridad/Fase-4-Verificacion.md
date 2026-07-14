# Fase 4 — Verificación post-deploy

Checklist para validar Fases 1–3 en producción (o staging). Marcar al desplegar.

## Público / Live

- [ ] `/resultados/{id}` sin login carga
- [ ] Network: `/hubs/timing/negotiate` 200 (no `/hubs/results`)
- [ ] Actualización en vivo al largar / cortar tiempos

## Auth

- [ ] Login con usuario válido OK
- [ ] `POST /api/Auth/register` sin token → 401
- [ ] Tras muchos intentos de login → 429 (opcional)

## Jueces

- [ ] Largador larga
- [ ] Cronometrista envía tiempos
- [ ] Mesa de control ve sync + tiempos + timer

## Admin / SIGDEF

- [ ] FrontSigdef / SportTrack admin con sesión: listados OK
- [ ] `GET /api/Atleta` sin token → 401
- [ ] Upload documento válido OK; `.exe` rechazado

## Negativos (seguridad)

- [ ] Hub `RequestStartRace` / `SendTime` sin JWT → error
- [ ] `POST /api/Fases/{id}/Iniciar` sin token → 401
- [ ] CORS desde origen no listado → bloqueado en browser

## Ops

- [ ] Render: `TokenKey` y `AllowedOrigins` seteados
- [ ] API arranca (sin error TokenKey)
- [ ] Mercado Pago: **no aplica** por ahora
