# DocumentacionFederacionPersona.cs

## Qué es este archivo

Almacena un **documento** (archivo) asociado a una persona del padrón: tipo, URL (p. ej. Cloudinary), public id, fecha de carga.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| `[Key]`, `[Required]`, `[MaxLength]`, `[ForeignKey]` | Metadatos. |
| FK por string | `[ForeignKey("PersonaId")]`. |
| `int?` para tipo | Comentario indica que debería ser enum. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- Solo DataAnnotations / Schema.

## Miembros

| Propiedad | Atributos | Tipo | Negocio |
|-----------|-----------|------|---------|
| `Id` | `[Key]` | `int` | PK. |
| `PersonaId` | — | `int?` | FK a participante. |
| `Participante` | `[ForeignKey("PersonaId")]` | `Participante` | Persona. |
| `TipoDocumento` | — | `int?` | Discriminador (DNI, pasaporte…). |
| `UrlArchivo` | `[Required]` | `string` | URL del archivo. |
| `PublicId` | `[MaxLength(100)]` | `string?` | Id Cloudinary. |
| `FechaCarga` | — | `DateTime` | Default UTC. |

## Relaciones

N→1 `Participante` (colección `Documentacion` en la persona).

## Notas de estudio

1. El binario **no** vive en la entidad: solo URL/metadata (buen patrón con storage externo).
2. `TipoDocumento` como `int?` es candidato claro a enum tipado.
3. `PersonaId` nullable con navegación `= null!` es una tensión de nullability a revisar.
