# IAuthService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Auth/IAuthService.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Auth`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Auth.Dtos;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `LoginAsync`

- **Firma:** `Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string? clientApp = null)`
- **Retorno:** `Task<AuthResponseDto>`
- **Parámetros:**

- `loginDto` (`LoginDto`)
- `clientApp` (`string?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `RegisterAsync`

- **Firma:** `Task<bool> RegisterAsync(RegisterDto registerDto)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `registerDto` (`RegisterDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UserExistsAsync`

- **Firma:** `Task<bool> UserExistsAsync(string username)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `username` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetUsuariosAsync`

- **Firma:** `Task<System.Collections.Generic.IEnumerable<UsuarioDto>> GetUsuariosAsync(string? requesterUsername = null)`
- **Retorno:** `Task<System.Collections.Generic.IEnumerable<UsuarioDto>>`
- **Parámetros:**

- `requesterUsername` (`string?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdatePasswordAsync`

- **Firma:** `Task<bool> UpdatePasswordAsync(int id, string newPassword)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)
- `newPassword` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetMeAsync`

- **Firma:** `Task<UsuarioDto> GetMeAsync(string username, string? clientApp = null)`
- **Retorno:** `Task<UsuarioDto>`
- **Parámetros:**

- `username` (`string`)
- `clientApp` (`string?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `ToggleActivoAsync`

- **Firma:** `Task<bool> ToggleActivoAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdatePerfilAsync`

- **Firma:** `Task<bool> UpdatePerfilAsync(int id, UpdatePerfilDto dto)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)
- `dto` (`UpdatePerfilDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Auth combina verificación de password (BCrypt), emisión de JWT y auditoría de intentos.
- Ruta relativa en el proyecto: `Auth/IAuthService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
