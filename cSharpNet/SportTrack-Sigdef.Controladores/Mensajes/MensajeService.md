# MensajeService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Mensajes/MensajeService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IMensajeService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Mensajes`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Exceptions;`
  - `using SportTrack_Sigdef.Controladores.Mensajes.Dtos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_repository` — tipo `IMensajeRepository` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `MensajeService(...)`

**Parámetros:**

- `repository` (`IMensajeRepository`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetHilosAsync`

- **Firma:** `async Task<List<HiloListItemDto>> GetHilosAsync(string username, string sistemaOrigen, int? campanaId = null)`
- **Retorno:** `Task<List<HiloListItemDto>>`
- **Parámetros:**

- `username` (`string`)
- `sistemaOrigen` (`string`)
- `campanaId` (`int?`)

- **Qué hace:** Obtiene/consulta datos. filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `RequireUsuarioAsync(...)`, `_repository.GetHilosVisiblesAsync(...)`

#### `GetHiloDetalleAsync`

- **Firma:** `async Task<HiloDetalleDto> GetHiloDetalleAsync(int hiloId, string username, string sistemaOrigen)`
- **Retorno:** `Task<HiloDetalleDto>`
- **Parámetros:**

- `hiloId` (`int`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `RequireUsuarioAsync(...)`, `RequireHiloDelSistemaAsync(...)`, `RequireAccesoHiloAsync(...)`

#### `CrearHiloAsync`

- **Firma:** `async Task<HiloDetalleDto> CrearHiloAsync(CrearHiloDto dto, string username, string sistemaOrigen)`
- **Retorno:** `Task<HiloDetalleDto>`
- **Parámetros:**

- `dto` (`CrearHiloDto`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `RequireUsuarioAsync(...)`, `_repository.GetUsuarioByIdAsync(...)`, `_repository.AddHiloAsync(...)`, `_repository.AddMensajeAsync(...)`, `_repository.SaveChangesAsync(...)`, `_repository.GetHiloConMensajesAsync(...)`

#### `EnviarMasivoAsync`

- **Firma:** `async Task<EnviarMasivoResultDto> EnviarMasivoAsync(EnviarMasivoDto dto, string username, string sistemaOrigen)`
- **Retorno:** `Task<EnviarMasivoResultDto>`
- **Parámetros:**

- `dto` (`EnviarMasivoDto`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; puede lanzar `UnauthorizedException` por autenticación/autorización; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `RequireUsuarioAsync(...)`, `_repository.GetUsuariosByIdsAsync(...)`, `_repository.AddCampanaAsync(...)`, `_repository.SaveChangesAsync(...)`, `_repository.AddHiloAsync(...)`, `_repository.AddMensajeAsync(...)`

#### `GetCampanasAsync`

- **Firma:** `async Task<List<CampanaListItemDto>> GetCampanasAsync(string username, string sistemaOrigen)`
- **Retorno:** `Task<List<CampanaListItemDto>>`
- **Parámetros:**

- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Obtiene/consulta datos. puede lanzar `UnauthorizedException` por autenticación/autorización; operación asíncrona (`await`).
- **Llamadas await destacadas:** `RequireUsuarioAsync(...)`, `_repository.GetCampanasByRemitenteAsync(...)`

#### `GetCampanaDetalleAsync`

- **Firma:** `async Task<CampanaDetalleDto> GetCampanaDetalleAsync(int campanaId, string username, string sistemaOrigen)`
- **Retorno:** `Task<CampanaDetalleDto>`
- **Parámetros:**

- `campanaId` (`int`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Obtiene/consulta datos. lanza `NotFoundException` si no encuentra el recurso; puede lanzar `UnauthorizedException` por autenticación/autorización; operación asíncrona (`await`).
- **Llamadas await destacadas:** `RequireUsuarioAsync(...)`, `_repository.GetCampanaDetalleAsync(...)`

#### `ResponderHiloAsync`

- **Firma:** `async Task<HiloDetalleDto> ResponderHiloAsync(int hiloId, ResponderHiloDto dto, string username, string sistemaOrigen)`
- **Retorno:** `Task<HiloDetalleDto>`
- **Parámetros:**

- `hiloId` (`int`)
- `dto` (`ResponderHiloDto`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `RequireUsuarioAsync(...)`, `RequireHiloDelSistemaAsync(...)`, `RequireAccesoHiloAsync(...)`, `_repository.GetUsuarioByIdAsync(...)`, `_repository.AddMensajeAsync(...)`, `_repository.SaveChangesAsync(...)`, `_repository.GetHiloConMensajesAsync(...)`

#### `MarcarHiloLeidoAsync`

- **Firma:** `async Task MarcarHiloLeidoAsync(int hiloId, string username, string sistemaOrigen)`
- **Retorno:** `Task`
- **Parámetros:**

- `hiloId` (`int`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; filtra con LINQ (`Where`); notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `RequireUsuarioAsync(...)`, `RequireHiloDelSistemaAsync(...)`, `RequireAccesoHiloAsync(...)`, `_repository.SaveChangesAsync(...)`

#### `GetNoLeidosCountAsync`

- **Firma:** `async Task<int> GetNoLeidosCountAsync(string username, string sistemaOrigen)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `RequireUsuarioAsync(...)`, `_repository.CountNoLeidosAsync(...)`

#### `SolicitarResetPasswordAsync`

- **Firma:** `async Task SolicitarResetPasswordAsync(string username, string? nota, string sistemaOrigen)`
- **Retorno:** `Task`
- **Parámetros:**

- `username` (`string`)
- `nota` (`string?`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. valida reglas de negocio y puede lanzar `BadRequestException`; persiste cambios con `SaveChangesAsync`; filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_repository.GetUsuarioByUsernameAsync(...)`, `_repository.GetUsuariosSuperAdminAsync(...)`, `_repository.GetUsuariosAdminByFederacionAsync(...)`, `_repository.AddHiloAsync(...)`, `_repository.AddMensajeAsync(...)`, `_repository.SaveChangesAsync(...)`

#### `ValidarContenido`

- **Firma:** ` ValidarContenido(asunto, cuerpo)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`asunto`)
- `?` (`cuerpo`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `RequireUsuarioAsync`

- **Firma:** `async Task<Usuario> RequireUsuarioAsync(string username)`
- **Retorno:** `Task<Usuario>`
- **Parámetros:**

- `username` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. puede lanzar `UnauthorizedException` por autenticación/autorización; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_repository.GetUsuarioByUsernameAsync(...)`

#### `RequireHiloDelSistemaAsync`

- **Firma:** `async Task<Hilo> RequireHiloDelSistemaAsync(int hiloId, string sistemaOrigen)`
- **Retorno:** `Task<Hilo>`
- **Parámetros:**

- `hiloId` (`int`)
- `sistemaOrigen` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_repository.GetHiloConMensajesAsync(...)`

#### `RequireAccesoHiloAsync`

- **Firma:** `async Task RequireAccesoHiloAsync(Hilo hilo, Usuario usuario)`
- **Retorno:** `Task`
- **Parámetros:**

- `hilo` (`Hilo`)
- `usuario` (`Usuario`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. puede lanzar `UnauthorizedException` por autenticación/autorización; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_repository.UsuarioParticipaEnHiloAsync(...)`

#### `EsSuperAdmin`

- **Firma:** `bool EsSuperAdmin(Usuario usuario)`
- **Retorno:** `bool`
- **Parámetros:**

- `usuario` (`Usuario`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `EsAdmin`

- **Firma:** `bool EsAdmin(Usuario usuario)`
- **Retorno:** `bool`
- **Parámetros:**

- `usuario` (`Usuario`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `EsClub`

- **Firma:** `bool EsClub(Usuario usuario)`
- **Retorno:** `bool`
- **Parámetros:**

- `usuario` (`Usuario`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ResolverFederacionId`

- **Firma:** `int? ResolverFederacionId(Usuario usuario)`
- **Retorno:** `int?`
- **Parámetros:**

- `usuario` (`Usuario`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `MismaFederacion`

- **Firma:** `bool MismaFederacion(Usuario a, Usuario b)`
- **Retorno:** `bool`
- **Parámetros:**

- `a` (`Usuario`)
- `b` (`Usuario`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `PuedeEscribir`

- **Firma:** `bool PuedeEscribir(Usuario emisor, Usuario destinatario)`
- **Retorno:** `bool`
- **Parámetros:**

- `emisor` (`Usuario`)
- `destinatario` (`Usuario`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ValidarNuevoMensaje`

- **Firma:** `void ValidarNuevoMensaje(Usuario emisor, Usuario destinatario)`
- **Retorno:** `void`
- **Parámetros:**

- `emisor` (`Usuario`)
- `destinatario` (`Usuario`)

- **Qué hace:** Valida reglas de negocio. valida reglas de negocio y puede lanzar `BadRequestException`; puede lanzar `UnauthorizedException` por autenticación/autorización.

#### `ValidarRespuesta`

- **Firma:** `void ValidarRespuesta(Usuario emisor, Usuario destinatario)`
- **Retorno:** `void`
- **Parámetros:**

- `emisor` (`Usuario`)
- `destinatario` (`Usuario`)

- **Qué hace:** Valida reglas de negocio. valida reglas de negocio y puede lanzar `BadRequestException`; puede lanzar `UnauthorizedException` por autenticación/autorización.

#### `ValidarContenido`

- **Firma:** `void ValidarContenido(string asunto, string cuerpo)`
- **Retorno:** `void`
- **Parámetros:**

- `asunto` (`string`)
- `cuerpo` (`string`)

- **Qué hace:** Valida reglas de negocio. valida reglas de negocio y puede lanzar `BadRequestException`.

#### `ValidarCuerpo`

- **Firma:** `void ValidarCuerpo(string cuerpo)`
- **Retorno:** `void`
- **Parámetros:**

- `cuerpo` (`string`)

- **Qué hace:** Valida reglas de negocio. valida reglas de negocio y puede lanzar `BadRequestException`.

#### `ObtenerContraparteId`

- **Firma:** `int ObtenerContraparteId(Hilo hilo, int usuarioId)`
- **Retorno:** `int`
- **Parámetros:**

- `hilo` (`Hilo`)
- `usuarioId` (`int`)

- **Qué hace:** Obtiene/consulta datos. valida reglas de negocio y puede lanzar `BadRequestException`; filtra con LINQ (`Where`).

#### `NombreDisplay`

- **Firma:** `string NombreDisplay(Usuario usuario)`
- **Retorno:** `string`
- **Parámetros:**

- `usuario` (`Usuario`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `MapHiloListItem`

- **Firma:** `HiloListItemDto? MapHiloListItem(Hilo hilo, int usuarioId, bool esSuperAdmin)`
- **Retorno:** `HiloListItemDto?`
- **Parámetros:**

- `hilo` (`Hilo`)
- `usuarioId` (`int`)
- `esSuperAdmin` (`bool`)

- **Qué hace:** Configura o aplica mapeos. filtra con LINQ (`Where`); ordena resultados.

#### `MapHiloDetalle`

- **Firma:** `HiloDetalleDto MapHiloDetalle(Hilo hilo, int usuarioId, bool esSuperAdmin)`
- **Retorno:** `HiloDetalleDto`
- **Parámetros:**

- `hilo` (`Hilo`)
- `usuarioId` (`int`)
- `esSuperAdmin` (`bool`)

- **Qué hace:** Configura o aplica mapeos. filtra con LINQ (`Where`); ordena resultados.

#### `MapCampanaListItem`

- **Firma:** `CampanaListItemDto MapCampanaListItem(CampanaEnvio campana, int remitenteId)`
- **Retorno:** `CampanaListItemDto`
- **Parámetros:**

- `campana` (`CampanaEnvio`)
- `remitenteId` (`int`)

- **Qué hace:** Configura o aplica mapeos. ordena resultados.

#### `MapCampanaDetalle`

- **Firma:** `CampanaDetalleDto MapCampanaDetalle(CampanaEnvio campana, int remitenteId)`
- **Retorno:** `CampanaDetalleDto`
- **Parámetros:**

- `campana` (`CampanaEnvio`)
- `remitenteId` (`int`)

- **Qué hace:** Configura o aplica mapeos. ordena resultados.

#### `EsMensajeVisible`

- **Firma:** `bool EsMensajeVisible(Mensaje mensaje, int usuarioId, bool esSuperAdmin)`
- **Retorno:** `bool`
- **Parámetros:**

- `mensaje` (`Mensaje`)
- `usuarioId` (`int`)
- `esSuperAdmin` (`bool`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `MapUsuarioResumen`

- **Firma:** `UsuarioResumenDto MapUsuarioResumen(Usuario usuario)`
- **Retorno:** `UsuarioResumenDto`
- **Parámetros:**

- `usuario` (`Usuario`)

- **Qué hace:** Configura o aplica mapeos.

#### `Truncar`

- **Firma:** `string Truncar(string texto, int max)`
- **Retorno:** `string`
- **Parámetros:**

- `texto` (`string`)
- `max` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Mensajes/MensajeService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
