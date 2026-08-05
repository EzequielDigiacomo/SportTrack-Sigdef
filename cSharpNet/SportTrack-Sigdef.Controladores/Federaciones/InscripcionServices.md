# InscripcionServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/InscripcionServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IInscripcionServices`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SIGDEF.API.Services`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Inscripcion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Evento;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`
  - `using Microsoft.AspNetCore.Mvc;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `InscripcionServices(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetInscripciones`

- **Firma:** `async Task<ActionResult<IEnumerable<InscripcionDto>>> GetInscripciones()`
- **Retorno:** `Task<ActionResult<IEnumerable<InscripcionDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `GetInscripcion`

- **Firma:** `async Task<ActionResult<InscripcionDetailDto>> GetInscripcion(int id)`
- **Retorno:** `Task<ActionResult<InscripcionDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetInscripcionesPorAtleta`

- **Firma:** `async Task<ActionResult<IEnumerable<InscripcionDto>>> GetInscripcionesPorAtleta(int ParticipanteId)`
- **Retorno:** `Task<ActionResult<IEnumerable<InscripcionDto>>>`
- **Parámetros:**

- `ParticipanteId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetInscripcionesPorEvento`

- **Firma:** `async Task<ActionResult<IEnumerable<InscripcionDto>>> GetInscripcionesPorEvento(int idEvento)`
- **Retorno:** `Task<ActionResult<IEnumerable<InscripcionDto>>>`
- **Parámetros:**

- `idEvento` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `PostInscripcion`

- **Firma:** `async Task<ActionResult<InscripcionDto>> PostInscripcion(InscripcionCreateDto inscripcionCreateDto)`
- **Retorno:** `Task<ActionResult<InscripcionDto>>`
- **Parámetros:**

- `inscripcionCreateDto` (`InscripcionCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.AtletasFederados.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `_context.Entry(...)`

#### `PutInscripcion`

- **Firma:** `async Task<IActionResult> PutInscripcion(int id, InscripcionCreateDto inscripcionCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `inscripcionCreateDto` (`InscripcionCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Inscripciones.FindAsync(...)`, `_context.AtletasFederados.AnyAsync(...)`, `_context.EventoPruebas.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `InscripcionExistsAsync(...)`

#### `DeleteInscripcion`

- **Firma:** `async Task<IActionResult> DeleteInscripcion(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Inscripciones.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `InscripcionExistsAsync`

- **Firma:** `async Task<bool> InscripcionExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Inscripciones.AnyAsync(...)`

#### `GetEstadoEvento`

- **Firma:** `string GetEstadoEvento(DateTime fechaInicio, DateTime fechaFin)`
- **Retorno:** `string`
- **Parámetros:**

- `fechaInicio` (`DateTime`)
- `fechaFin` (`DateTime`)

- **Qué hace:** Obtiene/consulta datos.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/InscripcionServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
