# CrearHiloDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Mensajes/Dtos/MensajeDtos.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class CrearHiloDto`
- `class ResponderHiloDto`
- `class UsuarioResumenDto`
- `class MensajeItemDto`
- `class HiloListItemDto`
- `class HiloDetalleDto`
- `class EnviarMasivoDto`
- `class HiloCampanaItemDto`
- `class CampanaListItemDto`
- `class CampanaDetalleDto`
- `class EnviarMasivoResultDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Mensajes.Dtos`
- **Usings:**
  - _(ninguno explícito)_

## 4. Detalle del tipo — `class CrearHiloDto`

### Propiedades

- `DestinatarioId` (`int`)
- `Asunto` (`string`)
- `Cuerpo` (`string`)

## 4. Detalle del tipo — `class ResponderHiloDto`

### Propiedades

- `Cuerpo` (`string`)

## 4. Detalle del tipo — `class UsuarioResumenDto`

### Propiedades

- `Id` (`int`)
- `Username` (`string`)
- `RolFederacion` (`string`)
- `Nombre` (`string?`)
- `Apellido` (`string?`)
- `FederacionId` (`int?`)

## 4. Detalle del tipo — `class MensajeItemDto`

### Propiedades

- `IdMensaje` (`int`)
- `RemitenteId` (`int`)
- `DestinatarioId` (`int`)
- `Remitente` (`UsuarioResumenDto`)
- `Cuerpo` (`string`)
- `EnviadoEn` (`DateTime`)
- `LeidoEn` (`DateTime?`)
- `EsPropio` (`bool`)

## 4. Detalle del tipo — `class HiloListItemDto`

### Propiedades

- `IdHilo` (`int`)
- `Asunto` (`string`)
- `UltimoMensajeEn` (`DateTime`)
- `Contraparte` (`UsuarioResumenDto`)
- `UltimoMensajePreview` (`string`)
- `CantidadNoLeidos` (`int`)

## 4. Detalle del tipo — `class HiloDetalleDto`

### Propiedades

- `IdHilo` (`int`)
- `Asunto` (`string`)
- `CreadoEn` (`DateTime`)
- `UltimoMensajeEn` (`DateTime`)
- `IdCampana` (`int?`)
- `Mensajes` (`List<MensajeItemDto>`)

## 4. Detalle del tipo — `class EnviarMasivoDto`

### Propiedades

- `Asunto` (`string`)
- `Cuerpo` (`string`)
- `DestinatarioIds` (`List<int>`)

## 4. Detalle del tipo — `class HiloCampanaItemDto`

### Propiedades

- `HiloId` (`int`)
- `DestinatarioId` (`int`)
- `DestinatarioNombre` (`string`)
- `DestinatarioUsername` (`string`)
- `Leido` (`bool`)
- `Respondido` (`bool`)
- `UltimoMensajeEn` (`DateTime?`)

## 4. Detalle del tipo — `class CampanaListItemDto`

### Propiedades

- `IdCampana` (`int`)
- `Asunto` (`string`)
- `EnviadoEn` (`DateTime`)
- `CantidadDestinatarios` (`int`)
- `TipoCampana` (`string`)
- `CantidadLeidos` (`int`)
- `CantidadRespondidos` (`int`)

## 4. Detalle del tipo — `class CampanaDetalleDto`

### Propiedades

- `IdCampana` (`int`)
- `Asunto` (`string`)
- `Cuerpo` (`string`)
- `EnviadoEn` (`DateTime`)
- `CantidadDestinatarios` (`int`)
- `TipoCampana` (`string`)
- `Hilos` (`List<HiloCampanaItemDto>`)

## 4. Detalle del tipo — `class EnviarMasivoResultDto`

### Propiedades

- `CampanaId` (`int`)
- `CantidadHilos` (`int`)
- `Hilos` (`List<HiloCampanaItemDto>`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `Mensajes/Dtos/MensajeDtos.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
