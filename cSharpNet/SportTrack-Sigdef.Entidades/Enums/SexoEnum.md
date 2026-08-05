# SexoEnum.cs

## Qué es este archivo

Sexo/categoría de competencia tipada: Masculino, Femenino, Mixto (con Display).

## Conceptos C# que aparecen

`enum` + `[Display]`; valores desde 1.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums` + DataAnnotations.

## Miembros

| Valor | Int | Display |
|-------|-----|---------|
| `Masculino` | 1 | Masculino |
| `Femenino` | 2 | Femenino |
| `Mixto` | 3 | Mixto |

## Relaciones

Paralelo a entidad `Sexo` y a `SexoCompetencia` (otro enum, base 0).

## Notas de estudio

`Mixto` aplica más a pruebas que a personas; el mismo enum sirve a ambos contextos con cuidado.
