# CategoriaEdadExtensions

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Extensions/CategoriaEdadExtensions.cs`

## 1. Qué es este archivo

Es un **Métodos de extensión** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Extension methods**: métodos estáticos con `this` que añaden API a tipos existentes.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Extensions`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `ToDisplayString`

- **Firma:** `string ToDisplayString(this CategoriaEdad categoria)`
- **Retorno:** `string`
- **Parámetros:**

- `categoria` (`this CategoriaEdad`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ToCodigo`

- **Firma:** `string ToCodigo(this CategoriaEdad categoria)`
- **Retorno:** `string`
- **Parámetros:**

- `categoria` (`this CategoriaEdad`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `static`

- **Firma:** `public static(int? min, int? max)`
- **Retorno:** `public`
- **Parámetros:**

- `min` (`int?`)
- `max` (`int?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetEdadMinima`

- **Firma:** `int? GetEdadMinima(this CategoriaEdad categoria)`
- **Retorno:** `int?`
- **Parámetros:**

- `categoria` (`this CategoriaEdad`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetEdadMaxima`

- **Firma:** `int? GetEdadMaxima(this CategoriaEdad categoria)`
- **Retorno:** `int?`
- **Parámetros:**

- `categoria` (`this CategoriaEdad`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetSexoDefault`

- **Firma:** `string GetSexoDefault(this CategoriaEdad categoria)`
- **Retorno:** `string`
- **Parámetros:**

- `categoria` (`this CategoriaEdad`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetDescripcion`

- **Firma:** `string GetDescripcion(this CategoriaEdad categoria)`
- **Retorno:** `string`
- **Parámetros:**

- `categoria` (`this CategoriaEdad`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetCategoriaPorEdad`

- **Firma:** `CategoriaEdad GetCategoriaPorEdad(int edad)`
- **Retorno:** `CategoriaEdad`
- **Parámetros:**

- `edad` (`int`)

- **Qué hace:** Obtiene/consulta datos. puede lanzar excepciones de dominio.

## 5. Notas de estudio

- Leé el `.cs` junto a este `.md` y marcá cada `using` e interfaz inyectada hasta entender el grafo de dependencias.
- Ruta relativa en el proyecto: `Extensions/CategoriaEdadExtensions.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
