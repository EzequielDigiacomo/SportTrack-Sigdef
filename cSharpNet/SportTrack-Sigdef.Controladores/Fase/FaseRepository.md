# IFaseRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Fase/FaseRepository.cs`

## 1. Qué es este archivo

Es un **Interfaz de repositorio (contrato de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `interface IFaseRepository`
- `class FaseRepository` : `IFaseRepository`

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Fase`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — `interface IFaseRepository`

### Métodos

#### `GetByEventoPruebaIdAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Fase>> GetByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Fase>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetByIdAsync`

- **Firma:** `Task<Entidades.Entidades.Fase?> GetByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.Fase?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateAsync`

- **Firma:** `Task<Entidades.Entidades.Fase> CreateAsync(Entidades.Entidades.Fase fase)`
- **Retorno:** `Task<Entidades.Entidades.Fase>`
- **Parámetros:**

- `fase` (`Entidades.Entidades.Fase`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateManyAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Fase>> CreateManyAsync(IEnumerable<Entidades.Entidades.Fase> fases)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Fase>>`
- **Parámetros:**

- `fases` (`IEnumerable<Entidades.Entidades.Fase>`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteByEventoPruebaIdAsync`

- **Firma:** `Task DeleteByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteAsync`

- **Firma:** `Task DeleteAsync(int id)`
- **Retorno:** `Task`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateAsync`

- **Firma:** `Task<Entidades.Entidades.Fase> UpdateAsync(Entidades.Entidades.Fase fase)`
- **Retorno:** `Task<Entidades.Entidades.Fase>`
- **Parámetros:**

- `fase` (`Entidades.Entidades.Fase`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetByEventoIdAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Fase>> GetByEventoIdAsync(int eventoId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Fase>>`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetResultadoByIdAsync`

- **Firma:** `Task<SportTrack_Sigdef.Entidades.Entidades.Resultado?> GetResultadoByIdAsync(int id)`
- **Retorno:** `Task<SportTrack_Sigdef.Entidades.Entidades.Resultado?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateResultadoAsync`

- **Firma:** `Task UpdateResultadoAsync(SportTrack_Sigdef.Entidades.Entidades.Resultado resultado)`
- **Retorno:** `Task`
- **Parámetros:**

- `resultado` (`SportTrack_Sigdef.Entidades.Entidades.Resultado`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEventoIdByFaseIdAsync`

- **Firma:** `Task<int?> GetEventoIdByFaseIdAsync(int faseId)`
- **Retorno:** `Task<int?>`
- **Parámetros:**

- `faseId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEventoIdByResultadoIdAsync`

- **Firma:** `Task<int?> GetEventoIdByResultadoIdAsync(int resultadoId)`
- **Retorno:** `Task<int?>`
- **Parámetros:**

- `resultadoId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 4. Detalle del tipo — `class FaseRepository`

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `FaseRepository(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetByEventoPruebaIdAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Fase>> GetByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Fase>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `GetByIdAsync`

- **Firma:** `async Task<Entidades.Entidades.Fase?> GetByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.Fase?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `CreateAsync`

- **Firma:** `async Task<Entidades.Entidades.Fase> CreateAsync(Entidades.Entidades.Fase fase)`
- **Retorno:** `Task<Entidades.Entidades.Fase>`
- **Parámetros:**

- `fase` (`Entidades.Entidades.Fase`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `CreateManyAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Fase>> CreateManyAsync(IEnumerable<Entidades.Entidades.Fase> fases)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Fase>>`
- **Parámetros:**

- `fases` (`IEnumerable<Entidades.Entidades.Fase>`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `DeleteByEventoPruebaIdAsync`

- **Firma:** `async Task DeleteByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Fases.Where(...)`, `_context.SaveChangesAsync(...)`

#### `DeleteAsync`

- **Firma:** `async Task DeleteAsync(int id)`
- **Retorno:** `Task`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Fases.FirstOrDefaultAsync(...)`, `_context.SaveChangesAsync(...)`

#### `UpdateAsync`

- **Firma:** `async Task<Entidades.Entidades.Fase> UpdateAsync(Entidades.Entidades.Fase fase)`
- **Retorno:** `Task<Entidades.Entidades.Fase>`
- **Parámetros:**

- `fase` (`Entidades.Entidades.Fase`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `GetByEventoIdAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Fase>> GetByEventoIdAsync(int eventoId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Fase>>`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `GetResultadoByIdAsync`

- **Firma:** `async Task<SportTrack_Sigdef.Entidades.Entidades.Resultado?> GetResultadoByIdAsync(int id)`
- **Retorno:** `Task<SportTrack_Sigdef.Entidades.Entidades.Resultado?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Resultados.FirstOrDefaultAsync(...)`

#### `UpdateResultadoAsync`

- **Firma:** `async Task UpdateResultadoAsync(SportTrack_Sigdef.Entidades.Entidades.Resultado resultado)`
- **Retorno:** `Task`
- **Parámetros:**

- `resultado` (`SportTrack_Sigdef.Entidades.Entidades.Resultado`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `GetEventoIdByFaseIdAsync`

- **Firma:** `async Task<int?> GetEventoIdByFaseIdAsync(int faseId)`
- **Retorno:** `Task<int?>`
- **Parámetros:**

- `faseId` (`int`)

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetEventoIdByResultadoIdAsync`

- **Firma:** `async Task<int?> GetEventoIdByResultadoIdAsync(int resultadoId)`
- **Retorno:** `Task<int?>`
- **Parámetros:**

- `resultadoId` (`int`)

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La progresión de series/fases es lógica de negocio densa: leé primero los modelos y luego el engine.
- Ruta relativa en el proyecto: `Fase/FaseRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
