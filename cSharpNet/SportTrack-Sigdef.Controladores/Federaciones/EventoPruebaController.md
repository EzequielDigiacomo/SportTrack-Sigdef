# EventoPruebaController

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/EventoPruebaController.cs`

## 1. Qué es este archivo

Es un **Controlador (nota: vive en Controladores; normalmente los HTTP controllers están en la API)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `ControllerBase`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SIGDEF.Controllers`
- **Usings:**
  - `using Microsoft.AspNetCore.Authorization;`
  - `using Microsoft.AspNetCore.Mvc;`
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.DTOs.EventoPrueba;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Prueba;`
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Atributos del tipo

- `[ApiController]`
- `[Route("api/legacy/eventos/{idEvento}/pruebas")]`
- `[Authorize]`

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `EventoPruebaController(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetEventoPruebas`

- **Firma:** `async Task<ActionResult<IEnumerable<EventoPruebaDto>>> GetEventoPruebas(int idEvento)`
- **Retorno:** `Task<ActionResult<IEnumerable<EventoPruebaDto>>>`
- **Atributos:** `[HttpGet]`
- **Parámetros:**

- `idEvento` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Eventos.AnyAsync(...)`

#### `GetEventoPrueba`

- **Firma:** `async Task<ActionResult<EventoPruebaDto>> GetEventoPrueba(int idEvento, int idPrueba)`
- **Retorno:** `Task<ActionResult<EventoPruebaDto>>`
- **Atributos:** `[HttpGet("{idPrueba}")]`
- **Parámetros:**

- `idEvento` (`int`)
- `idPrueba` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `NotFound`

- **Firma:** `return NotFound(new { message = $"Evento con ID {idEvento} no encontrado" })`
- **Retorno:** `return`
- **Parámetros:**

- `message` (`new {`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `BadRequest`

- **Firma:** `return BadRequest(new { message = $"La prueba con ID {eventoPruebaDto.IdPrueba} no existe en el catálogo." })`
- **Retorno:** `return`
- **Parámetros:**

- `message` (`new {`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `BadRequest`

- **Firma:** `return BadRequest(new { message = "El ID de la ruta no coincide con el ID de la prueba" })`
- **Retorno:** `return`
- **Parámetros:**

- `message` (`new {`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `BadRequest`

- **Firma:** `return BadRequest(new { message = $"La prueba con ID {eventoPruebaDto.IdPrueba} no existe en el catálogo." })`
- **Retorno:** `return`
- **Parámetros:**

- `message` (`new {`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `catch`

- **Firma:** ` catch(DbUpdateConcurrencyException)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`DbUpdateConcurrencyException`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `NoContent`

- **Firma:** `return NoContent()`
- **Retorno:** `return`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteEventoPrueba`

- **Firma:** `async Task<IActionResult> DeleteEventoPrueba(int idEvento, int idPrueba)`
- **Retorno:** `Task<IActionResult>`
- **Atributos:** `[HttpDelete("{idPrueba}")]`
- **Parámetros:**

- `idEvento` (`int`)
- `idPrueba` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `EventoPruebaExists`

- **Firma:** `bool EventoPruebaExists(int idEvento, int idPrueba)`
- **Retorno:** `bool`
- **Parámetros:**

- `idEvento` (`int`)
- `idPrueba` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `MapDistanciaToEnum`

- **Firma:** `DistanciaRegata MapDistanciaToEnum(int distanciaId)`
- **Retorno:** `DistanciaRegata`
- **Parámetros:**

- `distanciaId` (`int`)

- **Qué hace:** Configura o aplica mapeos.

## 5. Notas de estudio

- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/EventoPruebaController.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
