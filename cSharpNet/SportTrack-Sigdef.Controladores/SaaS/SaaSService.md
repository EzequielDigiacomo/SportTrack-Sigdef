# SaaSService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/SaaS/SaaSService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `ISaaSService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.SaaS`
- **Usings:**
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Controladores.SaaS.Dtos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using SportTrack_Sigdef.Controladores.Audit;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_auditService` — tipo `IAuditService` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `SaaSService(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `auditService` (`IAuditService`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetPlanesAsync`

- **Firma:** `async Task<IEnumerable<PlanSaaSDto>> GetPlanesAsync()`
- **Retorno:** `Task<IEnumerable<PlanSaaSDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.PlanesSaaS.ToListAsync(...)`

#### `GetPlanByIdAsync`

- **Firma:** `async Task<PlanSaaSDto> GetPlanByIdAsync(int id)`
- **Retorno:** `Task<PlanSaaSDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.PlanesSaaS.FindAsync(...)`

#### `AsignarPlanAClubAsync`

- **Firma:** `async Task AsignarPlanAClubAsync(int federacionId, int planId)`
- **Retorno:** `Task`
- **Parámetros:**

- `federacionId` (`int`)
- `planId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Federaciones.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `_context.PlanesSaaS.FindAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `GetClubesStatusAsync`

- **Firma:** `async Task<IEnumerable<ClubSaaSStatusDto>> GetClubesStatusAsync()`
- **Retorno:** `Task<IEnumerable<ClubSaaSStatusDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.PlanesSaaS.FirstOrDefaultAsync(...)`

#### `ToggleClubActivoAsync`

- **Firma:** `async Task ToggleClubActivoAsync(int federacionId)`
- **Retorno:** `Task`
- **Parámetros:**

- `federacionId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Federaciones.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `_auditService.RegistrarAccionAsync(...)`

#### `CreateFederacionWithAdminAsync`

- **Firma:** `async Task<int> CreateFederacionWithAdminAsync(SaaSCreateFederacionDto dto)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `dto` (`SaaSCreateFederacionDto`)

- **Qué hace:** Crea/registra un nuevo recurso. persiste cambios con `SaveChangesAsync`; usa transacción de base de datos; verifica/hashea contraseñas con BCrypt; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Database.BeginTransactionAsync(...)`, `_context.SaveChangesAsync(...)`, `transaction.CommitAsync(...)`, `transaction.RollbackAsync(...)`

#### `GetGlobalMetricsAsync`

- **Firma:** `async Task<GlobalMetricsDto> GetGlobalMetricsAsync()`
- **Retorno:** `Task<GlobalMetricsDto>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); ordena resultados; agrupa datos; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.CountAsync(...)`, `_context.Clubes.CountAsync(...)`, `_context.Eventos.CountAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `SaaS/SaaSService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
