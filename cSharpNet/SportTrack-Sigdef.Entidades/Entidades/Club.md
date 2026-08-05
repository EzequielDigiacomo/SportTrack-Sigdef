# Club.cs

> Fuente: `SportTrack-Sigdef.Entidades/Entidades/Club.cs`

## Que es este archivo

Archivo C# del proyecto **SportTrack-Sigdef.Entidades**. Define: `Club` (tipo principal: **class**).
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
- `System.Collections.Generic`
- `System.ComponentModel.DataAnnotations.Schema`
- `SportTrack_Sigdef.Entidades.Enums`

## Atributos detectados

_Sin atributos destacados._

## Propiedades

| Propiedad | Tipo | Notas |
|-----------|------|-------|
| `IdClub` | `int` | No-null (segun anotaciones) |
| `Nombre` | `string` | No-null (segun anotaciones) |
| `Siglas` | `string?` | Puede ser null |
| `Email` | `string?` | Puede ser null |
| `Telefono` | `string?` | Puede ser null |
| `Direccion` | `string?` | Puede ser null |
| `Ubicacion` | `string?` | Puede ser null |
| `Activo` | `bool` | No-null (segun anotaciones) |
| `IdFederacion` | `int?` | Puede ser null |
| `Federacion` | `Federacion?` | Puede ser null |
| `PlanSaaSId` | `int?` | Puede ser null |
| `PlanSaaS` | `PlanSaaS?` | Puede ser null |
| `FrecuenciaPago` | `string?` | Puede ser null |
| `FechaAltaPlan` | `DateTime?` | Puede ser null |
| `FechaVencimientoPlan` | `DateTime?` | Puede ser null |
| `BloqueadoPorFaltaDePago` | `bool` | No-null (segun anotaciones) |
| `PagoAfiliacionAlDia` | `bool` | No-null (segun anotaciones) |
| `SolicitudPagoPendiente` | `bool` | No-null (segun anotaciones) |
| `EstadoMatricula` | `EstadoPago` | No-null (segun anotaciones) |
| `Participantes` | `ICollection<Participante>` | No-null (segun anotaciones) |
| `Usuarios` | `ICollection<Usuario>` | No-null (segun anotaciones) |
| `AtletasFederados` | `ICollection<AtletaFederacion>` | No-null (segun anotaciones) |
| `Entrenadores` | `ICollection<EntrenadorFederacion>` | No-null (segun anotaciones) |
| `Representantes` | `ICollection<DelegadoFederacionClub>` | No-null (segun anotaciones) |
| `Pagos` | `ICollection<PagoFederacionTransaccion>` | No-null (segun anotaciones) |

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