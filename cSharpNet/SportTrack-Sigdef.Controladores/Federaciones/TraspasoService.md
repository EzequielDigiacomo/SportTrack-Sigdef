# TraspasoService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/TraspasoService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `ITraspasoService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **IQueryable**: consulta composable que EF Core traduce a SQL al materializar (ToListAsync, FirstOrDefaultAsync, etc.).
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Federaciones`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Controladores.Audit;`
  - `using SportTrack_Sigdef.Controladores.Exceptions;`
  - `using SportTrack_Sigdef.Controladores.Mensajes;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Traspaso;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Security.Claims;`
  - `using System.Text;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_tenantProvider` — tipo `ITenantProvider` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `IAuditService` (típicamente dependencia inyectada o estado privado)
- `_notificacionService` — tipo `ITraspasoNotificacionService` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `GetPeriodosAsync`

- **Firma:** `async Task<IEnumerable<PeriodoTraspasoDto>> GetPeriodosAsync()`
- **Retorno:** `Task<IEnumerable<PeriodoTraspasoDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `GetPeriodoActivoAsync`

- **Firma:** `async Task<PeriodoTraspasoDto?> GetPeriodoActivoAsync()`
- **Retorno:** `Task<PeriodoTraspasoDto?>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `FindPeriodoActivoEntityAsync(...)`

#### `CreatePeriodoAsync`

- **Firma:** `async Task<PeriodoTraspasoDto> CreatePeriodoAsync(PeriodoTraspasoCreateDto dto)`
- **Retorno:** `Task<PeriodoTraspasoDto>`
- **Parámetros:**

