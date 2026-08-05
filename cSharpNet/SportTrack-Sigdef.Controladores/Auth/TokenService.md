# TokenService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Auth/TokenService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `ITokenService`.

## 2. Conceptos C# / .NET que aparecen

- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Auth`
- **Usings:**
  - `using Microsoft.Extensions.Configuration;`
  - `using Microsoft.Extensions.Hosting;`
  - `using Microsoft.IdentityModel.Tokens;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.IdentityModel.Tokens.Jwt;`
  - `using System.Security.Claims;`
  - `using System.Text;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_key` — tipo `SymmetricSecurityKey` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `TokenService(...)`

**Parámetros:**

- `config` (`IConfiguration`)
- `env` (`IHostEnvironment`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `ResolveTokenKey`

- **Firma:** `string ResolveTokenKey(IConfiguration config, IHostEnvironment env)`
- **Retorno:** `string`
- **Parámetros:**

- `config` (`IConfiguration`)
- `env` (`IHostEnvironment`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. puede lanzar excepciones de dominio.

#### `CreateToken`

- **Firma:** `string CreateToken(Usuario usuario)`
- **Retorno:** `string`
- **Parámetros:**

- `usuario` (`Usuario`)

- **Qué hace:** Crea/registra un nuevo recurso. trabaja con tokens JWT.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- Auth combina verificación de password (BCrypt), emisión de JWT y auditoría de intentos.
- Ruta relativa en el proyecto: `Auth/TokenService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
