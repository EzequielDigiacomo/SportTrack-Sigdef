# IUsuarioServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/IUsuarioServices.cs`

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
  - `using SportTrack_Sigdef.Entidades.DTOs.Usuario;`
  - `using System.Collections.Generic;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetUsuario`

- **Firma:** `Task<ActionResult<UsuarioDetailDto>> GetUsuario(int id)`
- **Retorno:** `Task<ActionResult<UsuarioDetailDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetUsuarios`

- **Firma:** `Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()`
- **Retorno:** `Task<ActionResult<IEnumerable<UsuarioDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetUsuarioPorUsername`

- **Firma:** `Task<ActionResult<UsuarioDto>> GetUsuarioPorUsername(string username)`
- **Retorno:** `Task<ActionResult<UsuarioDto>>`
- **Parámetros:**

- `username` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PostUsuario`

- **Firma:** `Task<ActionResult<UsuarioDto>> PostUsuario(UsuarioCreateDto usuarioCreateDto)`
- **Retorno:** `Task<ActionResult<UsuarioDto>>`
- **Parámetros:**

- `usuarioCreateDto` (`UsuarioCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `Login`

- **Firma:** `Task<ActionResult<UsuarioDto>> Login(UsuarioLoginDto loginDto)`
- **Retorno:** `Task<ActionResult<UsuarioDto>>`
- **Parámetros:**

- `loginDto` (`UsuarioLoginDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `PutUsuario`

- **Firma:** `Task<IActionResult> PutUsuario(int id, UsuarioUpdateDto usuarioUpdateDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `usuarioUpdateDto` (`UsuarioUpdateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ChangePassword`

- **Firma:** `Task<IActionResult> ChangePassword(int id, UsuarioChangePasswordDto changePasswordDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `changePasswordDto` (`UsuarioChangePasswordDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteUsuario`

- **Firma:** `Task<IActionResult> DeleteUsuario(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ResetPassword`

- **Firma:** `Task<ActionResult<string>> ResetPassword(int id)`
- **Retorno:** `Task<ActionResult<string>>`
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
- Ruta relativa en el proyecto: `Federaciones/IUsuarioServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
