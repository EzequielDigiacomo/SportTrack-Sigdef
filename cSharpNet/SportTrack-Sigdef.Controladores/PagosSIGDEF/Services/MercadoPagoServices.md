# MercadoPagoService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/PagosSIGDEF/Services/MercadoPagoServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **Options pattern / Settings**: clases de configuración enlazadas a `appsettings.json`.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.PagosSIGDEF`
- **Usings:**
  - `using MercadoPago.Client.Common;`
  - `using MercadoPago.Client.Preference;`
  - `using MercadoPago.Config;`
  - `using MercadoPago.Resource.Preference;`
  - `using Microsoft.Extensions.Options;`
  - `using SportTrack_Sigdef.Controladores.PagosSIGDEF.Models.Dtos;`
  - `using SportTrack_Sigdef.Controladores.PagosSIGDEF.Config;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_accessToken` — tipo `string` (típicamente dependencia inyectada o estado privado)
- `_notificationUrl` — tipo `string` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `MercadoPagoService(...)`

**Parámetros:**

- `config` (`IOptions<MercadoPagoSettings>`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `CreatePreferenceAsync`

- **Firma:** `async Task<PaymentResponse> CreatePreferenceAsync(PaymentRequest request)`
- **Retorno:** `Task<PaymentResponse>`
- **Parámetros:**

- `request` (`PaymentRequest`)

- **Qué hace:** Crea/registra un nuevo recurso. operación asíncrona (`await`).
- **Llamadas await destacadas:** `client.CreateAsync(...)`

#### `GetPaymentStatusAsync`

- **Firma:** `async Task<PaymentResponse> GetPaymentStatusAsync(string paymentId)`
- **Retorno:** `Task<PaymentResponse>`
- **Parámetros:**

- `paymentId` (`string`)

- **Qué hace:** Obtiene/consulta datos. usa transacción de base de datos; realiza llamadas HTTP externas; operación asíncrona (`await`).
- **Llamadas await destacadas:** `client.GetAsync(...)`

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Integración con Mercado Pago: separá configuración (`Config`), modelos/DTOs y servicios HTTP.
- Ruta relativa en el proyecto: `PagosSIGDEF/Services/MercadoPagoServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
