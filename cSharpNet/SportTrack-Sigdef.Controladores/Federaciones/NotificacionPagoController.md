# NotificacionController

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/NotificacionPagoController.cs`

## 1. Qué es este archivo

Es un **Controlador (nota: vive en Controladores; normalmente los HTTP controllers están en la API)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `ControllerBase`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Controllers`
- **Usings:**
  - `using Microsoft.AspNetCore.Authorization;`
  - `using Microsoft.AspNetCore.Mvc;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Controladores.PagosSIGDEF.Services;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using Microsoft.EntityFrameworkCore;`
  - `using MercadoPago.Client.Payment;`
  - `using Microsoft.Extensions.Logging;`

## 4. Detalle del tipo — tipo principal

### Atributos del tipo

- `[Authorize]`

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_paymentService` — tipo `PaymentService` (típicamente dependencia inyectada o estado privado)
- `_logger` — tipo `ILogger<NotificacionController>` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `NotificacionController(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `paymentService` (`PaymentService`)
- `logger` (`ILogger<NotificacionController>`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `RecibirNotificacion`

- **Firma:** `async Task<IActionResult> RecibirNotificacion([FromQuery] string topic, [FromQuery] string id)`
- **Retorno:** `Task<IActionResult>`
- **Atributos:** `[AllowAnonymous]`, `[HttpPost("webhook")]`
- **Parámetros:**

- `topic` (`[FromQuery] string`)
- `id` (`[FromQuery] string`)

- **Qué hace:** Envía notificaciones. persiste cambios con `SaveChangesAsync`; realiza llamadas HTTP externas; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_paymentService.GetPaymentStatusAsync(...)`, `client.GetAsync(...)`, `_context.PagosTransacciones.FindAsync(...)`, `_context.SaveChangesAsync(...)`

## 5. Notas de estudio

- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/NotificacionPagoController.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
