# IInscripcionService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Inscripcion/IInscripcionService.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Inscripcion`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetAllInscripcionesAsync`

- **Firma:** `Task<IEnumerable<InscripcionDto>> GetAllInscripcionesAsync()`
- **Retorno:** `Task<IEnumerable<InscripcionDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetInscripcionByIdAsync`

- **Firma:** `Task<InscripcionDto> GetInscripcionByIdAsync(int id)`
- **Retorno:** `Task<InscripcionDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateInscripcionAsync`

- **Firma:** `Task<InscripcionDto> CreateInscripcionAsync(InscripcionCreateDto inscripcionDto)`
- **Retorno:** `Task<InscripcionDto>`
- **Parámetros:**

- `inscripcionDto` (`InscripcionCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateInscripcionAsync`

- **Firma:** `Task<InscripcionDto> UpdateInscripcionAsync(int id, InscripcionUpdateDto inscripcionDto)`
- **Retorno:** `Task<InscripcionDto>`
- **Parámetros:**

- `id` (`int`)
- `inscripcionDto` (`InscripcionUpdateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteInscripcionAsync`

- **Firma:** `Task<bool> DeleteInscripcionAsync(int id, bool allowWhenClosed = false)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)
- `allowWhenClosed` (`bool`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetCountByEventoPruebaIdAsync`

- **Firma:** `Task<int> GetCountByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetInscripcionesByEventoPruebaIdAsync`

- **Firma:** `Task<IEnumerable<InscripcionDto>> GetInscripcionesByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<InscripcionDto>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetInscripcionesByEventoAndClubAsync`

- **Firma:** `Task<IEnumerable<InscripcionDto>> GetInscripcionesByEventoAndClubAsync(int eventoId, int clubId)`
- **Retorno:** `Task<IEnumerable<InscripcionDto>>`
- **Parámetros:**

- `eventoId` (`int`)
- `clubId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ToggleEsCabezaDeSerieAsync`

- **Firma:** `Task<bool> ToggleEsCabezaDeSerieAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Inscripcion/IInscripcionService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
