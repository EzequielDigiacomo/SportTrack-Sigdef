# EntrenadorServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/EntrenadorServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IEntrenadorServices`.

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
  - `using SportTrack_Sigdef.Entidades.DTOs.EntrenadorFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Club;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`
  - `using Microsoft.AspNetCore.Mvc;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_tenantProvider` — tipo `ITenantProvider` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `SportTrack_Sigdef.Controladores.Audit.IAuditService` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `GetEntrenadores`

- **Firma:** `async Task<ActionResult<IEnumerable<EntrenadorDto>>> GetEntrenadores()`
- **Retorno:** `Task<ActionResult<IEnumerable<EntrenadorDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetEntrenador`

- **Firma:** `async Task<ActionResult<EntrenadorDetailDto>> GetEntrenador(int id)`
- **Retorno:** `Task<ActionResult<EntrenadorDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetEntrenadoresPorClub`

- **Firma:** `async Task<ActionResult<IEnumerable<EntrenadorDto>>> GetEntrenadoresPorClub(int idClub)`
- **Retorno:** `Task<ActionResult<IEnumerable<EntrenadorDto>>>`
- **Parámetros:**

- `idClub` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetEntrenadoresSeleccion`

- **Firma:** `async Task<ActionResult<IEnumerable<EntrenadorDto>>> GetEntrenadoresSeleccion()`
- **Retorno:** `Task<ActionResult<IEnumerable<EntrenadorDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `SearchEntrenadores`

- **Firma:** `async Task<ActionResult<IEnumerable<EntrenadorDto>>> SearchEntrenadores(string term)`
- **Retorno:** `Task<ActionResult<IEnumerable<EntrenadorDto>>>`
- **Parámetros:**

- `term` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `PostEntrenador`

- **Firma:** `async Task<ActionResult<EntrenadorDto>> PostEntrenador(EntrenadorCreateDto entrenadorCreateDto)`
- **Retorno:** `Task<ActionResult<EntrenadorDto>>`
- **Parámetros:**

- `entrenadorCreateDto` (`EntrenadorCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.AnyAsync(...)`, `_context.Clubes.AnyAsync(...)`, `_context.Entrenadores.AnyAsync(...)`, `_context.Clubes.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `_context.Entry(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `PutEntrenador`

- **Firma:** `async Task<IActionResult> PutEntrenador(int id, EntrenadorCreateDto entrenadorCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `entrenadorCreateDto` (`EntrenadorCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Entrenadores.FindAsync(...)`, `_context.Clubes.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `EntrenadorExistsAsync(...)`

#### `DeleteEntrenador`

- **Firma:** `async Task<IActionResult> DeleteEntrenador(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Entrenadores.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `EntrenadorExistsAsync`

- **Firma:** `async Task<bool> EntrenadorExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Entrenadores.AnyAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/EntrenadorServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
