# BoteService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Bote/BoteService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IBoteService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **AutoMapper**: biblioteca que mapea propiedades entre entidades y DTOs (`CreateMap`, `_mapper.Map`).
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Bote`
- **Usings:**
  - `using AutoMapper;`
  - `using SportTrack_Sigdef.Controladores.Bote.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Exceptions;`
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_boteRepository` — tipo `IBoteRepository` (típicamente dependencia inyectada o estado privado)
- `_mapper` — tipo `IMapper` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `BoteService(...)`

**Parámetros:**

- `boteRepository` (`IBoteRepository`)
- `mapper` (`IMapper`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetAllBotesAsync`

- **Firma:** `async Task<IEnumerable<BoteDto>> GetAllBotesAsync()`
- **Retorno:** `Task<IEnumerable<BoteDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_boteRepository.GetAllAsync(...)`

#### `GetBoteByIdAsync`

- **Firma:** `async Task<BoteDto> GetBoteByIdAsync(int id)`
- **Retorno:** `Task<BoteDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_boteRepository.GetByIdAsync(...)`

#### `CreateBoteAsync`

- **Firma:** `async Task<BoteDto> CreateBoteAsync(BoteCreateDto boteDto)`
- **Retorno:** `Task<BoteDto>`
- **Parámetros:**

- `boteDto` (`BoteCreateDto`)

- **Qué hace:** Crea/registra un nuevo recurso. usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_boteRepository.CreateAsync(...)`

#### `UpdateBoteAsync`

- **Firma:** `async Task<BoteDto> UpdateBoteAsync(int id, BoteUpdateDto boteDto)`
- **Retorno:** `Task<BoteDto>`
- **Parámetros:**

- `id` (`int`)
- `boteDto` (`BoteUpdateDto`)

- **Qué hace:** Actualiza un recurso existente. lanza `NotFoundException` si no encuentra el recurso; usa AutoMapper para convertir entre entidad y DTO; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_boteRepository.GetByIdAsync(...)`, `_boteRepository.UpdateAsync(...)`

#### `DeleteBoteAsync`

- **Firma:** `async Task<bool> DeleteBoteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. lanza `NotFoundException` si no encuentra el recurso; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_boteRepository.ExistsAsync(...)`, `_boteRepository.DeleteAsync(...)`

#### `GetTiposBoteAsync`

- **Firma:** `async Task<IEnumerable<TipoBoteDto>> GetTiposBoteAsync()`
- **Retorno:** `Task<IEnumerable<TipoBoteDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. envuelve un resultado síncrono en `Task.FromResult`; enumeración de valores de un `enum`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `Task.FromResult(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- Revisá `Mappings/MappingProfile.cs` para ver cómo se mapea esta entidad/DTO.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Bote/BoteService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
