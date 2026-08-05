# EventoPruebaResponseDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Evento/EventoPruebaResponseDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.Evento`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `IdEventoPrueba` (`int`)
- `DistanciaId` (`int`)
- `DistanciaCodigo` (`string`)
- `DistanciaNombre` (`string`)
- `Metros` (`decimal`)
- `CategoriaEdad` (`int`)
- `PrecioCategoria` (`decimal?`)
- `DistanciaRegata` (`int`)
- `TipoBote` (`int`)
- `TipoBoteNombre` (`string`)
- `SexoCompetencia` (`int`)

### Métodos

#### `MapToDistanciaRegata`

- **Firma:** `SportTrack_Sigdef.Entidades.Enums.DistanciaRegata MapToDistanciaRegata(DistanciaRegataEnum enumVal)`
- **Retorno:** `SportTrack_Sigdef.Entidades.Enums.DistanciaRegata`
- **Parámetros:**

- `enumVal` (`DistanciaRegataEnum`)

- **Qué hace:** Configura o aplica mapeos.

#### `FromEntity`

- **Firma:** `EventoPruebaResponseDto FromEntity(Entidades.EventoPrueba eventoPrueba)`
- **Retorno:** `EventoPruebaResponseDto`
- **Parámetros:**

- `eventoPrueba` (`Entidades.EventoPrueba`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetDistanciaDisplay`

- **Firma:** `string GetDistanciaDisplay(SportTrack_Sigdef.Entidades.Enums.DistanciaRegata distancia)`
- **Retorno:** `string`
- **Parámetros:**

- `distancia` (`SportTrack_Sigdef.Entidades.Enums.DistanciaRegata`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetDistanciaMetros`

- **Firma:** `decimal GetDistanciaMetros(SportTrack_Sigdef.Entidades.Enums.DistanciaRegata distancia)`
- **Retorno:** `decimal`
- **Parámetros:**

- `distancia` (`SportTrack_Sigdef.Entidades.Enums.DistanciaRegata`)

- **Qué hace:** Obtiene/consulta datos.

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Evento/EventoPruebaResponseDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
