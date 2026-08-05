# TipoPenalizacionEnum.cs

## Qué es este archivo

Tipos de **penalización** en regata (salida nula, fuera de pista, antideportivo, etc.).

## Conceptos C# que aparecen

`enum`, valores explícitos, `[Display(Name)]` con texto en español para UI.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums` + DataAnnotations (+ usings de template).

## Miembros

| Valor | Int | Display |
|-------|-----|---------|
| `SalidaNula` | 1 | Salida Nula |
| `Obstaculizo` | 2 | Obstaculizo |
| `FueraPista` | 3 | Fuera de Pista |
| `MaterialInadecuado` | 4 | Material Inadecuado |
| `ComportamientoAntideportivo` | 5 | Comportamiento Antideportivo |
| `NollegaAMeta` | 6 | No llega a la meta |

## Relaciones

`Penalizacion.TipoPenalizacion`.

## Notas de estudio

1. El identificador C# no puede tener espacios; Display cubre la presentación.
2. `NollegaAMeta` muestra convención imperfecta de naming (preferible `NoLlegaAMeta`).
