# Changelog — 2026-07 — Mensajería: SistemaOrigen

## Cambio

Aislamiento de mensajería interna entre **SportTrack** y **SIGDEF**.

- Entidades `Hilo` y `CampanaEnvio`: propiedad `SistemaOrigen`.
- Migración `AddSistemaOrigenMensajeria` + backfill a `sporttrack`.
- Servicios/repositorio/controller leen `X-Client-App` y filtran por origen.

## Impacto

- Fronts deben enviar siempre `X-Client-App` (ya lo hacían).
- Mensajes nuevos de FrontSigdef quedan con origen `sigdef` y no contaminan SportTrack.

## Docs

Ver [../guias/mensajeria-aislamiento.md](../guias/mensajeria-aislamiento.md).
