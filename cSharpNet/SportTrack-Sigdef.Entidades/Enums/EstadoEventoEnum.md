# EstadoEventoEnum.cs

## Qué es este archivo

Estado de un **evento** o **evento-prueba**: programada, en curso, finalizado, cancelado.

## Conceptos C# que aparecen

`enum` + `[Display]` con espacios en el nombre visible (“En Curso”).

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums` + DataAnnotations.

## Miembros

| Valor | Int | Display |
|-------|-----|---------|
| `Programada` | 1 | Programada |
| `EnCurso` | 2 | En Curso |
| `Finalizado` | 3 | Finalizado |
| `Cancelado` | 4 | Cancelado |

## Relaciones

`Evento.Estado`, `EventoPrueba.Estado`. (`Fase.Estado` sigue siendo string.)

## Notas de estudio

Género gramatical “Programada” encaja con “prueba/carrera”; al reutilizar para “evento” a veces se prefiere neutro.
