# PagoTransaccionController

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/PagoTransaccionController.cs`

## 1. Qué es este archivo

Es un **Controlador (nota: vive en Controladores; normalmente los HTTP controllers están en la API)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `ControllerBase`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Controllers`
- **Usings:**
  - `using Microsoft.AspNetCore.Authorization;`
  - `using Microsoft.AspNetCore.Mvc;`
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.Controladores.PagosSIGDEF.Models.Dtos;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SIGDEF.DTOs;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Club;`
  - `using SportTrack_Sigdef.Entidades.DTOs.PagoFederacionTransaccion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Atributos del tipo

- `[Authorize]`

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_paymentService` — tipo `SportTrack_Sigdef.Controladores.PagosSIGDEF.Services.PaymentService` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `PagoTransaccionController(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `paymentService` (`SportTrack_Sigdef.Controladores.PagosSIGDEF.Services.PaymentService`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `CrearPreferenciaPago`

- **Firma:** `async Task<ActionResult<PaymentResponse>> CrearPreferenciaPago(PagoTransaccionCreateDto pagoDto)`
- **Retorno:** `Task<ActionResult<PaymentResponse>>`
- **Atributos:** `[HttpPost("preferencia")]`
- **Parámetros:**

- `pagoDto` (`PagoTransaccionCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.FindAsync(...)`, `_context.Clubes.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `_paymentService.ProcessPaymentAsync(...)`

#### `GetPagosTransaccion`

- **Firma:** `async Task<ActionResult<IEnumerable<PagoTransaccionDto>>> GetPagosTransaccion()`
- **Retorno:** `Task<ActionResult<IEnumerable<PagoTransaccionDto>>>`
- **Atributos:** `[HttpGet]`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `GetPagoTransaccion`

- **Firma:** `async Task<ActionResult<PagoTransaccionDetailDto>> GetPagoTransaccion(int id)`
- **Retorno:** `Task<ActionResult<PagoTransaccionDetailDto>>`
- **Atributos:** `[HttpGet("{id}")]`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetPagosPorPersona`

- **Firma:** `async Task<ActionResult<IEnumerable<PagoTransaccionDto>>> GetPagosPorPersona(int ParticipanteId)`
- **Retorno:** `Task<ActionResult<IEnumerable<PagoTransaccionDto>>>`
- **Atributos:** `[HttpGet("Participante/{ParticipanteId}")]`
- **Parámetros:**

- `ParticipanteId` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetPagosPorClub`

- **Firma:** `async Task<ActionResult<IEnumerable<PagoTransaccionDto>>> GetPagosPorClub(int idClub)`
- **Retorno:** `Task<ActionResult<IEnumerable<PagoTransaccionDto>>>`
- **Atributos:** `[HttpGet("club/{idClub}")]`
- **Parámetros:**

- `idClub` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetPagosPorEstado`

- **Firma:** `async Task<ActionResult<IEnumerable<PagoTransaccionDto>>> GetPagosPorEstado(EstadoPagoTransaccion estado)`
- **Retorno:** `Task<ActionResult<IEnumerable<PagoTransaccionDto>>>`
- **Atributos:** `[HttpGet("estado/{estado}")]`
- **Parámetros:**

- `estado` (`EstadoPagoTransaccion`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `PostPagoTransaccion`

- **Firma:** `async Task<ActionResult<PagoTransaccionDto>> PostPagoTransaccion(PagoTransaccionCreateDto pagoTransaccionCreateDto)`
- **Retorno:** `Task<ActionResult<PagoTransaccionDto>>`
- **Atributos:** `[HttpPost]`
- **Parámetros:**

- `pagoTransaccionCreateDto` (`PagoTransaccionCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.AnyAsync(...)`, `_context.Clubes.AnyAsync(...)`, `_context.SaveChangesAsync(...)`, `_context.Entry(...)`

#### `PutPagoTransaccion`

- **Firma:** `async Task<IActionResult> PutPagoTransaccion(int id, PagoTransaccionCreateDto pagoTransaccionCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Atributos:** `[HttpPut("{id}")]`
- **Parámetros:**

- `id` (`int`)
- `pagoTransaccionCreateDto` (`PagoTransaccionCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.PagosTransacciones.FindAsync(...)`, `_context.Participantes.AnyAsync(...)`, `_context.Clubes.AnyAsync(...)`, `_context.SaveChangesAsync(...)`

#### `UpdateEstadoPago`

- **Firma:** `async Task<IActionResult> UpdateEstadoPago(int id, [FromBody] EstadoPagoTransaccion nuevoEstado)`
- **Retorno:** `Task<IActionResult>`
- **Atributos:** `[HttpPatch("{id}/estado")]`
- **Parámetros:**

- `id` (`int`)
- `nuevoEstado` (`[FromBody] EstadoPagoTransaccion`)

- **Qué hace:** Actualiza un recurso existente. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.PagosTransacciones.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `DeletePagoTransaccion`

- **Firma:** `async Task<IActionResult> DeletePagoTransaccion(int id)`
- **Retorno:** `Task<IActionResult>`
- **Atributos:** `[HttpDelete("{id}")]`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.PagosTransacciones.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `GetEstadisticasPagos`

- **Firma:** `async Task<ActionResult<object>> GetEstadisticasPagos()`
- **Retorno:** `Task<ActionResult<object>>`
- **Atributos:** `[HttpGet("estadisticas")]`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. filtra con LINQ (`Where`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.PagosTransacciones.CountAsync(...)`, `_context.PagosTransacciones.SumAsync(...)`

#### `PagoTransaccionExists`

- **Firma:** `bool PagoTransaccionExists(int id)`
- **Retorno:** `bool`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetEstadoDescripcion`

- **Firma:** `string GetEstadoDescripcion(EstadoPagoTransaccion estado)`
- **Retorno:** `string`
- **Parámetros:**

- `estado` (`EstadoPagoTransaccion`)

- **Qué hace:** Obtiene/consulta datos.

## 5. Notas de estudio

- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/PagoTransaccionController.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
