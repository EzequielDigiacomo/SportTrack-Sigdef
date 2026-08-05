# DistanciaService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Distancia/DistanciaService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IDistanciaService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **AutoMapper**: biblioteca que mapea propiedades entre entidades y DTOs (`CreateMap`, `_mapper.Map`).
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Distancia`
- **Usings:**
  - `using AutoMapper;`
  - `using SportTrack_Sigdef.Controladores.Distancia.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Exceptions;`
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_distanciaRepository` — tipo `IDistanciaRepository` (típicamente dependencia inyectada o estado privado)
- `_mapper` — tipo `IMapper` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `DistanciaService(...)`

**Parámetros:**

- `distanciaRepository` (`IDistanciaRepository`)
- `mapper` (`IMapper`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetAllDistanciasAsync`

- **Firma:** `async Task<IEnumerable<DistanciaDto>> GetAllDistanciasAsync()`
- **Retorno:** `Task<IEnumerable<DistanciaDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_distanciaRepository.GetAllAsync(...)`

#### `GetDistanciaByIdAsync`

- **Firma:** `async Task<DistanciaDto> GetDistanciaByIdAsync(int id)`
- **Retorno:** `Task<DistanciaDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_distanciaRepository.GetByIdAsync(...)`

#### `CreateDistanciaAsync`

- **Firma:** `async Task<DistanciaDto> CreateDistanciaAsync(DistanciaCreateDto distanciaDto)`
- **Retorno:** `Task<DistanciaDto>`
- **Parámetros:**

- `distanciaDto` (`DistanciaCreateDto`)

- **Qué hace:** Crea/registra un nuevo recurso. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_distanciaRepository.CreateAsync(...)`

#### `UpdateDistanciaAsync`

- **Firma:** `async Task<DistanciaDto> UpdateDistanciaAsync(int id, DistanciaUpdateDto distanciaDto)`
- **Retorno:** `Task<DistanciaDto>`
- **Parámetros:**

- `id` (`int`)
- `distanciaDto` (`DistanciaUpdateDto`)

- **Qué hace:** Actualiza un recurso existente. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_distanciaRepository.GetByIdAsync(...)`, `_distanciaRepository.UpdateAsync(...)`

#### `DeleteDistanciaAsync`

- **Firma:** `async Task<bool> DeleteDistanciaAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. lanza `NotFoundException` si no encuentra el recurso; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_distanciaRepository.ExistsAsync(...)`, `_distanciaRepository.DeleteAsync(...)`

#### `GetDistanciasRegataAsync`

- **Firma:** `async Task<IEnumerable<DistanciaRegataDto>> GetDistanciasRegataAsync()`
- **Retorno:** `Task<IEnumerable<DistanciaRegataDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. envuelve un resultado síncrono en `Task.FromResult`; enumeración de valores de un `enum`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `Task.FromResult(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- Revisá `Mappings/MappingProfile.cs` para ver cómo se mapea esta entidad/DTO.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Distancia/DistanciaService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
