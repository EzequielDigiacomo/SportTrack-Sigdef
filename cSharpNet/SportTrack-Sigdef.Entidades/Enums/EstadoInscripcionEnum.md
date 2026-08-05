# EstadoInscripcionEnum.cs

## Qué es este archivo

Estados de una **inscripción** a prueba: inscrito, confirmado, retirado, ausente.

## Conceptos C# que aparecen

`enum` + `[Display]`, valores desde 1.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums` + DataAnnotations.

## Miembros

| Valor | Int | Display |
|-------|-----|---------|
| `Inscrito` | 1 | Inscrito |
| `Confirmado` | 2 | Confirmado |
| `Retirado` | 3 | Retirado |
| `Ausente` | 4 | Ausente |

## Relaciones

`Inscripcion.Estado`.

## Notas de estudio

`Ausente` (no se presentó) ≠ `Retirado` (se dio de baja). DNS a nivel resultado es otro concepto.
