# IMensajeRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Mensajes/IMensajeRepository.cs`

## 1. Qué es este archivo

Es un **Interfaz de repositorio (contrato de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Mensajes`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Entidades;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetUsuarioByUsernameAsync`

- **Firma:** `Task<Usuario?> GetUsuarioByUsernameAsync(string username)`
- **Retorno:** `Task<Usuario?>`
- **Parámetros:**

- `username` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetUsuarioByIdAsync`

- **Firma:** `Task<Usuario?> GetUsuarioByIdAsync(int id)`
- **Retorno:** `Task<Usuario?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetUsuariosByIdsAsync`

- **Firma:** `Task<List<Usuario>> GetUsuariosByIdsAsync(IEnumerable<int> ids)`
- **Retorno:** `Task<List<Usuario>>`
- **Parámetros:**

- `ids` (`IEnumerable<int>`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetHilosVisiblesAsync`

- **Firma:** `Task<List<Hilo>> GetHilosVisiblesAsync(int usuarioId, bool esSuperAdmin, string sistemaOrigen, int? campanaId = null)`
- **Retorno:** `Task<List<Hilo>>`
- **Parámetros:**

- `usuarioId` (`int`)
- `esSuperAdmin` (`bool`)
- `sistemaOrigen` (`string`)
- `campanaId` (`int?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetHiloConMensajesAsync`

- **Firma:** `Task<Hilo?> GetHiloConMensajesAsync(int hiloId)`
- **Retorno:** `Task<Hilo?>`
- **Parámetros:**

- `hiloId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UsuarioParticipaEnHiloAsync`

- **Firma:** `Task<bool> UsuarioParticipaEnHiloAsync(int hiloId, int usuarioId)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `hiloId` (`int`)
- `usuarioId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddHiloAsync`

- **Firma:** `Task AddHiloAsync(Hilo hilo)`
- **Retorno:** `Task`
- **Parámetros:**

- `hilo` (`Hilo`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddMensajeAsync`

- **Firma:** `Task AddMensajeAsync(Mensaje mensaje)`
- **Retorno:** `Task`
- **Parámetros:**

- `mensaje` (`Mensaje`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddCampanaAsync`

- **Firma:** `Task AddCampanaAsync(CampanaEnvio campana)`
- **Retorno:** `Task`
- **Parámetros:**

- `campana` (`CampanaEnvio`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetCampanasByRemitenteAsync`

- **Firma:** `Task<List<CampanaEnvio>> GetCampanasByRemitenteAsync(int remitenteId, string sistemaOrigen)`
- **Retorno:** `Task<List<CampanaEnvio>>`
- **Parámetros:**

- `remitenteId` (`int`)
- `sistemaOrigen` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetCampanaDetalleAsync`

- **Firma:** `Task<CampanaEnvio?> GetCampanaDetalleAsync(int campanaId)`
- **Retorno:** `Task<CampanaEnvio?>`
- **Parámetros:**

- `campanaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `SaveChangesAsync`

- **Firma:** `Task SaveChangesAsync()`
- **Retorno:** `Task`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CountNoLeidosAsync`

- **Firma:** `Task<int> CountNoLeidosAsync(int usuarioId, string sistemaOrigen)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `usuarioId` (`int`)
- `sistemaOrigen` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEmisorNotificacionFederacionAsync`

- **Firma:** `Task<Usuario?> GetEmisorNotificacionFederacionAsync(int idFederacion)`
- **Retorno:** `Task<Usuario?>`
- **Parámetros:**

- `idFederacion` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetUsuariosActivosByClubAsync`

- **Firma:** `Task<List<Usuario>> GetUsuariosActivosByClubAsync(int clubId)`
- **Retorno:** `Task<List<Usuario>>`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetUsuariosAdminByFederacionAsync`

- **Firma:** `Task<List<Usuario>> GetUsuariosAdminByFederacionAsync(int idFederacion)`
- **Retorno:** `Task<List<Usuario>>`
- **Parámetros:**

- `idFederacion` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetUsuariosSuperAdminAsync`

- **Firma:** `Task<List<Usuario>> GetUsuariosSuperAdminAsync()`
- **Retorno:** `Task<List<Usuario>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Mensajes/IMensajeRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
