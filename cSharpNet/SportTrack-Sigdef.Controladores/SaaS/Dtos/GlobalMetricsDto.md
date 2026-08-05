# GlobalMetricsDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/SaaS/Dtos/GlobalMetricsDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class GlobalMetricsDto`
- `class MonthlyGrowthDto`
- `class FederacionMetricDto`
- `class PlanDistributionDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.SaaS.Dtos`
- **Usings:**
  - `using System.Collections.Generic;`

## 4. Detalle del tipo — `class GlobalMetricsDto`

### Propiedades

- `TotalFederaciones` (`int`)
- `TotalClubesAfiliados` (`int`)
- `TotalAtletasGlobales` (`int`)
- `TorneosActivosGlobales` (`int`)
- `IngresosMensuales` (`decimal`)
- `FederacionesFacturando` (`int`)
- `PorcentajeCrecimientoAtletas` (`decimal`)
- `CrecimientoMensual` (`List<MonthlyGrowthDto>`)
- `TopFederaciones` (`List<FederacionMetricDto>`)
- `DistribucionPlanes` (`List<PlanDistributionDto>`)

## 4. Detalle del tipo — `class MonthlyGrowthDto`

### Propiedades

- `Mes` (`string`)
- `Cantidad` (`int`)

## 4. Detalle del tipo — `class FederacionMetricDto`

### Propiedades

- `Nombre` (`string`)
- `AtletasCount` (`int`)
- `ClubesCount` (`int`)

## 4. Detalle del tipo — `class PlanDistributionDto`

### Propiedades

- `Nombre` (`string`)
- `Cantidad` (`int`)
- `Precio` (`decimal`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `SaaS/Dtos/GlobalMetricsDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
