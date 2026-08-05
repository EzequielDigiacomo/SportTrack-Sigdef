# DistanciaRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Distancia/DistanciaRepository.cs`

## 1. Qué es este archivo

Es un **Repositorio (implementación de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IDistanciaRepository`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `(sin namespace declarado)`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Controladores.Distancia;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `DistanciaRepository(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetAllAsync`

- **Firma:** `async Task<IEnumerable<Distancia>> GetAllAsync()`
- **Retorno:** `Task<IEnumerable<Distancia>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); ordena resultados; operación asíncrona (`await`).

#### `GetByIdAsync`

- **Firma:** `async Task<Distancia?> GetByIdAsync(int id)`
- **Retorno:** `Task<Distancia?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).

#### `CreateAsync`

- **Firma:** `async Task<Distancia> CreateAsync(Distancia distancia)`
- **Retorno:** `Task<Distancia>`
- **Parámetros:**

- `distancia` (`Distancia`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `UpdateAsync`

- **Firma:** `async Task<Distancia> UpdateAsync(Distancia distancia)`
- **Retorno:** `Task<Distancia>`
- **Parámetros:**

- `distancia` (`Distancia`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `DeleteAsync`

- **Firma:** `async Task<bool> DeleteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Distancias.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `ExistsAsync`

- **Firma:** `async Task<bool> ExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Comprueba existencia. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Distancias.AnyAsync(...)`

#### `ExistsByDistanciaRegataAsync`

- **Firma:** `async Task<bool> ExistsByDistanciaRegataAsync(DistanciaRegataEnum distanciaRegata, int? excludeId = null)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `distanciaRegata` (`DistanciaRegataEnum`)
- `excludeId` (`int?`)

- **Qué hace:** Comprueba existencia. filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `query.AnyAsync(...)`

#### `GetByDistanciaRegataAsync`

- **Firma:** `async Task<Distancia?> GetByDistanciaRegataAsync(DistanciaRegataEnum distanciaRegata)`
- **Retorno:** `Task<Distancia?>`
- **Parámetros:**

- `distanciaRegata` (`DistanciaRegataEnum`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).

#### `GetByRangoMetrosAsync`

- **Firma:** `async Task<IEnumerable<Distancia>> GetByRangoMetrosAsync(int metrosMin, int metrosMax)`
- **Retorno:** `Task<IEnumerable<Distancia>>`
- **Parámetros:**

- `metrosMin` (`int`)
- `metrosMax` (`int`)

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Distancias.AsNoTracking(...)`

## 5. Notas de estudio

- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Distancia/DistanciaRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
