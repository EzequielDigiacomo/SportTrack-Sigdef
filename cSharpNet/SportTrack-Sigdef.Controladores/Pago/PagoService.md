# PagoService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Pago/PagoService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IPagoService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Pago`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Controladores.Audit;`
  - `using SportTrack_Sigdef.Controladores.Exceptions;`
  - `using SportTrack_Sigdef.Controladores.Pago.Dtos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `IAuditService` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `PagoService(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `auditService` (`IAuditService`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetHistorialPagosAsync`

- **Firma:** `async Task<IEnumerable<PagoDto>> GetHistorialPagosAsync(int? fedId, string? role)`
- **Retorno:** `Task<IEnumerable<PagoDto>>`
- **Parámetros:**

- `fedId` (`int?`)
- `role` (`string?`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `RegistrarPagoAsync`

- **Firma:** `async Task<PagoDto> RegistrarPagoAsync(RegistrarPagoDto dto, string registradoPor)`
- **Retorno:** `Task<PagoDto>`
- **Parámetros:**

- `dto` (`RegistrarPagoDto`)
- `registradoPor` (`string`)

- **Qué hace:** Crea/registra un nuevo recurso. lanza `NotFoundException` si no encuentra el recurso; valida reglas de negocio y puede lanzar `BadRequestException`; carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Clubes.FindAsync(...)`, `_context.Participantes.FindAsync(...)`, `_context.AtletasFederados.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `ToggleClubPagoStatusAsync`

- **Firma:** `async Task<bool> ToggleClubPagoStatusAsync(int clubId, bool alDia)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `clubId` (`int`)
- `alDia` (`bool`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Clubes.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `SetSolicitudPagoPendienteAsync`

- **Firma:** `async Task<bool> SetSolicitudPagoPendienteAsync(int clubId, bool pendiente)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `clubId` (`int`)
- `pendiente` (`bool`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Clubes.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `ToggleAtletaPagoStatusAsync`

- **Firma:** `async Task<bool> ToggleAtletaPagoStatusAsync(int participanteId, bool alDia)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `participanteId` (`int`)
- `alDia` (`bool`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.FindAsync(...)`, `_context.AtletasFederados.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `ToggleInscripcionPagoStatusAsync`

- **Firma:** `async Task<bool> ToggleInscripcionPagoStatusAsync(int inscripcionId, bool pagado)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `inscripcionId` (`int`)
- `pagado` (`bool`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `EliminarPagoAsync`

- **Firma:** `async Task<bool> EliminarPagoAsync(int pagoId, string eliminadoPor)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `pagoId` (`int`)
- `eliminadoPor` (`string`)

- **Qué hace:** Elimina o desactiva un recurso. lanza `NotFoundException` si no encuentra el recurso; carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `EliminarPagosAsync`

- **Firma:** `async Task<int> EliminarPagosAsync(IEnumerable<int> pagoIds, string eliminadoPor)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `pagoIds` (`IEnumerable<int>`)
- `eliminadoPor` (`string`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Pago/PagoService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
