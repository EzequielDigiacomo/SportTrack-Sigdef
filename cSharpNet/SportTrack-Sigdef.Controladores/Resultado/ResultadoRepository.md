# IResultadoRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Resultado/ResultadoRepository.cs`

## 1. Qué es este archivo

Es un **Interfaz de repositorio (contrato de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `interface IResultadoRepository`
- `class ResultadoRepository` : `IResultadoRepository`

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Resultado`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — `interface IResultadoRepository`

### Métodos

#### `GetByFaseIdAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Resultado>> GetByFaseIdAsync(int faseId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Resultado>>`
- **Parámetros:**

- `faseId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetByIdAsync`

- **Firma:** `Task<Entidades.Entidades.Resultado?> GetByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.Resultado?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateAsync`

- **Firma:** `Task<Entidades.Entidades.Resultado> UpdateAsync(Entidades.Entidades.Resultado resultado)`
- **Retorno:** `Task<Entidades.Entidades.Resultado>`
- **Parámetros:**

- `resultado` (`Entidades.Entidades.Resultado`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateManyAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Resultado>> UpdateManyAsync(IEnumerable<Entidades.Entidades.Resultado> resultados)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Resultado>>`
- **Parámetros:**

- `resultados` (`IEnumerable<Entidades.Entidades.Resultado>`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 4. Detalle del tipo — `class ResultadoRepository`

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `ResultadoRepository(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetByFaseIdAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Resultado>> GetByFaseIdAsync(int faseId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Resultado>>`
- **Parámetros:**

- `faseId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `GetByIdAsync`

- **Firma:** `async Task<Entidades.Entidades.Resultado?> GetByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.Resultado?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `UpdateAsync`

- **Firma:** `async Task<Entidades.Entidades.Resultado> UpdateAsync(Entidades.Entidades.Resultado resultado)`
- **Retorno:** `Task<Entidades.Entidades.Resultado>`
- **Parámetros:**

- `resultado` (`Entidades.Entidades.Resultado`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `UpdateManyAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Resultado>> UpdateManyAsync(IEnumerable<Entidades.Entidades.Resultado> resultados)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Resultado>>`
- **Parámetros:**

- `resultados` (`IEnumerable<Entidades.Entidades.Resultado>`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Resultado/ResultadoRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
