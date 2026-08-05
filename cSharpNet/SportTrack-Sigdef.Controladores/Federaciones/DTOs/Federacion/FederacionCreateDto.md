# FederacionCreateDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Federacion/FederacionCreateDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.Federacion`
- **Usings:**
  - `using System.ComponentModel.DataAnnotations;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Nombre` (`string`) — atributos: `[Required, MaxLength(100)]`
- `Cuit` (`string`) — atributos: `[MaxLength(20)]`
- `Email` (`string`) — atributos: `[MaxLength(100)]`
- `Telefono` (`string`) — atributos: `[MaxLength(20)]`
- `Direccion` (`string`) — atributos: `[MaxLength(200)]`
- `BancoNombre` (`string`) — atributos: `[MaxLength(100)]`
- `TipoCuenta` (`string`) — atributos: `[MaxLength(50)]`
- `NumeroCuenta` (`string`) — atributos: `[MaxLength(50)]`
- `TitularCuenta` (`string`) — atributos: `[MaxLength(100)]`
- `EmailCobro` (`string`) — atributos: `[MaxLength(100)]`
- `PlanSaaSId` (`int?`)
- `FechaAltaPlan` (`DateTime?`)
- `FechaVencimientoPlan` (`DateTime?`)
- `FrecuenciaPago` (`string?`)
- `BloqueadaPorFaltaDePago` (`bool?`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Federacion/FederacionCreateDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
