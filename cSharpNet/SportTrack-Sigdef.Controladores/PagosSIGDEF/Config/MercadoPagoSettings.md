# MercadoPagoSettings

**Archivo fuente:** `SportTrack-Sigdef.Controladores/PagosSIGDEF/Config/MercadoPagoSettings.cs`

## 1. Qué es este archivo

Es un **Clase de configuración** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Options pattern / Settings**: clases de configuración enlazadas a `appsettings.json`.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.PagosSIGDEF.Config`
- **Usings:**
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Text;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `AccessToken` (`string`)
- `PublicKey` (`string`)
- `WebhookSecret` (`string`)
- `NotificationUrl` (`string`)
- `SandboxMode` (`bool`)

## 5. Notas de estudio

- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Integración con Mercado Pago: separá configuración (`Config`), modelos/DTOs y servicios HTTP.
- Ruta relativa en el proyecto: `PagosSIGDEF/Config/MercadoPagoSettings.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
