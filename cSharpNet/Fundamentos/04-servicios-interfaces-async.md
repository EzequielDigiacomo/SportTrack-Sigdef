# 04 — Servicios, interfaces y async/await

Lección alineada con la capa **`SportTrack-Sigdef.Controladores`**: cómo se separa el contrato (`interface`) de la implementación (`class`), cómo se inyectan dependencias, y por qué casi todo es `async Task`.

## 1. ¿Qué es un servicio?

Un **servicio** es una clase que concentra **reglas de negocio**. No es un Controller (HTTP) ni una entidad (tabla).

Ejemplo típico en este repo:

```
IBoteService          ← contrato
    ↑ implementa
BoteService           ← lógica
    ↓ usa
IBoteRepository       ← acceso a datos
IMapper               ← AutoMapper
```

Flujo de una petición:

```
Controller (API) → IBoteService → BoteService → IBoteRepository → DbContext → SQL
                         ↓
                      BoteDto
```

## 2. Interfaces: el contrato

```csharp
public interface IBoteService
{
    Task<IEnumerable<BoteDto>> GetAllBotesAsync();
    Task<BoteDto> GetBoteByIdAsync(int id);
    Task<BoteDto> CreateBoteAsync(BoteCreateDto boteDto);
}
```

Ideas clave:

| Concepto | Qué significa |
|----------|----------------|
| **Interface** | Solo declara firmas; no tiene cuerpo (en el estilo clásico) |
| **Nombre con `I`** | Convención C#: `IBoteService`, `IAuthService` |
| **Por qué existe** | Desacoplar: el Controller depende del contrato, no de la clase concreta |
| **Tests** | Podés mockear `IBoteService` sin tocar la base de datos |

La clase concreta:

```csharp
public class BoteService : IBoteService
{
    // debe implementar TODOS los métodos de la interfaz
}
```

## 3. Dependency Injection (DI)

En el constructor **recibís** lo que necesitás; no hacés `new BoteRepository()` adentro.

```csharp
public class BoteService : IBoteService
{
    private readonly IBoteRepository _boteRepository;
    private readonly IMapper _mapper;

    public BoteService(IBoteRepository boteRepository, IMapper mapper)
    {
        _boteRepository = boteRepository;
        _mapper = mapper;
    }
}
```

- `readonly`: se asigna una vez (en el ctor) y no se reemplaza.
- El contenedor de ASP.NET Core resuelve `IBoteRepository` → `BoteRepository` según el registro en `Program.cs`.
- Esto es el **constructor injection** (la forma más común de DI).

Pregunta de estudio: ¿quién instancia `BoteService`? Respuesta: el framework, cuando un Controller pide `IBoteService`.

## 4. async / await / Task

Casi todos los métodos de servicios terminan en `Async` y retornan `Task` o `Task<T>`:

```csharp
public async Task<BoteDto> GetBoteByIdAsync(int id)
{
    var bote = await _boteRepository.GetByIdAsync(id);
    if (bote == null)
        throw new NotFoundException($"Bote con ID {id} no encontrado");

    return _mapper.Map<BoteDto>(bote);
}
```

| Pieza | Rol |
|-------|-----|
| `Task` | Representa una operación que terminará en el futuro |
| `Task<T>` | Igual, pero con resultado de tipo `T` |
| `async` | Permite usar `await` dentro del método |
| `await` | Espera el resultado sin bloquear el hilo del servidor |

**Regla de oro en ASP.NET:** no uses `.Result` ni `.Wait()` sobre un `Task`. Encadená `await` de punta a punta (Controller → Service → Repository).

### ¿Por qué async en bases de datos?

Mientras SQL responde, el hilo puede atender otras requests. Eso escala mejor bajo carga.

## 5. Repository vs Service

| Capa | Responsabilidad |
|------|-----------------|
| **Repository** | “Cómo leo/escribo datos” (queries EF, `SaveChanges`, Includes) |
| **Service** | “Qué está permitido y qué significa” (validar, mapear, lanzar excepciones de dominio, orquestar varios repos) |

En este proyecto a veces el Service usa el `DbContext` directo (ej. `AuthService`). El patrón no es dogmático, pero la idea se mantiene: **la regla de negocio no vive en el Controller**.

## 6. Excepciones de dominio

En `Exceptions/` vas a ver cosas como:

- `NotFoundException` → recurso inexistente
- `BadRequestException` → datos inválidos / regla rota
- `UnauthorizedException` → auth fallida

El Service **lanza**; la API (middleware/filtros) **traduce** a HTTP 404 / 400 / 401.

## 7. Mini-ejercicios

1. Abrí `Bote/IBoteService.cs` y `Bote/BoteService.cs` lado a lado: marcá cada método del contrato en la implementación.
2. Contá cuántas dependencias inyecta `AuthService` y para qué sirve cada una.
3. Buscá un `await` dentro de un `foreach`: ¿qué implica hacer N consultas? (problema N+1).

## 8. Dónde seguir

- DTOs y patrones: [`05-dtos-y-patrones.md`](05-dtos-y-patrones.md)
- Docs del proyecto: [`../SportTrack-Sigdef.Controladores/README.md`](../SportTrack-Sigdef.Controladores/README.md)
- Ejemplo guiado: `Bote/`, luego `Auth/`, luego un servicio grande en `Federaciones/`
