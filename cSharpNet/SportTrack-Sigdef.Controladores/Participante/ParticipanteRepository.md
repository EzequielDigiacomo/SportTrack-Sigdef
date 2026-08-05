# ParticipanteRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Participante/ParticipanteRepository.cs`

## 1. Qué es este archivo

Es un **Repositorio (implementación de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IParticipanteRepository`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **IQueryable**: consulta composable que EF Core traduce a SQL al materializar (ToListAsync, FirstOrDefaultAsync, etc.).
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Participante`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `ParticipanteRepository(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `SoloAtletas`

- **Firma:** `IQueryable<Entidades.Entidades.Participante> SoloAtletas(IQueryable<Entidades.Entidades.Participante> query)`
- **Retorno:** `IQueryable<Entidades.Entidades.Participante>`
- **Parámetros:**

- `query` (`IQueryable<Entidades.Entidades.Participante>`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. filtra con LINQ (`Where`).

#### `GetAllAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Participante>> GetAllAsync()`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Participante>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `SoloAtletas(...)`

#### `GetByIdAsync`

- **Firma:** `async Task<Entidades.Entidades.Participante?> GetByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.Participante?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `GetByClubIdAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Participante>> GetByClubIdAsync(int clubId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Participante>>`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `SoloAtletas(...)`

#### `GetByFederationIdAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Participante>> GetByFederationIdAsync(int federationId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Participante>>`
- **Parámetros:**

- `federationId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `SoloAtletas(...)`

#### `CreateAsync`

- **Firma:** `async Task<Entidades.Entidades.Participante> CreateAsync(Entidades.Entidades.Participante participante)`
- **Retorno:** `Task<Entidades.Entidades.Participante>`
- **Parámetros:**

- `participante` (`Entidades.Entidades.Participante`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `UpdateAsync`

- **Firma:** `async Task<Entidades.Entidades.Participante> UpdateAsync(Entidades.Entidades.Participante participante)`
- **Retorno:** `Task<Entidades.Entidades.Participante>`
- **Parámetros:**

- `participante` (`Entidades.Entidades.Participante`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `DeleteAsync`

- **Firma:** `async Task<bool> DeleteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `ExistsAsync`

- **Firma:** `async Task<bool> ExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Comprueba existencia. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.AnyAsync(...)`

#### `CountByClubIdAsync`

- **Firma:** `async Task<int> CountByClubIdAsync(int clubId)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `SoloAtletas(...)`

#### `CountByFederationIdAsync`

- **Firma:** `async Task<int> CountByFederationIdAsync(int federationId)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `federationId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `SoloAtletas(...)`

## 5. Notas de estudio

- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Participante/ParticipanteRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
