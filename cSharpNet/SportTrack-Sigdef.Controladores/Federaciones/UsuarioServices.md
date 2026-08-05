# UsuarioServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/UsuarioServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IUsuarioServices`.

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
  - `using SportTrack_Sigdef.Entidades.DTOs.Usuario;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using System.Security.Cryptography;`
  - `using System.Text;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`
  - `using Microsoft.AspNetCore.Mvc;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_tenantProvider` — tipo `ITenantProvider` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `SportTrack_Sigdef.Controladores.Audit.IAuditService` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `GetUsuarios`

- **Firma:** `async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()`
- **Retorno:** `Task<ActionResult<IEnumerable<UsuarioDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetUsuario`

- **Firma:** `async Task<ActionResult<UsuarioDetailDto>> GetUsuario(int id)`
- **Retorno:** `Task<ActionResult<UsuarioDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetUsuarioPorUsername`

- **Firma:** `async Task<ActionResult<UsuarioDto>> GetUsuarioPorUsername(string username)`
- **Retorno:** `Task<ActionResult<UsuarioDto>>`
- **Parámetros:**

- `username` (`string`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `PostUsuario`

- **Firma:** `async Task<ActionResult<UsuarioDto>> PostUsuario(UsuarioCreateDto usuarioCreateDto)`
- **Retorno:** `Task<ActionResult<UsuarioDto>>`
- **Parámetros:**

- `usuarioCreateDto` (`UsuarioCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.AnyAsync(...)`, `_context.Usuarios.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `_context.Entry(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `Login`

- **Firma:** `async Task<ActionResult<UsuarioDto>> Login(UsuarioLoginDto loginDto)`
- **Retorno:** `Task<ActionResult<UsuarioDto>>`
- **Parámetros:**

- `loginDto` (`UsuarioLoginDto`)

- **Qué hace:** Gestiona autenticación. carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `PutUsuario`

- **Firma:** `async Task<IActionResult> PutUsuario(int id, UsuarioUpdateDto usuarioUpdateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `usuarioUpdateDto` (`UsuarioUpdateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Usuarios.FindAsync(...)`, `_context.Usuarios.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `UsuarioExistsAsync(...)`

#### `ChangePassword`

- **Firma:** `async Task<IActionResult> ChangePassword(int id, UsuarioChangePasswordDto changePasswordDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `changePasswordDto` (`UsuarioChangePasswordDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Usuarios.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `DeleteUsuario`

- **Firma:** `async Task<IActionResult> DeleteUsuario(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Usuarios.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `ResetPassword`

- **Firma:** `async Task<ActionResult<string>> ResetPassword(int id)`
- **Retorno:** `Task<ActionResult<string>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Usuarios.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `UsuarioExistsAsync`

- **Firma:** `async Task<bool> UsuarioExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Usuarios.AnyAsync(...)`

#### `HashPassword`

- **Firma:** `string HashPassword(string password)`
- **Retorno:** `string`
- **Parámetros:**

- `password` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `VerifyPassword`

- **Firma:** `bool VerifyPassword(string password, string storedHash)`
- **Retorno:** `bool`
- **Parámetros:**

- `password` (`string`)
- `storedHash` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/UsuarioServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
