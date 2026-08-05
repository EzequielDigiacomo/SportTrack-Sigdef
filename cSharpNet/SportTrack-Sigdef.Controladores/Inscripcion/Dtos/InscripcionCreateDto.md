# InscripcionCreateDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Inscripcion/Dtos/InscripcionCreateDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class InscripcionCreateDto`
- `class InscripcionTripulanteCreateDto`
- `class InscripcionUpdateDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Inscripcion.Dtos`
- **Usings:**
  - `using System.Collections.Generic;`

## 4. Detalle del tipo — `class InscripcionCreateDto`

### Propiedades

- `EventoPruebaId` (`int`)
- `ParticipanteId` (`int?`)
- `NumeroCompetidor` (`string`)
- `Pagado` (`bool`)
- `Tripulantes` (`ICollection<InscripcionTripulanteCreateDto>`)

## 4. Detalle del tipo — `class InscripcionTripulanteCreateDto`

### Propiedades

- `ParticipanteId` (`int`)
- `PosicionEnBote` (`int?`)

## 4. Detalle del tipo — `class InscripcionUpdateDto`

### Propiedades

- `EventoPruebaId` (`int?`)
- `Estado` (`string?`)
- `NumeroCompetidor` (`string?`)
- `Pagado` (`bool?`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `Inscripcion/Dtos/InscripcionCreateDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
