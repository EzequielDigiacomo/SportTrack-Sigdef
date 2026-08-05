# AltaAtletaService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/AltaAtletaService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IAltaAtletaService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Federaciones`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using SIGDEF.DTOs;`
  - `using SportTrack_Sigdef.Controladores.Participante.Dtos;`
  - `using System;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_tenantProvider` — tipo `ITenantProvider` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `AltaAtletaService(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `tenantProvider` (`ITenantProvider`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `NormalizarDocumento`

- **Firma:** `string NormalizarDocumento(string? documento)`
- **Retorno:** `string`
- **Parámetros:**

- `documento` (`string?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `BuscarPorDocumentoAsync`

- **Firma:** `async Task<Entidades.Entidades.Participante?> BuscarPorDocumentoAsync(string documento)`
- **Retorno:** `Task<Entidades.Entidades.Participante?>`
- **Parámetros:**

- `documento` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).

#### `FromPersonaCreateDto`

- **Firma:** `AltaAtletaParticipanteInput FromPersonaCreateDto(PersonaCreateDto dto, int? idClub = null)`
- **Retorno:** `AltaAtletaParticipanteInput`
- **Parámetros:**

- `dto` (`PersonaCreateDto`)
- `idClub` (`int?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `FromParticipanteCreateDto`

- **Firma:** `AltaAtletaParticipanteInput FromParticipanteCreateDto(ParticipanteCreateDto dto)`
- **Retorno:** `AltaAtletaParticipanteInput`
- **Parámetros:**

- `dto` (`ParticipanteCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `FromAtletaCreateDto`

- **Firma:** `AltaAtletaFederacionInput FromAtletaCreateDto(AtletaCreateDto dto)`
- **Retorno:** `AltaAtletaFederacionInput`
- **Parámetros:**

- `dto` (`AtletaCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `DefaultsFromClub`

- **Firma:** `AltaAtletaFederacionInput DefaultsFromClub(int? idClub, int? idFederacion = null)`
- **Retorno:** `AltaAtletaFederacionInput`
- **Parámetros:**

- `idClub` (`int?`)
- `idFederacion` (`int?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `UpsertParticipanteAsync`

- **Firma:** `async Task<AltaAtletaResult> UpsertParticipanteAsync(AltaAtletaParticipanteInput input)`
- **Retorno:** `Task<AltaAtletaResult>`
- **Parámetros:**

- `input` (`AltaAtletaParticipanteInput`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `BuscarPorDocumentoAsync(...)`, `ResolverCategoriaPorEdadAsync(...)`, `_context.SaveChangesAsync(...)`

#### `EnsureAtletaFederacionAsync`

- **Firma:** `async Task<AtletaFederacion> EnsureAtletaFederacionAsync(int participanteId, AltaAtletaFederacionInput input)`
- **Retorno:** `Task<AtletaFederacion>`
- **Parámetros:**

- `participanteId` (`int`)
- `input` (`AltaAtletaFederacionInput`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Participantes.FindAsync(...)`, `ResolverFederacionIdAsync(...)`, `_context.AtletasFederados.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `ResolverFederacionIdAsync`

- **Firma:** `async Task<int?> ResolverFederacionIdAsync(int? explicitFedId, int? idClub)`
- **Retorno:** `Task<int?>`
- **Parámetros:**

- `explicitFedId` (`int?`)
- `idClub` (`int?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. consulta en modo solo-lectura (`AsNoTracking`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Clubes.AsNoTracking(...)`

#### `ResolverCategoriaPorEdadAsync`

- **Firma:** `async Task<int?> ResolverCategoriaPorEdadAsync(DateTime fechaNacimiento)`
- **Retorno:** `Task<int?>`
- **Parámetros:**

- `fechaNacimiento` (`DateTime`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `ResolverSexoIdDesdePersona`

- **Firma:** `int ResolverSexoIdDesdePersona(PersonaCreateDto dto)`
- **Retorno:** `int`
- **Parámetros:**

- `dto` (`PersonaCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/AltaAtletaService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
