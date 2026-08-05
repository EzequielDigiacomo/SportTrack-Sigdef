# EventoEstadoBackgroundService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Evento/EventoEstadoBackgroundService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `BackgroundService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **CancellationToken**: permite cancelar operaciones asíncronas largas.
- **BackgroundService**: servicio en segundo plano del host de ASP.NET Core.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Evento`
- **Usings:**
  - `using Microsoft.Extensions.DependencyInjection;`
  - `using Microsoft.Extensions.Hosting;`
  - `using Microsoft.Extensions.Logging;`
  - `using System;`
  - `using System.Threading;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `Interval` — tipo `TimeSpan` (típicamente dependencia inyectada o estado privado)
- `_scopeFactory` — tipo `IServiceScopeFactory` (típicamente dependencia inyectada o estado privado)
- `_logger` — tipo `ILogger<EventoEstadoBackgroundService>` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `ExecuteAsync`

- **Firma:** `async Task ExecuteAsync(CancellationToken stoppingToken)`
- **Retorno:** `Task`
- **Parámetros:**

- `stoppingToken` (`CancellationToken`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `Task.Delay(...)`, `syncService.SyncAllAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Evento/EventoEstadoBackgroundService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
