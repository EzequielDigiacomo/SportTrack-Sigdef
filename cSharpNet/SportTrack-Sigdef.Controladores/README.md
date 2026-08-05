# SportTrack-Sigdef.Controladores — Lógica de negocio

Nombre histórico del proyecto: aquí viven **services**, **repositories**, **DTOs**, **hubs**, pagos, auth, etc.  
No son los Controllers HTTP (esos están en el proyecto API `SportTrack-Sigdef`).

## Mapa de carpetas

| Carpeta | Rol |
|---------|-----|
| `Auth/` | Login, JWT, policies |
| `Evento/`, `Inscripcion/`, `Participante/`, ... | Dominio SportTrack (competición) |
| `Federaciones/` | Módulo SIGDEF (atletas, clubes, roles, traspasos) |
| `Fase/Progression/` | Motor de progresión de series/finales |
| `Mensajes/` | Mensajería interna |
| `PagosSIGDEF/` | Mercado Pago |
| `SaaS/` | Planes y métricas |
| `Hubs/` | SignalR (timing en vivo) |
| `Mappings/` | AutoMapper profiles |
| `Exceptions/` | Errores de negocio tipados |
| `Caching/` | Cache de lecturas live |

## Patrón habitual

```
IXxxService  ←  XxxService  ←  usa DbContext / IXxxRepository
IXxxRepository ← XxxRepository
Dtos/          ← Create/Update/Detail DTOs
```

## Orden de lectura sugerido

1. `Exceptions/` — cómo se modelan errores
2. `Auth/` — token + policies
3. `Categoria/` o `Bote/` — CRUD completo chico (interface + service + repo + DTOs)
4. `Evento/` — servicio más complejo
5. `Federaciones/AtletaServices.md` — módulo SIGDEF
6. `Fase/Progression/` — lógica avanzada
7. `Hubs/TimingHub.md` — tiempo real

Continúa en:

- [`../Fundamentos/04-servicios-interfaces-async.md`](../Fundamentos/04-servicios-interfaces-async.md)
- [`../Fundamentos/05-dtos-y-patrones.md`](../Fundamentos/05-dtos-y-patrones.md)
