# AtletaServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/AtletaServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IAtletaServices`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Services`
- **Usings:**
  - `using Microsoft.AspNetCore.Mvc;`
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`
  - `using SIGDEF.DTOs;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Base;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Club;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Inscripcion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_tenantProvider` — tipo `ITenantProvider` (típicamente dependencia inyectada o estado privado)
- `_altaAtletaService` — tipo `IAltaAtletaService` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `SportTrack_Sigdef.Controladores.Audit.IAuditService` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `GetAtletas`

- **Firma:** `async Task<ActionResult<IEnumerable<AtletaDetailDto>>> GetAtletas()`
- **Retorno:** `Task<ActionResult<IEnumerable<AtletaDetailDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetAtletasByClub`

- **Firma:** `async Task<ActionResult<IEnumerable<AtletaDetailDto>>> GetAtletasByClub(int clubId)`
- **Retorno:** `Task<ActionResult<IEnumerable<AtletaDetailDto>>>`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetAtletasPaginadosAsync`

- **Firma:** `async Task<ActionResult<PagedResponseDto<AtletaListDto>>> GetAtletasPaginadosAsync(PaginationParamsDto parameters)`
- **Retorno:** `Task<ActionResult<PagedResponseDto<AtletaListDto>>>`
- **Parámetros:**

- `parameters` (`PaginationParamsDto`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).
- **Llamadas await destacadas:** `query.CountAsync(...)`

#### `GetAtleta`

- **Firma:** `async Task<ActionResult<AtletaDetailDto>> GetAtleta(int id)`
- **Retorno:** `Task<ActionResult<AtletaDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `PostAtleta`

- **Firma:** `async Task<ActionResult<AtletaDto>> PostAtleta(AtletaCreateDto atletaCreateDto)`
- **Retorno:** `Task<ActionResult<AtletaDto>>`
- **Parámetros:**

- `atletaCreateDto` (`AtletaCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.AnyAsync(...)`, `_context.Clubes.AnyAsync(...)`, `_context.AtletasFederados.AnyAsync(...)`, `_altaAtletaService.EnsureAtletaFederacionAsync(...)`, `_context.Entry(...)`

#### `PostAtletaFull`

- **Firma:** `async Task<ActionResult<AtletaDto>> PostAtletaFull(AtletaFullCreateDto dto)`
- **Retorno:** `Task<ActionResult<AtletaDto>>`
- **Parámetros:**

- `dto` (`AtletaFullCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; usa transacción de base de datos; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Database.BeginTransactionAsync(...)`, `EnsureMaxAtletasAsync(...)`, `_altaAtletaService.AltaAtletaCompletaAsync(...)`, `_altaAtletaService.UpsertParticipanteAsync(...)`, `_context.Tutores.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `transaction.CommitAsync(...)`, `_auditService.RegistrarAccionAsync(...)`, `GetAtleta(...)`, `transaction.RollbackAsync(...)`

#### `PutAtleta`

- **Firma:** `async Task<IActionResult> PutAtleta(int id, AtletaCreateDto atletaCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `atletaCreateDto` (`AtletaCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.AtletasFederados.FindAsync(...)`, `_context.Clubes.AnyAsync(...)`, `_context.Participantes.FindAsync(...)`, `_auditService.RegistrarAccionAsync(...)`, `_context.SaveChangesAsync(...)`, `AtletaExistsAsync(...)`

#### `DeleteAtleta`

- **Firma:** `async Task<IActionResult> DeleteAtleta(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; filtra con LINQ (`Where`); usa transacción de base de datos; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Database.BeginTransactionAsync(...)`, `_context.Usuarios.AnyAsync(...)`, `_context.Entrenadores.AnyAsync(...)`, `_context.DelegadosClub.AnyAsync(...)`, `_context.Tutores.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `transaction.CommitAsync(...)`, `transaction.RollbackAsync(...)`

#### `EnsureMaxAtletasAsync`

- **Firma:** `async Task EnsureMaxAtletasAsync(int? clubId)`
- **Retorno:** `Task`
- **Parámetros:**

- `clubId` (`int?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.CountAsync(...)`

#### `AtletaExistsAsync`

- **Firma:** `async Task<bool> AtletaExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.AtletasFederados.AnyAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/AtletaServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
