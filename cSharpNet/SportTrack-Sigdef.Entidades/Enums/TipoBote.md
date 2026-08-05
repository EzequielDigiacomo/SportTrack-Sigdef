# TipoBote.cs

## Qué es este archivo

Enum alternativo/legacy de tipos de bote con códigos cortos (`K1`, `K2`…) numerados desde **0**.

## Conceptos C# que aparecen

`enum` sin Display; identificadores iguales a la jerga deportiva.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums`.

## Miembros

| Valor | Int |
|-------|-----|
| `K1` | 0 |
| `K2` | 1 |
| `K4` | 2 |
| `C1` | 3 |
| `C2` | 4 |
| `C4` | 5 |

## Relaciones

Convive con `TipoBoteEnum` y tabla `Bote`.

## Notas de estudio

Dos enums para lo mismo = deuda técnica. Al integrar, elegí uno como fuente de verdad y mapeá el otro.
