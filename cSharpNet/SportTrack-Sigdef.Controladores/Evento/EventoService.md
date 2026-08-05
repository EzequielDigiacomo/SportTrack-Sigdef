# EventoService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Evento/EventoService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IEventoService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **AutoMapper**: biblioteca que mapea propiedades entre entidades y DTOs (`CreateMap`, `_mapper.Map`).
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Evento`
- **Usings:**
  - `using AutoMapper;`
  - `using SportTrack_Sigdef.Controladores.Caching;`
  - `using SportTrack_Sigdef.Controladores.Evento.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Exceptions;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `LiveReadTtl` — tipo `TimeSpan` (típicamente dependencia inyectada o estado privado)
- `_eventoRepository` — tipo `IEventoRepository` (típicamente dependencia inyectada o estado privado)
- `_mapper` — tipo `IMapper` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `Audit.IAuditService` (típicamente dependencia inyectada o estado privado)
- `_estadoSyncService` — tipo `IEventoEstadoSyncService` (típicamente dependencia inyectada o estado privado)
- `_liveCache` — tipo `ILiveCacheService` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `GetAllEventosAsync`

- **Firma:** `async Task<IEnumerable<EventoDto>> GetAllEventosAsync(int? clubId = null, string? rol = null)`
- **Retorno:** `Task<IEnumerable<EventoDto>>`
- **Parámetros:**

- `clubId` (`int?`)
- `rol` (`string?`)

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_estadoSyncService.SyncAllAsync(...)`, `_eventoRepository.GetAllAsync(...)`

#### `GetEventoByIdAsync`

- **Firma:** `async Task<EventoDto> GetEventoByIdAsync(int id)`
- **Retorno:** `Task<EventoDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_liveCache.GetOrCreateAsync(...)`, `_estadoSyncService.SyncEventoAsync(...)`, `_eventoRepository.GetByIdAsync(...)`

#### `CreateEventoAsync`

- **Firma:** `async Task<EventoDto> CreateEventoAsync(EventoCreateDto eventoDto)`
- **Retorno:** `Task<EventoDto>`
- **Parámetros:**

- `eventoDto` (`EventoCreateDto`)

- **Qué hace:** Crea/registra un nuevo recurso. usa AutoMapper para convertir entre entidad y DTO; interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_eventoRepository.CreateAsync(...)`, `_eventoRepository.GetByIdAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `UpdateEventoAsync`

- **Firma:** `async Task<EventoDto> UpdateEventoAsync(int id, EventoUpdateDto eventoDto, int? clubId = null)`
- **Retorno:** `Task<EventoDto>`
- **Parámetros:**

- `id` (`int`)
- `eventoDto` (`EventoUpdateDto`)
- `clubId` (`int?`)

- **Qué hace:** Actualiza un recurso existente. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_eventoRepository.GetByIdAsync(...)`, `_eventoRepository.UpdateAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `DeleteEventoAsync`

- **Firma:** `async Task<bool> DeleteEventoAsync(int id, int? clubId = null)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)
- `clubId` (`int?`)

- **Qué hace:** Elimina o desactiva un recurso. lanza `NotFoundException` si no encuentra el recurso; interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_eventoRepository.GetByIdAsync(...)`, `_eventoRepository.DeleteAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `GetProximosEventosAsync`

- **Firma:** `async Task<IEnumerable<EventoDto>> GetProximosEventosAsync(int? clubId = null, string? rol = null)`
- **Retorno:** `Task<IEnumerable<EventoDto>>`
- **Parámetros:**

- `clubId` (`int?`)
- `rol` (`string?`)

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_estadoSyncService.SyncAllAsync(...)`, `_eventoRepository.GetProximosAsync(...)`

#### `GetPruebasByEventoAsync`

- **Firma:** `async Task<IEnumerable<EventoPruebaDto>> GetPruebasByEventoAsync(int eventoId)`
- **Retorno:** `Task<IEnumerable<EventoPruebaDto>>`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_liveCache.GetOrCreateAsync(...)`, `_eventoRepository.GetPruebasByEventoIdAsync(...)`

#### `AssignPruebaToEventoAsync`

- **Firma:** `async Task<EventoPruebaDto> AssignPruebaToEventoAsync(int eventoId, EventoPruebaCreateDto assignDto)`
- **Retorno:** `Task<EventoPruebaDto>`
- **Parámetros:**

- `eventoId` (`int`)
- `assignDto` (`EventoPruebaCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. usa AutoMapper para convertir entre entidad y DTO; interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_eventoRepository.GetPruebaAsync(...)`, `_eventoRepository.CreatePruebaAsync(...)`, `_eventoRepository.AssignPruebaAsync(...)`

#### `UpdateEventoPruebaAsync`

- **Firma:** `async Task<EventoPruebaDto> UpdateEventoPruebaAsync(int eventoPruebaId, EventoPruebaCreateDto updateDto)`
- **Retorno:** `Task<EventoPruebaDto>`
- **Parámetros:**

- `eventoPruebaId` (`int`)
- `updateDto` (`EventoPruebaCreateDto`)

- **Qué hace:** Actualiza un recurso existente. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_eventoRepository.GetEventoPruebaByIdAsync(...)`, `_eventoRepository.GetPruebaAsync(...)`, `_eventoRepository.CreatePruebaAsync(...)`, `_eventoRepository.UpdateEventoPruebaAsync(...)`

#### `DeleteEventoPruebaAsync`

- **Firma:** `async Task<bool> DeleteEventoPruebaAsync(int eventoPruebaId)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. interactúa con caché; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_eventoRepository.GetEventoPruebaByIdAsync(...)`, `_eventoRepository.UnassignPruebaAsync(...)`

#### `MapDistanciaToEnum`

- **Firma:** `DistanciaRegata MapDistanciaToEnum(int distanciaId)`
- **Retorno:** `DistanciaRegata`
- **Parámetros:**

- `distanciaId` (`int`)

- **Qué hace:** Configura o aplica mapeos.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- Revisá `Mappings/MappingProfile.cs` para ver cómo se mapea esta entidad/DTO.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Evento/EventoService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
