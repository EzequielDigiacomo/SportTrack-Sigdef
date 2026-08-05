# EstadoSolicitudTraspaso.cs

## Qué es este archivo

Enum del **ciclo de vida** de una solicitud de traspaso entre clubes.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| `enum` | Conjunto cerrado de valores enteros con nombre. |
| Valores explícitos | `= 1`, `= 2`… controlan el número persistido. |
| `[Display(Name)]` | Texto/metadato para UI. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Enums`
- `System.ComponentModel.DataAnnotations` — `[Display]`.

## Miembros

| Valor | Int | Significado |
|-------|-----|-------------|
| `PendienteOrigen` | 1 | Esperando respuesta del club origen. |
| `RechazadoOrigen` | 2 | Rechazado por origen. |
| `PendienteFederacion` | 3 | Pasó origen; espera federación. |
| `Aprobado` | 4 | Aprobado (listo/ejecutado según flujo). |
| `RechazadoFederacion` | 5 | Rechazo federativo. |
| `Cancelado` | 6 | Cancelado por solicitante/admin. |
| `Vencido` | 7 | Expiró por tiempo/periodo. |

## Relaciones

Usado por `SolicitudTraspaso.Estado`.

## Notas de estudio

1. Numerar desde 1 evita confundir “0 = no seteado” con un estado real si el default de enum es 0.
2. Los nombres del Display aquí repiten el identificador; en otros enums el Display es más legible (“En Curso”).
