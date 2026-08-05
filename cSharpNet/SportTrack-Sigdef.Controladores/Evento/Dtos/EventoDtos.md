# EventoDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Evento/Dtos/EventoDtos.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class EventoDto`
- `class EventoCreateDto`
- `class EventoUpdateDto`
- `class EventoPruebaDto`
- `class EventoPruebaCreateDto`
- `class PruebaDto`
- `class SexoDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Evento.Dtos`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Bote.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Categoria.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Distancia.Dtos;`
  - `using System;`
  - `using System.Collections.Generic;`

## 4. Detalle del tipo — `class EventoDto`

### Campos (dependencias / estado)

- `InscripcionesAbiertas` — tipo `bool` (típicamente dependencia inyectada o estado privado)

### Propiedades

- `Id` (`int`)
- `Nombre` (`string`)
- `Fecha` (`DateTime`)
- `FechaFin` (`DateTime?`)
- `Ubicacion` (`string?`)
- `Estado` (`string`)
- `FechaCreacion` (`DateTime`)
- `FechaFinInscripciones` (`DateTime?`)
- `ClubId` (`int?`)
- `FederacionId` (`int?`)
- `ClubNombre` (`string?`)
- `InscripcionesHabilitadas` (`bool`)
- `RestringirSoloCategoriaPropia` (`bool`)
- `PermitirSub23EnSenior` (`bool`)
- `PermitirMasterBajarASenior` (`bool`)
- `PermitirCompletarK4` (`bool`)
- `LimitacionBotesAB` (`bool`)
- `HoraInicioEvento` (`string`)
- `CarrilesDisponibles` (`int`)
- `PerfilTiempo` (`string`)
- `HoraInicioReceso` (`string`)
- `HoraFinReceso` (`string`)
- `SinReceso` (`bool`)
- `GapEntrePruebas` (`int`)
- `GapRecuperacionMinutos` (`int`)
- `PermitirCombinadas` (`bool`)
- `UsarGapVariable` (`bool`)
- `TimeZoneId` (`string`)
- `ResultadosTiempoReal` (`bool`)
- `CategoriasHabilitadas` (`string?`)
- `BotesHabilitados` (`string?`)
- `DistanciasHabilitadas` (`string?`)

## 4. Detalle del tipo — `class EventoCreateDto`

### Propiedades

- `Nombre` (`string`)
- `Fecha` (`DateTime`)
- `FechaFin` (`DateTime?`)
- `Ubicacion` (`string?`)
- `FechaFinInscripciones` (`DateTime?`)
- `RestringirSoloCategoriaPropia` (`bool`)
- `PermitirSub23EnSenior` (`bool`)
- `PermitirMasterBajarASenior` (`bool`)
- `PermitirCompletarK4` (`bool`)
- `LimitacionBotesAB` (`bool`)
- `ClubId` (`int?`)
- `FederacionId` (`int?`)
- `InscripcionesHabilitadas` (`bool`)
- `HoraInicioEvento` (`string`)
- `CarrilesDisponibles` (`int`)
- `PerfilTiempo` (`string`)
- `HoraInicioReceso` (`string`)
- `HoraFinReceso` (`string`)
- `SinReceso` (`bool`)
- `GapEntrePruebas` (`int`)
- `GapRecuperacionMinutos` (`int`)
- `PermitirCombinadas` (`bool`)
- `UsarGapVariable` (`bool`)
- `TimeZoneId` (`string`)
- `CategoriasHabilitadas` (`string?`)
- `BotesHabilitados` (`string?`)
- `DistanciasHabilitadas` (`string?`)

## 4. Detalle del tipo — `class EventoUpdateDto`

### Propiedades

- `Nombre` (`string?`)
- `Fecha` (`DateTime?`)
- `FechaFin` (`DateTime?`)
- `Ubicacion` (`string?`)
- `Estado` (`string?`)
- `FechaFinInscripciones` (`DateTime?`)
- `RestringirSoloCategoriaPropia` (`bool?`)
- `PermitirSub23EnSenior` (`bool?`)
- `PermitirMasterBajarASenior` (`bool?`)
- `PermitirCompletarK4` (`bool?`)
- `LimitacionBotesAB` (`bool?`)
- `InscripcionesHabilitadas` (`bool?`)
- `ClubId` (`int?`)
- `HoraInicioEvento` (`string?`)
- `CarrilesDisponibles` (`int?`)
- `PerfilTiempo` (`string?`)
- `HoraInicioReceso` (`string?`)
- `HoraFinReceso` (`string?`)
- `SinReceso` (`bool?`)
- `GapEntrePruebas` (`int?`)
- `GapRecuperacionMinutos` (`int?`)
- `PermitirCombinadas` (`bool?`)
- `UsarGapVariable` (`bool?`)
- `TimeZoneId` (`string?`)
- `CategoriasHabilitadas` (`string?`)
- `BotesHabilitados` (`string?`)
- `DistanciasHabilitadas` (`string?`)

## 4. Detalle del tipo — `class EventoPruebaDto`

### Propiedades

- `Id` (`int`)
- `EventoId` (`int`)
- `PruebaId` (`int`)
- `Prueba` (`PruebaDto?`)
- `FechaHora` (`DateTime`)
- `Estado` (`string?`)
- `CantidadInscritos` (`int`)
- `PlanProgresionAsignado` (`string?`)

## 4. Detalle del tipo — `class EventoPruebaCreateDto`

### Propiedades

- `CategoriaId` (`int`)
- `BoteId` (`int`)
- `DistanciaId` (`int`)
- `SexoId` (`int`)
- `FechaHora` (`DateTime?`)

## 4. Detalle del tipo — `class PruebaDto`

### Propiedades

- `Id` (`int`)
- `Nombre` (`string`)
- `Categoria` (`CategoriaDto`)
- `Bote` (`BoteDto`)
- `Distancia` (`DistanciaDto`)
- `Sexo` (`SexoDto?`)
- `SexoNombre` (`string`)
- `SexoId` (`int`)

## 4. Detalle del tipo — `class SexoDto`

### Propiedades

- `Id` (`int`)
- `Nombre` (`string`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `Evento/Dtos/EventoDtos.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
