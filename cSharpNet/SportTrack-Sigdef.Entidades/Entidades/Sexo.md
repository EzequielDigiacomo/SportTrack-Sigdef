# Sexo.cs

## Qué es este archivo

Tabla catálogo de **sexo** (Masculino/Femenino/Mixto, etc.) usada por participantes y pruebas. Complementa a los enums `SexoEnum` / `SexoCompetencia`.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Entidad catálogo | Filas en BD en lugar de solo enum. |
| Colecciones inversas | `Participantes`, `Pruebas`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `Id` | `int` | PK. |
| `Nombre` | `string` | Etiqueta. |
| `Participantes` | colección | Personas con este sexo. |
| `Pruebas` | colección | Pruebas de esa competencia. |

## Relaciones

1→N `Participante` y `Prueba` (vía FKs `SexoId` / `SexoCompetencia` según mapeo).

## Notas de estudio

1. ¿Por qué entidad y también enums? Flexibilidad de catálogo editable vs tipado fuerte en código.
2. Al seedear BD, los `Id` suelen alinearse con los valores del enum.
