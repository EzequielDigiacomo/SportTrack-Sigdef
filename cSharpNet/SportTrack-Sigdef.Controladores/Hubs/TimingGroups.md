# TimingGroups

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Hubs/TimingGroups.cs`

## 1. Qué es este archivo

Es un **Tipo `class` de la capa Controladores** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Tipos C#**: clases/interfaces organizadas por namespace en la capa de lógica de negocio.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Hubs`
- **Usings:**
  - _(ninguno explícito)_

## 4. Detalle del tipo — tipo principal

### Constantes

- `Operators` (`string`)

### Métodos

#### `Race`

- **Firma:** `string Race(int faseId)`
- **Retorno:** `string`
- **Parámetros:**

- `faseId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `Race`

- **Firma:** `string Race(string faseId)`
- **Retorno:** `string`
- **Parámetros:**

- `faseId` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `Event`

- **Firma:** `string Event(int eventoId)`
- **Retorno:** `string`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `Event`

- **Firma:** `string Event(string eventoId)`
- **Retorno:** `string`
- **Parámetros:**

- `eventoId` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Leé el `.cs` junto a este `.md` y marcá cada `using` e interfaz inyectada hasta entender el grafo de dependencias.
- Ruta relativa en el proyecto: `Hubs/TimingGroups.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
