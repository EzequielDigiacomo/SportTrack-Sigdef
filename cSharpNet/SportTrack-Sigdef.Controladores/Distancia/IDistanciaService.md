# IDistanciaService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Distancia/IDistanciaService.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Distancia`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Distancia.Dtos;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Text;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `GetAllDistanciasAsync`

- **Firma:** `Task<IEnumerable<DistanciaDto>> GetAllDistanciasAsync()`
- **Retorno:** `Task<IEnumerable<DistanciaDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetDistanciaByIdAsync`

- **Firma:** `Task<DistanciaDto> GetDistanciaByIdAsync(int id)`
- **Retorno:** `Task<DistanciaDto>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `CreateDistanciaAsync`

- **Firma:** `Task<DistanciaDto> CreateDistanciaAsync(DistanciaCreateDto distanciaDto)`
- **Retorno:** `Task<DistanciaDto>`
- **Parámetros:**

- `distanciaDto` (`DistanciaCreateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `UpdateDistanciaAsync`

- **Firma:** `Task<DistanciaDto> UpdateDistanciaAsync(int id, DistanciaUpdateDto distanciaDto)`
- **Retorno:** `Task<DistanciaDto>`
- **Parámetros:**

- `id` (`int`)
- `distanciaDto` (`DistanciaUpdateDto`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `DeleteDistanciaAsync`

- **Firma:** `Task<bool> DeleteDistanciaAsync(int id)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `id` (`int`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `GetDistanciasRegataAsync`

- **Firma:** `Task<IEnumerable<DistanciaRegataDto>> GetDistanciasRegataAsync()`
- **Retorno:** `Task<IEnumerable<DistanciaRegataDto>>`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Distancia/IDistanciaService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
