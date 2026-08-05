# Penalizacion.cs

## Qué es este archivo

Sanción aplicada a un **resultado**: tipo (salida nula, DSQ comportamental, etc.), severidad, tiempo de penalización, juez y fecha.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Dos enums | `TipoPenalizacionEnum`, `SeveridadPenalizacionEnum`. |
| `TimeSpan?` | Penalización temporal opcional. |
| Navigation requerida | `Resultado`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- `SportTrack_Sigdef.Entidades.Enums`.

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `Id` | `int` | PK. |
| `ResultadoId` / `Resultado` | `int` / `Resultado` | Resultado afectado. |
| `TipoPenalizacion` | `TipoPenalizacionEnum` | Motivo tipado. |
| `Descripcion` | `string?` | Detalle. |
| `TiempoPenalizacion` | `TimeSpan?` | Segundos/minutos añadidos. |
| `Severidad` | `SeveridadPenalizacionEnum` | Leve/Media/Grave. |
| `FechaRegistro` | `DateTime` | Default UTC. |
| `JuezAsignado` | `string?` | Quién sancionó. |

## Relaciones

N→1 `Resultado` (colección `Penalizaciones`).

## Notas de estudio

1. Separar tipo y severidad permite políticas (“grave ⇒ DSQ”) en servicios.
2. Los `[Display]` de los enums alimentan UI vía `GetDisplayName()`.
