# ProgressionEngine

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Fase/Progression/ProgressionEngine.cs`

## 1. Qué es este archivo

Es un **Lógica de progresión de fases/series** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Fase.Progression`
- **Usings:**
  - `using InscripcionEntity = SportTrack_Sigdef.Entidades.Entidades.Inscripcion;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `PromoteFromEliminatoria`

- **Firma:** `ProgressionResult PromoteFromEliminatoria(PlanDefinition plan, RankedHeatContext ctx)`
- **Retorno:** `ProgressionResult`
- **Parámetros:**

- `plan` (`PlanDefinition`)
- `ctx` (`RankedHeatContext`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `PromoteFromSemifinal`

- **Firma:** `ProgressionResult PromoteFromSemifinal(PlanDefinition plan, RankedHeatContext ctx)`
- **Retorno:** `ProgressionResult`
- **Parámetros:**

- `plan` (`PlanDefinition`)
- `ctx` (`RankedHeatContext`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ApplySlots`

- **Firma:** ` ApplySlots(plan.ElimToFinalA, elimCtx, result, used, "H")`
- **Retorno:** ``
- **Parámetros:**

- `?` (`plan.ElimToFinalA`)
- `?` (`elimCtx`)
- `?` (`result`)
- `?` (`used`)
- `?` (`"H"`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetAtPosition`

- **Firma:** `InscripcionEntity? GetAtPosition(RankedHeatContext ctx, int heat, int position)`
- **Retorno:** `InscripcionEntity?`
- **Parámetros:**

- `ctx` (`RankedHeatContext`)
- `heat` (`int`)
- `position` (`int`)

- **Qué hace:** Obtiene/consulta datos.

#### `NormalizePlanId`

- **Firma:** `string NormalizePlanId(string? planProgresion, int inscriptosCount)`
- **Retorno:** `string`
- **Parámetros:**

- `planProgresion` (`string?`)
- `inscriptosCount` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

## 5. Notas de estudio

- La progresión de series/fases es lógica de negocio densa: leé primero los modelos y luego el engine.
- Ruta relativa en el proyecto: `Fase/Progression/ProgressionEngine.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
