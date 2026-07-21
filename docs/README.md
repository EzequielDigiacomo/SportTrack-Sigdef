# Documentación SportTrack-Sigdef (API)

**Única carpeta de documentación del backend** unificado SportTrack + SIGDEF.

Los mappings detallados de entidades siguen en los proyectos `*.Entidades/Docs` y `*.Controladores/Docs` (código); este `docs/` es el índice operativo, guías, cambios y diagramas.

**Última actualización:** 2026-07-21

---

## Organización

| Carpeta | Para qué sirve |
|---------|----------------|
| [guias/](./guias/) | Guías de uso de la API y operaciones |
| [tecnico/](./tecnico/) | Diagramas UML / arquitectura / ER canónico |
| [casos-de-uso/](./casos-de-uso/) | Casos de uso y contratos |
| [criterios/](./criterios/) | Criterios de aceptación API |
| [cambios/](./cambios/) | Changelogs de lo guardado |
| [seguridad/](./seguridad/) | Planes y políticas de seguridad |
| [referencia/](./referencia/) | Contexto legado y punteros a mappings |

**SIGDEF — plan traspasos:** ver `Front-Sigdef/docs/referencia/PLAN_TRASPASOS_ATLETAS.md` · [casos-de-uso/plan-traspasos-sigdef.md](./casos-de-uso/plan-traspasos-sigdef.md)

---

## Stack

- ASP.NET Core 8 + PostgreSQL + EF Core  
- JWT Auth (`/api/Auth/...`)  
- CRUD SIGDEF (`/api/Atleta`, `/Club`, `/Tutor`, `/Usuario`, …)  
- Eventos / timing / Live (SportTrack)  
- Mensajería interna con aislamiento por `X-Client-App` / `SistemaOrigen` (ver [guias/mensajeria-aislamiento.md](./guias/mensajeria-aislamiento.md))

## Diagramas

Índice: [tecnico/diagramas-sistema.md](./tecnico/diagramas-sistema.md) · carpeta [tecnico/diagramas/](./tecnico/diagramas/)

Estados de competencia (evento, fases, resultados): [tecnico/estados-eventos-fases-sporttrack.md](./tecnico/estados-eventos-fases-sporttrack.md)

## Dev local

```powershell
cd SportTrack-Sigdef
dotnet run
```

Swagger: `http://localhost:5029/swagger`

---

## Frontend relacionado

| Repo | Docs / diagramas |
|------|------------------|
| **FrontSigdef** | `docs/tecnico/diagramas/` |
| **SportTrack-Front** | `docs/tecnico/diagramas/` |
