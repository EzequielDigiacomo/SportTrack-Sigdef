# IClubRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Club/IClubInterfaces.cs`

## 1. Qué es este archivo

Es un **Interfaz de repositorio (contrato de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `interface IClubRepository`
- `interface IClubService`

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Club`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Club.Dtos;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — `interface IClubRepository`

### Métodos

#### `GetAllAsync`

- **Firma:** `Task<IEnumerable<Entidades.Entidades.Club>> GetAllAsync()`
- **Retorno:** `Task<IEnumerable<Entidades.Entidades.Club>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetByIdAsync`

- **Firma:** `Task<Entidades.Entidades.Club?> GetByIdAsync(int id)`
- **Retorno:** `Task<Entidades.Entidades.Club?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateAsync`

- **Firma:** `Task<Entidades.Entidades.Club> CreateAsync(Entidades.Entidades.Club club)`
- **Retorno:** `Task<Entidades.Entidades.Club>`
- **Parámetros:**

- `club` (`Entidades.Entidades.Club`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateAsync`

- **Firma:** `Task<Entidades.Entidades.Club> UpdateAsync(Entidades.Entidades.Club club)`
- **Retorno:** `Task<Entidades.Entidades.Club>`
- **Parámetros:**

- `club` (`Entidades.Entidades.Club`)

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

## 4. Detalle del tipo — `interface IClubService`

### Métodos

#### `GetAllClubesAsync`

- **Firma:** `Task<IEnumerable<ClubDto>> GetAllClubesAsync()`
- **Retorno:** `Task<IEnumerable<ClubDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetClubByIdAsync`

- **Firma:** `Task<ClubDto> GetClubByIdAsync(int id)`
- **Retorno:** `Task<ClubDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateClubAsync`

- **Firma:** `Task<ClubDto> CreateClubAsync(ClubCreateDto clubDto)`
- **Retorno:** `Task<ClubDto>`
- **Parámetros:**

- `clubDto` (`ClubCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateClubAsync`

- **Firma:** `Task<ClubDto> UpdateClubAsync(int id, ClubUpdateDto clubDto)`
- **Retorno:** `Task<ClubDto>`
- **Parámetros:**

- `id` (`int`)
- `clubDto` (`ClubUpdateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteClubAsync`

- **Firma:** `Task<bool> DeleteClubAsync(int id)`
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
- Ruta relativa en el proyecto: `Club/IClubInterfaces.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
