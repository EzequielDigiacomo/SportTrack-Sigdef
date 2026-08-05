# Participante.cs

> Fuente: `SportTrack-Sigdef.Entidades/Entidades/Participante.cs`

## Que es este archivo

Archivo C# del proyecto **SportTrack-Sigdef.Entidades**. Define: `Participante` (tipo principal: **class**).
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
- `System.Linq`
- `System.Text`
- `System.Threading.Tasks`

## Atributos detectados

_Sin atributos destacados._

## Propiedades

| Propiedad | Tipo | Notas |
|-----------|------|-------|
| `ParticipanteId` | `int` | No-null (segun anotaciones) |
| `Nombre` | `string` | No-null (segun anotaciones) |
| `Apellido` | `string` | No-null (segun anotaciones) |
| `FechaNacimiento` | `DateTime` | No-null (segun anotaciones) |
| `SexoId` | `int` | No-null (segun anotaciones) |
| `CategoriaId` | `int?` | Puede ser null |
| `Pais` | `string?` | Puede ser null |
| `IdClub` | `int?` | Puede ser null |
| `Club` | `Club?` | Puede ser null |
| `Documento` | `string?` | Puede ser null |
| `Email` | `string?` | Puede ser null |
| `Telefono` | `string?` | Puede ser null |
| `Direccion` | `string?` | Puede ser null |
| `PagoAfiliacionAlDia` | `bool` | No-null (segun anotaciones) |
| `Sexo` | `Sexo` | No-null (segun anotaciones) |
| `Categoria` | `Categoria?` | Puede ser null |
| `Inscripciones` | `ICollection<Inscripcion>` | No-null (segun anotaciones) |
| `DelegadoFederacionClub` | `DelegadoFederacionClub?` | Puede ser null |
| `EntrenadorFederacion` | `EntrenadorFederacion?` | Puede ser null |
| `TutorFederacion` | `TutorFederacion?` | Puede ser null |
| `AtletaFederacion` | `AtletaFederacion?` | Puede ser null |
| `Documentacion` | `ICollection<DocumentacionFederacionPersona>` | No-null (segun anotaciones) |
| `Usuario` | `Usuario?` | Puede ser null |
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