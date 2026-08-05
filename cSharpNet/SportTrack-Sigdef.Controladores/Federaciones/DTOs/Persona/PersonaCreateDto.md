# PersonaCreateDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Persona/PersonaCreateDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.Participante`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.ComponentModel.DataAnnotations;`
  - `using System.Linq;`
  - `using System.Text;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Nombre` (`string`) — atributos: `[Required(ErrorMessage = "El nombre es requerido")]`, `[MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]`
- `Apellido` (`string`) — atributos: `[Required(ErrorMessage = "El apellido es requerido")]`, `[MaxLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]`
- `Documento` (`string`) — atributos: `[Required(ErrorMessage = "El documento es requerido")]`, `[MaxLength(50, ErrorMessage = "El documento no puede exceder 50 caracteres")]`
- `FechaNacimiento` (`DateTime`) — atributos: `[Required(ErrorMessage = "La fecha de nacimiento es requerida")]`
- `Email` (`string?`) — atributos: `[EmailAddress(ErrorMessage = "El formato del email no es válido")]`, `[MaxLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]`
- `Telefono` (`string?`) — atributos: `[MaxLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]`
- `Direccion` (`string`) — atributos: `[MaxLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]`
- `Sexo` (`Sexo?`)
- `SexoId` (`int`)
- `SexoDisplay` (`string`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Persona/PersonaCreateDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
