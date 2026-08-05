# EventoServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/EventoServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IEventoServices`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SIGDEF.API.Services`
- **Usings:**
  - `using Microsoft.AspNetCore.Mvc;`
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`
  - `using SIGDEF.DTOs;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.DTOs;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Evento;`
  - `using SportTrack_Sigdef.Entidades.DTOs.EventoPrueba;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_tenantProvider` — tipo `ITenantProvider` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `EventoServices(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `tenantProvider` (`ITenantProvider`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `OkObjectResult`

- **Firma:** `return new OkObjectResult(result)`
- **Retorno:** `return new`
- **Parámetros:**

- `?` (`result`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `catch`

- **Firma:** ` catch(Exception)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`Exception`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetEvento`

- **Firma:** `async Task<ActionResult<EventoResponseDto>> GetEvento(int id)`
- **Retorno:** `Task<ActionResult<EventoResponseDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `GetEventoDetalle`

- **Firma:** `async Task<ActionResult<EventoDetailDto>> GetEventoDetalle(int id)`
- **Retorno:** `Task<ActionResult<EventoDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).

#### `PostEvento`

- **Firma:** `async Task<ActionResult<EventoResponseDto>> PostEvento(EventoCreateDTO eventoDto)`
- **Retorno:** `Task<ActionResult<EventoResponseDto>>`
- **Parámetros:**

- `eventoDto` (`EventoCreateDTO`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `GetOrCreatePruebaId(...)`, `_context.SaveChangesAsync(...)`, `_context.Entry(...)`

#### `PutEvento`

- **Firma:** `async Task<IActionResult> PutEvento(int id, EventoUpdateDto eventoDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `eventoDto` (`EventoUpdateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `GetOrCreatePruebaId(...)`, `_context.SaveChangesAsync(...)`, `EventoExistsAsync(...)`

#### `ActivarEvento`

- **Firma:** `async Task<IActionResult> ActivarEvento(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Eventos.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `DesactivarEvento`

- **Firma:** `async Task<IActionResult> DesactivarEvento(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Eventos.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `DeleteEvento`

- **Firma:** `async Task<IActionResult> DeleteEvento(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.SaveChangesAsync(...)`

#### `GetProximosEventos`

- **Firma:** `async Task<ActionResult<IEnumerable<EventoDto>>> GetProximosEventos()`
- **Retorno:** `Task<ActionResult<IEnumerable<EventoDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); ordena resultados; agrupa datos; operación asíncrona (`await`).

#### `GetEventosConInscripcionesAbiertas`

- **Firma:** `async Task<ActionResult<IEnumerable<EventoResponseDto>>> GetEventosConInscripcionesAbiertas()`
- **Retorno:** `Task<ActionResult<IEnumerable<EventoResponseDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `GetEventosPorDistancia`

- **Firma:** `async Task<ActionResult<IEnumerable<EventoResponseDto>>> GetEventosPorDistancia(int distancia)`
- **Retorno:** `Task<ActionResult<IEnumerable<EventoResponseDto>>>`
- **Parámetros:**

- `distancia` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `GetFormConfig`

- **Firma:** `async Task<ActionResult<EventoFormConfigDto>> GetFormConfig()`
- **Retorno:** `Task<ActionResult<EventoFormConfigDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. enumeración de valores de un `enum`.

#### `EventoExistsAsync`

- **Firma:** `async Task<bool> EventoExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Eventos.AnyAsync(...)`

#### `GetOrCreatePruebaId`

- **Firma:** `async Task<int> GetOrCreatePruebaId(DistanciaRegata distancia, CategoriaEdad categoria, SexoCompetencia sexo, TipoBote bote)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `distancia` (`DistanciaRegata`)
- `categoria` (`CategoriaEdad`)
- `sexo` (`SexoCompetencia`)
- `bote` (`TipoBote`)

- **Qué hace:** Obtiene/consulta datos. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Pruebas.FirstOrDefaultAsync(...)`, `_context.SaveChangesAsync(...)`

#### `GetDistanciaCodigo`

- **Firma:** `string GetDistanciaCodigo(DistanciaRegata distancia)`
- **Retorno:** `string`
- **Parámetros:**

- `distancia` (`DistanciaRegata`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetDistanciaEnMetros`

- **Firma:** `decimal GetDistanciaEnMetros(DistanciaRegata distancia)`
- **Retorno:** `decimal`
- **Parámetros:**

- `distancia` (`DistanciaRegata`)

- **Qué hace:** Obtiene/consulta datos.

#### `GetTipoDistancia`

- **Firma:** `string GetTipoDistancia(DistanciaRegata distancia)`
- **Retorno:** `string`
- **Parámetros:**

- `distancia` (`DistanciaRegata`)

- **Qué hace:** Obtiene/consulta datos.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/EventoServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
