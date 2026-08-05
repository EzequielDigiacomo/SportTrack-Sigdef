# PersonaServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/PersonaServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IPersonaServices`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Services`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Usuario;`
  - `using SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub;`
  - `using SportTrack_Sigdef.Entidades.DTOs.EntrenadorFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.TutorFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.PagoFederacionTransaccion;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`
  - `using Microsoft.AspNetCore.Mvc;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_altaAtletaService` — tipo `IAltaAtletaService` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `PersonaServices(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `altaAtletaService` (`IAltaAtletaService`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetPersonas`

- **Firma:** `async Task<ActionResult<IEnumerable<PersonaDto>>> GetPersonas()`
- **Retorno:** `Task<ActionResult<IEnumerable<PersonaDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).

#### `GetPersona`

- **Firma:** `async Task<ActionResult<PersonaDetailDto>> GetPersona(int id)`
- **Retorno:** `Task<ActionResult<PersonaDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `GetPersonaByDocumento`

- **Firma:** `async Task<ActionResult<PersonaDto>> GetPersonaByDocumento(string documento)`
- **Retorno:** `Task<ActionResult<PersonaDto>>`
- **Parámetros:**

- `documento` (`string`)

- **Qué hace:** Obtiene/consulta datos. carga relaciones con `Include`/`ThenInclude` (eager loading); filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `PostPersona`

- **Firma:** `async Task<ActionResult<PersonaDto>> PostPersona(PersonaCreateDto personaCreateDto)`
- **Retorno:** `Task<ActionResult<PersonaDto>>`
- **Parámetros:**

- `personaCreateDto` (`PersonaCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. carga relaciones con `Include`/`ThenInclude` (eager loading); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_altaAtletaService.UpsertParticipanteAsync(...)`

#### `PutPersona`

- **Firma:** `async Task<IActionResult> PutPersona(int id, PersonaCreateDto personaCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `personaCreateDto` (`PersonaCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `PersonaExistsAsync(...)`

#### `DeletePersona`

- **Firma:** `async Task<IActionResult> DeletePersona(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `PersonaExistsAsync`

- **Firma:** `async Task<bool> PersonaExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.AnyAsync(...)`

#### `CalcularEdad`

- **Firma:** `int CalcularEdad(DateTime fechaNacimiento)`
- **Retorno:** `int`
- **Parámetros:**

- `fechaNacimiento` (`DateTime`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `GetTipoPersona`

- **Firma:** `string GetTipoPersona(SportTrack_Sigdef.Entidades.Entidades.Participante Participante)`
- **Retorno:** `string`
- **Parámetros:**

- `Participante` (`SportTrack_Sigdef.Entidades.Entidades.Participante`)

- **Qué hace:** Obtiene/consulta datos.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/PersonaServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
