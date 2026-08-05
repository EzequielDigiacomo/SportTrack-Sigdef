# IPagoService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Pago/IPagoInterfaces.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Pago`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Pago.Dtos;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetHistorialPagosAsync`

- **Firma:** `Task<IEnumerable<PagoDto>> GetHistorialPagosAsync(int? fedId, string? role)`
- **Retorno:** `Task<IEnumerable<PagoDto>>`
- **Parámetros:**

- `fedId` (`int?`)
- `role` (`string?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `RegistrarPagoAsync`

- **Firma:** `Task<PagoDto> RegistrarPagoAsync(RegistrarPagoDto dto, string registradoPor)`
- **Retorno:** `Task<PagoDto>`
- **Parámetros:**

- `dto` (`RegistrarPagoDto`)
- `registradoPor` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ToggleClubPagoStatusAsync`

- **Firma:** `Task<bool> ToggleClubPagoStatusAsync(int clubId, bool alDia)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `clubId` (`int`)
- `alDia` (`bool`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ToggleAtletaPagoStatusAsync`

- **Firma:** `Task<bool> ToggleAtletaPagoStatusAsync(int participanteId, bool alDia)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `participanteId` (`int`)
- `alDia` (`bool`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ToggleInscripcionPagoStatusAsync`

- **Firma:** `Task<bool> ToggleInscripcionPagoStatusAsync(int inscripcionId, bool pagado)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `inscripcionId` (`int`)
- `pagado` (`bool`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `SetSolicitudPagoPendienteAsync`

- **Firma:** `Task<bool> SetSolicitudPagoPendienteAsync(int clubId, bool pendiente)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `clubId` (`int`)
- `pendiente` (`bool`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `EliminarPagoAsync`

- **Firma:** `Task<bool> EliminarPagoAsync(int pagoId, string eliminadoPor)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `pagoId` (`int`)
- `eliminadoPor` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `EliminarPagosAsync`

- **Firma:** `Task<int> EliminarPagosAsync(IEnumerable<int> pagoIds, string eliminadoPor)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `pagoIds` (`IEnumerable<int>`)
- `eliminadoPor` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Pago/IPagoInterfaces.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
