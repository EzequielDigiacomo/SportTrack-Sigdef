# InscripcionDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Inscripcion/Dtos/InscripcionDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class InscripcionDto`
- `class InscripcionTripulanteDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Inscripcion.Dtos`
- **Usings:**
  - `using System;`
  - `using System.Collections.Generic;`

## 4. Detalle del tipo — `class InscripcionDto`

### Propiedades

- `Id` (`int`)
- `EventoPruebaId` (`int`)
- `ParticipanteId` (`int?`)
- `ParticipanteNombreCompleto` (`string?`)
- `ClubNombre` (`string?`)
- `ClubSigla` (`string?`)
- `FechaInscripcion` (`DateTime`)
- `NumeroCompetidor` (`string`)
- `EsCabezaDeSerie` (`bool`)
- `Estado` (`string`)
- `Pagado` (`bool`)
- `ClubId` (`int?`)
- `ParticipanteClubId` (`int?`)
- `EventoNombre` (`string?`)
- `PruebaNombre` (`string?`)
- `Tripulantes` (`ICollection<InscripcionTripulanteDto>`)

## 4. Detalle del tipo — `class InscripcionTripulanteDto`

### Propiedades

- `Id` (`int`)
- `ParticipanteId` (`int`)
- `ParticipanteNombreCompleto` (`string?`)
- `PosicionEnBote` (`int?`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `Inscripcion/Dtos/InscripcionDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
