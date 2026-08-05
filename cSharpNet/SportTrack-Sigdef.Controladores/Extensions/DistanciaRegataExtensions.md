# DistanciaRegataExtensions

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Extensions/DistanciaRegataExtensions.cs`

## 1. Qué es este archivo

Es un **Métodos de extensión** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Extension methods**: métodos estáticos con `this` que añaden API a tipos existentes.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Extensions`
- **Usings:**
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Text;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `ToDisplayString`

- **Firma:** `string ToDisplayString(this DistanciaRegata distancia)`
- **Retorno:** `string`
- **Parámetros:**

- `distancia` (`this DistanciaRegata`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ToNombreCompleto`

- **Firma:** `string ToNombreCompleto(this DistanciaRegata distancia)`
- **Retorno:** `string`
- **Parámetros:**

- `distancia` (`this DistanciaRegata`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetMetros`

- **Firma:** `decimal GetMetros(this DistanciaRegata distancia)`
- **Retorno:** `decimal`
- **Parámetros:**

- `distancia` (`this DistanciaRegata`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetKilometros`

- **Firma:** `decimal GetKilometros(this DistanciaRegata distancia)`
- **Retorno:** `decimal`
- **Parámetros:**

- `distancia` (`this DistanciaRegata`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetUnidad`

- **Firma:** `string GetUnidad(this DistanciaRegata distancia)`
- **Retorno:** `string`
- **Parámetros:**

- `distancia` (`this DistanciaRegata`)

- **Qué hace:** Obtiene/consulta datos.

#### `EsDistanciaPista`

- **Firma:** `bool EsDistanciaPista(this DistanciaRegata distancia)`
- **Retorno:** `bool`
- **Parámetros:**

- `distancia` (`this DistanciaRegata`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `EsDistanciaRuta`

- **Firma:** `bool EsDistanciaRuta(this DistanciaRegata distancia)`
- **Retorno:** `bool`
- **Parámetros:**

- `distancia` (`this DistanciaRegata`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetTipoCarrera`

- **Firma:** `string GetTipoCarrera(this DistanciaRegata distancia)`
- **Retorno:** `string`
- **Parámetros:**

- `distancia` (`this DistanciaRegata`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetPorTipo`

- **Firma:** `List<DistanciaRegata> GetPorTipo(string tipo)`
- **Retorno:** `List<DistanciaRegata>`
- **Parámetros:**

- `tipo` (`string`)

- **Qué hace:** Obtiene/consulta datos. filtra con LINQ (`Where`); enumeración de valores de un `enum`.

## 5. Notas de estudio

- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Extensions/DistanciaRegataExtensions.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
