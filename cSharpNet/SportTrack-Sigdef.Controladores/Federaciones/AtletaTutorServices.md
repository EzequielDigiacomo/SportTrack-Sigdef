# AtletaTutorServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/AtletaTutorServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IAtletaTutorServices`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Services`
- **Usings:**
  - `using Microsoft.AspNetCore.Mvc;`
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.TutorFederacion;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `AtletaTutorServices(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetAtletasTutores`

- **Firma:** `async Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetAtletasTutores()`
- **Retorno:** `Task<ActionResult<IEnumerable<AtletaTutorDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `GetTutoresPorAtleta`

- **Firma:** `async Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetTutoresPorAtleta(int ParticipanteId)`
- **Retorno:** `Task<ActionResult<IEnumerable<AtletaTutorDto>>>`
- **Parámetros:**

- `ParticipanteId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetAtletasPorTutor`

- **Firma:** `async Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetAtletasPorTutor(int idTutor)`
- **Retorno:** `Task<ActionResult<IEnumerable<AtletaTutorDto>>>`
- **Parámetros:**

- `idTutor` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetAtletaTutor`

- **Firma:** `async Task<ActionResult<AtletaTutorDetailDto>> GetAtletaTutor(int ParticipanteId, int idTutor)`
- **Retorno:** `Task<ActionResult<AtletaTutorDetailDto>>`
- **Parámetros:**

- `ParticipanteId` (`int`)
- `idTutor` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `PostAtletaTutor`

- **Firma:** `async Task<ActionResult<AtletaTutorDto>> PostAtletaTutor(AtletaTutorCreateDto atletaTutorCreateDto)`
- **Retorno:** `Task<ActionResult<AtletaTutorDto>>`
- **Parámetros:**

- `atletaTutorCreateDto` (`AtletaTutorCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.AtletasFederados.AnyAsync(...)`, `_context.Tutores.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `_context.Entry(...)`

#### `PutAtletaTutor`

- **Firma:** `async Task<IActionResult> PutAtletaTutor(int ParticipanteId, int idTutor, AtletaTutorCreateDto atletaTutorCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `ParticipanteId` (`int`)
- `idTutor` (`int`)
- `atletaTutorCreateDto` (`AtletaTutorCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`, `AtletaTutorExistsAsync(...)`

#### `DeleteAtletaTutor`

- **Firma:** `async Task<IActionResult> DeleteAtletaTutor(int ParticipanteId, int idTutor)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `ParticipanteId` (`int`)
- `idTutor` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `AtletaTutorExistsAsync`

- **Firma:** `async Task<bool> AtletaTutorExistsAsync(int ParticipanteId, int idTutor)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `ParticipanteId` (`int`)
- `idTutor` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.AtletasTutores.AnyAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/AtletaTutorServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
