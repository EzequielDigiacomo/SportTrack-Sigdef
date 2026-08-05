# RegistroInscripcionDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Inscripcion/Dtos/RegistroInscripcionDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Inscripcion.Dtos`
- **Usings:**
  - `using System;`
  - `using System.Collections.Generic;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Id` (`int`)
- `ParticipanteId` (`int`)
- `ParticipanteNombre` (`string`)
- `ParticipanteDocumento` (`string?`)
- `ClubId` (`int?`)
- `ClubNombre` (`string?`)
- `EventoId` (`int`)
- `EventoNombre` (`string`)
- `EventoPruebaId` (`int`)
- `PruebaNombre` (`string`)
- `FechaInscripcion` (`DateTime`)
- `FechaInicioEvento` (`DateTime?`)
- `FechaFinEvento` (`DateTime?`)
- `Estado` (`string`)
- `Pagado` (`bool`)
- `TripulantesNombres` (`List<string>`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `Inscripcion/Dtos/RegistroInscripcionDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
