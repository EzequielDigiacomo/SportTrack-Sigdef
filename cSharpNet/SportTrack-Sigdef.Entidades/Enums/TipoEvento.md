# TipoEvento.cs

## Qué es este archivo

Clasificación de **clase de evento** (oficial, campeonato, recreativo, entrenamiento, clasificatorio).

## Conceptos C# que aparecen

`enum` con comentarios inline por valor; sin `[Display]`.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums`.

## Miembros

| Valor | Int | Negocio |
|-------|-----|---------|
| `CarreraOficial` | 1 | Competencia con ranking. |
| `Campeonato` | 2 | Con premios. |
| `Recreativo` | 3 | Sin competencia formal. |
| `Entrenamiento` | 4 | Sesión grupal. |
| `Clasificatorio` | 5 | Clasifica a otro evento. |

## Relaciones

Conceptualmente alinea con `Evento.TipoEvento` (hoy `string` en la entidad — posible deuda).

## Notas de estudio

Usar este enum en la entidad en lugar de string mejoraría tipado y validación.
