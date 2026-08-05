# PlanSaaSDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/SaaS/Dtos/PlanSaaSDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.SaaS.Dtos`
- **Usings:**
  - _(ninguno explícito)_

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Id` (`int`)
- `Nombre` (`string`)
- `Precio` (`decimal`)
- `MaxAtletas` (`int`)
- `MaxTorneosActivos` (`int`)
- `ResultadosTiempoReal` (`bool`)
- `ExportacionExcel` (`bool`)
- `ExportacionPdf` (`bool`)
- `SoportePrioritario` (`bool`)
- `AccesoDashboardClub` (`bool`)
- `PermitirCargaImagenes` (`bool`)
- `AccesoSigdef` (`bool`)
- `AccesoSportTrack` (`bool`)
- `AccesoControlesLive` (`bool`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `SaaS/Dtos/PlanSaaSDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
