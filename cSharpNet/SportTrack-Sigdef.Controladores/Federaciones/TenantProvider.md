# TenantProvider

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/TenantProvider.cs`

## 1. Qué es este archivo

Es un **Tipo `class` de la capa Controladores** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `ITenantProvider`.

## 2. Conceptos C# / .NET que aparecen

- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Federaciones`
- **Usings:**
  - `using Microsoft.AspNetCore.Http;`
  - `using System;`
  - `using System.Security.Claims;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_httpContextAccessor` — tipo `IHttpContextAccessor` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `TenantProvider(...)`

**Parámetros:**

- `httpContextAccessor` (`IHttpContextAccessor`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetUser`

- **Firma:** `ClaimsPrincipal? GetUser()`
- **Retorno:** `ClaimsPrincipal?`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos.

#### `GetFederacionId`

- **Firma:** `int? GetFederacionId()`
- **Retorno:** `int?`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos.

#### `GetClubId`

- **Firma:** `int? GetClubId()`
- **Retorno:** `int?`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos.

#### `GetRol`

- **Firma:** `string GetRol()`
- **Retorno:** `string`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos.

#### `IsGlobalAdmin`

- **Firma:** `bool IsGlobalAdmin()`
- **Retorno:** `bool`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `ParseQueryInt`

- **Firma:** `int? ParseQueryInt(params string[] keys)`
- **Retorno:** `int?`
- **Parámetros:**

- `keys` (`string[]`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

## 5. Notas de estudio

- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/TenantProvider.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
