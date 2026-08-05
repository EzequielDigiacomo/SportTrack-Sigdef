# PeriodoTraspaso.cs

> Fuente: `SportTrack-Sigdef.Entidades/Entidades/PeriodoTraspaso.cs`

## Que es este archivo

Archivo C# del proyecto **SportTrack-Sigdef.Entidades**. Define: `PeriodoTraspaso` (tipo principal: **class**).
Sirve como material de estudio del codigo real de SportTrack-Sigdef.

## Conceptos C# / .NET que aparecen

- **class**: tipo referencia; define datos (propiedades) y comportamiento (metodos).
- **Propiedades auto-implementadas**: el compilador crea el campo privado (get; set;).
- **Nullable**: string? / int? indican que el valor puede ser null.

## Namespace

```
SportTrack_Sigdef.Entidades.Entidades
```

## Usings (importaciones)

- `System`

## Atributos detectados

_Sin atributos destacados._

## Propiedades

| Propiedad | Tipo | Notas |
|-----------|------|-------|
| `IdPeriodoTraspaso` | `int` | No-null (segun anotaciones) |
| `IdFederacion` | `int` | No-null (segun anotaciones) |
| `Federacion` | `Federacion` | No-null (segun anotaciones) |
| `FechaInicio` | `DateTime` | No-null (segun anotaciones) |
| `FechaFin` | `DateTime` | No-null (segun anotaciones) |
| `Activo` | `bool` | No-null (segun anotaciones) |
| `Observaciones` | `string?` | Puede ser null |
| `CreadoPorUsuarioId` | `int?` | Puede ser null |
| `FechaCreacion` | `DateTime` | No-null (segun anotaciones) |

## Metodos

_Sin metodos publicos/protegidos detectados (puede ser solo DTO/entidad)._

## Como estudiarlo

1. Abre el `.cs` original en el IDE.
2. Identifica el tipo (class) y su responsabilidad.
3. Lee cada propiedad: tipo, nullabilidad y significado de negocio.
4. Si hay metodos, sigue el flujo (validaciones -> persistencia -> retorno DTO).
5. Busca en este mismo directorio los tipos relacionados (DTOs, interfaces, entidades).

## Notas de estudio

- En C#, casi todo vive dentro de un **tipo** (class / interface / enum / record).
- Los corchetes `[Atributo]` agregan metadatos usados por el runtime, EF Core, ASP.NET, Swagger, etc.
- `Task` / `async` aparecen cuando hay trabajo de I/O (base de datos, HTTP, archivos).
- Las interfaces (`I...`) desacoplan el contrato de la implementacion (util para testing y DI).