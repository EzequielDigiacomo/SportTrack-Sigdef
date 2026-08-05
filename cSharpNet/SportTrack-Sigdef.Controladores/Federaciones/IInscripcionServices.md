# IInscripcionServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/IInscripcionServices.cs`

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
  - `using SportTrack_Sigdef.Entidades.DTOs.Inscripcion;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetInscripcion`

- **Firma:** `Task<ActionResult<InscripcionDetailDto>> GetInscripcion(int id)`
- **Retorno:** `Task<ActionResult<InscripcionDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetInscripciones`

- **Firma:** `Task<ActionResult<IEnumerable<InscripcionDto>>> GetInscripciones()`
- **Retorno:** `Task<ActionResult<IEnumerable<InscripcionDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetInscripcionesPorAtleta`

- **Firma:** `Task<ActionResult<IEnumerable<InscripcionDto>>> GetInscripcionesPorAtleta(int ParticipanteId)`
- **Retorno:** `Task<ActionResult<IEnumerable<InscripcionDto>>>`
- **Parámetros:**

- `ParticipanteId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetInscripcionesPorEvento`

- **Firma:** `Task<ActionResult<IEnumerable<InscripcionDto>>> GetInscripcionesPorEvento(int idEvento)`
- **Retorno:** `Task<ActionResult<IEnumerable<InscripcionDto>>>`
- **Parámetros:**

- `idEvento` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PostInscripcion`

- **Firma:** `Task<ActionResult<InscripcionDto>> PostInscripcion(InscripcionCreateDto inscripcionCreateDto)`
- **Retorno:** `Task<ActionResult<InscripcionDto>>`
- **Parámetros:**

- `inscripcionCreateDto` (`InscripcionCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PutInscripcion`

- **Firma:** `Task<IActionResult> PutInscripcion(int id, InscripcionCreateDto inscripcionCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `inscripcionCreateDto` (`InscripcionCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteInscripcion`

- **Firma:** `Task<IActionResult> DeleteInscripcion(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/IInscripcionServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
