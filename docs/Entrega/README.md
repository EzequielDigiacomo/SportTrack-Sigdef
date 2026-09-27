# Pack de Entrega — Backend SportTrack-Sigdef

**Última actualización: 2026-09-27**

> Documentación de entrega del backend unificado. Este pack es la fuente de verdad *de entrega*; los documentos de `docs/` (guías, técnico, seguridad, cambios) se conservan como histórico y contexto de diseño.

---

## Índice

| # | Documento | Contenido |
|---|-----------|-----------|
| 1 | [01-Requerimientos-Usuario.md](./01-Requerimientos-Usuario.md) | Actores/roles, RF, RNF (ISO 25010), historias de usuario (Gherkin), reglas de negocio y matriz de trazabilidad |
| 2 | [02-Casos-de-Uso.md](./02-Casos-de-Uso.md) | Diagrama de casos de uso y especificación de CU-01…CU-12 |
| 3 | [03-Diagramas-de-Flujo.md](./03-Diagramas-de-Flujo.md) | Diagramas Mermaid: pipeline, estados, motor ICF, timing, reseteo, SaaS, mensajería, login, backup |
| 4 | [04-Manual-de-Uso-API.md](./04-Manual-de-Uso-API.md) | Manual del integrador/administrador de la API: auth, consumo por caso de uso, errores, FAQ y uso para soporte |
| 5 | [05-Manual-Tecnico.md](./05-Manual-Tecnico.md) | Arquitectura, stack, controllers/servicios, modelo de datos, seguridad, SignalR, deploy, deuda técnica |
| 6 | [06-Manual-Instalacion-Despliegue.md](./06-Manual-Instalacion-Despliegue.md) | Requisitos, configuración, ejecución local, migraciones, Docker y despliegue en Render |

Documentación histórica y de contexto (se referencia, no se reemplaza): [`docs/README.md`](../README.md).

> 📄 **PDF consolidado del pack:** [`Pack-Entrega-SportTrack-Sigdef-Backend.pdf`](./Pack-Entrega-SportTrack-Sigdef-Backend.pdf)
> Contiene los 7 documentos en orden, con todas las tablas y los **14 diagramas renderizados**. A4, 59 páginas, con índice navegable y marcadores (bookmarks) por sección.

---

## 1. Ficha del producto

| Atributo | Valor |
|----------|-------|
| **Nombre** | SportTrack-Sigdef (backend unificado) |
| **Tipo** | Aplicación web API REST + tiempo real (SignalR), multi-tenant SaaS |
| **Dominios** | **SportTrack**: competencias, regatas, cronometraje y resultados en vivo. **SIGDEF**: federación (federaciones, clubes, atletas, delegados, tutores, entrenadores, traspasos, pagos). **SaaS**: planes, métricas, auditoría, backups |
| **Versión documentada** | 2026-09-27 (migración a .NET 10) |
| **Stack real** | **.NET 10** (ASP.NET Core Web API) · PostgreSQL 14+ · EF Core (Npgsql) · SignalR · JWT (HMAC-SHA512) · Cloudinary · MercadoPago SDK |
| **Target framework** | `net10.0` en los 4 proyectos |
| **Solución** | `SportTrack-Sigdef.sln` |
| **Despliegue** | Render (Docker, multi-stage, puerto 8080) |
| **Repositorio** | `c:\Users\EZEQU\source\repos\SportTrack-Sigdef` |

> ⚠️ **Discrepancia corregida:** documentos anteriores (`docs/README.md`, guías) indicaban **ASP.NET Core 8** y Swagger en el puerto **5029**. El target real es **.NET 10** y los puertos locales son **http://localhost:5012** / **https://localhost:7232**. Ver §5 y el [Manual Técnico](./05-Manual-Tecnico.md#13-deuda-técnica-y-discrepancias-conocidas).

---

## 2. Propósito y alcance

El backend unifica en un solo servicio:

- **SportTrack** — gestión de eventos/regatas, pruebas, fases, progresión ICF, cronometraje en vivo (SignalR), resultados y clasificaciones.
- **SIGDEF** — administración federativa multi-tenant: federaciones, clubes, atletas federados, tutores, delegados, entrenadores, roles, documentación y traspasos.
- **SaaS** — planes comerciales (SIGDEF / SportTrack / Pack Dúo en S/M/L), enforcement de acceso, métricas globales, auditoría y backups.

### 2.1 Alcance

Incluye API REST bajo `api/**`, hub SignalR `/hubs/timing`, seguridad (JWT, CORS, rate limiting, security headers), persistencia PostgreSQL con migraciones automáticas al arranque, y servicios en segundo plano (sincronización de estados de evento, snapshots de audiencia).

### 2.2 Límites (fuera de alcance)

- **No** incluye el frontend (SPAs en `SportTrack-Front`, `FrontSigdef`, `WebSPA-SIGDEF`).
- **No** existe CI/CD ni `docker-compose` en el repositorio; el despliegue es *push a `main` → auto-deploy en Render*.
- **No** hay token de cronometrista separado ni validación de firma del webhook de MercadoPago (ver deuda técnica).
- **No** se aplica aún el tope `MaxAtletas` de enforcement de planes (ver deuda técnica).

