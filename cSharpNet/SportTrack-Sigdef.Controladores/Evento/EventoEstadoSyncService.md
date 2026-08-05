# EventoEstadoSyncService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Evento/EventoEstadoSyncService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IEventoEstadoSyncService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **CancellationToken**: permite cancelar operaciones asíncronas largas.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Evento`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using Microsoft.Extensions.Logging;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.Linq;`
  - `using System.Threading;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_logger` — tipo `ILogger<EventoEstadoSyncService>` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `EventoEstadoSyncService(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `logger` (`ILogger<EventoEstadoSyncService>`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `SyncAllAsync`

- **Firma:** `async Task<int> SyncAllAsync(CancellationToken cancellationToken = default)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `cancellationToken` (`CancellationToken`)

- **Qué hace:** Sincroniza o actualiza estado. persiste cambios con `SaveChangesAsync`; filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `SyncEventoAsync`

- **Firma:** `async Task<bool> SyncEventoAsync(int eventoId, CancellationToken cancellationToken = default)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `eventoId` (`int`)
- `cancellationToken` (`CancellationToken`)

- **Qué hace:** Sincroniza o actualiza estado. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Eventos.FirstOrDefaultAsync(...)`, `_context.SaveChangesAsync(...)`

#### `ApplyEstadoIfChanged`

- **Firma:** `bool ApplyEstadoIfChanged(Entidades.Entidades.Evento evento, DateTime utcNow)`
- **Retorno:** `bool`
- **Parámetros:**

- `evento` (`Entidades.Entidades.Evento`)
- `utcNow` (`DateTime`)

- **Qué hace:** Sincroniza o actualiza estado.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Evento/EventoEstadoSyncService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
