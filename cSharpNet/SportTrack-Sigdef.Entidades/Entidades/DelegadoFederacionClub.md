# DelegadoFederacionClub.cs

## Qué es este archivo

Asocia un **participante** con un **rol federativo**, opcionalmente en el ámbito de una **federación** y un **club** (delegado/representante).

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| PK nullable inusual | `int? IdParticipante` marcado `[Key]` — caso a estudiar. |
| Múltiples `[ForeignKey]` | Rol, federación, club. |
| `virtual` | Navegaciones. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- Enums importado pero no usado en propiedades; DataAnnotations/Schema sí.

## Miembros

| Propiedad | Atributos | Tipo | Negocio |
|-----------|-----------|------|---------|
| `IdParticipante` | `[Key]`, `[ForeignKey(Participante)]` | `int?` | PK/FK persona. |
| `Participante` | — | `Participante` | Persona. |
| `IdRol` / `RolFederacion` | `[ForeignKey]` | `int` / `RolFederacion` | Rol. |
| `IdFederacion` / `Federacion` | `[ForeignKey]` | `int?` / `Federacion` | Ámbito. |
| `ClubIdClub` / `Club` | `[ForeignKey]` | `int?` / `Club?` | Club (nombre de FK poco idiomático). |

## Relaciones

1:1-ish con `Participante`; N→1 `RolFederacion`, `Federacion`, `Club`.

## Notas de estudio

1. `ClubIdClub` sugiere generación automática de EF o rename incompleto; en código nuevo preferí `IdClub`.
2. Una PK `int?` es poco habitual: las PKs suelen ser no nullable.
3. Comentario del fuente remarca que `[Key]` es obligatorio — buena pista de que el mapeo falló sin él.
