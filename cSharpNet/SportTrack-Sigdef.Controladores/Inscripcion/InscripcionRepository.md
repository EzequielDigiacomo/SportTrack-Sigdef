# InscripcionRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Inscripcion/InscripcionRepository.cs`

## 1. Qué es este archivo

Es un **Repositorio (implementación de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IInscripcionRepository`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Inscripcion`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.Inscripcion;`
  - `using SportTrack_Sigdef.Controladores.Exceptions;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `InscripcionRepository(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetAllAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetAllAsync()`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Inscripcion>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); operación asíncrona (`await`).

#### `GetByIdAsync`

- **Firma:** `async Task<Entidades.Entidades.Inscripcion?> GetByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.Inscripcion?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `CreateAsync`

- **Firma:** `async Task<Entidades.Entidades.Inscripcion> CreateAsync(Entidades.Entidades.Inscripcion inscripcion)`
- **Retorno:** `Task<Entidades.Entidades.Inscripcion>`
- **Parámetros:**

- `inscripcion` (`Entidades.Entidades.Inscripcion`)

- **Qué hace:** Crea/registra un nuevo recurso. lanza `NotFoundException` si no encuentra el recurso; valida reglas de negocio y puede lanzar `BadRequestException`; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Inscripciones.AnyAsync(...)`, `_context.SaveChangesAsync(...)`

#### `UpdateAsync`

- **Firma:** `async Task<Entidades.Entidades.Inscripcion> UpdateAsync(Entidades.Entidades.Inscripcion inscripcion)`
- **Retorno:** `Task<Entidades.Entidades.Inscripcion>`
- **Parámetros:**

- `inscripcion` (`Entidades.Entidades.Inscripcion`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `DeleteAsync`

- **Firma:** `async Task<bool> DeleteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Inscripciones.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `ExistsAsync`

- **Firma:** `async Task<bool> ExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Comprueba existencia. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Inscripciones.AnyAsync(...)`

#### `CountByEventoPruebaIdAsync`

- **Firma:** `async Task<int> CountByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).

#### `GetByEventoPruebaIdAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Inscripcion>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetByEventoAndClubAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetByEventoAndClubAsync(int eventoId, int clubId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Inscripcion>>`
- **Parámetros:**

- `eventoId` (`int`)
- `clubId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

## 5. Notas de estudio

- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Inscripcion/InscripcionRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
