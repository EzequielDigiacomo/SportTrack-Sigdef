# AuditService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Audit/AuditService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IAuditService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Audit`
- **Usings:**
  - `using Microsoft.AspNetCore.Http;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_httpContextAccessor` — tipo `IHttpContextAccessor` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `AuditService(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `httpContextAccessor` (`IHttpContextAccessor`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `RegistrarAccionAsync`

- **Firma:** `async Task RegistrarAccionAsync(string accion, string detalle, string? usuario = null, string modulo = "General")`
- **Retorno:** `Task`
- **Parámetros:**

- `accion` (`string`)
- `detalle` (`string`)
- `usuario` (`string?`)
- `modulo` (`string`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `RegistrarErrorAsync`

- **Firma:** `async Task RegistrarErrorAsync(Exception ex, string modulo = "System")`
- **Retorno:** `Task`
- **Parámetros:**

- `ex` (`Exception`)
- `modulo` (`string`)

- **Qué hace:** Crea/registra un nuevo recurso. operación asíncrona (`await`).
- **Llamadas await destacadas:** `RegistrarAccionAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Audit/AuditService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
