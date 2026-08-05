# FederacionServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/FederacionServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IFederacionServices`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Federaciones`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Federacion;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`
  - `using Microsoft.AspNetCore.Mvc;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `FederacionServices(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetFederaciones`

- **Firma:** `async Task<ActionResult<IEnumerable<FederacionDto>>> GetFederaciones()`
- **Retorno:** `Task<ActionResult<IEnumerable<FederacionDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).

#### `GetFederacion`

- **Firma:** `async Task<ActionResult<FederacionDto>> GetFederacion(int id)`
- **Retorno:** `Task<ActionResult<FederacionDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `PostFederacion`

- **Firma:** `async Task<ActionResult<FederacionDto>> PostFederacion(FederacionCreateDto federacionCreateDto)`
- **Retorno:** `Task<ActionResult<FederacionDto>>`
- **Parámetros:**

- `federacionCreateDto` (`FederacionCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `PutFederacion`

- **Firma:** `async Task<IActionResult> PutFederacion(int id, FederacionCreateDto federacionCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `federacionCreateDto` (`FederacionCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Federaciones.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `FederacionExistsAsync(...)`

#### `DeleteFederacion`

- **Firma:** `async Task<IActionResult> DeleteFederacion(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Federaciones.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `FederacionExistsAsync`

- **Firma:** `async Task<bool> FederacionExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Federaciones.AnyAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/FederacionServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
