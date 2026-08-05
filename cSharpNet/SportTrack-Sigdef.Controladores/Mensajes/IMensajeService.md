# IMensajeService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Mensajes/IMensajeService.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Mensajes`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Mensajes.Dtos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetHilosAsync`

- **Firma:** `Task<List<HiloListItemDto>> GetHilosAsync(string username, string sistemaOrigen, int? campanaId = null)`
- **Retorno:** `Task<List<HiloListItemDto>>`
- **Parámetros:**

- `username` (`string`)
- `sistemaOrigen` (`string`)
- `campanaId` (`int?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetHiloDetalleAsync`

- **Firma:** `Task<HiloDetalleDto> GetHiloDetalleAsync(int hiloId, string username, string sistemaOrigen)`
- **Retorno:** `Task<HiloDetalleDto>`
- **Parámetros:**

- `hiloId` (`int`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CrearHiloAsync`

- **Firma:** `Task<HiloDetalleDto> CrearHiloAsync(CrearHiloDto dto, string username, string sistemaOrigen)`
- **Retorno:** `Task<HiloDetalleDto>`
- **Parámetros:**

- `dto` (`CrearHiloDto`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `EnviarMasivoAsync`

- **Firma:** `Task<EnviarMasivoResultDto> EnviarMasivoAsync(EnviarMasivoDto dto, string username, string sistemaOrigen)`
- **Retorno:** `Task<EnviarMasivoResultDto>`
- **Parámetros:**

- `dto` (`EnviarMasivoDto`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetCampanasAsync`

- **Firma:** `Task<List<CampanaListItemDto>> GetCampanasAsync(string username, string sistemaOrigen)`
- **Retorno:** `Task<List<CampanaListItemDto>>`
- **Parámetros:**

- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetCampanaDetalleAsync`

- **Firma:** `Task<CampanaDetalleDto> GetCampanaDetalleAsync(int campanaId, string username, string sistemaOrigen)`
- **Retorno:** `Task<CampanaDetalleDto>`
- **Parámetros:**

- `campanaId` (`int`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ResponderHiloAsync`

- **Firma:** `Task<HiloDetalleDto> ResponderHiloAsync(int hiloId, ResponderHiloDto dto, string username, string sistemaOrigen)`
- **Retorno:** `Task<HiloDetalleDto>`
- **Parámetros:**

- `hiloId` (`int`)
- `dto` (`ResponderHiloDto`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `MarcarHiloLeidoAsync`

- **Firma:** `Task MarcarHiloLeidoAsync(int hiloId, string username, string sistemaOrigen)`
- **Retorno:** `Task`
- **Parámetros:**

- `hiloId` (`int`)
- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetNoLeidosCountAsync`

- **Firma:** `Task<int> GetNoLeidosCountAsync(string username, string sistemaOrigen)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `username` (`string`)
- `sistemaOrigen` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `SolicitarResetPasswordAsync`

- **Firma:** `Task SolicitarResetPasswordAsync(string username, string? nota, string sistemaOrigen)`
- **Retorno:** `Task`
- **Parámetros:**

- `username` (`string`)
- `nota` (`string?`)
- `sistemaOrigen` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Mensajes/IMensajeService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
