# ISaaSService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/SaaS/ISaaSService.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.SaaS`
- **Usings:**
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.SaaS.Dtos;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetPlanesAsync`

- **Firma:** `Task<IEnumerable<PlanSaaSDto>> GetPlanesAsync()`
- **Retorno:** `Task<IEnumerable<PlanSaaSDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetPlanByIdAsync`

- **Firma:** `Task<PlanSaaSDto> GetPlanByIdAsync(int id)`
- **Retorno:** `Task<PlanSaaSDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AsignarPlanAClubAsync`

- **Firma:** `Task AsignarPlanAClubAsync(int clubId, int planId)`
- **Retorno:** `Task`
- **Parámetros:**

- `clubId` (`int`)
- `planId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetClubesStatusAsync`

- **Firma:** `Task<IEnumerable<ClubSaaSStatusDto>> GetClubesStatusAsync()`
- **Retorno:** `Task<IEnumerable<ClubSaaSStatusDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ToggleClubActivoAsync`

- **Firma:** `Task ToggleClubActivoAsync(int clubId)`
- **Retorno:** `Task`
- **Parámetros:**

- `clubId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateFederacionWithAdminAsync`

- **Firma:** `Task<int> CreateFederacionWithAdminAsync(SaaSCreateFederacionDto dto)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `dto` (`SaaSCreateFederacionDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetGlobalMetricsAsync`

- **Firma:** `Task<GlobalMetricsDto> GetGlobalMetricsAsync()`
- **Retorno:** `Task<GlobalMetricsDto>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `SaaS/ISaaSService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
