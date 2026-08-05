# FaseDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Fase/Dtos/FaseDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class FaseDto`
- `class ResultadoFaseDto`
- `class FaseBatchUpdateDto`
- `class FaseDetailsUpdateDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Fase.Dtos`
- **Usings:**
  - `using System;`
  - `using System.Collections.Generic;`

## 4. Detalle del tipo — `class FaseDto`

### Propiedades

- `Id` (`int`)
- `EtapaId` (`int`)
- `EtapaNombre` (`string`)
- `EventoPruebaId` (`int`)
- `NombreFase` (`string`)
- `NumeroFase` (`int`)
- `EtapaOrden` (`int`)
- `FechaHoraProgramada` (`DateTime?`)
- `Estado` (`string`)
- `Prueba` (`SportTrack_Sigdef.Controladores.Evento.Dtos.EventoPruebaDto?`)
- `Resultados` (`List<ResultadoFaseDto>`)

## 4. Detalle del tipo — `class ResultadoFaseDto`

### Propiedades

- `Id` (`int`)
- `FaseId` (`int`)
- `InscripcionId` (`int`)
- `ParticipanteId` (`int?`)
- `NumeroCompetidor` (`string?`)
- `ParticipanteNombre` (`string?`)
- `ClubNombre` (`string?`)
- `ClubSigla` (`string?`)
- `Carril` (`int?`)
- `EsCabezaDeSerie` (`bool`)
- `Tripulantes` (`List<SportTrack_Sigdef.Controladores.Inscripcion.Dtos.InscripcionTripulanteDto>`)
- `TiempoOficial` (`TimeSpan?`)
- `Posicion` (`int?`)
- `Estado` (`string`)

## 4. Detalle del tipo — `class FaseBatchUpdateDto`

### Propiedades

- `Id` (`int`)
- `FechaHoraProgramada` (`DateTime`)

## 4. Detalle del tipo — `class FaseDetailsUpdateDto`

_Tipo sin miembros propios (por ejemplo, hereda todo de otra clase o es un marcador vacío)._

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La progresión de series/fases es lógica de negocio densa: leé primero los modelos y luego el engine.
- Ruta relativa en el proyecto: `Fase/Dtos/FaseDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
