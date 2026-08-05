# IParticipanteRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Participante/IParticipanteInterfaces.cs`

## 1. Qué es este archivo

Es un **Interfaz de repositorio (contrato de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `interface IParticipanteRepository`
- `interface IParticipanteService`

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Participante`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Participante.Dtos;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — `interface IParticipanteRepository`

### Métodos

#### `GetAllAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Participante>> GetAllAsync()`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Participante>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetByIdAsync`

- **Firma:** `Task<Entidades.Entidades.Participante?> GetByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.Participante?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetByClubIdAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Participante>> GetByClubIdAsync(int clubId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Participante>>`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetByFederationIdAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Participante>> GetByFederationIdAsync(int federationId)`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Participante>>`
- **Parámetros:**

- `federationId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateAsync`

- **Firma:** `Task<Entidades.Entidades.Participante> CreateAsync(Entidades.Entidades.Participante participante)`
- **Retorno:** `Task<Entidades.Entidades.Participante>`
- **Parámetros:**

- `participante` (`Entidades.Entidades.Participante`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateAsync`

- **Firma:** `Task<Entidades.Entidades.Participante> UpdateAsync(Entidades.Entidades.Participante participante)`
- **Retorno:** `Task<Entidades.Entidades.Participante>`
- **Parámetros:**

- `participante` (`Entidades.Entidades.Participante`)

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

#### `CountByClubIdAsync`

- **Firma:** `Task<int> CountByClubIdAsync(int clubId)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CountByFederationIdAsync`

- **Firma:** `Task<int> CountByFederationIdAsync(int federationId)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `federationId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 4. Detalle del tipo — `interface IParticipanteService`

### Métodos

#### `GetAllParticipantesAsync`

- **Firma:** `Task<IEnumerable<ParticipanteDto>> GetAllParticipantesAsync(int? clubId = null, string? rol = null, int? federacionId = null)`
- **Retorno:** `Task<IEnumerable<ParticipanteDto>>`
- **Parámetros:**

- `clubId` (`int?`)
- `rol` (`string?`)
- `federacionId` (`int?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetParticipanteByIdAsync`

- **Firma:** `Task<ParticipanteDto> GetParticipanteByIdAsync(int id)`
- **Retorno:** `Task<ParticipanteDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetParticipantesByClubAsync`

- **Firma:** `Task<IEnumerable<ParticipanteDto>> GetParticipantesByClubAsync(int clubId)`
- **Retorno:** `Task<IEnumerable<ParticipanteDto>>`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateParticipanteAsync`

- **Firma:** `Task<ParticipanteDto> CreateParticipanteAsync(ParticipanteCreateDto participanteDto)`
- **Retorno:** `Task<ParticipanteDto>`
- **Parámetros:**

- `participanteDto` (`ParticipanteCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateParticipanteAsync`

- **Firma:** `Task<ParticipanteDto> UpdateParticipanteAsync(int id, ParticipanteCreateDto participanteDto)`
- **Retorno:** `Task<ParticipanteDto>`
- **Parámetros:**

- `id` (`int`)
- `participanteDto` (`ParticipanteCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteParticipanteAsync`

- **Firma:** `Task<bool> DeleteParticipanteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Participante/IParticipanteInterfaces.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
