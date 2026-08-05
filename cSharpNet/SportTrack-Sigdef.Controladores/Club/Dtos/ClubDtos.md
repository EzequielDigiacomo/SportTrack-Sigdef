# ClubDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Club/Dtos/ClubDtos.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class ClubDto`
- `class ClubCreateDto`
- `class ClubUpdateDto` : `ClubCreateDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Club.Dtos`
- **Usings:**
  - `using System.ComponentModel.DataAnnotations;`

## 4. Detalle del tipo — `class ClubDto`

### Propiedades

- `Id` (`int`)
- `Nombre` (`string`)
- `Sigla` (`string?`)
- `Email` (`string?`)
- `Telefono` (`string?`)
- `Direccion` (`string?`)
- `Ubicacion` (`string?`)
- `Activo` (`bool`)
- `CantidadAtletas` (`int`)
- `FederacionId` (`int?`)
- `FederacionNombre` (`string?`)
- `PlanSaaSId` (`int?`)
- `PlanNombre` (`string?`)
- `FrecuenciaPago` (`string?`)
- `FechaAltaPlan` (`DateTime?`)
- `FechaVencimientoPlan` (`DateTime?`)
- `BloqueadoPorFaltaDePago` (`bool`)
- `PagoAfiliacionAlDia` (`bool`)
- `SolicitudPagoPendiente` (`bool`)

## 4. Detalle del tipo — `class ClubCreateDto`

### Propiedades

- `Nombre` (`string`) — atributos: `[Required(ErrorMessage = "El nombre del club es obligatorio")]`, `[StringLength(100)]`
- `Sigla` (`string?`) — atributos: `[StringLength(10)]`
- `Email` (`string?`) — atributos: `[EmailAddress]`
- `Telefono` (`string?`)
- `Direccion` (`string?`)
- `Ubicacion` (`string?`)
- `Activo` (`bool`)
- `FederacionId` (`int?`)
- `FrecuenciaPago` (`string?`)
- `FechaAltaPlan` (`DateTime?`)
- `FechaVencimientoPlan` (`DateTime?`)
- `BloqueadoPorFaltaDePago` (`bool`)
- `PagoAfiliacionAlDia` (`bool`)
- `SolicitudPagoPendiente` (`bool`)

## 4. Detalle del tipo — `class ClubUpdateDto`

_Tipo sin miembros propios (por ejemplo, hereda todo de otra clase o es un marcador vacío)._

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `Club/Dtos/ClubDtos.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
