# ProgressionAssignment

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Fase/Progression/ProgressionModels.cs`

## 1. Qué es este archivo

Es un **Lógica de progresión de fases/series** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `record ProgressionAssignment`
- `class ProgressionResult`

## 2. Conceptos C# / .NET que aparecen

- **record vs class**: `record` enfatiza igualdad por valor e inmutabilidad; `class` es referencia mutable típica.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Fase.Progression`
- **Usings:**
  - `using InscripcionEntity = SportTrack_Sigdef.Entidades.Entidades.Inscripcion;`

## 4. Detalle del tipo — `record ProgressionAssignment`

### Propiedades

#### `PlanId`

- **Tipo:** `string`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `ElimToSemi`

- **Tipo:** `List<SlotRule>`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `ElimToSemiBt`

- **Tipo:** `List<BtSlotRule>`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `ElimToFinalA`

- **Tipo:** `List<SlotRule>`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `ElimToFinalB`

- **Tipo:** `List<SlotRule>`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `SemiToFinalA`

- **Tipo:** `List<SlotRule>`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `SemiToFinalB`

- **Tipo:** `List<SlotRule>`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `SemiToFinalC`

- **Tipo:** `List<SlotRule>`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `SemiToFinalBt`

- **Tipo:** `List<BtSlotRule>`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración


## 4. Detalle del tipo — `class ProgressionResult`

### Propiedades

#### `Destinos`

- **Tipo:** `Dictionary<string, Dictionary<int, InscripcionEntity>>`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `AuditTrail`

- **Tipo:** `List<ProgressionAssignment>`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración


### Métodos

#### `Assign`

- **Firma:** `void Assign(InscripcionEntity insc, string destino, int carril, string origen)`
- **Retorno:** `void`
- **Parámetros:**

- `insc` (`InscripcionEntity`)
- `destino` (`string`)
- `carril` (`int`)
- `origen` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. puede lanzar excepciones de dominio.

#### `GetInscripciones`

- **Firma:** `List<InscripcionEntity> GetInscripciones(string destino)`
- **Retorno:** `List<InscripcionEntity>`
- **Parámetros:**

- `destino` (`string`)

- **Qué hace:** Obtiene/consulta datos.

## 5. Notas de estudio

- La progresión de series/fases es lógica de negocio densa: leé primero los modelos y luego el engine.
- Ruta relativa en el proyecto: `Fase/Progression/ProgressionModels.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
