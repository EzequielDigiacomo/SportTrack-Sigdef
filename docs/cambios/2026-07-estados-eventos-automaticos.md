# Changelog — 2026-07 — Sync automático de estados de evento (SportTrack)

## Resumen

Los eventos de SportTrack pasan de `Programada` → `EnCurso` → `Finalizado` según fechas, sin intervención manual del admin.

## Comportamiento

- **Inicio:** `Fecha` + `HoraInicioEvento` en la zona horaria del evento (`TimeZoneId`).
- **Fin:** fin del día local de `FechaFin` (o `Fecha` si es un solo día).
- **`Cancelado`:** no se modifica por el sync.
- **Solo avanza:** un estado adelantado manualmente no retrocede.

## Componentes nuevos

| Archivo | Descripción |
|---------|-------------|
| `EventoEstadoSyncHelper.cs` | Cálculo del estado |
| `EventoEstadoSyncService.cs` | Persistencia |
| `EventoEstadoBackgroundService.cs` | Job cada 15 min |
| `IEventoEstadoSyncService.cs` | Contrato |

## Integración

- Registro DI y hosted service en `Program.cs`.
- Sync al arranque (después de migraciones).
- Sync en `EventoService` al listar o consultar eventos.

## Documentación

Especificación completa (evento + fases + resultados):  
[../tecnico/estados-eventos-fases-sporttrack.md](../tecnico/estados-eventos-fases-sporttrack.md)
