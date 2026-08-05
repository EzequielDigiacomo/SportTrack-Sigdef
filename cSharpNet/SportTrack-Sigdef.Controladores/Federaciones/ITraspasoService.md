# ITraspasoService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/ITraspasoService.cs`

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
  - `using SportTrack_Sigdef.Entidades.DTOs.Traspaso;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetPeriodosAsync`

- **Firma:** `Task<IEnumerable<PeriodoTraspasoDto>> GetPeriodosAsync()`
- **Retorno:** `Task<IEnumerable<PeriodoTraspasoDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetPeriodoActivoAsync`

- **Firma:** `Task<PeriodoTraspasoDto?> GetPeriodoActivoAsync()`
- **Retorno:** `Task<PeriodoTraspasoDto?>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreatePeriodoAsync`

- **Firma:** `Task<PeriodoTraspasoDto> CreatePeriodoAsync(PeriodoTraspasoCreateDto dto)`
- **Retorno:** `Task<PeriodoTraspasoDto>`
- **Parámetros:**

- `dto` (`PeriodoTraspasoCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdatePeriodoAsync`

- **Firma:** `Task<PeriodoTraspasoDto> UpdatePeriodoAsync(int id, PeriodoTraspasoUpdateDto dto)`
- **Retorno:** `Task<PeriodoTraspasoDto>`
- **Parámetros:**

- `id` (`int`)
- `dto` (`PeriodoTraspasoUpdateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetSolicitudesAsync`

- **Firma:** `Task<IEnumerable<SolicitudTraspasoDto>> GetSolicitudesAsync(string? estado = null)`
- **Retorno:** `Task<IEnumerable<SolicitudTraspasoDto>>`
- **Parámetros:**

- `estado` (`string?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetSolicitudByIdAsync`

- **Firma:** `Task<SolicitudTraspasoDto> GetSolicitudByIdAsync(int id)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetValidacionesAsync`

- **Firma:** `Task<TraspasoValidacionDto> GetValidacionesAsync(int id)`
- **Retorno:** `Task<TraspasoValidacionDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CrearSolicitudAsync`

- **Firma:** `Task<SolicitudTraspasoDto> CrearSolicitudAsync(SolicitudTraspasoCreateDto dto)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `dto` (`SolicitudTraspasoCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AceptarOrigenAsync`

- **Firma:** `Task<SolicitudTraspasoDto> AceptarOrigenAsync(int id)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `RechazarOrigenAsync`

- **Firma:** `Task<SolicitudTraspasoDto> RechazarOrigenAsync(int id, TraspasoMotivoDto dto)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)
- `dto` (`TraspasoMotivoDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AprobarFederacionAsync`

- **Firma:** `Task<SolicitudTraspasoDto> AprobarFederacionAsync(int id, bool forzar = false)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)
- `forzar` (`bool`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `RechazarFederacionAsync`

- **Firma:** `Task<SolicitudTraspasoDto> RechazarFederacionAsync(int id, TraspasoMotivoDto dto)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)
- `dto` (`TraspasoMotivoDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CancelarAsync`

- **Firma:** `Task<SolicitudTraspasoDto> CancelarAsync(int id)`
- **Retorno:** `Task<SolicitudTraspasoDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `BuscarAtletasAsync`

- **Firma:** `Task<IEnumerable<AtletaTraspasoBusquedaDto>> BuscarAtletasAsync(string term)`
- **Retorno:** `Task<IEnumerable<AtletaTraspasoBusquedaDto>>`
- **Parámetros:**

- `term` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetAuditoriaAsync`

- **Firma:** `Task<IEnumerable<TraspasoAuditoriaDto>> GetAuditoriaAsync(int limit = 50)`
- **Retorno:** `Task<IEnumerable<TraspasoAuditoriaDto>>`
- **Parámetros:**

- `limit` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ExportSolicitudesCsvAsync`

- **Firma:** `Task<byte[]> ExportSolicitudesCsvAsync(int? periodoId = null, string? estado = null)`
- **Retorno:** `Task<byte[]>`
- **Parámetros:**

- `periodoId` (`int?`)
- `estado` (`string?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/ITraspasoService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
