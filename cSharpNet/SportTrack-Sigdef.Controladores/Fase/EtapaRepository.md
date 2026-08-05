# IEtapaRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Fase/EtapaRepository.cs`

## 1. Qué es este archivo

Es un **Interfaz de repositorio (contrato de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `interface IEtapaRepository`
- `class EtapaRepository` : `IEtapaRepository`

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

## 4. Detalle del tipo — `interface IEtapaRepository`

### Métodos

#### `GetByEventoPruebaIdAsync`

- **Firma:** `Task<IEnumerable<Etapa>> GetByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<Etapa>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateAsync`

- **Firma:** `Task<Etapa> CreateAsync(Etapa etapa)`
- **Retorno:** `Task<Etapa>`
- **Parámetros:**

- `etapa` (`Etapa`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateManyAsync`

- **Firma:** `Task<IEnumerable<Etapa>> CreateManyAsync(IEnumerable<Etapa> etapas)`
- **Retorno:** `Task<IEnumerable<Etapa>>`
- **Parámetros:**

- `etapas` (`IEnumerable<Etapa>`)

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

## 4. Detalle del tipo — `class EtapaRepository`

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `EtapaRepository(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetByEventoPruebaIdAsync`

- **Firma:** `async Task<IEnumerable<Etapa>> GetByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<Etapa>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `CreateAsync`

- **Firma:** `async Task<Etapa> CreateAsync(Etapa etapa)`
- **Retorno:** `Task<Etapa>`
- **Parámetros:**

- `etapa` (`Etapa`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `CreateManyAsync`

- **Firma:** `async Task<IEnumerable<Etapa>> CreateManyAsync(IEnumerable<Etapa> etapas)`
- **Retorno:** `Task<IEnumerable<Etapa>>`
- **Parámetros:**

- `etapas` (`IEnumerable<Etapa>`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `DeleteByEventoPruebaIdAsync`

- **Firma:** `async Task DeleteByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Etapas.Where(...)`, `_context.SaveChangesAsync(...)`

#### `DeleteAsync`

- **Firma:** `async Task DeleteAsync(int id)`
- **Retorno:** `Task`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Etapas.FindAsync(...)`, `_context.SaveChangesAsync(...)`

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La progresión de series/fases es lógica de negocio densa: leé primero los modelos y luego el engine.
- Ruta relativa en el proyecto: `Fase/EtapaRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
