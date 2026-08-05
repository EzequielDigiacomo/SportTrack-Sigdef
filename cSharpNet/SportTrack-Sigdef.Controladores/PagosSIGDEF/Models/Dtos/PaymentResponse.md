# PaymentResponse

**Archivo fuente:** `SportTrack-Sigdef.Controladores/PagosSIGDEF/Models/Dtos/PaymentResponse.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.PagosSIGDEF.Models.Dtos`
- **Usings:**
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Text;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Success` (`bool`)
- `PaymentId` (`string`)
- `Status` (`string`)
- `PaymentUrl` (`string`)
- `SandboxPaymentUrl` (`string?`)
- `CreatedAt` (`DateTime`)
- `ErrorMessage` (`string?`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Integración con Mercado Pago: separá configuración (`Config`), modelos/DTOs y servicios HTTP.
- Ruta relativa en el proyecto: `PagosSIGDEF/Models/Dtos/PaymentResponse.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
