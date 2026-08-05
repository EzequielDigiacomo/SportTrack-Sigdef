# SeveridadPenalizacionEnum.cs

## Qué es este archivo

Gravedad de una penalización: Leve, Media, Grave.

## Conceptos C# que aparecen

`enum` + `[Display]`.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums` + DataAnnotations.

## Miembros

| Valor | Int | Display |
|-------|-----|---------|
| `Leve` | 1 | Leve |
| `Media` | 2 | Media |
| `Grave` | 3 | Grave |

## Relaciones

`Penalizacion.Severidad`.

## Notas de estudio

Combinar tipo + severidad permite matrices de decisión (ej. grave + antideportivo ⇒ descalificación).
