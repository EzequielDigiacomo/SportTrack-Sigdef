# IEventoRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Evento/IEventoInterfaces.cs`

## 1. Qué es este archivo

Es un **Interfaz de repositorio (contrato de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `interface IEventoRepository`
- `interface IEventoService`

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Evento`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Evento.Dtos;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — `interface IEventoRepository`

### Métodos

#### `GetAllAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Evento>> GetAllAsync(int? clubId = null, string? rol = null)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Evento>>`
- **Parámetros:**

- `clubId` (`int?`)
- `rol` (`string?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetByIdAsync`

- **Firma:** `Task<Entidades.Entidades.Evento?> GetByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.Evento?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateAsync`

- **Firma:** `Task<Entidades.Entidades.Evento> CreateAsync(Entidades.Entidades.Evento evento)`
- **Retorno:** `Task<Entidades.Entidades.Evento>`
- **Parámetros:**

- `evento` (`Entidades.Entidades.Evento`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateAsync`

- **Firma:** `Task<Entidades.Entidades.Evento> UpdateAsync(Entidades.Entidades.Evento evento)`
- **Retorno:** `Task<Entidades.Entidades.Evento>`
- **Parámetros:**

- `evento` (`Entidades.Entidades.Evento`)

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

#### `GetProximosAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Evento>> GetProximosAsync(int? clubId = null, string? rol = null)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Evento>>`
- **Parámetros:**

- `clubId` (`int?`)
- `rol` (`string?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetPruebasByEventoIdAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.EventoPrueba>> GetPruebasByEventoIdAsync(int eventoId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.EventoPrueba>>`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEventoPruebaByIdAsync`

- **Firma:** `Task<Entidades.Entidades.EventoPrueba?> GetEventoPruebaByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.EventoPrueba?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AssignPruebaAsync`

- **Firma:** `Task<Entidades.Entidades.EventoPrueba> AssignPruebaAsync(Entidades.Entidades.EventoPrueba eventoPrueba)`
- **Retorno:** `Task<Entidades.Entidades.EventoPrueba>`
- **Parámetros:**

- `eventoPrueba` (`Entidades.Entidades.EventoPrueba`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateEventoPruebaAsync`

- **Firma:** `Task<Entidades.Entidades.EventoPrueba> UpdateEventoPruebaAsync(Entidades.Entidades.EventoPrueba eventoPrueba)`
- **Retorno:** `Task<Entidades.Entidades.EventoPrueba>`
- **Parámetros:**

- `eventoPrueba` (`Entidades.Entidades.EventoPrueba`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UnassignPruebaAsync`

- **Firma:** `Task<bool> UnassignPruebaAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetPruebaAsync`

- **Firma:** `Task<Entidades.Entidades.Prueba?> GetPruebaAsync(int categoriaId, int boteId, int distanciaId, int sexoId)`
- **Retorno:** `Task<Entidades.Entidades.Prueba?>`
- **Parámetros:**

- `categoriaId` (`int`)
- `boteId` (`int`)
- `distanciaId` (`int`)
- `sexoId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreatePruebaAsync`

- **Firma:** `Task<Entidades.Entidades.Prueba> CreatePruebaAsync(Entidades.Entidades.Prueba prueba)`
- **Retorno:** `Task<Entidades.Entidades.Prueba>`
- **Parámetros:**

- `prueba` (`Entidades.Entidades.Prueba`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 4. Detalle del tipo — `interface IEventoService`

### Métodos

#### `GetAllEventosAsync`

- **Firma:** `Task<IEnumerable<EventoDto>> GetAllEventosAsync(int? clubId = null, string? rol = null)`
- **Retorno:** `Task<IEnumerable<EventoDto>>`
- **Parámetros:**

- `clubId` (`int?`)
- `rol` (`string?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEventoByIdAsync`

- **Firma:** `Task<EventoDto> GetEventoByIdAsync(int id)`
- **Retorno:** `Task<EventoDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateEventoAsync`

- **Firma:** `Task<EventoDto> CreateEventoAsync(EventoCreateDto eventoDto)`
- **Retorno:** `Task<EventoDto>`
- **Parámetros:**

- `eventoDto` (`EventoCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateEventoAsync`

- **Firma:** `Task<EventoDto> UpdateEventoAsync(int id, EventoUpdateDto eventoDto, int? clubId = null)`
- **Retorno:** `Task<EventoDto>`
- **Parámetros:**

- `id` (`int`)
- `eventoDto` (`EventoUpdateDto`)
- `clubId` (`int?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteEventoAsync`

- **Firma:** `Task<bool> DeleteEventoAsync(int id, int? clubId = null)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)
- `clubId` (`int?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetProximosEventosAsync`

- **Firma:** `Task<IEnumerable<EventoDto>> GetProximosEventosAsync(int? clubId = null, string? rol = null)`
- **Retorno:** `Task<IEnumerable<EventoDto>>`
- **Parámetros:**

- `clubId` (`int?`)
- `rol` (`string?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetPruebasByEventoAsync`

- **Firma:** `Task<IEnumerable<EventoPruebaDto>> GetPruebasByEventoAsync(int eventoId)`
- **Retorno:** `Task<IEnumerable<EventoPruebaDto>>`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AssignPruebaToEventoAsync`

- **Firma:** `Task<EventoPruebaDto> AssignPruebaToEventoAsync(int eventoId, EventoPruebaCreateDto assignDto)`
- **Retorno:** `Task<EventoPruebaDto>`
- **Parámetros:**

- `eventoId` (`int`)
- `assignDto` (`EventoPruebaCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateEventoPruebaAsync`

- **Firma:** `Task<EventoPruebaDto> UpdateEventoPruebaAsync(int eventoPruebaId, EventoPruebaCreateDto updateDto)`
- **Retorno:** `Task<EventoPruebaDto>`
- **Parámetros:**

- `eventoPruebaId` (`int`)
- `updateDto` (`EventoPruebaCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteEventoPruebaAsync`

- **Firma:** `Task<bool> DeleteEventoPruebaAsync(int eventoPruebaId)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `eventoPruebaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Evento/IEventoInterfaces.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
