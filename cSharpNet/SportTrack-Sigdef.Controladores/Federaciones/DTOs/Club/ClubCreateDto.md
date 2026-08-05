# ClubCreateDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Club/ClubCreateDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.Club`
- **Usings:**
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.ComponentModel.DataAnnotations;`
  - `using System.Linq;`
  - `using System.Text;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Nombre` (`string`) — atributos: `[Required(ErrorMessage = "El nombre del club es requerido")]`, `[MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]`
- `Direccion` (`string`) — atributos: `[MaxLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]`
- `Telefono` (`string`) — atributos: `[MaxLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]`
- `Siglas` (`string`) — atributos: `[Required(ErrorMessage = "Las siglas son requeridas")]`, `[MaxLength(10, ErrorMessage = "Las siglas no pueden exceder 10 caracteres")]`
- `Email` (`string`)
- `IdFederacion` (`int?`)
- `EstadoMatricula` (`Enums.EstadoPago`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Club/ClubCreateDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
