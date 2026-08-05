# RolTipo.cs

## Qué es este archivo

Roles del ecosistema federativo (admin, presidente, delegado, entrenador, atleta, secretario).

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| `enum RolTipo : int` | Underlying type explícito (`int`). |
| Valores desde 1 | Evita default 0 ambiguo. |

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums`.

## Miembros

| Valor | Int |
|-------|-----|
| `Administrador` | 1 |
| `PresidenteFederacion` | 2 |
| `DelegadoClub` | 3 |
| `Entrenador` | 4 |
| `EntrenadorSeleccion` | 5 |
| `Atleta` | 6 |
| `Secretario` | 7 |

## Relaciones

Parseado desde `RolFederacion.Tipo` vía `TipoEnum`. Distinto del string `Usuario.RolFederacion`.

## Notas de estudio

`: int` es redundante (default de enum) pero documenta la intención de persistir como entero.
