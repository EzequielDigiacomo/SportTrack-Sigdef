# ProgressionPlanRegistry

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Fase/Progression/ProgressionPlanRegistry.cs`

## 1. Qué es este archivo

Es un **Lógica de progresión de fases/series** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Fase.Progression`
- **Usings:**
  - _(ninguno explícito)_

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_plans` — tipo `readonly Dictionary<string, PlanDefinition>` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `RegisterAll`

- **Firma:** ` RegisterAll()`
- **Retorno:** ``
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `TryGet`

- **Firma:** `bool TryGet(string planId, out PlanDefinition plan)`
- **Retorno:** `bool`
- **Parámetros:**

- `planId` (`string`)
- `plan` (`PlanDefinition`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `Get`

- **Firma:** `PlanDefinition Get(string planId)`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

- `planId` (`string`)

- **Qué hace:** Obtiene/consulta datos.

#### `ResolveDefaultPlan`

- **Firma:** `string ResolveDefaultPlan(int count)`
- **Retorno:** `string`
- **Parámetros:**

- `count` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `RegisterAll`

- **Firma:** `void RegisterAll()`
- **Retorno:** `void`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `Register`

- **Firma:** `void Register(PlanDefinition plan)`
- **Retorno:** `void`
- **Parámetros:**

- `plan` (`PlanDefinition`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ValidatePlan`

- **Firma:** `void ValidatePlan(PlanDefinition plan)`
- **Retorno:** `void`
- **Parámetros:**

- `plan` (`PlanDefinition`)

- **Qué hace:** Valida reglas de negocio.

#### `ValidateStaticSlots`

- **Firma:** `void ValidateStaticSlots(PlanDefinition plan, IEnumerable<SlotRule> rules, string context)`
- **Retorno:** `void`
- **Parámetros:**

- `plan` (`PlanDefinition`)
- `rules` (`IEnumerable<SlotRule>`)
- `context` (`string`)

- **Qué hace:** Valida reglas de negocio. puede lanzar excepciones de dominio.

#### `S`

- **Firma:** `SlotRule S(int h, int p, string d, int l)`
- **Retorno:** `SlotRule`
- **Parámetros:**

- `h` (`int`)
- `p` (`int`)
- `d` (`string`)
- `l` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `B`

- **Firma:** `BtSlotRule B(int pos, int rank, string d, int l)`
- **Retorno:** `BtSlotRule`
- **Parámetros:**

- `pos` (`int`)
- `rank` (`int`)
- `d` (`string`)
- `l` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `A1`

- **Firma:** `PlanDefinition A1()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `A2`

- **Firma:** `PlanDefinition A2()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `B1`

- **Firma:** `PlanDefinition B1()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `B2`

- **Firma:** `PlanDefinition B2()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `C1`

- **Firma:** `PlanDefinition C1()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `C2`

- **Firma:** `PlanDefinition C2()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `D1`

- **Firma:** `PlanDefinition D1()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `D2`

- **Firma:** `PlanDefinition D2()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `E1`

- **Firma:** `PlanDefinition E1()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `E2`

- **Firma:** `PlanDefinition E2()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `F1`

- **Firma:** `PlanDefinition F1()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `F2`

- **Firma:** `PlanDefinition F2()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `G1`

- **Firma:** `PlanDefinition G1()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `G2`

- **Firma:** `PlanDefinition G2()`
- **Retorno:** `PlanDefinition`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

## 5. Notas de estudio

- La progresión de series/fases es lógica de negocio densa: leé primero los modelos y luego el engine.
- Ruta relativa en el proyecto: `Fase/Progression/ProgressionPlanRegistry.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
