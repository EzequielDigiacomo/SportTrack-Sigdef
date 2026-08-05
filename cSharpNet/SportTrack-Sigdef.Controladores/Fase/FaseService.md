# IFaseService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Fase/FaseService.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `interface IFaseService`
- `class FaseService` : `IFaseService`

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **AutoMapper**: biblioteca que mapea propiedades entre entidades y DTOs (`CreateMap`, `_mapper.Map`).
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **SignalR Hub**: canal en tiempo real hacia clientes conectados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Fase`
- **Usings:**
  - `using AutoMapper;`
  - `using SportTrack_Sigdef.Controladores.Caching;`
  - `using SportTrack_Sigdef.Controladores.Fase.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Hubs;`
  - `using SportTrack_Sigdef.Controladores.Inscripcion;`
  - `using SportTrack_Sigdef.Controladores.Evento;`
  - `using SportTrack_Sigdef.Controladores.Fase.Progression;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using Microsoft.AspNetCore.SignalR;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — `interface IFaseService`

### Métodos

#### `GetFasesPorEventoPruebaAsync`

- **Firma:** `Task<IEnumerable<FaseDto>> GetFasesPorEventoPruebaAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<FaseDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GenerarFasesAutoAsync`

- **Firma:** `Task<IEnumerable<FaseDto>> GenerarFasesAutoAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<FaseDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PromoverFasesAsync`

- **Firma:** `Task<IEnumerable<FaseDto>> PromoverFasesAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<FaseDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `IniciarFaseAsync`

- **Firma:** `Task<FaseDto> IniciarFaseAsync(int id, DateTime? manualStartTime = null)`
- **Retorno:** `Task<FaseDto>`
- **Parámetros:**

- `id` (`int`)
- `manualStartTime` (`DateTime?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `FinalizarFaseAsync`

- **Firma:** `Task<FaseDto> FinalizarFaseAsync(int id)`
- **Retorno:** `Task<FaseDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteFaseAsync`

- **Firma:** `Task<bool> DeleteFaseAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ReiniciarFaseAsync`

- **Firma:** `Task<FaseDto> ReiniciarFaseAsync(int id)`
- **Retorno:** `Task<FaseDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `EnviarARevisionAsync`

- **Firma:** `Task<FaseDto> EnviarARevisionAsync(int id)`
- **Retorno:** `Task<FaseDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetFasesPorEventoAsync`

- **Firma:** `Task<IEnumerable<FaseDto>> GetFasesPorEventoAsync(int eventoId)`
- **Retorno:** `Task<IEnumerable<FaseDto>>`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `BatchUpdateFasesAsync`

- **Firma:** `Task BatchUpdateFasesAsync(List<FaseBatchUpdateDto> dto)`
- **Retorno:** `Task`
- **Parámetros:**

- `dto` (`List<FaseBatchUpdateDto>`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GenerarFasesManualAsync`

- **Firma:** `Task<IEnumerable<FaseDto>> GenerarFasesManualAsync(int eventoPruebaId, List<ManualPlacementDto> placements)`
- **Retorno:** `Task<IEnumerable<FaseDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)
- `placements` (`List<ManualPlacementDto>`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateResultadoStatusAsync`

- **Firma:** `Task UpdateResultadoStatusAsync(int resultadoId, string status)`
- **Retorno:** `Task`
- **Parámetros:**

- `resultadoId` (`int`)
- `status` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetProgressionAuditAsync`

- **Firma:** `Task<IEnumerable<ProgressionAuditDto>> GetProgressionAuditAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<ProgressionAuditDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEventoIdByFaseIdAsync`

- **Firma:** `Task<int?> GetEventoIdByFaseIdAsync(int faseId)`
- **Retorno:** `Task<int?>`
- **Parámetros:**

- `faseId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 4. Detalle del tipo — `class FaseService`

### Campos (dependencias / estado)

- `LiveReadTtl` — tipo `TimeSpan` (típicamente dependencia inyectada o estado privado)
- `_faseRepository` — tipo `IFaseRepository` (típicamente dependencia inyectada o estado privado)
- `_etapaRepository` — tipo `IEtapaRepository` (típicamente dependencia inyectada o estado privado)
- `_inscripcionRepository` — tipo `IInscripcionRepository` (típicamente dependencia inyectada o estado privado)
- `_eventoRepository` — tipo `IEventoRepository` (típicamente dependencia inyectada o estado privado)
- `_hubContext` — tipo `Microsoft.AspNetCore.SignalR.IHubContext<SportTrack_Sigdef.Controladores.Hubs.TimingHub>` (típicamente dependencia inyectada o estado privado)
- `_mapper` — tipo `IMapper` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `Audit.IAuditService` (típicamente dependencia inyectada o estado privado)
- `_liveCache` — tipo `ILiveCacheService` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `GetEventoIdByFaseIdAsync`

- **Firma:** `async Task<int?> GetEventoIdByFaseIdAsync(int faseId)`
- **Retorno:** `Task<int?>`
- **Parámetros:**

