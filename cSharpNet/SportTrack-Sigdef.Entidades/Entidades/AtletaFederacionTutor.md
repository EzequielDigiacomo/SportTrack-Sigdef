# AtletaFederacionTutor.cs

## Qué es este archivo

Tabla intermedia **atleta↔tutor** con clave compuesta y el **parentesco** entre ambos. Evita ciclos JSON con `[JsonIgnore]` en las navegaciones.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Clave compuesta | Dos `[Key]` con `[Column(Order)]`. |
| `[JsonIgnore]` | No serializar navegaciones (anti-ciclos). |
| Enum `Parentesco` | Padre, Madre, TutorLegal… |
| `[ForeignKey(nameof(...))]` | FK tipada con nameof (refactor-safe). |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- Enums, DataAnnotations, Schema, `System.Text.Json.Serialization`.

## Miembros

| Propiedad | Atributos | Tipo | Negocio |
|-----------|-----------|------|---------|
| `IdAtleta` | `[Key, Column(Order=0)]` | `int` | Parte PK → atleta. |
| `IdTutor` | `[Key, Column(Order=1)]` | `int` | Parte PK → tutor. |
| `AtletaFederacion` | `[JsonIgnore]`, `[ForeignKey]` | `AtletaFederacion` | Navegación. |
| `TutorFederacion` | `[JsonIgnore]`, `[ForeignKey]` | `TutorFederacion` | Navegación. |
| `Parentesco` | — | `Parentesco` | Vínculo familiar/legal. |

## Relaciones

N→1 `AtletaFederacion` y `TutorFederacion`. Es el “payload” de la relación muchos a muchos.

## Notas de estudio

1. `nameof(IdAtleta)` evita strings mágicos rotos al renombrar.
2. Clave compuesta = un tutor no se duplica para el mismo atleta (salvo que cambies el modelo).
3. `[JsonIgnore]` en ambos lados corta el grafo atleta→tutor→atletas→…
