# PlanSaaSAccessHelper

**Archivo fuente:** `SportTrack-Sigdef.Controladores/SaaS/PlanSaaSAccessHelper.cs`

## 1. Qué es este archivo

Es un **Helper / utilidad de negocio** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Tipos C#**: clases/interfaces organizadas por namespace en la capa de lógica de negocio.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.SaaS`
- **Usings:**
  - `using System;`
  - `using System.Linq;`
  - `using SportTrack_Sigdef.Controladores.SaaS.Dtos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `FromEntity`

- **Firma:** `PlanSaaSDto FromEntity(PlanSaaS plan)`
- **Retorno:** `PlanSaaSDto`
- **Parámetros:**

- `plan` (`PlanSaaS`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ApplyAccessFlags`

- **Firma:** `void ApplyAccessFlags(PlanSaaSDto dto)`
- **Retorno:** `void`
- **Parámetros:**

- `dto` (`PlanSaaSDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `static`

- **Firma:** `private static(bool sigdef, bool sporttrack, bool live)`
- **Retorno:** `private`
- **Parámetros:**

- `sigdef` (`bool`)
- `sporttrack` (`bool`)
- `live` (`bool`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `CanCreateRole`

- **Firma:** `bool CanCreateRole(PlanSaaSDto? plan, string? rol)`
- **Retorno:** `bool`
- **Parámetros:**

- `plan` (`PlanSaaSDto?`)
- `rol` (`string?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `IsJudgeRole`

- **Firma:** `bool IsJudgeRole(string? rol)`
- **Retorno:** `bool`
- **Parámetros:**

- `rol` (`string?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

## 5. Notas de estudio

- Leé el `.cs` junto a este `.md` y marcá cada `using` e interfaz inyectada hasta entender el grafo de dependencias.
- Ruta relativa en el proyecto: `SaaS/PlanSaaSAccessHelper.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
