# IAltaAtletaService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/IAltaAtletaService.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Federaciones`
- **Usings:**
  - `using SIGDEF.DTOs;`
  - `using SportTrack_Sigdef.Controladores.Participante.Dtos;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `NormalizarDocumento`

- **Firma:** `string NormalizarDocumento(string? documento)`
- **Retorno:** `string`
- **Parámetros:**

- `documento` (`string?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `BuscarPorDocumentoAsync`

- **Firma:** `Task<Entidades.Entidades.Participante?> BuscarPorDocumentoAsync(string documento)`
- **Retorno:** `Task<Entidades.Entidades.Participante?>`
- **Parámetros:**

- `documento` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpsertParticipanteAsync`

- **Firma:** `Task<AltaAtletaResult> UpsertParticipanteAsync(AltaAtletaParticipanteInput input)`
- **Retorno:** `Task<AltaAtletaResult>`
- **Parámetros:**

- `input` (`AltaAtletaParticipanteInput`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `EnsureAtletaFederacionAsync`

- **Firma:** `Task<AtletaFederacion> EnsureAtletaFederacionAsync(int participanteId, AltaAtletaFederacionInput input)`
- **Retorno:** `Task<AtletaFederacion>`
- **Parámetros:**

- `participanteId` (`int`)
- `input` (`AltaAtletaFederacionInput`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `FromPersonaCreateDto`

- **Firma:** `AltaAtletaParticipanteInput FromPersonaCreateDto(PersonaCreateDto dto, int? idClub = null)`
- **Retorno:** `AltaAtletaParticipanteInput`
- **Parámetros:**

- `dto` (`PersonaCreateDto`)
- `idClub` (`int?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `FromParticipanteCreateDto`

- **Firma:** `AltaAtletaParticipanteInput FromParticipanteCreateDto(ParticipanteCreateDto dto)`
- **Retorno:** `AltaAtletaParticipanteInput`
- **Parámetros:**

- `dto` (`ParticipanteCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `FromAtletaCreateDto`

- **Firma:** `AltaAtletaFederacionInput FromAtletaCreateDto(AtletaCreateDto dto)`
- **Retorno:** `AltaAtletaFederacionInput`
- **Parámetros:**

- `dto` (`AtletaCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DefaultsFromClub`

- **Firma:** `AltaAtletaFederacionInput DefaultsFromClub(int? idClub, int? idFederacion = null)`
- **Retorno:** `AltaAtletaFederacionInput`
- **Parámetros:**

- `idClub` (`int?`)
- `idFederacion` (`int?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/IAltaAtletaService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
