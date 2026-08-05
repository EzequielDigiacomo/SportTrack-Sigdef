# RolServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/RolServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IRolServices`.

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
  - `using SportTrack_Sigdef.Controladores.Extensions;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.DTOs.RolFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
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

#### Constructor 1: `RolServices(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetRoles`

- **Firma:** `async Task<ActionResult<IEnumerable<RolDto>>> GetRoles()`
- **Retorno:** `Task<ActionResult<IEnumerable<RolDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).

#### `GetRol`

- **Firma:** `async Task<ActionResult<RolDetailDto>> GetRol(int id)`
- **Retorno:** `Task<ActionResult<RolDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `SearchRoles`

- **Firma:** `async Task<ActionResult<IEnumerable<RolDto>>> SearchRoles(string term)`
- **Retorno:** `Task<ActionResult<IEnumerable<RolDto>>>`
- **Parámetros:**

- `term` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetRolPorTipo`

- **Firma:** `async Task<ActionResult<RolDto>> GetRolPorTipo(string tipo)`
- **Retorno:** `Task<ActionResult<RolDto>>`
- **Parámetros:**

- `tipo` (`string`)

- **Qué hace:** Obtiene/consulta datos. filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Roles.GetByTipoAsync(...)`

#### `GetRolesPredefinidos`

- **Firma:** `async Task<ActionResult<IEnumerable<RolDto>>> GetRolesPredefinidos()`
- **Retorno:** `Task<ActionResult<IEnumerable<RolDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Roles.GetByTiposAsync(...)`

#### `GetEnumValues`

- **Firma:** `async Task<ActionResult> GetEnumValues()`
- **Retorno:** `Task<ActionResult>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. enumeración de valores de un `enum`.

#### `GetRolPorEnumId`

- **Firma:** `async Task<ActionResult<RolDto>> GetRolPorEnumId(int enumId)`
- **Retorno:** `Task<ActionResult<RolDto>>`
- **Parámetros:**

- `enumId` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Roles.GetByTipoAsync(...)`

#### `PostRol`

- **Firma:** `async Task<ActionResult<RolDto>> PostRol(RolCreateDto rolCreateDto)`
- **Retorno:** `Task<ActionResult<RolDto>>`
- **Parámetros:**

- `rolCreateDto` (`RolCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Roles.ExistsByTipoAsync(...)`, `_context.SaveChangesAsync(...)`

#### `PutRol`

- **Firma:** `async Task<IActionResult> PutRol(int id, RolCreateDto rolCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `rolCreateDto` (`RolCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Roles.FindAsync(...)`, `_context.Roles.GetByTipoAsync(...)`, `_context.SaveChangesAsync(...)`, `RolExistsAsync(...)`

#### `DeleteRol`

- **Firma:** `async Task<IActionResult> DeleteRol(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `RolExistsAsync`

- **Firma:** `async Task<bool> RolExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Roles.AnyAsync(...)`

#### `GetRoleDescription`

- **Firma:** `string GetRoleDescription(RolTipo tipo)`
- **Retorno:** `string`
- **Parámetros:**

- `tipo` (`RolTipo`)

- **Qué hace:** Obtiene/consulta datos.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/RolServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
