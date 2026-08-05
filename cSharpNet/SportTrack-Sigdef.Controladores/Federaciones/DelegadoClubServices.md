# DelegadoClubServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DelegadoClubServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IDelegadoClubServices`.

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
  - `using SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using SportTrack_Sigdef.Entidades.DTOs.RolFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Federacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Club;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_tenantProvider` — tipo `ITenantProvider` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `SportTrack_Sigdef.Controladores.Audit.IAuditService` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `GetDelegadosClub`

- **Firma:** `async Task<ActionResult<IEnumerable<DelegadoClubDto>>> GetDelegadosClub()`
- **Retorno:** `Task<ActionResult<IEnumerable<DelegadoClubDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetDelegadoClub`

- **Firma:** `async Task<ActionResult<DelegadoClubDetailDto>> GetDelegadoClub(int id)`
- **Retorno:** `Task<ActionResult<DelegadoClubDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetDelegadosPorFederacion`

- **Firma:** `async Task<ActionResult<IEnumerable<DelegadoClubDto>>> GetDelegadosPorFederacion(int idFederacion)`
- **Retorno:** `Task<ActionResult<IEnumerable<DelegadoClubDto>>>`
- **Parámetros:**

- `idFederacion` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `PostDelegadoClub`

- **Firma:** `async Task<ActionResult<DelegadoClubDto>> PostDelegadoClub(DelegadoClubCreateDto delegadoClubCreateDto)`
- **Retorno:** `Task<ActionResult<DelegadoClubDto>>`
- **Parámetros:**

- `delegadoClubCreateDto` (`DelegadoClubCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.AnyAsync(...)`, `_context.Roles.AnyAsync(...)`, `_context.Federaciones.AnyAsync(...)`, `_context.Clubes.AnyAsync(...)`, `_context.DelegadosClub.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `_context.Entry(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `PutDelegadoClub`

- **Firma:** `async Task<IActionResult> PutDelegadoClub(int id, DelegadoClubCreateDto delegadoClubCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `delegadoClubCreateDto` (`DelegadoClubCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.DelegadosClub.FindAsync(...)`, `_context.Roles.AnyAsync(...)`, `_context.Federaciones.AnyAsync(...)`, `_context.Clubes.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `DelegadoClubExistsAsync(...)`

#### `DeleteDelegadoClub`

- **Firma:** `async Task<IActionResult> DeleteDelegadoClub(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.DelegadosClub.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `DelegadoClubExistsAsync`

- **Firma:** `async Task<bool> DelegadoClubExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.DelegadosClub.AnyAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DelegadoClubServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
