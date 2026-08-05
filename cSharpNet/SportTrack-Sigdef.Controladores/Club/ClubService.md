# ClubService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Club/ClubService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IClubService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **AutoMapper**: biblioteca que mapea propiedades entre entidades y DTOs (`CreateMap`, `_mapper.Map`).
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Club`
- **Usings:**
  - `using AutoMapper;`
  - `using SportTrack_Sigdef.Controladores.Club.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Exceptions;`
  - `using SportTrack_Sigdef.Controladores.Audit;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_clubRepository` — tipo `IClubRepository` (típicamente dependencia inyectada o estado privado)
- `_mapper` — tipo `IMapper` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `IAuditService` (típicamente dependencia inyectada o estado privado)
- `_tenantProvider` — tipo `ITenantProvider` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `ClubService(...)`

**Parámetros:**

- `clubRepository` (`IClubRepository`)
- `mapper` (`IMapper`)
- `auditService` (`IAuditService`)
- `tenantProvider` (`ITenantProvider`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetAllClubesAsync`

- **Firma:** `async Task<IEnumerable<ClubDto>> GetAllClubesAsync()`
- **Retorno:** `Task<IEnumerable<ClubDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_clubRepository.GetAllAsync(...)`

#### `GetClubByIdAsync`

- **Firma:** `async Task<ClubDto> GetClubByIdAsync(int id)`
- **Retorno:** `Task<ClubDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_clubRepository.GetByIdAsync(...)`

#### `CreateClubAsync`

- **Firma:** `async Task<ClubDto> CreateClubAsync(ClubCreateDto clubDto)`
- **Retorno:** `Task<ClubDto>`
- **Parámetros:**

- `clubDto` (`ClubCreateDto`)

- **Qué hace:** Crea/registra un nuevo recurso. valida reglas de negocio y puede lanzar `BadRequestException`; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_clubRepository.CreateAsync(...)`

#### `UpdateClubAsync`

- **Firma:** `async Task<ClubDto> UpdateClubAsync(int id, ClubUpdateDto clubDto)`
- **Retorno:** `Task<ClubDto>`
- **Parámetros:**

- `id` (`int`)
- `clubDto` (`ClubUpdateDto`)

- **Qué hace:** Actualiza un recurso existente. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_clubRepository.GetByIdAsync(...)`, `_clubRepository.UpdateAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `DeleteClubAsync`

- **Firma:** `async Task<bool> DeleteClubAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. lanza `NotFoundException` si no encuentra el recurso; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_clubRepository.ExistsAsync(...)`, `_clubRepository.DeleteAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- Revisá `Mappings/MappingProfile.cs` para ver cómo se mapea esta entidad/DTO.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Club/ClubService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
