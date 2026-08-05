# MensajeRepository

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Mensajes/MensajeRepository.cs`

## 1. Qué es este archivo

Es un **Repositorio (implementación de acceso a datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IMensajeRepository`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Repository pattern**: abstrae el acceso a datos; el servicio no habla directo con SQL/EF en cada detalle.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Mensajes`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `MensajeRepository(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetUsuarioByUsernameAsync`

- **Firma:** `async Task<Usuario?> GetUsuarioByUsernameAsync(string username)`
- **Retorno:** `Task<Usuario?>`
- **Parámetros:**

- `username` (`string`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`).

#### `GetUsuarioByIdAsync`

- **Firma:** `async Task<Usuario?> GetUsuarioByIdAsync(int id)`
- **Retorno:** `Task<Usuario?>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetUsuariosByIdsAsync`

- **Firma:** `async Task<List<Usuario>> GetUsuariosByIdsAsync(IEnumerable<int> ids)`
- **Retorno:** `Task<List<Usuario>>`
- **Parámetros:**

- `ids` (`IEnumerable<int>`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`).

#### `GetHilosVisiblesAsync`

- **Firma:** `async Task<List<Hilo>> GetHilosVisiblesAsync(int usuarioId, bool esSuperAdmin, string sistemaOrigen, int? campanaId = null)`
- **Retorno:** `Task<List<Hilo>>`
- **Parámetros:**

- `usuarioId` (`int`)
- `esSuperAdmin` (`bool`)
- `sistemaOrigen` (`string`)
- `campanaId` (`int?`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `GetHiloConMensajesAsync`

- **Firma:** `async Task<Hilo?> GetHiloConMensajesAsync(int hiloId)`
- **Retorno:** `Task<Hilo?>`
- **Parámetros:**

- `hiloId` (`int`)

- **Qué hace:** Obtiene/consulta datos.

#### `UsuarioParticipaEnHiloAsync`

- **Firma:** `async Task<bool> UsuarioParticipaEnHiloAsync(int hiloId, int usuarioId)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `hiloId` (`int`)
- `usuarioId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `AddHiloAsync`

- **Firma:** `async Task AddHiloAsync(Hilo hilo)`
- **Retorno:** `Task`
- **Parámetros:**

- `hilo` (`Hilo`)

- **Qué hace:** Crea/registra un nuevo recurso. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Hilos.AddAsync(...)`

#### `AddMensajeAsync`

- **Firma:** `async Task AddMensajeAsync(Mensaje mensaje)`
- **Retorno:** `Task`
- **Parámetros:**

- `mensaje` (`Mensaje`)

- **Qué hace:** Crea/registra un nuevo recurso. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Mensajes.AddAsync(...)`

#### `AddCampanaAsync`

- **Firma:** `async Task AddCampanaAsync(CampanaEnvio campana)`
- **Retorno:** `Task`
- **Parámetros:**

- `campana` (`CampanaEnvio`)

- **Qué hace:** Crea/registra un nuevo recurso. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.CampanasEnvio.AddAsync(...)`

#### `GetCampanasByRemitenteAsync`

- **Firma:** `async Task<List<CampanaEnvio>> GetCampanasByRemitenteAsync(int remitenteId, string sistemaOrigen)`
- **Retorno:** `Task<List<CampanaEnvio>>`
- **Parámetros:**

- `remitenteId` (`int`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetCampanaDetalleAsync`

- **Firma:** `async Task<CampanaEnvio?> GetCampanaDetalleAsync(int campanaId)`
- **Retorno:** `Task<CampanaEnvio?>`
- **Parámetros:**

- `campanaId` (`int`)

- **Qué hace:** Obtiene/consulta datos.

#### `SaveChangesAsync`

- **Firma:** `Task SaveChangesAsync()`
- **Retorno:** `Task`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CountNoLeidosAsync`

- **Firma:** `async Task<int> CountNoLeidosAsync(int usuarioId, string sistemaOrigen)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `usuarioId` (`int`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetEmisorNotificacionFederacionAsync`

- **Firma:** `async Task<Usuario?> GetEmisorNotificacionFederacionAsync(int idFederacion)`
- **Retorno:** `Task<Usuario?>`
- **Parámetros:**

- `idFederacion` (`int`)

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `GetUsuariosActivosByClubAsync`

- **Firma:** `async Task<List<Usuario>> GetUsuariosActivosByClubAsync(int clubId)`
- **Retorno:** `Task<List<Usuario>>`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetUsuariosAdminByFederacionAsync`

- **Firma:** `async Task<List<Usuario>> GetUsuariosAdminByFederacionAsync(int idFederacion)`
- **Retorno:** `Task<List<Usuario>>`
- **Parámetros:**

- `idFederacion` (`int`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetUsuariosSuperAdminAsync`

- **Firma:** `async Task<List<Usuario>> GetUsuariosSuperAdminAsync()`
- **Retorno:** `Task<List<Usuario>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos.

## 5. Notas de estudio

- El repositorio debería limitar la lógica a consultas/persistencia; las reglas de negocio van en el Service.
- Fijate si retorna entidades o ya proyecta a DTOs: el estilo puede variar en el proyecto.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Mensajes/MensajeRepository.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
