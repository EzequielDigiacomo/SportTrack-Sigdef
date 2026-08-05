# PagoTransaccionCreateDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/PagoTransaccion/PagoTransaccionCreateDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.PagoFederacionTransaccion`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.ComponentModel.DataAnnotations;`
  - `using System.Linq;`
  - `using System.Text;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Concepto` (`string`) — atributos: `[Required(ErrorMessage = "El concepto es requerido")]`, `[MaxLength(100, ErrorMessage = "El concepto no puede exceder 100 caracteres")]`
- `Monto` (`decimal`) — atributos: `[Required(ErrorMessage = "El monto es requerido")]`, `[Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]`
- `Estado` (`EstadoPagoTransaccion`) — atributos: `[Required(ErrorMessage = "El estado es requerido")]`
- `ParticipanteId` (`int`) — atributos: `[Required(ErrorMessage = "El ID de la Participante es requerido")]`
- `IdClub` (`int`) — atributos: `[Required(ErrorMessage = "El ID del club es requerido")]`
- `IdMercadoPago` (`string`) — atributos: `[MaxLength(100, ErrorMessage = "El ID de MercadoPago no puede exceder 100 caracteres")]`

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/PagoTransaccion/PagoTransaccionCreateDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
