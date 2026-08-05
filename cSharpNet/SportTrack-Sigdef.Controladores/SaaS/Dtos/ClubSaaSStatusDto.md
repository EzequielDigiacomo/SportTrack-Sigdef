# ClubSaaSStatusDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/SaaS/Dtos/ClubSaaSStatusDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class ClubSaaSStatusDto`
- `class TorneoSaaSDetailDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.SaaS.Dtos`
- **Usings:**
  - `using System;`
  - `using System.Collections.Generic;`

## 4. Detalle del tipo — `class ClubSaaSStatusDto`

### Propiedades

- `ClubId` (`int`)
- `ClubNombre` (`string`)
- `Sigla` (`string?`)
- `Email` (`string?`)
- `Telefono` (`string?`)
- `Direccion` (`string?`)
- `Ubicacion` (`string?`)
- `PlanSaaSId` (`int?`)
- `PlanNombre` (`string`)
- `MaxAtletas` (`int`)
- `AtletasRegistrados` (`int`)
- `ClubesAfiliadosCount` (`int`)
- `UsuariosCount` (`int`)
- `MaxTorneos` (`int`)
- `TorneosActivosCount` (`int`)
- `TorneosActivos` (`List<TorneoSaaSDetailDto>`)
- `PlanAlDia` (`bool`)
- `Activo` (`bool`)
- `FrecuenciaPago` (`string`)
- `FechaAltaPlan` (`DateTime?`)
- `FechaVencimientoPlan` (`DateTime?`)
- `BloqueadoPorFaltaDePago` (`bool`)

## 4. Detalle del tipo — `class TorneoSaaSDetailDto`

### Propiedades

- `Id` (`int`)
- `Nombre` (`string`)
- `Fecha` (`DateTime`)
- `Estado` (`string`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `SaaS/Dtos/ClubSaaSStatusDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