- `dto` (`PeriodoTraspasoCreateDto`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `GetCurrentUsuarioIdAsync(...)`, `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `UpdatePeriodoAsync`

- **Firma:** `async Task<PeriodoTraspasoDto> UpdatePeriodoAsync(int id, PeriodoTraspasoUpdateDto dto)`
- **Retorno:** `Task<PeriodoTraspasoDto>`
- **Parámetros:**

- `id` (`int`)
- `dto` (`PeriodoTraspasoUpdateDto`)

- **Qué hace:** Actualiza un recurso existente. lanza `NotFoundException` si no encuentra el recurso; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `GetSolicitudesAsync`

- **Firma:** `async Task<IEnumerable<SolicitudTraspasoDto>> GetSolicitudesAsync(string? estado = null)`
- **Retorno:** `Task<IEnumerable<SolicitudTraspasoDto>>`
- **Parámetros:**

- `estado` (`string?`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).
- **Llamadas await destacadas:** `HealLegacyPendienteOrigenAsync(...)`

#### `GetSolicitudByIdAsync`

- **Firma:** `async Task<SolicitudTraspasoDto> GetSolicitudByIdAsync(int id)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `LoadSolicitudScopedAsync(...)`

#### `GetValidacionesAsync`

- **Firma:** `async Task<TraspasoValidacionDto> GetValidacionesAsync(int id)`
- **Retorno:** `Task<TraspasoValidacionDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `LoadSolicitudScopedAsync(...)`, `BuildValidacionesAsync(...)`

#### `CrearSolicitudAsync`

- **Firma:** `async Task<SolicitudTraspasoDto> CrearSolicitudAsync(SolicitudTraspasoCreateDto dto)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `dto` (`SolicitudTraspasoCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; valida reglas de negocio y puede lanzar `BadRequestException`; carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `RequireFederacionFromClubAsync(...)`, `EnsurePeriodoActivoAsync(...)`, `EnsureMismaFederacionAsync(...)`, `EnsureSinSolicitudActivaAsync(...)`, `GetCurrentUsuarioIdAsync(...)`, `_context.SaveChangesAsync(...)`, `ReloadNavigationForDto(...)`, `_auditService.RegistrarAccionAsync(...)`, `_notificacionService.NotificarAsync(...)`

#### `AceptarOrigenAsync`

- **Firma:** `async Task<SolicitudTraspasoDto> AceptarOrigenAsync(int id)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `LoadSolicitudForOrigenAsync(...)`, `EjecutarTraspasoAsync(...)`, `ReloadNavigationForDto(...)`, `_auditService.RegistrarAccionAsync(...)`, `_notificacionService.NotificarAsync(...)`

#### `RechazarOrigenAsync`

- **Firma:** `async Task<SolicitudTraspasoDto> RechazarOrigenAsync(int id, TraspasoMotivoDto dto)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)
- `dto` (`TraspasoMotivoDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `LoadSolicitudForOrigenAsync(...)`, `_context.SaveChangesAsync(...)`, `ReloadNavigationForDto(...)`, `_auditService.RegistrarAccionAsync(...)`, `_notificacionService.NotificarAsync(...)`

#### `AprobarFederacionAsync`

- **Firma:** `async Task<SolicitudTraspasoDto> AprobarFederacionAsync(int id, bool forzar = false)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)
- `forzar` (`bool`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; puede lanzar `UnauthorizedException` por autenticación/autorización; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `LoadSolicitudForFederacionAsync(...)`, `BuildValidacionesAsync(...)`, `GetCurrentUsuarioIdAsync(...)`, `_context.SaveChangesAsync(...)`, `ReloadNavigationForDto(...)`, `_auditService.RegistrarAccionAsync(...)`, `_notificacionService.NotificarAsync(...)`

#### `RechazarFederacionAsync`

- **Firma:** `async Task<SolicitudTraspasoDto> RechazarFederacionAsync(int id, TraspasoMotivoDto dto)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)
- `dto` (`TraspasoMotivoDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `LoadSolicitudForFederacionAsync(...)`, `GetCurrentUsuarioIdAsync(...)`, `_context.SaveChangesAsync(...)`, `ReloadNavigationForDto(...)`, `_auditService.RegistrarAccionAsync(...)`, `_notificacionService.NotificarAsync(...)`

#### `CancelarAsync`

- **Firma:** `async Task<SolicitudTraspasoDto> CancelarAsync(int id)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; valida reglas de negocio y puede lanzar `BadRequestException`; puede lanzar `UnauthorizedException` por autenticación/autorización; carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`, `_notificacionService.NotificarAsync(...)`

#### `BuscarAtletasAsync`

- **Firma:** `async Task<IEnumerable<AtletaTraspasoBusquedaDto>> BuscarAtletasAsync(string term)`
- **Retorno:** `Task<IEnumerable<AtletaTraspasoBusquedaDto>>`
- **Parámetros:**

- `term` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; puede lanzar `UnauthorizedException` por autenticación/autorización; carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).
- **Llamadas await destacadas:** `RequireFederacionFromClubAsync(...)`

#### `GetAuditoriaAsync`

- **Firma:** `async Task<IEnumerable<TraspasoAuditoriaDto>> GetAuditoriaAsync(int limit = 50)`
- **Retorno:** `Task<IEnumerable<TraspasoAuditoriaDto>>`
- **Parámetros:**

- `limit` (`int`)

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `ExportSolicitudesCsvAsync`

- **Firma:** `async Task<byte[]> ExportSolicitudesCsvAsync(int? periodoId = null, string? estado = null)`
- **Retorno:** `Task<byte[]>`
- **Parámetros:**

- `periodoId` (`int?`)
- `estado` (`string?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.PeriodosTraspaso.AsNoTracking(...)`, `query.OrderByDescending(...)`

#### `EjecutarTraspasoAsync`

- **Firma:** `async Task EjecutarTraspasoAsync(SolicitudTraspaso solicitud, bool forzado)`
- **Retorno:** `Task`
- **Parámetros:**

- `solicitud` (`SolicitudTraspaso`)
- `forzado` (`bool`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; persiste cambios con `SaveChangesAsync`; usa transacción de base de datos; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Database.BeginTransactionAsync(...)`, `GetCurrentUsuarioIdAsync(...)`, `_context.SaveChangesAsync(...)`, `tx.CommitAsync(...)`, `_auditService.RegistrarAccionAsync(...)`, `tx.RollbackAsync(...)`

#### `BuildValidacionesAsync`

- **Firma:** `async Task<TraspasoValidacionDto> BuildValidacionesAsync(SolicitudTraspaso solicitud)`
- **Retorno:** `Task<TraspasoValidacionDto>`
- **Parámetros:**

- `solicitud` (`SolicitudTraspaso`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `FindPeriodoActivoEntityAsync(...)`, `_context.Clubes.AsNoTracking(...)`, `_context.AtletasFederados.AsNoTracking(...)`

#### `BuildPagoClubItem`

- **Firma:** `TraspasoValidacionItemDto BuildPagoClubItem(string codigo, string desc, SportTrack_Sigdef.Entidades.Entidades.Club? club)`
- **Retorno:** `TraspasoValidacionItemDto`
- **Parámetros:**

- `codigo` (`string`)
- `desc` (`string`)
- `club` (`SportTrack_Sigdef.Entidades.Entidades.Club?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ApplySolicitudScope`

- **Firma:** `IQueryable<SolicitudTraspaso> ApplySolicitudScope(IQueryable<SolicitudTraspaso> query)`
- **Retorno:** `IQueryable<SolicitudTraspaso>`
- **Parámetros:**

- `query` (`IQueryable<SolicitudTraspaso>`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. puede lanzar `UnauthorizedException` por autenticación/autorización; filtra con LINQ (`Where`).

#### `LoadSolicitudScopedAsync`

- **Firma:** `async Task<SolicitudTraspaso> LoadSolicitudScopedAsync(int id)`
- **Retorno:** `Task<SolicitudTraspaso>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `LoadSolicitudForOrigenAsync`

- **Firma:** `async Task<SolicitudTraspaso> LoadSolicitudForOrigenAsync(int id)`
- **Retorno:** `Task<SolicitudTraspaso>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; puede lanzar `UnauthorizedException` por autenticación/autorización; carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `LoadSolicitudForFederacionAsync`

- **Firma:** `async Task<SolicitudTraspaso> LoadSolicitudForFederacionAsync(int id)`
- **Retorno:** `Task<SolicitudTraspaso>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `HealLegacyPendienteOrigenAsync`

- **Firma:** `async Task HealLegacyPendienteOrigenAsync()`
- **Retorno:** `Task`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `EnsurePeriodoActivoAsync`

- **Firma:** `async Task EnsurePeriodoActivoAsync(int idFederacion)`
- **Retorno:** `Task`
- **Parámetros:**

- `idFederacion` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `FindPeriodoActivoEntityAsync(...)`

#### `FindPeriodoActivoEntityAsync`

- **Firma:** `async Task<PeriodoTraspaso?> FindPeriodoActivoEntityAsync(int idFederacion, DateTime utcNow)`
- **Retorno:** `Task<PeriodoTraspaso?>`
- **Parámetros:**

- `idFederacion` (`int`)
- `utcNow` (`DateTime`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `EnsureSinSolicitudActivaAsync`

- **Firma:** `async Task EnsureSinSolicitudActivaAsync(int participanteId)`
- **Retorno:** `Task`
- **Parámetros:**

- `participanteId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SolicitudesTraspaso.AnyAsync(...)`

#### `EnsureMismaFederacionAsync`

- **Firma:** `async Task EnsureMismaFederacionAsync(int clubOrigen, int clubDestino, int fedId)`
- **Retorno:** `Task`
- **Parámetros:**

- `clubOrigen` (`int`)
- `clubDestino` (`int`)
- `fedId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Clubes.AsNoTracking(...)`

#### `RequireClubDestinoId`

- **Firma:** `int RequireClubDestinoId(int dtoClubDestino)`
- **Retorno:** `int`
- **Parámetros:**

- `dtoClubDestino` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. puede lanzar `UnauthorizedException` por autenticación/autorización.

#### `RequireFederacionFromClubAsync`

- **Firma:** `async Task<int> RequireFederacionFromClubAsync(int clubId)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; consulta en modo solo-lectura (`AsNoTracking`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Clubes.AsNoTracking(...)`

#### `RequireFederacionId`

- **Firma:** `int RequireFederacionId()`
- **Retorno:** `int`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. puede lanzar `UnauthorizedException` por autenticación/autorización.

#### `ResolveFederacionId`

- **Firma:** `int? ResolveFederacionId()`
- **Retorno:** `int?`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `RequireFedAdmin`

- **Firma:** `void RequireFedAdmin()`
- **Retorno:** `void`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. puede lanzar `UnauthorizedException` por autenticación/autorización.

#### `IsFedAdmin`

- **Firma:** `bool IsFedAdmin()`
- **Retorno:** `bool`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `IsGlobalAdmin`

- **Firma:** `bool IsGlobalAdmin()`
- **Retorno:** `bool`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetCurrentUsuarioIdAsync`

- **Firma:** `async Task<int?> GetCurrentUsuarioIdAsync()`
- **Retorno:** `Task<int?>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Usuarios.AsNoTracking(...)`

#### `ReloadNavigationForDto`

- **Firma:** `async Task ReloadNavigationForDto(SolicitudTraspaso solicitud)`
- **Retorno:** `Task`
- **Parámetros:**

- `solicitud` (`SolicitudTraspaso`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Entry(...)`

#### `ValidateRangoFechas`

- **Firma:** `void ValidateRangoFechas(DateTime inicio, DateTime fin)`
- **Retorno:** `void`
- **Parámetros:**

- `inicio` (`DateTime`)
- `fin` (`DateTime`)

- **Qué hace:** Valida reglas de negocio. valida reglas de negocio y puede lanzar `BadRequestException`.

#### `ToUtc`

- **Firma:** `DateTime ToUtc(DateTime value)`
- **Retorno:** `DateTime`
- **Parámetros:**

- `value` (`DateTime`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ToUtcEndOfDay`

- **Firma:** `DateTime ToUtcEndOfDay(DateTime value)`
- **Retorno:** `DateTime`
- **Parámetros:**

- `value` (`DateTime`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `MapPeriodo`

- **Firma:** `PeriodoTraspasoDto MapPeriodo(PeriodoTraspaso p, DateTime now)`
- **Retorno:** `PeriodoTraspasoDto`
- **Parámetros:**

- `p` (`PeriodoTraspaso`)
- `now` (`DateTime`)

- **Qué hace:** Configura o aplica mapeos.

#### `MapSolicitud`

- **Firma:** `SolicitudTraspasoDto MapSolicitud(SolicitudTraspaso s)`
- **Retorno:** `SolicitudTraspasoDto`
- **Parámetros:**

- `s` (`SolicitudTraspaso`)

- **Qué hace:** Configura o aplica mapeos.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/TraspasoService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
