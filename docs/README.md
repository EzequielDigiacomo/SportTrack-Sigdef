# Documentación SportTrack-Sigdef (API)

**Única carpeta de documentación del backend** unificado SportTrack + SIGDEF.

Los mappings detallados de entidades siguen en los proyectos `*.Entidades/Docs` y `*.Controladores/Docs` (código); este `docs/` es el índice operativo, guías, cambios y diagramas.

**Última actualización:** 2026-09-27

---

## Organización

| Carpeta | Para qué sirve |
|---------|----------------|
| [Entrega/](./Entrega/) | **Pack de Entrega** — requerimientos, casos de uso, diagramas, manuales (API, técnico, instalación) |
| [guias/](./guias/) | Guías de uso de la API y operaciones |
| [tecnico/](./tecnico/) | Diagramas UML / arquitectura / ER canónico |
| [casos-de-uso/](./casos-de-uso/) | Casos de uso y contratos |
| [criterios/](./criterios/) | Criterios de aceptación API |
| [cambios/](./cambios/) | Changelogs de lo guardado |
| [seguridad/](./seguridad/) | Planes y políticas de seguridad |
| [referencia/](./referencia/) | Contexto legado y punteros a mappings |

**SIGDEF — plan traspasos:** ver `Front-Sigdef/docs/referencia/PLAN_TRASPASOS_ATLETAS.md` · [casos-de-uso/plan-traspasos-sigdef.md](./casos-de-uso/plan-traspasos-sigdef.md)

---

## Entrega

Pack de entrega del backend (fuente de verdad de entrega):

| # | Documento |
|---|-----------|
| — | [Entrega/README.md — Portada y ficha](./Entrega/README.md) |
| 01 | [Requerimientos de Usuario](./Entrega/01-Requerimientos-Usuario.md) |
| 02 | [Casos de Uso](./Entrega/02-Casos-de-Uso.md) |
| 03 | [Diagramas de Flujo](./Entrega/03-Diagramas-de-Flujo.md) |
| 04 | [Manual de Uso de la API](./Entrega/04-Manual-de-Uso-API.md) |
| 05 | [Manual Técnico](./Entrega/05-Manual-Tecnico.md) |
| 06 | [Manual de Instalación y Despliegue](./Entrega/06-Manual-Instalacion-Despliegue.md) |

> 📄 **PDF consolidado:** [Entrega/Pack-Entrega-SportTrack-Sigdef-Backend.pdf](./Entrega/Pack-Entrega-SportTrack-Sigdef-Backend.pdf) — todo el pack en un único archivo (7 documentos, tablas y 14 diagramas, 59 páginas).

---

## Stack

- **ASP.NET Core 10** (target `net10.0`) + PostgreSQL + EF Core (Npgsql) + SignalR
- JWT Auth (`/api/Auth/...`) — `Authorization: Bearer` / cookie `X-Access-Token` / `?access_token`
- CRUD SIGDEF (`/api/Atleta`, `/Club`, `/Tutor`, `/Usuario`, …)
- Eventos / timing / Live (SportTrack)
- Mensajería interna con aislamiento por `X-Client-App` / `SistemaOrigen` (ver [guias/mensajeria-aislamiento.md](./guias/mensajeria-aislamiento.md))

> Nota: la migración a **.NET 10** es reciente; documentos previos indicaban ".NET 8". El puerto local correcto es **5012** (no 5029). Ver [Entrega/05 §13](./Entrega/05-Manual-Tecnico.md#13-deuda-técnica-y-discrepancias-conocidas).

## Diagramas

Índice: [tecnico/diagramas-sistema.md](./tecnico/diagramas-sistema.md) · carpeta [tecnico/diagramas/](./tecnico/diagramas/)

Estados de competencia (evento, fases, resultados): [tecnico/estados-eventos-fases-sporttrack.md](./tecnico/estados-eventos-fases-sporttrack.md)

## Dev local

```powershell
cd SportTrack-Sigdef
dotnet run
```

Swagger (solo Development): `http://localhost:5012/swagger`

Health: `http://localhost:5012/api/Health` · `http://localhost:5012/api/Health/db`

Ver [Entrega/06 — Instalación y Despliegue](./Entrega/06-Manual-Instalacion-Despliegue.md).

---

## Frontend relacionado

| Repo | Docs / diagramas |
|------|------------------|
| **FrontSigdef** | `docs/tecnico/diagramas/` |
| **SportTrack-Front** | `docs/tecnico/diagramas/` |
