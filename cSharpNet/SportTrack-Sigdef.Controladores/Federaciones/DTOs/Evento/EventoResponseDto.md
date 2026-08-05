# EventoResponseDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Evento/EventoResponseDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.Evento`
- **Usings:**
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `DistanciaId` — tipo `int` (típicamente dependencia inyectada o estado privado)
- `DistanciaCodigo` — tipo `string` (típicamente dependencia inyectada o estado privado)
- `DistanciaNombre` — tipo `string` (típicamente dependencia inyectada o estado privado)
- `DistanciaMetros` — tipo `decimal` (típicamente dependencia inyectada o estado privado)
- `DistanciasDisplay` — tipo `string` (típicamente dependencia inyectada o estado privado)

### Propiedades

- `IdEvento` (`int`)
- `Nombre` (`string`)
- `Descripcion` (`string?`)
- `TipoEventoId` (`int`)
- `TipoEventoNombre` (`string`)
- `TipoEventoIcono` (`string`)
- `TipoEventoColor` (`string`)
- `FechaInicio` (`DateTime`)
- `FechaFin` (`DateTime`)
- `FechaInicioInscripciones` (`DateTime?`)
- `FechaFinInscripciones` (`DateTime?`)
- `Ubicacion` (`string?`)
- `Ciudad` (`string?`)
- `Provincia` (`string?`)
- `Pruebas` (`List<EventoPruebaResponseDto>`)
- `PrecioBase` (`decimal`)
- `CupoMaximo` (`int`)
- `TieneCronometraje` (`bool`)
- `RequiereCertificadoMedico` (`bool`)
- `EstaActivo` (`bool`)
- `FechaCreacion` (`DateTime`)
- `Observaciones` (`string?`)
- `TotalInscritos` (`int`)
- `CuposDisponibles` (`int`)
- `InscripcionesAbiertas` (`bool`)
- `TieneCupoDisponible` (`bool`)
- `DiasRestantes` (`int`)
- `FechasDisplay` (`string`)
- `PeriodoInscripcionesDisplay` (`string`)
- `EstadoDisplay` (`string`)
- `UbicacionCompleta` (`string`)
- `PrecioDisplay` (`string`)
- `CupoDisplay` (`string`)

### Métodos

#### `FromEntity`

- **Firma:** `EventoResponseDto FromEntity(Entidades.Evento evento)`
- **Retorno:** `EventoResponseDto`
- **Parámetros:**

- `evento` (`Entidades.Evento`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. filtra con LINQ (`Where`).

#### `GetTipoEventoDisplay`

- **Firma:** `string GetTipoEventoDisplay(Enums.TipoEvento tipo)`
- **Retorno:** `string`
- **Parámetros:**

- `tipo` (`Enums.TipoEvento`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetTipoEventoIcono`

- **Firma:** `string GetTipoEventoIcono(Enums.TipoEvento tipo)`
- **Retorno:** `string`
- **Parámetros:**

- `tipo` (`Enums.TipoEvento`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetTipoEventoColor`

- **Firma:** `string GetTipoEventoColor(Enums.TipoEvento tipo)`
- **Retorno:** `string`
- **Parámetros:**

- `tipo` (`Enums.TipoEvento`)

- **Qué hace:** Obtiene/consulta datos.

#### `CalcularInscripcionesAbiertas`

- **Firma:** `bool CalcularInscripcionesAbiertas(Entidades.Evento evento)`
- **Retorno:** `bool`
- **Parámetros:**

- `evento` (`Entidades.Evento`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Evento/EventoResponseDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