---

## 3. Cómo leer este pack

| Si sos… | Empezá por |
|---------|-----------|
| Analista / QA | 01 Requerimientos y 02 Casos de uso |
| Integrador frontend | 04 Manual de Uso API y 03 Diagramas de flujo |
| DevOps / SRE | 06 Instalación y despliegue, 05 Manual Técnico |
| Desarrollador backend | 05 Manual Técnico (completo) |
| Soporte / operación | 04 §Uso para soporte y 03 §cronometraje |

---

## 4. Glosario

| Término | Definición |
|---------|------------|
| **Evento** | Competencia/regata, con una o más pruebas y fechas. Estados: `Programada`, `EnCurso`, `Finalizada` |
| **Prueba** | Definición de una competencia (bote, categoría, distancia, sexo) dentro de un evento |
| **EventoPrueba** | Instancia de una prueba dentro de un evento (la unidad sobre la que se generan fases) |
| **Fase** | Serie/heat, semifinal o final dentro de un `EventoPrueba` (ej. `A1`, `B2`, `Final A`) |
| **ICF** | Federación Internacional de Canotaje. Reglamento de progresión entre series, semifinales y finales |
| **Progresión** | Mecanismo que ordena resultados y arma la siguiente etapa según un plan (A1–G2, variantes 1/2) |
| **Resultado** | Registro de un participante en una fase (carril, tiempo, posición, estado) |
| **TiempoOficial** | Tiempo validado oficialmente (`interval`); tiene prioridad sobre el tiempo crudo |
| **Outbox de timing** | Cola que persiste envíos de cronometrista offline para commit posterior |
| **Cronometrista** | Rol operativo que toma tiempos en pista y los envía por SignalR/outbox |
| **Largador** | Rol que da la largada y puede solicitar reseteo de regata |
| **Tenant** | Federación (multi-tenant por `IdFederacion`); el aislamiento se aplica por federación y por sistema de origen |
| **Sistema de origen** | Cabecera `X-Client-App` (`sporttrack` \| `sigdef`) que aísla mensajería y logins |
| **Agente Libre** | Atleta liberada por su club, sin pertenencia activa |
| **Traspaso** | Solicitud de cambio de club de un atleta federado, con flujo de aceptación/rechazo/aprobación |
| **Enforcement de plan** | Verificación en backend de los flags del `PlanSaaS` de la federación (crear roles, Live, imágenes, etc.) |
| **Live** | Resultados públicos anónimos servidos con rate limit `live` y cache corto |

---

## 5. Nota sobre documentos históricos

Los documentos en `docs/` (carpetas `guias/`, `tecnico/`, `seguridad/`, `casos-de-uso/`, `criterios/`, `cambios/`, `referencia/`) se **conservan** como registro histórico de diseño y decisiones. Algunos contienen afirmaciones ya desmentidas por el código:

| Documento histórico | Afirmación | Estado real |
|---------------------|-----------|-------------|
| `docs/README.md` | "ASP.NET Core 8" | Target real **`net10.0`** |
| `docs/README.md`, `guias/operacion-local.md` | Swagger `http://localhost:5029` | **`http://localhost:5012`** (`launchSettings.json`) |
| `docs/seguridad/Diseno-Enforcement-Planes.md` §5.3 | `MaxAtletas` implementado | **No aplicado** en `AltaAtletaService` (deuda) |
| `docs/seguridad/Diseno-Enforcement-Planes.md` | `MaxTorneosActivos` operativo | Migración `RemovePlanTournamentLimits` lo deja en `-1` (sin enforcement) |
| `docs/seguridad/*` | Cookie-first JWT "fuera de alcance" | **Implementado**: `X-Access-Token` cookie + `Authorization: Bearer` |
| `scripts/backfill-auditoria-id-evento.sql` | Asume columna `IdEvento` en `Auditoria` | `Auditoria.IdEvento` es `[NotMapped]`; ver inconsistencia en `AuditoriaController` |

Estas correcciones se detallan en el [Manual Técnico §13](./05-Manual-Tecnico.md#13-deuda-técnica-y-discrepancias-conocidas).

---

## Pie de página

- ➡️ [01 · Requerimientos de Usuario](./01-Requerimientos-Usuario.md)
- ➡️ [02 · Casos de Uso](./02-Casos-de-Uso.md)
- ➡️ [03 · Diagramas de Flujo](./03-Diagramas-de-Flujo.md)
- ➡️ [04 · Manual de Uso de la API](./04-Manual-de-Uso-API.md)
- ➡️ [05 · Manual Técnico](./05-Manual-Tecnico.md)
- ➡️ [06 · Manual de Instalación y Despliegue](./06-Manual-Instalacion-Despliegue.md)
- ⬅️ [Índice de documentación histórica](../README.md)
