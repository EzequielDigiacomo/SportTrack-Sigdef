# LiveCacheService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Caching/LiveCacheService.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `ILiveCacheService`.

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Caching`
- **Usings:**
  - `using Microsoft.Extensions.Caching.Memory;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `_cache` — tipo `IMemoryCache` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `LiveCacheService(...)`

**Parámetros:**

- `cache` (`IMemoryCache`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `Remove`

- **Firma:** `void Remove(string key)`
- **Retorno:** `void`
- **Parámetros:**

- `key` (`string`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `InvalidateEvento`

- **Firma:** `void InvalidateEvento(int eventoId)`
- **Retorno:** `void`
- **Parámetros:**

- `eventoId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. interactúa con caché.

#### `InvalidateEventoPrueba`

- **Firma:** `void InvalidateEventoPrueba(int eventoPruebaId, int? eventoId = null)`
- **Retorno:** `void`
- **Parámetros:**

- `eventoPruebaId` (`int`)
- `eventoId` (`int?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. interactúa con caché.

#### `InvalidateFase`

- **Firma:** `void InvalidateFase(int faseId, int? eventoId = null, int? eventoPruebaId = null)`
- **Retorno:** `void`
- **Parámetros:**

- `faseId` (`int`)
- `eventoId` (`int?`)
- `eventoPruebaId` (`int?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. interactúa con caché.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Caching/LiveCacheService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
