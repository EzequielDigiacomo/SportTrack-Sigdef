# ILiveCacheService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Caching/ILiveCacheService.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Caching`
- **Usings:**
  - _(ninguno explícito)_

## 4. Detalle del tipo — tipo principal

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

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `InvalidateEventoPrueba`

- **Firma:** `void InvalidateEventoPrueba(int eventoPruebaId, int? eventoId = null)`
- **Retorno:** `void`
- **Parámetros:**

- `eventoPruebaId` (`int`)
- `eventoId` (`int?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `InvalidateFase`

- **Firma:** `void InvalidateFase(int faseId, int? eventoId = null, int? eventoPruebaId = null)`
- **Retorno:** `void`
- **Parámetros:**

- `faseId` (`int`)
- `eventoId` (`int?`)
- `eventoPruebaId` (`int?`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Caching/ILiveCacheService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
