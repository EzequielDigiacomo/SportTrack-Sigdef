# TutorFederacion.cs

## Qué es este archivo

Perfil 1:1 de **tutor** sobre `Participante`, con tipo de tutor y la colección de vínculos a atletas menores (`AtletaFederacionTutor`).

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| PK compartida | `ParticipanteId` es Key y FK. |
| `[MaxLength]` | `TipoTutor`. |
| Colección de join | `AtletasTutores`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- DataAnnotations / Schema.

## Miembros

| Propiedad | Atributos | Tipo | Negocio |
|-----------|-----------|------|---------|
| `ParticipanteId` | `[Key]`, `[ForeignKey]` | `int` | PK/FK a persona. |
| `Participante` | — | `Participante` | Persona. |
| `TipoTutor` | `[MaxLength(50)]` | `string` | Clasificación textual. |
| `AtletasTutores` | — | colección | Relación con atletas. |

## Relaciones

1:1 `Participante`; 1→N `AtletaFederacionTutor` (y de ahí a `AtletaFederacion`). El parentesco tipado está en la tabla intermedia (`Parentesco` enum).

## Notas de estudio

1. `TipoTutor` es string; el parentesco legal/familiar está mejor modelado en `AtletaFederacionTutor.Parentesco`.
2. Misma forma estructural que `EntrenadorFederacion` / `AtletaFederacion`.
