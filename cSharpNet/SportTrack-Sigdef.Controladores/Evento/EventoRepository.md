# EventoRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Evento/EventoRepository.cs`

## 1. Qué es este archivo

Es un **Repositorio (implementación de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IEventoRepository`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **IQueryable**: consulta composable que EF Core traduce a SQL al materializar (ToListAsync, FirstOrDefaultAsync, etc.).
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Evento`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `EventoRepository(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `IsSuperAdmin`

- **Firma:** `bool IsSuperAdmin(string? rol)`
- **Retorno:** `bool`
- **Parámetros:**

- `rol` (`string?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `IsRolAdministrativo`

- **Firma:** `bool IsRolAdministrativo(string? rol)`
- **Retorno:** `bool`
- **Parámetros:**

- `rol` (`string?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetAllAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Evento>> GetAllAsync(int? clubId = null, string? rol = null)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Evento>>`
- **Parámetros:**

- `clubId` (`int?`)
- `rol` (`string?`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); ordena resultados; operación asíncrona (`await`).
- **Llamadas await destacadas:** `ApplyScopeFilterAsync(...)`

#### `GetByIdAsync`

- **Firma:** `async Task<Entidades.Entidades.Evento?> GetByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.Evento?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `CreateAsync`

- **Firma:** `async Task<Entidades.Entidades.Evento> CreateAsync(Entidades.Entidades.Evento evento)`
- **Retorno:** `Task<Entidades.Entidades.Evento>`
- **Parámetros:**

- `evento` (`Entidades.Entidades.Evento`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `UpdateAsync`

- **Firma:** `async Task<Entidades.Entidades.Evento> UpdateAsync(Entidades.Entidades.Evento evento)`
- **Retorno:** `Task<Entidades.Entidades.Evento>`
- **Parámetros:**

- `evento` (`Entidades.Entidades.Evento`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `DeleteAsync`

- **Firma:** `async Task<bool> DeleteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Eventos.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `ExistsAsync`

- **Firma:** `async Task<bool> ExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Comprueba existencia. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Eventos.AnyAsync(...)`

#### `GetProximosAsync`

- **Firma:** `async Task<IEnumerable<Entidades.Entidades.Evento>> GetProximosAsync(int? clubId = null, string? rol = null)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Evento>>`
- **Parámetros:**

- `clubId` (`int?`)
- `rol` (`string?`)

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).
- **Llamadas await destacadas:** `ApplyScopeFilterAsync(...)`

#### `GetPruebasByEventoIdAsync`

- **Firma:** `async Task<IEnumerable<EventoPrueba>> GetPruebasByEventoIdAsync(int eventoId)`
- **Retorno:** `Task<IEnumerable<EventoPrueba>>`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `GetEventoPruebaByIdAsync`

- **Firma:** `async Task<EventoPrueba?> GetEventoPruebaByIdAsync(int id)`
- **Retorno:** `Task<EventoPrueba?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `AssignPruebaAsync`

- **Firma:** `async Task<EventoPrueba> AssignPruebaAsync(EventoPrueba eventoPrueba)`
- **Retorno:** `Task<EventoPrueba>`
- **Parámetros:**

- `eventoPrueba` (`EventoPrueba`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `UpdateEventoPruebaAsync`

- **Firma:** `async Task<EventoPrueba> UpdateEventoPruebaAsync(EventoPrueba eventoPrueba)`
- **Retorno:** `Task<EventoPrueba>`
- **Parámetros:**

- `eventoPrueba` (`EventoPrueba`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `UnassignPruebaAsync`

- **Firma:** `async Task<bool> UnassignPruebaAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.EventoPruebas.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `GetPruebaAsync`

- **Firma:** `async Task<Entidades.Entidades.Prueba?> GetPruebaAsync(int categoriaId, int boteId, int distanciaId, int sexoId)`
- **Retorno:** `Task<Entidades.Entidades.Prueba?>`
- **Parámetros:**

- `categoriaId` (`int`)
- `boteId` (`int`)
- `distanciaId` (`int`)
- `sexoId` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).

#### `CreatePruebaAsync`

- **Firma:** `async Task<Entidades.Entidades.Prueba> CreatePruebaAsync(Entidades.Entidades.Prueba prueba)`
- **Retorno:** `Task<Entidades.Entidades.Prueba>`
- **Parámetros:**

- `prueba` (`Entidades.Entidades.Prueba`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

## 5. Notas de estudio

- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Evento/EventoRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
