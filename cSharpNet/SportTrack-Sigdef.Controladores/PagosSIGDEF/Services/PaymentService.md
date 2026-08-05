# PaymentService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/PagosSIGDEF/Services/PaymentService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.PagosSIGDEF.Services`
- **Usings:**
  - `using Microsoft.Extensions.Logging;`
  - `using SportTrack_Sigdef.Controladores.PagosSIGDEF.Models.Dtos;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_mercadoPagoService` — tipo `MercadoPagoService` (típicamente dependencia inyectada o estado privado)
- `_logger` — tipo `ILogger<PaymentService>` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `ProcessPaymentAsync`

- **Firma:** `async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)`
- **Retorno:** `Task<PaymentResponse>`
- **Parámetros:**

- `request` (`PaymentRequest`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. puede lanzar excepciones de dominio; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_mercadoPagoService.CreatePreferenceAsync(...)`

#### `GetPaymentStatusAsync`

- **Firma:** `async Task<PaymentResponse> GetPaymentStatusAsync(string gateway, string paymentId)`
- **Retorno:** `Task<PaymentResponse>`
- **Parámetros:**

- `gateway` (`string`)
- `paymentId` (`string`)

- **Qué hace:** Obtiene/consulta datos. puede lanzar excepciones de dominio; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_mercadoPagoService.GetPaymentStatusAsync(...)`

#### `IsGatewayAvailable`

- **Firma:** `bool IsGatewayAvailable(string gateway)`
- **Retorno:** `bool`
- **Parámetros:**

- `gateway` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Integración con Mercado Pago: separá configuración (`Config`), modelos/DTOs y servicios HTTP.
- Ruta relativa en el proyecto: `PagosSIGDEF/Services/PaymentService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
