# ITutorServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/ITutorServices.cs`

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
  - `using SportTrack_Sigdef.Entidades.DTOs.TutorFederacion;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetTutor`

- **Firma:** `Task<ActionResult<TutorDetailDto>> GetTutor(int id)`
- **Retorno:** `Task<ActionResult<TutorDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetTutores`

- **Firma:** `Task<ActionResult<IEnumerable<TutorDto>>> GetTutores()`
- **Retorno:** `Task<ActionResult<IEnumerable<TutorDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetTutoresPorTipo`

- **Firma:** `Task<ActionResult<IEnumerable<TutorDto>>> GetTutoresPorTipo(string tipoTutor)`
- **Retorno:** `Task<ActionResult<IEnumerable<TutorDto>>>`
- **Parámetros:**

- `tipoTutor` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetTutoresSinAtletas`

- **Firma:** `Task<ActionResult<IEnumerable<TutorDto>>> GetTutoresSinAtletas()`
- **Retorno:** `Task<ActionResult<IEnumerable<TutorDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetTiposTutor`

- **Firma:** `Task<ActionResult<IEnumerable<string>>> GetTiposTutor()`
- **Retorno:** `Task<ActionResult<IEnumerable<string>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `SearchTutores`

- **Firma:** `Task<ActionResult<IEnumerable<TutorDto>>> SearchTutores(string term)`
- **Retorno:** `Task<ActionResult<IEnumerable<TutorDto>>>`
- **Parámetros:**

- `term` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PostTutor`

- **Firma:** `Task<ActionResult<TutorDto>> PostTutor(TutorCreateDto tutorCreateDto)`
- **Retorno:** `Task<ActionResult<TutorDto>>`
- **Parámetros:**

- `tutorCreateDto` (`TutorCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PutTutor`

- **Firma:** `Task<IActionResult> PutTutor(int id, TutorCreateDto tutorCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `tutorCreateDto` (`TutorCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteTutor`

- **Firma:** `Task<IActionResult> DeleteTutor(int id)`
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
- Ruta relativa en el proyecto: `Federaciones/ITutorServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
