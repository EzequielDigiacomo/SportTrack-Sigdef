# ClubServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/ClubServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IClubServices`.

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
  - `using SportTrack_Sigdef.Entidades.DTOs.Club;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.EntrenadorFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub;`
  - `using SportTrack_Sigdef.Entidades.DTOs.PagoFederacionTransaccion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Evento;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_tenantProvider` — tipo `ITenantProvider` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `ClubServices(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `tenantProvider` (`ITenantProvider`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetClubes`

- **Firma:** `async Task<ActionResult<IEnumerable<ClubDto>>> GetClubes()`
- **Retorno:** `Task<ActionResult<IEnumerable<ClubDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); agrupa datos; operación asíncrona (`await`).

#### `GetClub`

- **Firma:** `async Task<ActionResult<ClubDetailDto>> GetClub(int id)`
- **Retorno:** `Task<ActionResult<ClubDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetAtletasByClub`

- **Firma:** `async Task<ActionResult<IEnumerable<AtletaDto>>> GetAtletasByClub(int id)`
- **Retorno:** `Task<ActionResult<IEnumerable<AtletaDto>>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetEntrenadoresByClub`

- **Firma:** `async Task<ActionResult<IEnumerable<EntrenadorDto>>> GetEntrenadoresByClub(int id)`
- **Retorno:** `Task<ActionResult<IEnumerable<EntrenadorDto>>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetDelegadosByClub`

- **Firma:** `async Task<ActionResult<IEnumerable<DelegadoClubDto>>> GetDelegadosByClub(int id)`
- **Retorno:** `Task<ActionResult<IEnumerable<DelegadoClubDto>>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetEventosByClub`

- **Firma:** `async Task<ActionResult<IEnumerable<EventoDto>>> GetEventosByClub(int id)`
- **Retorno:** `Task<ActionResult<IEnumerable<EventoDto>>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `PostClub`

- **Firma:** `async Task<ActionResult<ClubDto>> PostClub(ClubCreateDto clubCreateDto)`
- **Retorno:** `Task<ActionResult<ClubDto>>`
- **Parámetros:**

- `clubCreateDto` (`ClubCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Clubes.AnyAsync(...)`, `_context.SaveChangesAsync(...)`

#### `PutClub`

- **Firma:** `async Task<IActionResult> PutClub(int id, ClubCreateDto clubCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `clubCreateDto` (`ClubCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Clubes.FindAsync(...)`, `_context.Clubes.AnyAsync(...)`, `_context.SaveChangesAsync(...)`

#### `DeleteClub`

- **Firma:** `async Task<IActionResult> DeleteClub(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `SearchClubes`

- **Firma:** `async Task<ActionResult<IEnumerable<ClubDto>>> SearchClubes(string term)`
- **Retorno:** `Task<ActionResult<IEnumerable<ClubDto>>>`
- **Parámetros:**

- `term` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `ClubExistsAsync`

- **Firma:** `async Task<bool> ClubExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Clubes.AnyAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/ClubServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
