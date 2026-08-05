# EstadoResultado.cs

## Qué es este archivo

Aunque el archivo se llama `EstadoResultado.cs`, declara el enum **`EstadoResultadoEnum`**: estados de un resultado de carrera (pendiente, oficial, DSQ, DNS, DNF…).

## Conceptos C# que aparecen

`enum` + `[Display]`; el **nombre del tipo** no coincide con el del archivo (válido en C#, confuso al navegar).

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums` + DataAnnotations.

## Miembros

| Valor | Int | Display |
|-------|-----|---------|
| `Pendiente` | 1 | Pendiente |
| `Preliminar` | 2 | Preliminar |
| `Oficial` | 3 | Oficial |
| `Descalificado` | 4 | DSQ |
| `Revisado` | 5 | Revisado |
| `DNS` | 6 | DNS (Did Not Start) |
| `DNF` | 7 | DNF (Did Not Finish) |

## Relaciones

`Resultado.Estado`.

## Notas de estudio

1. DNS/DNF/DSQ son jerga internacional de timing deportivo.
2. Alineá nombre de archivo y de tipo cuando puedas (`EstadoResultadoEnum.cs`).
