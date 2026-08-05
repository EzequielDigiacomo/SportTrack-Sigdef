# EntrenadorFederacion.cs

## Qué es este archivo

Perfil de **entrenador federado**: extensión 1:1 de `Participante` con club/federación, licencia, selección y becas.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| PK = FK | `[Key]` + `[ForeignKey]` en `ParticipanteId`. |
| `virtual` | Navegaciones EF. |
| `[MaxLength(50)]` | Límite de texto. |
| `bool?` | Flags tri-estado (null = desconocido). |
| `decimal?` | Monto de beca opcional. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- `DataAnnotations` y `Schema` para Key/ForeignKey/MaxLength.

## Miembros

| Propiedad | Atributos | Tipo | Negocio |
|-----------|-----------|------|---------|
| `ParticipanteId` | `[Key]`, `[ForeignKey(Participante)]` | `int` | PK/FK. |
| `Participante` | — | `Participante` | Persona base. |
| `IdClub` / `Club` | `[ForeignKey]` | `int?` / `Club?` | Club. |
| `IdFederacion` / `Federacion` | `[ForeignKey]` | `int?` / `Federacion?` | Federación. |
| `Licencia` | `[MaxLength(50)]` | `string?` | N° licencia. |
| `PerteneceSeleccion` | — | `bool?` | Selección nacional/regional. |
| `CategoriaSeleccion` | `[MaxLength(50)]` | `string?` | Categoría en selección. |
| `BecadoEnard` / `BecadoSdn` | — | `bool?` | Becas. |
| `MontoBeca` | — | `decimal?` | Importe. |
| `PresentoAptoMedico` | — | `bool?` | Documentación médica. |

## Relaciones

1:1 con `Participante`; N→1 opcionales con `Club` y `Federacion`. Comparte forma con `AtletaFederacion`.

## Notas de estudio

1. Patrón **table splitting / extension**: no duplicás Nombre/Apellido; viven en `Participante`.
2. `bool?` permite “no informado”, distinto de `false`.
3. Observá inconsistencia menor: `Club` es `Club?` pero inicializado `= null!` — el `?` manda en nullability.
