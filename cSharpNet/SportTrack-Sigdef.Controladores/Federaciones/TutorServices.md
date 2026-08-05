# TutorServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/TutorServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `ITutorServices`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Services`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.DTOs.TutorFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`
  - `using Microsoft.AspNetCore.Mvc;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `SportTrack_Sigdef.Controladores.Audit.IAuditService` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `GetTutores`

- **Firma:** `async Task<ActionResult<IEnumerable<TutorDto>>> GetTutores()`
- **Retorno:** `Task<ActionResult<IEnumerable<TutorDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `GetTutor`

- **Firma:** `async Task<ActionResult<TutorDetailDto>> GetTutor(int id)`
- **Retorno:** `Task<ActionResult<TutorDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetTutoresPorTipo`

- **Firma:** `async Task<ActionResult<IEnumerable<TutorDto>>> GetTutoresPorTipo(string tipoTutor)`
- **Retorno:** `Task<ActionResult<IEnumerable<TutorDto>>>`
- **Parámetros:**

- `tipoTutor` (`string`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetTutoresSinAtletas`

- **Firma:** `async Task<ActionResult<IEnumerable<TutorDto>>> GetTutoresSinAtletas()`
- **Retorno:** `Task<ActionResult<IEnumerable<TutorDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `SearchTutores`

- **Firma:** `async Task<ActionResult<IEnumerable<TutorDto>>> SearchTutores(string term)`
- **Retorno:** `Task<ActionResult<IEnumerable<TutorDto>>>`
- **Parámetros:**

- `term` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetTiposTutor`

- **Firma:** `async Task<ActionResult<IEnumerable<string>>> GetTiposTutor()`
- **Retorno:** `Task<ActionResult<IEnumerable<string>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).

#### `PostTutor`

- **Firma:** `async Task<ActionResult<TutorDto>> PostTutor(TutorCreateDto tutorCreateDto)`
- **Retorno:** `Task<ActionResult<TutorDto>>`
- **Parámetros:**

- `tutorCreateDto` (`TutorCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.AnyAsync(...)`, `_context.Tutores.AnyAsync(...)`, `_context.AtletasFederados.AnyAsync(...)`, `_context.Entrenadores.AnyAsync(...)`, `_context.DelegadosClub.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `_context.Entry(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `PutTutor`

- **Firma:** `async Task<IActionResult> PutTutor(int id, TutorCreateDto tutorCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `tutorCreateDto` (`TutorCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Tutores.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `TutorExistsAsync(...)`

#### `DeleteTutor`

- **Firma:** `async Task<IActionResult> DeleteTutor(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `TutorExistsAsync`

- **Firma:** `async Task<bool> TutorExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Tutores.AnyAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/TutorServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
