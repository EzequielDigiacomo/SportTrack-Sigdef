# TipoBoteEnum.cs

## Qué es este archivo

Enum “rico” de **embarcaciones** con Display corto (K1, K2, C1…).

## Conceptos C# que aparecen

`enum` + `[Display]`; nombres largos en código (`KayakIndividual`) y cortos en UI.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums` + DataAnnotations.

## Miembros

| Valor | Int | Display |
|-------|-----|---------|
| `KayakIndividual` | 1 | K1 |
| `KayakDoble` | 2 | K2 |
| `KayakCuadruple` | 3 | K4 |
| `CanoaIndividual` | 4 | C1 |
| `CanoaDoble` | 5 | C2 |
| `CanoaCuadruple` | 6 | C4 |

## Relaciones

Complementa entidad `Bote` y enum hermano `TipoBote` (valores distintos).

## Notas de estudio

1. El número de tripulantes está implícito en K1/K2/K4.
2. No mezcles los ints de `TipoBote` (empieza en 0) con estos (empieza en 1).
