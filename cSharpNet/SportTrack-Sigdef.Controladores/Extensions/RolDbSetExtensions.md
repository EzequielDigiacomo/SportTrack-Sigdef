# RolDbSetExtensions

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Extensions/RolDbSetExtensions.cs`

## 1. Qué es este archivo

Es un **Métodos de extensión** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Extension methods**: métodos estáticos con `this` que añaden API a tipos existentes.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Extensions`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System.Collections.Generic;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetByTipoAsync`

- **Firma:** `async Task<RolFederacion?> GetByTipoAsync(this DbSet<RolFederacion> roles, RolTipo tipo)`
- **Retorno:** `Task<RolFederacion?>`
- **Parámetros:**

- `roles` (`this DbSet<RolFederacion>`)
- `tipo` (`RolTipo`)

- **Qué hace:** Obtiene/consulta datos. puede lanzar excepciones de dominio; operación asíncrona (`await`).

#### `GetByTipoAsync`

- **Firma:** `async Task<RolFederacion?> GetByTipoAsync(this DbSet<RolFederacion> roles, string tipo)`
- **Retorno:** `Task<RolFederacion?>`
- **Parámetros:**

- `roles` (`this DbSet<RolFederacion>`)
- `tipo` (`string`)

- **Qué hace:** Obtiene/consulta datos. puede lanzar excepciones de dominio; operación asíncrona (`await`).

#### `GetIdByTipoAsync`

- **Firma:** `async Task<int?> GetIdByTipoAsync(this DbSet<RolFederacion> roles, RolTipo tipo)`
- **Retorno:** `Task<int?>`
- **Parámetros:**

- `roles` (`this DbSet<RolFederacion>`)
- `tipo` (`RolTipo`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `roles.GetByTipoAsync(...)`

#### `ExistsByTipoAsync`

- **Firma:** `async Task<bool> ExistsByTipoAsync(this DbSet<RolFederacion> roles, RolTipo tipo)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `roles` (`this DbSet<RolFederacion>`)
- `tipo` (`RolTipo`)

- **Qué hace:** Comprueba existencia. operación asíncrona (`await`).

#### `GetAllOrderedAsync`

- **Firma:** `async Task<List<RolFederacion>> GetAllOrderedAsync(this DbSet<RolFederacion> roles)`
- **Retorno:** `Task<List<RolFederacion>>`
- **Parámetros:**

- `roles` (`this DbSet<RolFederacion>`)

- **Qué hace:** Obtiene/consulta datos. ordena resultados; operación asíncrona (`await`).

#### `GetByTiposAsync`

- **Firma:** `async Task<List<RolFederacion>> GetByTiposAsync(this DbSet<RolFederacion> roles, params RolTipo[] tipos)`
- **Retorno:** `Task<List<RolFederacion>>`
- **Parámetros:**

- `roles` (`this DbSet<RolFederacion>`)
- `tipos` (`RolTipo[]`)

- **Qué hace:** Obtiene/consulta datos. filtra con LINQ (`Where`); operación asíncrona (`await`).

#### `ToDbString`

- **Firma:** `string ToDbString(this RolTipo tipo)`
- **Retorno:** `string`
- **Parámetros:**

- `tipo` (`this RolTipo`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

#### `TryParseFromDbString`

- **Firma:** `bool TryParseFromDbString(string dbString, out RolTipo tipo)`
- **Retorno:** `bool`
- **Parámetros:**

- `dbString` (`string`)
- `tipo` (`RolTipo`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

## 5. Notas de estudio

- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Extensions/RolDbSetExtensions.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
