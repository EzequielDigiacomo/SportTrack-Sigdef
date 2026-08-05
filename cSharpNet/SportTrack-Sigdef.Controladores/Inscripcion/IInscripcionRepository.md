# IInscripcionRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Inscripcion/IInscripcionRepository.cs`

## 1. Qué es este archivo

Es un **Interfaz de repositorio (contrato de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Inscripcion`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetByIdAsync`

- **Firma:** `Task<Entidades.Entidades.Inscripcion?> GetByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.Inscripcion?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetAllAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetAllAsync()`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Inscripcion>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateAsync`

- **Firma:** `Task<Entidades.Entidades.Inscripcion> CreateAsync(Entidades.Entidades.Inscripcion inscripcion)`
- **Retorno:** `Task<Entidades.Entidades.Inscripcion>`
- **Parámetros:**

- `inscripcion` (`Entidades.Entidades.Inscripcion`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateAsync`

- **Firma:** `Task<Entidades.Entidades.Inscripcion> UpdateAsync(Entidades.Entidades.Inscripcion inscripcion)`
- **Retorno:** `Task<Entidades.Entidades.Inscripcion>`
- **Parámetros:**

- `inscripcion` (`Entidades.Entidades.Inscripcion`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteAsync`

- **Firma:** `Task<bool> DeleteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ExistsAsync`

- **Firma:** `Task<bool> ExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CountByEventoPruebaIdAsync`

- **Firma:** `Task<int> CountByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetByEventoPruebaIdAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetByEventoPruebaIdAsync(int eventoPruebaId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Inscripcion>>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetByEventoAndClubAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Inscripcion>> GetByEventoAndClubAsync(int eventoId, int clubId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Inscripcion>>`
- **Parámetros:**

- `eventoId` (`int`)
- `clubId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Inscripcion/IInscripcionRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
