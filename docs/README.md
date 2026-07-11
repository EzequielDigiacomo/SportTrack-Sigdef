# Documentación SportTrack-Sigdef (API)

**Única carpeta de documentación del backend** unificado SportTrack + SIGDEF.

Los mappings detallados de entidades siguen en los proyectos `*.Entidades/Docs` y `*.Controladores/Docs` (código); este `docs/` es el índice operativo, guías y cambios.

**Última actualización:** 2026-07-11

---

## Organización

| Carpeta | Para qué sirve |
|---------|----------------|
| [guias/](./guias/) | Guías de uso de la API y operaciones |
| [casos-de-uso/](./casos-de-uso/) | Casos de uso de backend / contratos |
| [criterios/](./criterios/) | Criterios de aceptación API |
| [cambios/](./cambios/) | Changelogs de lo guardado |
| [seguridad/](./seguridad/) | Planes y políticas de seguridad |
| [referencia/](./referencia/) | Contexto legado y punteros a mappings |

---

## Stack

- ASP.NET Core 8 + PostgreSQL + EF Core  
- JWT Auth (`/api/Auth/...`)  
- CRUD SIGDEF (`/api/Atleta`, `/Club`, `/Tutor`, `/Usuario`, …)  
- Eventos / timing / Live (SportTrack)

## Dev local

```powershell
cd SportTrack-Sigdef
dotnet run
```

Swagger: `http://localhost:5029/swagger`

---

## Frontend relacionado

Documentación UI: repo **FrontSigdef** → `docs/`.
