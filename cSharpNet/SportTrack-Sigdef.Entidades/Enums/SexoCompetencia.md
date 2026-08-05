# SexoCompetencia.cs

## Qué es este archivo

Enum legacy/alternativo de sexo de competencia numerado desde **0**.

## Conceptos C# que aparecen

`enum` sin atributos.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums`.

## Miembros

| Valor | Int |
|-------|-----|
| `Masculino` | 0 |
| `Femenino` | 1 |
| `Mixto` | 2 |

## Relaciones

Nombre coincidente con propiedad `Prueba.SexoCompetencia` (`int` FK); no necesariamente este enum.

## Notas de estudio

Base 0 vs base 1 respecto a `SexoEnum` es una trampa clásica de bugs al castear.
