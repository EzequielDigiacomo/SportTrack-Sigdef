# Traspasos de atletas — puntero al plan (SIGDEF)

El módulo de **traspasos de atletas** se implementa en **SIGDEF**, no en SportTrack.

## Documentación

| Documento (Front-Sigdef) | Contenido |
|--------------------------|-----------|
| `docs/casos-de-uso/traspasos-atletas-flujo.md` | **Flujo vigente** (fed verifica deuda → origen acepta/ejecuta) |
| `docs/guias-usuario/traspasos-paso-a-paso.md` | Guía operativa paso a paso |
| `docs/referencia/PLAN_TRASPASOS_ATLETAS.md` | Plan por fases 1–5 |
| `docs/cambios/2026-07-traspasos-flujo-federacion-primero.md` | Changelog del cambio de orden |

## Estado

- Fases 1–4 ✅ completadas (2026-07-21)
- Flujo vigente: **federación primero** (no origen primero)
- Próxima fase: Fase 5 (Reglas avanzadas)

## Flujo vigente (resumen)

```text
Club destino solicita
  → PendienteFederacion (federación verifica deuda)
  → Habilita → PendienteOrigen
  → Club origen acepta → Aprobado (ejecuta cambio de club)
```

Al completar cada fase, actualizar el plan en Front-Sigdef y registrar changelog en `docs/cambios/`.
