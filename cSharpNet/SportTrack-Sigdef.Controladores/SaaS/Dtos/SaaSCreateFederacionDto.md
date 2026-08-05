# SaaSCreateFederacionDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/SaaS/Dtos/SaaSCreateFederacionDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.SaaS.Dtos`
- **Usings:**
  - `using System.ComponentModel.DataAnnotations;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Nombre` (`string`) — atributos: `[Required]`
- `Sigla` (`string?`)
- `Email` (`string`) — atributos: `[Required]`
- `Telefono` (`string?`)
- `Direccion` (`string?`)
- `AdminUsername` (`string`) — atributos: `[Required]`
- `AdminEmail` (`string`) — atributos: `[Required]`, `[EmailAddress]`
- `AdminPassword` (`string`) — atributos: `[Required]`

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `SaaS/Dtos/SaaSCreateFederacionDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
