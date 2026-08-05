# IEntrenadorServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/IEntrenadorServices.cs`

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
  - `using SportTrack_Sigdef.Entidades.DTOs.EntrenadorFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetEntrenador`

- **Firma:** `Task<ActionResult<EntrenadorDetailDto>> GetEntrenador(int id)`
- **Retorno:** `Task<ActionResult<EntrenadorDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEntrenadores`

- **Firma:** `Task<ActionResult<IEnumerable<EntrenadorDto>>> GetEntrenadores()`
- **Retorno:** `Task<ActionResult<IEnumerable<EntrenadorDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEntrenadoresPorClub`

- **Firma:** `Task<ActionResult<IEnumerable<EntrenadorDto>>> GetEntrenadoresPorClub(int idClub)`
- **Retorno:** `Task<ActionResult<IEnumerable<EntrenadorDto>>>`
- **Parámetros:**

- `idClub` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEntrenadoresSeleccion`

- **Firma:** `Task<ActionResult<IEnumerable<EntrenadorDto>>> GetEntrenadoresSeleccion()`
- **Retorno:** `Task<ActionResult<IEnumerable<EntrenadorDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `SearchEntrenadores`

- **Firma:** `Task<ActionResult<IEnumerable<EntrenadorDto>>> SearchEntrenadores(string term)`
- **Retorno:** `Task<ActionResult<IEnumerable<EntrenadorDto>>>`
- **Parámetros:**

- `term` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PostEntrenador`

- **Firma:** `Task<ActionResult<EntrenadorDto>> PostEntrenador(EntrenadorCreateDto entrenadorCreateDto)`
- **Retorno:** `Task<ActionResult<EntrenadorDto>>`
- **Parámetros:**

- `entrenadorCreateDto` (`EntrenadorCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PutEntrenador`

- **Firma:** `Task<IActionResult> PutEntrenador(int id, EntrenadorCreateDto entrenadorCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `entrenadorCreateDto` (`EntrenadorCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteEntrenador`

- **Firma:** `Task<IActionResult> DeleteEntrenador(int id)`
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
- Ruta relativa en el proyecto: `Federaciones/IEntrenadorServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
