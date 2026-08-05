# CategoriaService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Categoria/CategoriaService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `ICategoriaService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **AutoMapper**: biblioteca que mapea propiedades entre entidades y DTOs (`CreateMap`, `_mapper.Map`).
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Categoria`
- **Usings:**
  - `using AutoMapper;`
  - `using SportTrack_Sigdef.Controladores.Categoria.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Exceptions;`
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_categoriaRepository` — tipo `ICategoriaRepository` (típicamente dependencia inyectada o estado privado)
- `_mapper` — tipo `IMapper` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `CategoriaService(...)`

**Parámetros:**

- `categoriaRepository` (`ICategoriaRepository`)
- `mapper` (`IMapper`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetAllCategoriasAsync`

- **Firma:** `async Task<IEnumerable<CategoriaDto>> GetAllCategoriasAsync()`
- **Retorno:** `Task<IEnumerable<CategoriaDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_categoriaRepository.GetAllAsync(...)`

#### `GetCategoriaByIdAsync`

- **Firma:** `async Task<CategoriaDto> GetCategoriaByIdAsync(int id)`
- **Retorno:** `Task<CategoriaDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_categoriaRepository.GetByIdAsync(...)`

#### `CreateCategoriaAsync`

- **Firma:** `async Task<CategoriaDto> CreateCategoriaAsync(CategoriaCreateDto categoriaDto)`
- **Retorno:** `Task<CategoriaDto>`
- **Parámetros:**

- `categoriaDto` (`CategoriaCreateDto`)

- **Qué hace:** Crea/registra un nuevo recurso. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `ValidateCategoriaEdades(...)`, `_categoriaRepository.CreateAsync(...)`

#### `UpdateCategoriaAsync`

- **Firma:** `async Task<CategoriaDto> UpdateCategoriaAsync(int id, CategoriaUpdateDto categoriaDto)`
- **Retorno:** `Task<CategoriaDto>`
- **Parámetros:**

- `id` (`int`)
- `categoriaDto` (`CategoriaUpdateDto`)

- **Qué hace:** Actualiza un recurso existente. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_categoriaRepository.GetByIdAsync(...)`, `ValidateCategoriaEdades(...)`, `_categoriaRepository.UpdateAsync(...)`

#### `DeleteCategoriaAsync`

- **Firma:** `async Task<bool> DeleteCategoriaAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. lanza `NotFoundException` si no encuentra el recurso; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_categoriaRepository.ExistsAsync(...)`, `_categoriaRepository.DeleteAsync(...)`

#### `GetCategoriasEdadAsync`

- **Firma:** `async Task<IEnumerable<CategoriaEdadDto>> GetCategoriasEdadAsync()`
- **Retorno:** `Task<IEnumerable<CategoriaEdadDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. envuelve un resultado síncrono en `Task.FromResult`; enumeración de valores de un `enum`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `Task.FromResult(...)`

#### `GetCategoriasByEdadAsync`

- **Firma:** `async Task<IEnumerable<CategoriaDto>> GetCategoriasByEdadAsync(int edad)`
- **Retorno:** `Task<IEnumerable<CategoriaDto>>`
- **Parámetros:**

- `edad` (`int`)

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_categoriaRepository.GetByEdadAsync(...)`

#### `ValidateCategoriaEdades`

- **Firma:** `async Task ValidateCategoriaEdades(int? edadMin, int? edadMax, int? excludeId = null)`
- **Retorno:** `Task`
- **Parámetros:**

- `edadMin` (`int?`)
- `edadMax` (`int?`)
- `excludeId` (`int?`)

- **Qué hace:** Valida reglas de negocio.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- Revisá `Mappings/MappingProfile.cs` para ver cómo se mapea esta entidad/DTO.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Categoria/CategoriaService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
