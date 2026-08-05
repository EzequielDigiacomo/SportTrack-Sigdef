# Hilo.cs

> Fuente: `SportTrack-Sigdef.Entidades/Entidades/Hilo.cs`

## Que es este archivo

Archivo C# del proyecto **SportTrack-Sigdef.Entidades**. Define: `Hilo` (tipo principal: **class**).
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

_Ninguno o implicitos globales (GlobalUsings)._

## Atributos detectados

_Sin atributos destacados._

## Propiedades

| Propiedad | Tipo | Notas |
|-----------|------|-------|
| `IdHilo` | `int` | No-null (segun anotaciones) |
| `Asunto` | `string` | No-null (segun anotaciones) |
| `SistemaOrigen` | `string` | No-null (segun anotaciones) |
| `IdCampana` | `int?` | Puede ser null |
| `Campana` | `CampanaEnvio?` | Puede ser null |
| `CreadoEn` | `DateTime` | No-null (segun anotaciones) |
| `UltimoMensajeEn` | `DateTime` | No-null (segun anotaciones) |
| `Mensajes` | `ICollection<Mensaje>` | No-null (segun anotaciones) |

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