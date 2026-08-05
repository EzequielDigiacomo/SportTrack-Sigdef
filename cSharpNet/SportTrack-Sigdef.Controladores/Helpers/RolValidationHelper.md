# RolValidationHelper

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Helpers/RolValidationHelper.cs`

## 1. Qué es este archivo

Es un **Helper / utilidad de negocio** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Tipos C#**: clases/interfaces organizadas por namespace en la capa de lógica de negocio.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Helpers`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_validRolIds` — tipo `readonly HashSet<int>` (típicamente dependencia inyectada o estado privado)

### Métodos

#### `IsValidRolId`

- **Firma:** `bool IsValidRolId(int rolId)`
- **Retorno:** `bool`
- **Parámetros:**

- `rolId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `IsValidRolTipo`

- **Firma:** `bool IsValidRolTipo(string tipo)`
- **Retorno:** `bool`
- **Parámetros:**

- `tipo` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `IsValidRolTipo`

- **Firma:** `bool IsValidRolTipo(RolTipo tipo)`
- **Retorno:** `bool`
- **Parámetros:**

- `tipo` (`RolTipo`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetValidRoles`

- **Firma:** `List<KeyValuePair<int, string>> GetValidRoles()`
- **Retorno:** `List<KeyValuePair<int, string>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. enumeración de valores de un `enum`.

## 5. Notas de estudio

- Leé el `.cs` junto a este `.md` y marcá cada `using` e interfaz inyectada hasta entender el grafo de dependencias.
- Ruta relativa en el proyecto: `Helpers/RolValidationHelper.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
