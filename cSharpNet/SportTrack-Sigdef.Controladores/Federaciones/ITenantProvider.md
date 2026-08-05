# ITenantProvider

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/ITenantProvider.cs`

## 1. Qué es este archivo

Es un **Interfaz (contrato)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Federaciones`
- **Usings:**
  - `using System.Security.Claims;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetFederacionId`

- **Firma:** `int? GetFederacionId()`
- **Retorno:** `int?`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetClubId`

- **Firma:** `int? GetClubId()`
- **Retorno:** `int?`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetRol`

- **Firma:** `string GetRol()`
- **Retorno:** `string`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetUser`

- **Firma:** `ClaimsPrincipal? GetUser()`
- **Retorno:** `ClaimsPrincipal?`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/ITenantProvider.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
