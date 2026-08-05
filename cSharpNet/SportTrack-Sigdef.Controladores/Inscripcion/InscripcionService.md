# InscripcionService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Inscripcion/InscripcionService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IInscripcionService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **AutoMapper**: biblioteca que mapea propiedades entre entidades y DTOs (`CreateMap`, `_mapper.Map`).
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Inscripcion`
- **Usings:**
  - `using AutoMapper;`
  - `using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Inscripcion;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.Exceptions;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_inscripcionRepository` — tipo `IInscripcionRepository` (típicamente dependencia inyectada o estado privado)
- `_mapper` — tipo `IMapper` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `Audit.IAuditService` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `InscripcionService(...)`

**Parámetros:**

- `inscripcionRepository` (`IInscripcionRepository`)
- `mapper` (`IMapper`)
- `auditService` (`Audit.IAuditService`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetAllInscripcionesAsync`

- **Firma:** `async Task<IEnumerable<InscripcionDto>> GetAllInscripcionesAsync()`
- **Retorno:** `Task<IEnumerable<InscripcionDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_inscripcionRepository.GetAllAsync(...)`

#### `GetInscripcionByIdAsync`

- **Firma:** `async Task<InscripcionDto> GetInscripcionByIdAsync(int id)`
- **Retorno:** `Task<InscripcionDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_inscripcionRepository.GetByIdAsync(...)`

#### `CreateInscripcionAsync`

- **Firma:** `async Task<InscripcionDto> CreateInscripcionAsync(InscripcionCreateDto inscripcionDto)`
- **Retorno:** `Task<InscripcionDto>`
- **Parámetros:**

- `inscripcionDto` (`InscripcionCreateDto`)

- **Qué hace:** Crea/registra un nuevo recurso. valida reglas de negocio y puede lanzar `BadRequestException`; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_inscripcionRepository.CreateAsync(...)`, `_inscripcionRepository.GetByIdAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `UpdateInscripcionAsync`

- **Firma:** `async Task<InscripcionDto> UpdateInscripcionAsync(int id, InscripcionUpdateDto inscripcionDto)`
- **Retorno:** `Task<InscripcionDto>`
- **Parámetros:**

- `id` (`int`)
- `inscripcionDto` (`InscripcionUpdateDto`)

- **Qué hace:** Actualiza un recurso existente. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_inscripcionRepository.GetByIdAsync(...)`, `_inscripcionRepository.UpdateAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `DeleteInscripcionAsync`

- **Firma:** `async Task<bool> DeleteInscripcionAsync(int id, bool allowWhenClosed = false)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)
- `allowWhenClosed` (`bool`)

- **Qué hace:** Elimina o desactiva un recurso. lanza `NotFoundException` si no encuentra el recurso; valida reglas de negocio y puede lanzar `BadRequestException`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_inscripcionRepository.GetByIdAsync(...)`, `_inscripcionRepository.DeleteAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `EventoPermiteModificarInscripciones`

- **Firma:** `bool EventoPermiteModificarInscripciones(SportTrack_Sigdef.Entidades.Entidades.Evento evento)`
- **Retorno:** `bool`
- **Parámetros:**

- `evento` (`SportTrack_Sigdef.Entidades.Entidades.Evento`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetCountByEventoPruebaIdAsync`

- **Firma:** `async Task<int> GetCountByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_inscripcionRepository.CountByEventoPruebaIdAsync(...)`

#### `GetInscripcionesByEventoPruebaIdAsync`

- **Firma:** `async Task<IEnumerable<InscripcionDto>> GetInscripcionesByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<InscripcionDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_inscripcionRepository.GetByEventoPruebaIdAsync(...)`

#### `GetInscripcionesByEventoAndClubAsync`

- **Firma:** `async Task<IEnumerable<InscripcionDto>> GetInscripcionesByEventoAndClubAsync(int eventoId, int clubId)`
- **Retorno:** `Task<IEnumerable<InscripcionDto>>`
- **Parámetros:**

- `eventoId` (`int`)
- `clubId` (`int`)

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_inscripcionRepository.GetByEventoAndClubAsync(...)`

#### `ToggleEsCabezaDeSerieAsync`

- **Firma:** `async Task<bool> ToggleEsCabezaDeSerieAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; valida reglas de negocio y puede lanzar `BadRequestException`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_inscripcionRepository.GetByIdAsync(...)`, `_inscripcionRepository.GetByEventoPruebaIdAsync(...)`, `_inscripcionRepository.UpdateAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `MapToRegistroDto`

- **Firma:** `RegistroInscripcionDto MapToRegistroDto(Entidades.Entidades.Inscripcion i)`
- **Retorno:** `RegistroInscripcionDto`
- **Parámetros:**

- `i` (`Entidades.Entidades.Inscripcion`)

- **Qué hace:** Configura o aplica mapeos. filtra con LINQ (`Where`).

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- Revisá `Mappings/MappingProfile.cs` para ver cómo se mapea esta entidad/DTO.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Inscripcion/InscripcionService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
