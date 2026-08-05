# BoteRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Bote/BoteRepository.cs`

## 1. Qué es este archivo

Es un **Repositorio (implementación de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IBoteRepository`.

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
  - `using SportTrack_Sigdef.Controladores.Bote;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.AccesoDatos;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `BoteRepository(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetAllAsync`

- **Firma:** `async Task<IEnumerable<Bote>> GetAllAsync()`
- **Retorno:** `Task<IEnumerable<Bote>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); operación asíncrona (`await`).

#### `GetByIdAsync`

- **Firma:** `async Task<Bote?> GetByIdAsync(int id)`
- **Retorno:** `Task<Bote?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).

#### `CreateAsync`

- **Firma:** `async Task<Bote> CreateAsync(Bote bote)`
- **Retorno:** `Task<Bote>`
- **Parámetros:**

- `bote` (`Bote`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `UpdateAsync`

- **Firma:** `async Task<Bote> UpdateAsync(Bote bote)`
- **Retorno:** `Task<Bote>`
- **Parámetros:**

- `bote` (`Bote`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `DeleteAsync`

- **Firma:** `async Task<bool> DeleteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Botes.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `ExistsAsync`

- **Firma:** `async Task<bool> ExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Comprueba existencia. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Botes.AnyAsync(...)`

#### `ExistsByTipoAsync`

- **Firma:** `async Task<bool> ExistsByTipoAsync(string tipo)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `tipo` (`string`)

- **Qué hace:** Comprueba existencia. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Botes.AnyAsync(...)`

## 5. Notas de estudio

- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Bote/BoteRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
