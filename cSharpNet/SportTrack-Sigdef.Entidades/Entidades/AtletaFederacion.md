# AtletaFederacion.cs

## Qué es este archivo

Perfil de **atleta federado**: extensión de `Participante` con club, federación, estado de pago, selección, categoría etaria, becas, apto médico y vínculos a tutores.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| PK = `ParticipanteId` | 1:1 con persona base. |
| Enums | `EstadoPago`, `CategoriaEdad`. |
| `[JsonIgnore]` | Colecciones/navegaciones sensibles a ciclos. |
| Defaults | `FechaCreacion = UtcNow`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- Enums, DataAnnotations, Schema, Json.Serialization.

## Miembros

| Propiedad | Atributos / notas | Tipo | Negocio |
|-----------|-------------------|------|---------|
| `ParticipanteId` | `[Key]` | `int` | PK/FK. |
| `Participante` | `[ForeignKey]` | `Participante` | Persona. |
| `IdClub` / `Club` | `[ForeignKey]`, `[JsonIgnore]` en Club | `int?` / `Club?` | Club. |
| `IdFederacion` / `Federacion` | `[ForeignKey]` | `int?` / `Federacion?` | Federación. |
| `EstadoPago` | enum | `EstadoPago` | Matrícula/afiliación. |
| `PerteneceSeleccion` | — | `bool` | Selección. |
| `Categoria` | enum nullable | `CategoriaEdad?` | Categoría (ex CategoriaSeleccion). |
| `FechaCreacion` | — | `DateTime` | Alta. |
| `BecadoEnard` / `BecadoSdn` | — | `bool` | Becas. |
| `MontoBeca` | — | `decimal` | Importe (no nullable). |
| `PresentoAptoMedico` | — | `bool` | Flag. |
| `FechaAptoMedico` | — | `DateTime?` | Vigencia del apto. |
| `Inscripciones` | `[JsonIgnore]` | colección | (navegación auxiliar). |
| `Tutores` | `[JsonIgnore]` | colección | Join tutors. |

## Relaciones

1:1 `Participante`; N→1 `Club`/`Federacion`; 1→N `AtletaFederacionTutor`.

## Notas de estudio

1. Compará flags `bool` aquí vs `bool?` en `EntrenadorFederacion`: distinta semántica de “desconocido”.
2. Usa enum `CategoriaEdad` (no `CategoriaEdadEnum`) — ejemplo de dualidad histórica.
3. `[JsonIgnore]` en `Club` evita serializar el grafo completo del club al devolver un atleta.
