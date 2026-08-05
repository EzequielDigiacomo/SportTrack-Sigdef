# Usuario.cs

> Fuente: `SportTrack-Sigdef.Entidades/Entidades/Usuario.cs`

## Que es este archivo

Archivo C# del proyecto **SportTrack-Sigdef.Entidades**. Define: `Usuario` (tipo principal: **class**).
Sirve como material de estudio del codigo real de SportTrack-Sigdef.

## Conceptos C# / .NET que aparecen

- **class**: tipo referencia; define datos (propiedades) y comportamiento (metodos).
- **Propiedades auto-implementadas**: el compilador crea el campo privado (get; set;).
- **Nullable**: string? / int? indican que el valor puede ser null.
- **Data Annotations**: metadatos de validacion y mapeo a BD ([Required], [Key], etc.).

## Namespace

```
SportTrack_Sigdef.Entidades.Entidades
```

## Usings (importaciones)

- `System`
- `System.Collections.Generic`
- `System.ComponentModel.DataAnnotations.Schema`

## Atributos detectados

- `[ForeignKey(nameof(ParticipanteId))]` - Indica la FK de una relacion.

## Propiedades

| Propiedad | Tipo | Notas |
|-----------|------|-------|
| `IdUsuario` | `int` | No-null (segun anotaciones) |
| `Username` | `string` | No-null (segun anotaciones) |
| `PasswordHash` | `string` | No-null (segun anotaciones) |
| `Email` | `string` | No-null (segun anotaciones) |
| `RolFederacion` | `string` | No-null (segun anotaciones) |
| `IdClub` | `int?` | Puede ser null |
| `Club` | `Club?` | Puede ser null |
| `IdFederacion` | `int?` | Puede ser null |
| `Federacion` | `Federacion?` | Puede ser null |
| `FechaCreacion` | `DateTime` | No-null (segun anotaciones) |
| `EstaActivo` | `bool` | No-null (segun anotaciones) |
| `IntentosFallidos` | `int` | No-null (segun anotaciones) |
| `UltimoAcceso` | `DateTime?` | Puede ser null |
| `Nombre` | `string?` | Puede ser null |
| `Apellido` | `string?` | Puede ser null |
| `Dni` | `string?` | Puede ser null |
| `Telefono` | `string?` | Puede ser null |
| `ParticipanteId` | `int?` | Puede ser null |
| `Participante` | `Participante?` | Puede ser null |

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