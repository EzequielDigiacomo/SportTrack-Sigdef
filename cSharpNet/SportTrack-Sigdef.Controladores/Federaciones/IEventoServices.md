# IEventoServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/IEventoServices.cs`

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
  - `using SportTrack_Sigdef.Entidades.DTOs.Evento;`
  - `using SportTrack_Sigdef.Entidades.DTOs.EventoPrueba;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetEvento`

- **Firma:** `Task<ActionResult<EventoResponseDto>> GetEvento(int id)`
- **Retorno:** `Task<ActionResult<EventoResponseDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEventoDetalle`

- **Firma:** `Task<ActionResult<EventoDetailDto>> GetEventoDetalle(int id)`
- **Retorno:** `Task<ActionResult<EventoDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetProximosEventos`

- **Firma:** `Task<ActionResult<IEnumerable<EventoDto>>> GetProximosEventos()`
- **Retorno:** `Task<ActionResult<IEnumerable<EventoDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEventosConInscripcionesAbiertas`

- **Firma:** `Task<ActionResult<IEnumerable<EventoResponseDto>>> GetEventosConInscripcionesAbiertas()`
- **Retorno:** `Task<ActionResult<IEnumerable<EventoResponseDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEventosPorDistancia`

- **Firma:** `Task<ActionResult<IEnumerable<EventoResponseDto>>> GetEventosPorDistancia(int distancia)`
- **Retorno:** `Task<ActionResult<IEnumerable<EventoResponseDto>>>`
- **Parámetros:**

- `distancia` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetFormConfig`

- **Firma:** `Task<ActionResult<EventoFormConfigDto>> GetFormConfig()`
- **Retorno:** `Task<ActionResult<EventoFormConfigDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PostEvento`

- **Firma:** `Task<ActionResult<EventoResponseDto>> PostEvento(EventoCreateDTO eventoDto)`
- **Retorno:** `Task<ActionResult<EventoResponseDto>>`
- **Parámetros:**

- `eventoDto` (`EventoCreateDTO`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PutEvento`

- **Firma:** `Task<IActionResult> PutEvento(int id, EventoUpdateDto eventoDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `eventoDto` (`EventoUpdateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ActivarEvento`

- **Firma:** `Task<IActionResult> ActivarEvento(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DesactivarEvento`

- **Firma:** `Task<IActionResult> DesactivarEvento(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteEvento`

- **Firma:** `Task<IActionResult> DeleteEvento(int id)`
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
- Ruta relativa en el proyecto: `Federaciones/IEventoServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
