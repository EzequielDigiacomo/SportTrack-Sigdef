# IEventoEstadoSyncService

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Evento/IEventoEstadoSyncService.cs`

## 1. Qué es este archivo

Es un **Interfaz de servicio (contrato de lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Interface**: contrato que declara qué operaciones debe ofrecer un servicio/repositorio, sin implementarlas.
- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **CancellationToken**: permite cancelar operaciones asíncronas largas.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Evento`
- **Usings:**
  - `using System.Threading;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `SyncAllAsync`

- **Firma:** `Task<int> SyncAllAsync(CancellationToken cancellationToken = default)`
- **Retorno:** `Task<int>`
- **Parámetros:**

- `cancellationToken` (`CancellationToken`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `SyncEventoAsync`

- **Firma:** `Task<bool> SyncEventoAsync(int eventoId, CancellationToken cancellationToken = default)`
- **Retorno:** `Task<bool>`
- **Parámetros:**

- `eventoId` (`int`)
- `cancellationToken` (`CancellationToken`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 5. Notas de estudio

- Buscá la clase que implementa esta interfaz (mismo nombre sin la `I` inicial, o registrada en DI) para ver el código real.
- En tests, las interfaces permiten mockear dependencias fácilmente.
- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Evento/IEventoEstadoSyncService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
