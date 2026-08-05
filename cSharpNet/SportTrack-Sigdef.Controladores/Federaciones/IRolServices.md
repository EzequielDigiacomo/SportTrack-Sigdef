# IRolServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/IRolServices.cs`

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
  - `using Microsoft.AspNetCore.Mvc;`
  - `using SportTrack_Sigdef.Entidades.DTOs.RolFederacion;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetRol`

- **Firma:** `Task<ActionResult<RolDetailDto>> GetRol(int id)`
- **Retorno:** `Task<ActionResult<RolDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetRoles`

- **Firma:** `Task<ActionResult<IEnumerable<RolDto>>> GetRoles()`
- **Retorno:** `Task<ActionResult<IEnumerable<RolDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetRolesPredefinidos`

- **Firma:** `Task<ActionResult<IEnumerable<RolDto>>> GetRolesPredefinidos()`
- **Retorno:** `Task<ActionResult<IEnumerable<RolDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `SearchRoles`

- **Firma:** `Task<ActionResult<IEnumerable<RolDto>>> SearchRoles(string term)`
- **Retorno:** `Task<ActionResult<IEnumerable<RolDto>>>`
- **Parámetros:**

- `term` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetRolPorTipo`

- **Firma:** `Task<ActionResult<RolDto>> GetRolPorTipo(string tipo)`
- **Retorno:** `Task<ActionResult<RolDto>>`
- **Parámetros:**

- `tipo` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetRolPorEnumId`

- **Firma:** `Task<ActionResult<RolDto>> GetRolPorEnumId(int enumId)`
- **Retorno:** `Task<ActionResult<RolDto>>`
- **Parámetros:**

- `enumId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetEnumValues`

- **Firma:** `Task<ActionResult> GetEnumValues()`
- **Retorno:** `Task<ActionResult>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PostRol`

- **Firma:** `Task<ActionResult<RolDto>> PostRol(RolCreateDto rolCreateDto)`
- **Retorno:** `Task<ActionResult<RolDto>>`
- **Parámetros:**

- `rolCreateDto` (`RolCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PutRol`

- **Firma:** `Task<IActionResult> PutRol(int id, RolCreateDto rolCreateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `rolCreateDto` (`RolCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteRol`

- **Firma:** `Task<IActionResult> DeleteRol(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/IRolServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
