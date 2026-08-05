# AuthService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Auth/AuthService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IAuthService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **AutoMapper**: biblioteca que mapea propiedades entre entidades y DTOs (`CreateMap`, `_mapper.Map`).
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Auth`
- **Usings:**
  - `using AutoMapper;`
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Controladores.Auth.Dtos;`
  - `using SportTrack_Sigdef.Controladores.SaaS;`
  - `using SportTrack_Sigdef.Controladores.SaaS.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Exceptions;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_tokenService` — tipo `ITokenService` (típicamente dependencia inyectada o estado privado)
- `_mapper` — tipo `IMapper` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `Audit.IAuditService` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `AuthService(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `tokenService` (`ITokenService`)
- `mapper` (`IMapper`)
- `auditService` (`Audit.IAuditService`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `LoginAsync`

- **Firma:** `async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string? clientApp = null)`
- **Retorno:** `Task<AuthResponseDto>`
- **Parámetros:**

- `loginDto` (`LoginDto`)
- `clientApp` (`string?`)

- **Qué hace:** Gestiona autenticación. puede lanzar `UnauthorizedException` por autenticación/autorización; usa AutoMapper para convertir entre entidad y DTO; carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; verifica/hashea contraseñas con BCrypt; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_auditService.RegistrarAccionAsync(...)`, `_context.SaveChangesAsync(...)`, `ResolvePlanForUserAsync(...)`

#### `RegisterAsync`

- **Firma:** `async Task<bool> RegisterAsync(RegisterDto registerDto)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `registerDto` (`RegisterDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; usa AutoMapper para convertir entre entidad y DTO; carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; consulta en modo solo-lectura (`AsNoTracking`); verifica/hashea contraseñas con BCrypt; operación asíncrona (`await`).
- **Llamadas await destacadas:** `UserExistsAsync(...)`, `_context.Clubes.AsNoTracking(...)`, `_context.Federaciones.AsNoTracking(...)`, `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `UserExistsAsync`

- **Firma:** `async Task<bool> UserExistsAsync(string username)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `username` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Usuarios.AnyAsync(...)`

#### `GetUsuariosAsync`

- **Firma:** `async Task<System.Collections.Generic.IEnumerable<UsuarioDto>> GetUsuariosAsync(string? requesterUsername = null)`
- **Retorno:** `Task<System.Collections.Generic.IEnumerable<UsuarioDto>>`
- **Parámetros:**

- `requesterUsername` (`string?`)

- **Qué hace:** Obtiene/consulta datos. usa AutoMapper para convertir entre entidad y DTO; carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Usuarios.FirstOrDefaultAsync(...)`, `_context.Clubes.AsNoTracking(...)`, `query.ToListAsync(...)`

#### `UpdatePasswordAsync`

- **Firma:** `async Task<bool> UpdatePasswordAsync(int id, string newPassword)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)
- `newPassword` (`string`)

- **Qué hace:** Actualiza un recurso existente. lanza `NotFoundException` si no encuentra el recurso; persiste cambios con `SaveChangesAsync`; verifica/hashea contraseñas con BCrypt; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Usuarios.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `GetMeAsync`

- **Firma:** `async Task<UsuarioDto> GetMeAsync(string username, string? clientApp = null)`
- **Retorno:** `Task<UsuarioDto>`
- **Parámetros:**

- `username` (`string`)
- `clientApp` (`string?`)

- **Qué hace:** Obtiene/consulta datos. lanza `NotFoundException` si no encuentra el recurso; puede lanzar `UnauthorizedException` por autenticación/autorización; usa AutoMapper para convertir entre entidad y DTO; carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).
- **Llamadas await destacadas:** `ResolvePlanForUserAsync(...)`

#### `ToggleActivoAsync`

- **Firma:** `async Task<bool> ToggleActivoAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Usuarios.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `UpdatePerfilAsync`

- **Firma:** `async Task<bool> UpdatePerfilAsync(int id, UpdatePerfilDto dto)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)
- `dto` (`UpdatePerfilDto`)

- **Qué hace:** Actualiza un recurso existente. lanza `NotFoundException` si no encuentra el recurso; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Usuarios.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `ResolvePlanForUserAsync`

- **Firma:** `async Task<PlanSaaS?> ResolvePlanForUserAsync(Usuario user)`
- **Retorno:** `Task<PlanSaaS?>`
- **Parámetros:**

- `user` (`Usuario`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.PlanesSaaS.FindAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- Revisá `Mappings/MappingProfile.cs` para ver cómo se mapea esta entidad/DTO.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Auth combina verificación de password (BCrypt), emisión de JWT y auditoría de intentos.
- Ruta relativa en el proyecto: `Auth/AuthService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
