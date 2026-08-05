# IClubServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/IClubServices.cs`

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
  - `using SportTrack_Sigdef.Entidades.DTOs.Club;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.EntrenadorFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Evento;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetClub`

- **Firma:** `Task<ActionResult<ClubDetailDto>> GetClub(int id)`
- **Retorno:** `Task<ActionResult<ClubDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetClubes`

- **Firma:** `Task<ActionResult<IEnumerable<ClubDto>>> GetClubes()`
- **Retorno:** `Task<ActionResult<IEnumerable<ClubDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetAtletasByClub`

- **Firma:** `Task<ActionResult<IEnumerable<AtletaDto>>> GetAtletasByClub(int id)`
- **Retorno:** `Task<ActionResult<IEnumerable<AtletaDto>>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEntrenadoresByClub`

- **Firma:** `Task<ActionResult<IEnumerable<EntrenadorDto>>> GetEntrenadoresByClub(int id)`
- **Retorno:** `Task<ActionResult<IEnumerable<EntrenadorDto>>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetDelegadosByClub`

- **Firma:** `Task<ActionResult<IEnumerable<DelegadoClubDto>>> GetDelegadosByClub(int id)`
- **Retorno:** `Task<ActionResult<IEnumerable<DelegadoClubDto>>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEventosByClub`

- **Firma:** `Task<ActionResult<IEnumerable<EventoDto>>> GetEventosByClub(int id)`
- **Retorno:** `Task<ActionResult<IEnumerable<EventoDto>>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `SearchClubes`

- **Firma:** `Task<ActionResult<IEnumerable<ClubDto>>> SearchClubes(string term)`
- **Retorno:** `Task<ActionResult<IEnumerable<ClubDto>>>`
- **Parámetros:**

- `term` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PostClub`

- **Firma:** `Task<ActionResult<ClubDto>> PostClub(ClubCreateDto clubCreateDto)`
- **Retorno:** `Task<ActionResult<ClubDto>>`
- **Parámetros:**

- `clubCreateDto` (`ClubCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PutClub`

- **Firma:** `Task<IActionResult> PutClub(int id, ClubCreateDto clubCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `clubCreateDto` (`ClubCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteClub`

- **Firma:** `Task<IActionResult> DeleteClub(int id)`
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
- Ruta relativa en el proyecto: `Federaciones/IClubServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
