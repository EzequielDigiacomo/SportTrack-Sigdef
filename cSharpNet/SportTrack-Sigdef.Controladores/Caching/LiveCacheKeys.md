# LiveCacheKeys

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Caching/LiveCacheKeys.cs`

## 1. Qué es este archivo

Es un **Tipo `class` de la capa Controladores** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Tipos C#**: clases/interfaces organizadas por namespace en la capa de lógica de negocio.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Caching`
- **Usings:**
  - _(ninguno explícito)_

## 4. Detalle del tipo — tipo principal

### Métodos

#### `Evento`

- **Firma:** `string Evento(int eventoId)`
- **Retorno:** `string`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `FasesByEvento`

- **Firma:** `string FasesByEvento(int eventoId)`
- **Retorno:** `string`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `FasesByEventoPrueba`

- **Firma:** `string FasesByEventoPrueba(int eventoPruebaId)`
- **Retorno:** `string`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ResultadosByFase`

- **Firma:** `string ResultadosByFase(int faseId)`
- **Retorno:** `string`
- **Parámetros:**

- `faseId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PruebasByEvento`

- **Firma:** `string PruebasByEvento(int eventoId)`
- **Retorno:** `string`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Leé el `.cs` junto a este `.md` y marcá cada `using` e interfaz inyectada hasta entender el grafo de dependencias.
- Ruta relativa en el proyecto: `Caching/LiveCacheKeys.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