- `faseId` (`int`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetFasesPorEventoPruebaAsync`

- **Firma:** `async Task<IEnumerable<FaseDto>> GetFasesPorEventoPruebaAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<FaseDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_liveCache.GetOrCreateAsync(...)`, `_faseRepository.GetByEventoPruebaIdAsync(...)`

#### `GetFasesPorEventoPruebaFreshAsync`

- **Firma:** `async Task<IEnumerable<FaseDto>> GetFasesPorEventoPruebaFreshAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<FaseDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `InvalidateByEventoPruebaAsync(...)`, `GetFasesPorEventoPruebaAsync(...)`

#### `GenerarFasesAutoAsync`

- **Firma:** `async Task<IEnumerable<FaseDto>> GenerarFasesAutoAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<FaseDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. ordena resultados; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseRepository.GetByEventoPruebaIdAsync(...)`, `_etapaRepository.DeleteByEventoPruebaIdAsync(...)`, `_inscripcionRepository.GetByEventoPruebaIdAsync(...)`, `_eventoRepository.GetEventoPruebaByIdAsync(...)`, `_eventoRepository.UpdateEventoPruebaAsync(...)`, `_etapaRepository.CreateAsync(...)`, `_faseRepository.CreateAsync(...)`, `PreGenerarSiguientesEtapasAsync(...)`, `_auditService.RegistrarAccionAsync(...)`, `GetFasesPorEventoPruebaFreshAsync(...)`

#### `PreGenerarSiguientesEtapasAsync`

- **Firma:** `async Task PreGenerarSiguientesEtapasAsync(int eventoPruebaId, int inscriptosCount, int numSeries, DateTime nextTime)`
- **Retorno:** `Task`
- **Parámetros:**

- `eventoPruebaId` (`int`)
- `inscriptosCount` (`int`)
- `numSeries` (`int`)
- `nextTime` (`DateTime`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_etapaRepository.CreateAsync(...)`, `_faseRepository.CreateAsync(...)`

#### `DeterminarPlanProgresion`

- **Firma:** `string DeterminarPlanProgresion(int count)`
- **Retorno:** `string`
- **Parámetros:**

- `count` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetSemifinalCount`

- **Firma:** `int GetSemifinalCount(int numHeats)`
- **Retorno:** `int`
- **Parámetros:**

- `numHeats` (`int`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetFinalCount`

- **Firma:** `int GetFinalCount(int numHeats)`
- **Retorno:** `int`
- **Parámetros:**

- `numHeats` (`int`)

- **Qué hace:** Obtiene/consulta datos.

#### `CrearFaseConResultados`

- **Firma:** `Entidades.Entidades.Fase CrearFaseConResultados(int etapaId, string nombreFase, int numeroFase, List<Entidades.Entidades.Inscripcion> inscripcionesBase, DateTime? fechaHora = null)`
- **Retorno:** `Entidades.Entidades.Fase`
- **Parámetros:**

- `etapaId` (`int`)
- `nombreFase` (`string`)
- `numeroFase` (`int`)
- `inscripcionesBase` (`List<Entidades.Entidades.Inscripcion>`)
- `fechaHora` (`DateTime?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. filtra con LINQ (`Where`).

#### `PromoverFasesAsync`

- **Firma:** `async Task<IEnumerable<FaseDto>> PromoverFasesAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<FaseDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. filtra con LINQ (`Where`); ordena resultados; agrupa datos; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseRepository.GetByEventoPruebaIdAsync(...)`, `_etapaRepository.DeleteAsync(...)`, `_eventoRepository.GetEventoPruebaByIdAsync(...)`, `_inscripcionRepository.GetByEventoPruebaIdAsync(...)`, `_eventoRepository.UpdateEventoPruebaAsync(...)`, `AplicarProgresionIcfAsync(...)`, `_auditService.RegistrarAccionAsync(...)`, `GetFasesPorEventoPruebaFreshAsync(...)`

#### `GetProgressionAuditAsync`

- **Firma:** `async Task<IEnumerable<ProgressionAuditDto>> GetProgressionAuditAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<ProgressionAuditDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Obtiene/consulta datos. filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseRepository.GetByEventoPruebaIdAsync(...)`, `_eventoRepository.GetEventoPruebaByIdAsync(...)`, `_inscripcionRepository.GetByEventoPruebaIdAsync(...)`

#### `IniciarFaseAsync`

- **Firma:** `async Task<FaseDto> IniciarFaseAsync(int id, DateTime? manualStartTime = null)`
- **Retorno:** `Task<FaseDto>`
- **Parámetros:**

- `id` (`int`)
- `manualStartTime` (`DateTime?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; interactúa con caché; notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseRepository.GetByIdAsync(...)`, `_faseRepository.UpdateAsync(...)`, `_auditService.RegistrarAccionAsync(...)`, `ResolveFaseScopeAsync(...)`, `_hubContext.Clients.Group(...)`, `BroadcastToEventAndOperatorsAsync(...)`

#### `FinalizarFaseAsync`

- **Firma:** `async Task<FaseDto> FinalizarFaseAsync(int id)`
- **Retorno:** `Task<FaseDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; filtra con LINQ (`Where`); ordena resultados; interactúa con caché; notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseRepository.GetByIdAsync(...)`, `_faseRepository.UpdateAsync(...)`, `_auditService.RegistrarAccionAsync(...)`, `ResolveFaseScopeAsync(...)`, `_hubContext.Clients.Group(...)`, `BroadcastToEventAndOperatorsAsync(...)`

#### `DeleteFaseAsync`

- **Firma:** `async Task<bool> DeleteFaseAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseRepository.GetScopeByFaseIdAsync(...)`, `_faseRepository.DeleteAsync(...)`

#### `ReiniciarFaseAsync`

- **Firma:** `async Task<FaseDto> ReiniciarFaseAsync(int id)`
- **Retorno:** `Task<FaseDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; interactúa con caché; notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseRepository.GetByIdAsync(...)`, `_faseRepository.UpdateAsync(...)`, `_auditService.RegistrarAccionAsync(...)`, `ResolveFaseScopeAsync(...)`, `_hubContext.Clients.Group(...)`

#### `EnviarARevisionAsync`

- **Firma:** `async Task<FaseDto> EnviarARevisionAsync(int id)`
- **Retorno:** `Task<FaseDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; interactúa con caché; notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseRepository.GetByIdAsync(...)`, `_faseRepository.UpdateAsync(...)`, `_auditService.RegistrarAccionAsync(...)`, `ResolveFaseScopeAsync(...)`, `_hubContext.Clients.Group(...)`, `BroadcastToEventAndOperatorsAsync(...)`

#### `GetFasesPorEventoAsync`

- **Firma:** `async Task<IEnumerable<FaseDto>> GetFasesPorEventoAsync(int eventoId)`
- **Retorno:** `Task<IEnumerable<FaseDto>>`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_liveCache.GetOrCreateAsync(...)`, `_faseRepository.GetByEventoIdAsync(...)`

#### `BatchUpdateFasesAsync`

- **Firma:** `async Task BatchUpdateFasesAsync(List<FaseBatchUpdateDto> dto)`
- **Retorno:** `Task`
- **Parámetros:**

- `dto` (`List<FaseBatchUpdateDto>`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseRepository.GetByIdAsync(...)`, `_faseRepository.UpdateAsync(...)`, `ResolveFaseScopeAsync(...)`

#### `GetUtcTime`

- **Firma:** `DateTime GetUtcTime(DateTime localDateTime, string timeZoneId)`
- **Retorno:** `DateTime`
- **Parámetros:**

- `localDateTime` (`DateTime`)
- `timeZoneId` (`string`)

- **Qué hace:** Obtiene/consulta datos.

#### `GenerarFasesManualAsync`

- **Firma:** `async Task<IEnumerable<FaseDto>> GenerarFasesManualAsync(int eventoPruebaId, List<ManualPlacementDto> placements)`
- **Retorno:** `Task<IEnumerable<FaseDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)
- `placements` (`List<ManualPlacementDto>`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. filtra con LINQ (`Where`); ordena resultados; agrupa datos; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_etapaRepository.DeleteByEventoPruebaIdAsync(...)`, `_eventoRepository.GetEventoPruebaByIdAsync(...)`, `_eventoRepository.UpdateEventoPruebaAsync(...)`, `_etapaRepository.CreateAsync(...)`, `_faseRepository.CreateAsync(...)`, `PreGenerarSiguientesEtapasAsync(...)`, `GetFasesPorEventoPruebaFreshAsync(...)`

#### `UpdateResultadoStatusAsync`

- **Firma:** `async Task UpdateResultadoStatusAsync(int resultadoId, string status)`
- **Retorno:** `Task`
- **Parámetros:**

- `resultadoId` (`int`)
- `status` (`string`)

- **Qué hace:** Actualiza un recurso existente. interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseRepository.GetResultadoByIdAsync(...)`, `_faseRepository.UpdateResultadoAsync(...)`, `_faseRepository.GetEventoIdByResultadoIdAsync(...)`, `BroadcastToEventAndOperatorsAsync(...)`

#### `BroadcastToEventAndOperatorsAsync`

- **Firma:** `async Task BroadcastToEventAndOperatorsAsync(int? eventoId, string method, params object?[] args)`
- **Retorno:** `Task`
- **Parámetros:**

- `eventoId` (`int?`)
- `method` (`string`)
- `args` (`object?[]`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_hubContext.Clients.Group(...)`

#### `InvalidateByEventoPruebaAsync`

- **Firma:** `async Task InvalidateByEventoPruebaAsync(int eventoPruebaId)`
- **Retorno:** `Task`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_eventoRepository.GetEventoPruebaByIdAsync(...)`

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- Revisá `Mappings/MappingProfile.cs` para ver cómo se mapea esta entidad/DTO.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La progresión de series/fases es lógica de negocio densa: leé primero los modelos y luego el engine.
- Ruta relativa en el proyecto: `Fase/FaseService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
