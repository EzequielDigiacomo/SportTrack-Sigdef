# IAtletaServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/IAtletaServices.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Federaciones`
- **Usings:**
  - `using Microsoft.AspNetCore.Mvc;`
  - `using SIGDEF.DTOs;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Base;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `DeleteAtleta`

- **Firma:** `Task<IActionResult> DeleteAtleta(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetAtleta`

- **Firma:** `Task<ActionResult<AtletaDetailDto>> GetAtleta(int id)`
- **Retorno:** `Task<ActionResult<AtletaDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetAtletas`

- **Firma:** `Task<ActionResult<IEnumerable<AtletaDetailDto>>> GetAtletas()`
- **Retorno:** `Task<ActionResult<IEnumerable<AtletaDetailDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetAtletasByClub`

- **Firma:** `Task<ActionResult<IEnumerable<AtletaDetailDto>>> GetAtletasByClub(int clubId)`
- **Retorno:** `Task<ActionResult<IEnumerable<AtletaDetailDto>>>`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetAtletasPaginadosAsync`

- **Firma:** `Task<ActionResult<PagedResponseDto<AtletaListDto>>> GetAtletasPaginadosAsync(PaginationParamsDto parameters)`
- **Retorno:** `Task<ActionResult<PagedResponseDto<AtletaListDto>>>`
- **Parámetros:**

- `parameters` (`PaginationParamsDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PostAtleta`

- **Firma:** `Task<ActionResult<AtletaDto>> PostAtleta(AtletaCreateDto atletaCreateDto)`
- **Retorno:** `Task<ActionResult<AtletaDto>>`
- **Parámetros:**

- `atletaCreateDto` (`AtletaCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PostAtletaFull`

- **Firma:** `Task<ActionResult<AtletaDto>> PostAtletaFull(AtletaFullCreateDto atletaFullCreateDto)`
- **Retorno:** `Task<ActionResult<AtletaDto>>`
- **Parámetros:**

- `atletaFullCreateDto` (`AtletaFullCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PutAtleta`

- **Firma:** `Task<IActionResult> PutAtleta(int id, AtletaCreateDto atletaCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `atletaCreateDto` (`AtletaCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/IAtletaServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
