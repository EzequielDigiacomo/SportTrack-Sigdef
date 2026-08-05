# ParticipanteDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Participante/Dtos/ParticipanteDtos.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class ParticipanteDto`
- `class ParticipanteCreateDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Participante.Dtos`
- **Usings:**
  - `using System;`

## 4. Detalle del tipo — `class ParticipanteDto`

### Propiedades

- `Id` (`int`)
- `ParticipanteId` (`int`)
- `Nombre` (`string`)
- `Apellido` (`string`)
- `FechaNacimiento` (`DateTime`)
- `SexoId` (`int`)
- `SexoNombre` (`string`)
- `CategoriaId` (`int?`)
- `CategoriaNombre` (`string?`)
- `ClubId` (`int?`)
- `ClubNombre` (`string?`)
- `Pais` (`string?`)
- `Dni` (`string?`)
- `Email` (`string?`)
- `Edad` (`int`)
- `PagoAfiliacionAlDia` (`bool`)

## 4. Detalle del tipo — `class ParticipanteCreateDto`

### Propiedades

- `Nombre` (`string`)
- `Apellido` (`string`)
- `FechaNacimiento` (`DateTime`)
- `SexoId` (`int`)
- `CategoriaId` (`int?`)
- `ClubId` (`int?`)
- `FederacionId` (`int?`)
- `Pais` (`string?`)
- `Dni` (`string?`)
- `Email` (`string?`)
- `PagoAfiliacionAlDia` (`bool`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `Participante/Dtos/ParticipanteDtos.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
