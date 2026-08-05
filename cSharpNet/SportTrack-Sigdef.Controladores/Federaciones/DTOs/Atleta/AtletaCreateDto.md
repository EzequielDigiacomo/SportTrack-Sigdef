# AtletaCreateDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Atleta/AtletaCreateDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SIGDEF.DTOs`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.ComponentModel.DataAnnotations;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `ParticipanteId` (`int`) — atributos: `[Required]`
- `IdClub` (`int?`)
- `IdFederacion` (`int?`)
- `EstadoPago` (`EstadoPago`) — atributos: `[Required]`
- `PerteneceSeleccion` (`bool`) — atributos: `[Required]`
- `Categoria` (`CategoriaEdad?`)
- `BecadoEnard` (`bool`) — atributos: `[Required]`
- `BecadoSdn` (`bool`) — atributos: `[Required]`
- `MontoBeca` (`decimal`) — atributos: `[Required]`, `[Range(0, double.MaxValue, ErrorMessage = "El monto de la beca debe ser mayor o igual a 0")]`
- `PresentoAptoMedico` (`bool`) — atributos: `[Required]`
- `FechaAptoMedico` (`DateTime?`)
- `FechaCreacion` (`DateTime`)
- `NombrePersona` (`string?`)
- `NombreClub` (`string?`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Atleta/AtletaCreateDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
