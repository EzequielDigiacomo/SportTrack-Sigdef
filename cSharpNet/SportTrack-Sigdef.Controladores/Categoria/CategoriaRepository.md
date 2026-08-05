# CategoriaRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Categoria/CategoriaRepository.cs`

## 1. Qué es este archivo

Es un **Repositorio (implementación de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `ICategoriaRepository`.

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
  - `using SportTrack_Sigdef.Controladores.Categoria;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `CategoriaRepository(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetAllAsync`

- **Firma:** `async Task<IEnumerable<Categoria>> GetAllAsync()`
- **Retorno:** `Task<IEnumerable<Categoria>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); ordena resultados; operación asíncrona (`await`).

#### `GetByIdAsync`

- **Firma:** `async Task<Categoria?> GetByIdAsync(int id)`
- **Retorno:** `Task<Categoria?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).

#### `CreateAsync`

- **Firma:** `async Task<Categoria> CreateAsync(Categoria categoria)`
- **Retorno:** `Task<Categoria>`
- **Parámetros:**

- `categoria` (`Categoria`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `UpdateAsync`

- **Firma:** `async Task<Categoria> UpdateAsync(Categoria categoria)`
- **Retorno:** `Task<Categoria>`
- **Parámetros:**

- `categoria` (`Categoria`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `DeleteAsync`

- **Firma:** `async Task<bool> DeleteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Categorias.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `ExistsAsync`

- **Firma:** `async Task<bool> ExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Comprueba existencia. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Categorias.AnyAsync(...)`

#### `GetByEdadAsync`

- **Firma:** `async Task<IEnumerable<Categoria>> GetByEdadAsync(int edad)`
- **Retorno:** `Task<IEnumerable<Categoria>>`
- **Parámetros:**

- `edad` (`int`)

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `ExistsByNombreAsync`

- **Firma:** `async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `nombre` (`string`)
- `excludeId` (`int?`)

- **Qué hace:** Comprueba existencia. filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `query.AnyAsync(...)`

#### `ExistsRangoEdadSuperpuestoAsync`

- **Firma:** `async Task<bool> ExistsRangoEdadSuperpuestoAsync(int? edadMin, int? edadMax, int? excludeId = null)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `edadMin` (`int?`)
- `edadMax` (`int?`)
- `excludeId` (`int?`)

- **Qué hace:** Comprueba existencia. filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `query.AnyAsync(...)`

## 5. Notas de estudio

- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Categoria/CategoriaRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
