# TipoEventoExtensions

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Extensions/TipoEventoExtensions.cs`

## 1. Qué es este archivo

Es un **Métodos de extensión** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Extension methods**: métodos estáticos con `this` que añaden API a tipos existentes.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Extensions`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `ToDisplayString`

- **Firma:** `string ToDisplayString(this TipoEvento tipo)`
- **Retorno:** `string`
- **Parámetros:**

- `tipo` (`this TipoEvento`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ToCodigo`

- **Firma:** `string ToCodigo(this TipoEvento tipo)`
- **Retorno:** `string`
- **Parámetros:**

- `tipo` (`this TipoEvento`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetIcono`

- **Firma:** `string GetIcono(this TipoEvento tipo)`
- **Retorno:** `string`
- **Parámetros:**

- `tipo` (`this TipoEvento`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetColor`

- **Firma:** `string GetColor(this TipoEvento tipo)`
- **Retorno:** `string`
- **Parámetros:**

- `tipo` (`this TipoEvento`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetDescripcion`

- **Firma:** `string GetDescripcion(this TipoEvento tipo)`
- **Retorno:** `string`
- **Parámetros:**

- `tipo` (`this TipoEvento`)

- **Qué hace:** Obtiene/consulta datos.

#### `RequiereInscripcion`

- **Firma:** `bool RequiereInscripcion(this TipoEvento tipo)`
- **Retorno:** `bool`
- **Parámetros:**

- `tipo` (`this TipoEvento`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `PermiteMultipleDistancias`

- **Firma:** `bool PermiteMultipleDistancias(this TipoEvento tipo)`
- **Retorno:** `bool`
- **Parámetros:**

- `tipo` (`this TipoEvento`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `RequiereJueces`

- **Firma:** `bool RequiereJueces(this TipoEvento tipo)`
- **Retorno:** `bool`
- **Parámetros:**

- `tipo` (`this TipoEvento`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `EsCompetitivo`

- **Firma:** `bool EsCompetitivo(this TipoEvento tipo)`
- **Retorno:** `bool`
- **Parámetros:**

- `tipo` (`this TipoEvento`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `EsFormativo`

- **Firma:** `bool EsFormativo(this TipoEvento tipo)`
- **Retorno:** `bool`
- **Parámetros:**

- `tipo` (`this TipoEvento`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetNivelDificultad`

- **Firma:** `string GetNivelDificultad(this TipoEvento tipo)`
- **Retorno:** `string`
- **Parámetros:**

- `tipo` (`this TipoEvento`)

- **Qué hace:** Obtiene/consulta datos.

## 5. Notas de estudio

- Leé el `.cs` junto a este `.md` y marcá cada `using` e interfaz inyectada hasta entender el grafo de dependencias.
- Ruta relativa en el proyecto: `Extensions/TipoEventoExtensions.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
