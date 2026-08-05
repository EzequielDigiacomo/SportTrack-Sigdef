# IDocumentacionService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Documentacion/DocumentacionService.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `interface IDocumentacionService`
- `class DocumentacionService` : `IDocumentacionService`

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **EF Core / DbContext**: acceso a la base de datos mediante el contexto y `DbSet`.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.
- **Options pattern / Settings**: clases de configuración enlazadas a `appsettings.json`.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Documentacion`
- **Usings:**
  - `using CloudinaryDotNet;`
  - `using CloudinaryDotNet.Actions;`
  - `using Microsoft.AspNetCore.Http;`
  - `using Microsoft.EntityFrameworkCore;`
  - `using Microsoft.Extensions.Options;`
  - `using SportTrack_Sigdef.AccesoDatos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`

## 4. Detalle del tipo — `interface IDocumentacionService`

### Métodos

#### `UploadAsync`

- **Firma:** `Task<object> UploadAsync(IFormFile file, int personaId, int tipoDocumento)`
- **Retorno:** `Task<object>`
- **Parámetros:**

- `file` (`IFormFile`)
- `personaId` (`int`)
- `tipoDocumento` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetByPersonaAsync`

- **Firma:** `Task<IEnumerable<object>> GetByPersonaAsync(int personaId)`
- **Retorno:** `Task<IEnumerable<object>>`
- **Parámetros:**

- `personaId` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteAsync`

- **Firma:** `Task<bool> DeleteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 4. Detalle del tipo — `class DocumentacionService`

### Campos (dependencias / estado)

- `_context` — tipo `SportTrackDbContext` (típicamente dependencia inyectada o estado privado)
- `_cloudinary` — tipo `Cloudinary?` (típicamente dependencia inyectada o estado privado)
- `_cloudinaryConfigured` — tipo `bool` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `DocumentacionService(...)`

**Parámetros:**

- `context` (`SportTrackDbContext`)
- `options` (`IOptions<CloudinarySettings>`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `UploadAsync`

- **Firma:** `async Task<object> UploadAsync(IFormFile file, int personaId, int tipoDocumento)`
- **Retorno:** `Task<object>`
- **Parámetros:**

- `file` (`IFormFile`)
- `personaId` (`int`)
- `tipoDocumento` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. lanza `NotFoundException` si no encuentra el recurso; carga relaciones con `Include`/`ThenInclude` (eager loading); persiste cambios con `SaveChangesAsync`; consulta en modo solo-lectura (`AsNoTracking`); operación asíncrona (`await`).
- **Llamadas await destacadas:** `_cloudinary.UploadAsync(...)`, `file.CopyToAsync(...)`, `_context.SaveChangesAsync(...)`

#### `GetByPersonaAsync`

- **Firma:** `async Task<IEnumerable<object>> GetByPersonaAsync(int personaId)`
- **Retorno:** `Task<IEnumerable<object>>`
- **Parámetros:**

- `personaId` (`int`)

- **Qué hace:** Obtiene/consulta datos. consulta en modo solo-lectura (`AsNoTracking`); filtra con LINQ (`Where`); ordena resultados; operación asíncrona (`await`).

#### `DeleteAsync`

- **Firma:** `async Task<bool> DeleteAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** Elimina o desactiva un recurso. persiste cambios con `SaveChangesAsync`; operación asíncrona (`await`).
- **Llamadas await destacadas:** `_context.DocumentacionPersonas.FirstOrDefaultAsync(...)`, `_cloudinary.DestroyAsync(...)`, `_context.SaveChangesAsync(...)`

#### `MapDoc`

- **Firma:** `object MapDoc(DocumentacionFederacionPersona d)`
- **Retorno:** `object`
- **Parámetros:**

- `d` (`DocumentacionFederacionPersona`)

- **Qué hace:** Configura o aplica mapeos.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Documentacion/DocumentacionService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
