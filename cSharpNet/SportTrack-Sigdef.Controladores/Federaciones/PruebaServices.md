# PruebaServices

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/PruebaServices.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `IPruebaServices`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Services`
- **Usings:**
  - `using Microsoft.EntityFrameworkCore;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Prueba;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`
  - `using SportTrack_Sigdef.Controladores.Federaciones;`
  - `using Microsoft.AspNetCore.Mvc;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `PruebaServices(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `GetPruebas`

- **Firma:** `async Task<ActionResult<IEnumerable<PruebaDto>>> GetPruebas()`
- **Retorno:** `Task<ActionResult<IEnumerable<PruebaDto>>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Pruebas.ToListAsync(...)`

#### `GetPrueba`

- **Firma:** `async Task<ActionResult<PruebaDto>> GetPrueba(int id)`
- **Retorno:** `Task<ActionResult<PruebaDto>>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Obtiene/consulta datos. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Pruebas.FindAsync(...)`

#### `PostPrueba`

- **Firma:** `async Task<ActionResult<PruebaDto>> PostPrueba(PruebaCreateDto pruebaDto)`
- **Retorno:** `Task<ActionResult<PruebaDto>>`
- **Parámetros:**

- `pruebaDto` (`PruebaCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Pruebas.AnyAsync(...)`, `_context.SaveChangesAsync(...)`

#### `PutPrueba`

- **Firma:** `async Task<IActionResult> PutPrueba(int id, PruebaCreateDto pruebaDto)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)
- `pruebaDto` (`PruebaCreateDto`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Pruebas.FindAsync(...)`, `_context.SaveChangesAsync(...)`, `PruebaExistsAsync(...)`

#### `DeletePrueba`

- **Firma:** `async Task<IActionResult> DeletePrueba(int id)`
- **Retorno:** `Task<IActionResult>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Pruebas.FindAsync(...)`, `_context.SaveChangesAsync(...)`

#### `PruebaExistsAsync`

- **Firma:** `async Task<bool> PruebaExistsAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.Pruebas.AnyAsync(...)`

#### `MapDistanciaToEnum`

- **Firma:** `DistanciaRegata MapDistanciaToEnum(int distanciaId)`
- **Retorno:** `DistanciaRegata`
- **Parámetros:**

- `distanciaId` (`int`)

- **Qué hace:** Configura o aplica mapeos.

#### `MapEnumToDistanciaId`

- **Firma:** `int MapEnumToDistanciaId(DistanciaRegata distancia)`
- **Retorno:** `int`
- **Parámetros:**

- `distancia` (`DistanciaRegata`)

- **Qué hace:** Configura o aplica mapeos.

#### `MapEnumToCategoriaId`

- **Firma:** `int MapEnumToCategoriaId(CategoriaEdad cat)`
- **Retorno:** `int`
- **Parámetros:**

- `cat` (`CategoriaEdad`)

- **Qué hace:** Configura o aplica mapeos.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/PruebaServices.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
