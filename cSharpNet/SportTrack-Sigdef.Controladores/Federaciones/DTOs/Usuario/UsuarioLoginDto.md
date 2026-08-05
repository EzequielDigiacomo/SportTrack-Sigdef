# UsuarioLoginDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Usuario/UsuarioLoginDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.Usuario`
- **Usings:**
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.ComponentModel.DataAnnotations;`
  - `using System.Linq;`
  - `using System.Text;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Username` (`string`) — atributos: `[Required(ErrorMessage = "El nombre de usuario es requerido")]`, `[MaxLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres")]`
- `Password` (`string`) — atributos: `[Required(ErrorMessage = "La contraseña es requerida")]`, `[MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]`, `[DataType(DataType.Password)]`

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Usuario/UsuarioLoginDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
