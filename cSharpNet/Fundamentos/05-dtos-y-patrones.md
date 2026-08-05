# 05 — DTOs y patrones (Repository, Service, AutoMapper)

Cómo viajan los datos en **SportTrack-Sigdef** sin exponer entidades de EF Core directo a la API.

## 1. El problema que resuelven los DTOs

Una **entidad** (en `SportTrack-Sigdef.Entidades`) suele tener:

- claves y relaciones de navegación (`Club`, `Federacion`, colecciones…)
- detalles de persistencia
- a veces datos sensibles (hashes de password, flags internos)

Si devolvieras la entidad cruda en JSON:

- filtrarías de más o de menos
- acoplarías el contrato HTTP al esquema de BD
- arriesgarías ciclos de serialización (`Club` → `Atletas` → `Club` → …)

Un **DTO (Data Transfer Object)** es una clase (o record) **plana**, pensada solo para transportar datos.

```csharp
public class BoteDto
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
}
```

## 2. Familias de DTOs que vas a ver

| Sufijo / nombre | Uso típico |
|-----------------|------------|
| `XxxDto` | Lectura / respuesta |
| `XxxCreateDto` | Body de POST (alta) |
| `XxxUpdateDto` | Body de PUT/PATCH |
| `XxxDetailDto` / `XxxListDto` | Variantes con más/menos campos |
| `XxxResponseDto` | Respuesta específica (login, etc.) |
| `PagedResponseDto` + `PaginationParamsDto` | Listados paginados |

Ejemplo conceptual:

```
POST /botes  ← BoteCreateDto
         ↓ Service mapea a entidad Bote
         ↓ Repository guarda
         ↓ Service mapea a BoteDto
← 201     BoteDto
```

## 3. class vs record

En C# moderno muchos DTOs son `record`. En este proyecto predominan **`class`** con propiedades `{ get; set; }`.

| | `class` | `record` |
|--|---------|----------|
| Igualdad | Por referencia (por defecto) | Por valor (propiedades) |
| Estilo | Mutable, típico en APIs con setters | Más “datos inmutables” |
| Uso aquí | Mayoría de DTOs | Aparece en algunos modelos auxiliares |

No te trabes: lo importante es que el DTO **no tenga lógica de BD**.

## 4. Validación con Data Annotations

En DTOs de entrada verás atributos:

```csharp
[Required(ErrorMessage = "El nombre del club es obligatorio")]
[StringLength(100)]
public string Nombre { get; set; } = string.Empty;

[EmailAddress]
public string? Email { get; set; }
```

ASP.NET puede validar el model binding automáticamente (`ModelState`). El Service igual puede validar reglas más ricas (unicidad, multi-tenant, etc.).

## 5. AutoMapper: el puente entidad ↔ DTO

Archivo central: `Mappings/MappingProfile.cs` (hereda de `Profile`).

```csharp
CreateMap<Entidades.Entidades.Bote, BoteDto>().ReverseMap();
CreateMap<BoteCreateDto, Entidades.Entidades.Bote>();
```

En el servicio:

```csharp
var botes = await _boteRepository.GetAllAsync();
return _mapper.Map<IEnumerable<BoteDto>>(botes);
```

| API AutoMapper | Qué hace |
|----------------|----------|
| `CreateMap<A,B>()` | Define conversión A → B |
| `.ReverseMap()` | También B → A |
| `.ForMember(...)` | Personaliza una propiedad (nombres distintos, cálculos) |
| `.Ignore()` | No mapear ese miembro |
| `_mapper.Map<T>(src)` | Ejecuta el mapeo |
| `_mapper.Map(src, dest)` | Actualiza `dest` con datos de `src` (updates) |

**Nota de estudio:** cuando la entidad usa `IdClub` y el DTO usa `Id`, el `ForMember` en el Profile es tu pista.

## 6. Repository pattern

```csharp
public interface IBoteRepository
{
    Task<IEnumerable<Bote>> GetAllAsync();
    Task<Bote?> GetByIdAsync(int id);
    Task<Bote> CreateAsync(Bote bote);
    // ...
}
```

Beneficios:

- El Service no está lleno de detalles de EF
- Podés cambiar la implementación (o mockearla)
- Punto único para Includes / queries repetidas

En la práctica, algunos servicios usan `SportTrackDbContext` directo (sobre todo auth, reportes, casos complejos). Eso no anula el patrón: mirá cada feature.

## 7. Service layer (capa de aplicación)

El Service:

1. Valida
2. Llama repositorio(s) / DbContext
3. Aplica reglas (bloqueos, multi-tenant, estados)
4. Mapea a DTO
5. Lanza excepciones de dominio si corresponde

Ejemplo condensado:

```csharp
public async Task<BoteDto> UpdateBoteAsync(int id, BoteUpdateDto boteDto)
{
    var existing = await _boteRepository.GetByIdAsync(id);
    if (existing == null)
        throw new NotFoundException($"Bote con ID {id} no encontrado");

    _mapper.Map(boteDto, existing);
    var updated = await _boteRepository.UpdateAsync(existing);
    return _mapper.Map<BoteDto>(updated);
}
```

## 8. Otros patrones / piezas en Controladores

| Pieza | Idea |
|-------|------|
| **TenantProvider** | Saber “de qué federación/club” es el usuario actual |
| **Options / Settings** | `MercadoPagoSettings` ligado a configuración |
| **Caching** | `ILiveCacheService` para lecturas calientes (tiempos en vivo) |
| **SignalR Hub** | Empujar eventos a clientes conectados |
| **BackgroundService** | Sync de estados de evento en segundo plano |
| **Extension methods** | Helpers sobre enums/DbSet (`Extensions/`) |

## 9. Mapa mental del request

```
JSON (DTO in)
  → Controller
    → Service (reglas)
      → Repository / DbContext
        → Entidad
      ← Entidad
    → AutoMapper
  ← DTO out
← JSON
```

## 10. Mini-ejercicios

1. Compará `BoteCreateDto` vs entidad `Bote`: ¿qué campos no se aceptan en el alta?
2. En `MappingProfile`, buscá un `ForMember` y explicá por qué hace falta.
3. En `Federaciones/DTOs/Base`, leé paginación: ¿qué lleva el request y qué devuelve el response?
4. ¿Por qué `Usuario` no debería devolverse entero en un endpoint de listado?

## 11. Dónde seguir

- Servicios e interfaces: [`04-servicios-interfaces-async.md`](04-servicios-interfaces-async.md)
- Índice de la capa: [`../SportTrack-Sigdef.Controladores/README.md`](../SportTrack-Sigdef.Controladores/README.md)
- Empezá por `Bote/Dtos/`, después `Club/Dtos/ClubDtos.cs`, después un DTO “gordo” en `Federaciones/DTOs/Evento/`
