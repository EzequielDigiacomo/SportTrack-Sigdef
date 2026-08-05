# EventoEstadoSyncHelper

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Evento/EventoEstadoSyncHelper.cs`

## 1. Qué es este archivo

Es un **Helper / utilidad de negocio** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Tipos C#**: clases/interfaces organizadas por namespace en la capa de lógica de negocio.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Evento`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`

## 4. Detalle del tipo — tipo principal

### Constantes

- `DefaultTimeZoneId` (`string`)

### Métodos

#### `ComputeEstado`

- **Firma:** `EstadoEventoEnum ComputeEstado(SportTrack_Sigdef.Entidades.Entidades.Evento evento, DateTime utcNow)`
- **Retorno:** `EstadoEventoEnum`
- **Parámetros:**

- `evento` (`SportTrack_Sigdef.Entidades.Entidades.Evento`)
- `utcNow` (`DateTime`)

- **Qué hace:** Sincroniza o actualiza estado.

#### `ShouldAutoSync`

- **Firma:** `bool ShouldAutoSync(EstadoEventoEnum estado)`
- **Retorno:** `bool`
- **Parámetros:**

- `estado` (`EstadoEventoEnum`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ToLocalDateTime`

- **Firma:** `DateTime ToLocalDateTime(DateTime utcDate, TimeSpan timeOfDay, TimeZoneInfo tz)`
- **Retorno:** `DateTime`
- **Parámetros:**

- `utcDate` (`DateTime`)
- `timeOfDay` (`TimeSpan`)
- `tz` (`TimeZoneInfo`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ToLocalEndOfDay`

- **Firma:** `DateTime ToLocalEndOfDay(DateTime utcDate, TimeZoneInfo tz)`
- **Retorno:** `DateTime`
- **Parámetros:**

- `utcDate` (`DateTime`)
- `tz` (`TimeZoneInfo`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ResolveTimeZone`

- **Firma:** `TimeZoneInfo ResolveTimeZone(string? timeZoneId)`
- **Retorno:** `TimeZoneInfo`
- **Parámetros:**

- `timeZoneId` (`string?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso.

## 5. Notas de estudio

- Leé el `.cs` junto a este `.md` y marcá cada `using` e interfaz inyectada hasta entender el grafo de dependencias.
- Ruta relativa en el proyecto: `Evento/EventoEstadoSyncHelper.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
