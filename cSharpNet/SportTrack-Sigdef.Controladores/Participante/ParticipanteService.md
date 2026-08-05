# ParticipanteService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Participante/ParticipanteService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IParticipanteService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **AutoMapper**: biblioteca que mapea propiedades entre entidades y DTOs (`CreateMap`, `_mapper.Map`).
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Participante`
- **Usings:**
  - `using AutoMapper;`
  - `using SportTrack_Sigdef.Controladores.Exceptions;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`
  - `using SportTrack_Sigdef.Controladores.Participante.Dtos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_participanteRepository` — tipo `IParticipanteRepository` (típicamente dependencia inyectada o estado privado)
- `_clubRepository` — tipo `Club.IClubRepository` (típicamente dependencia inyectada o estado privado)
- `_mapper` — tipo `IMapper` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `Audit.IAuditService` (típicamente dependencia inyectada o estado privado)
- `_altaAtletaService` — tipo `IAltaAtletaService` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `GetAllParticipantesAsync`

- **Firma:** `async Task<IEnumerable<ParticipanteDto>> GetAllParticipantesAsync(int? clubId = null, string? rol = null, int? federacionId = null)`
- **Retorno:** `Task<IEnumerable<ParticipanteDto>>`
- **Parámetros:**

- `clubId` (`int?`)
- `rol` (`string?`)
- `federacionId` (`int?`)

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_participanteRepository.GetAllAsync(...)`, `_participanteRepository.GetByFederationIdAsync(...)`, `_participanteRepository.GetByClubIdAsync(...)`

#### `GetParticipanteByIdAsync`

- **Firma:** `async Task<ParticipanteDto> GetParticipanteByIdAsync(int id)`
- **Retorno:** `Task<ParticipanteDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_participanteRepository.GetByIdAsync(...)`

#### `GetParticipantesByClubAsync`

- **Firma:** `async Task<IEnumerable<ParticipanteDto>> GetParticipantesByClubAsync(int clubId)`
- **Retorno:** `Task<IEnumerable<ParticipanteDto>>`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_participanteRepository.GetByClubIdAsync(...)`

#### `CreateParticipanteAsync`

- **Firma:** `async Task<ParticipanteDto> CreateParticipanteAsync(ParticipanteCreateDto participanteDto)`
- **Retorno:** `Task<ParticipanteDto>`
- **Parámetros:**

- `participanteDto` (`ParticipanteCreateDto`)

- **Qué hace:** Crea/registra un nuevo recurso. valida reglas de negocio y puede lanzar `BadRequestException`; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_clubRepository.GetByIdAsync(...)`, `_participanteRepository.CountByFederationIdAsync(...)`, `_participanteRepository.CountByClubIdAsync(...)`, `_altaAtletaService.AltaAtletaCompletaAsync(...)`, `_auditService.RegistrarAccionAsync(...)`, `_participanteRepository.GetByIdAsync(...)`

#### `UpdateParticipanteAsync`

- **Firma:** `async Task<ParticipanteDto> UpdateParticipanteAsync(int id, ParticipanteCreateDto participanteDto)`
- **Retorno:** `Task<ParticipanteDto>`
- **Parámetros:**

- `id` (`int`)
- `participanteDto` (`ParticipanteCreateDto`)

- **Qué hace:** Actualiza un recurso existente. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_participanteRepository.GetByIdAsync(...)`, `_participanteRepository.UpdateAsync(...)`, `_altaAtletaService.EnsureAtletaFederacionAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `DeleteParticipanteAsync`

- **Firma:** `async Task<bool> DeleteParticipanteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. lanza `NotFoundException` si no encuentra el recurso; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_participanteRepository.GetByIdAsync(...)`, `_participanteRepository.DeleteAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- Revisá `Mappings/MappingProfile.cs` para ver cómo se mapea esta entidad/DTO.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Participante/ParticipanteService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
