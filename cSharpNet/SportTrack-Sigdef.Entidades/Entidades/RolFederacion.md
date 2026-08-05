# RolFederacion.cs

## Qué es este archivo

Catálogo de **roles federativos** (administrador, delegado, entrenador, etc.) guardados como string, con propiedades `[NotMapped]` que exponen el nombre y el intento de parseo a `RolTipo`.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| `[Key]`, `[Required]`, `[MaxLength]` | Metadatos. |
| `[NotMapped]` | No persiste en BD. |
| `Enum.TryParse` | Conversión segura string→enum. |
| Propiedad calculada con bloque | `TipoEnum` con get multi-línea. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- Enums + DataAnnotations + Schema.

## Miembros

| Propiedad | Atributos | Tipo | Significado |
|-----------|-----------|------|-------------|
| `IdRol` | `[Key]` | `int` | PK. |
| `Tipo` | `[Required, MaxLength(50)]` | `string` | Código/nombre del rol. |
| `TipoNombre` | `[NotMapped]` | `string` | Alias de `Tipo`. |
| `TipoEnum` | `[NotMapped]` | `RolTipo?` | Parseo a enum o null. |
| `DelegadosClub` | — | colección | Usuarios/delegados con este rol. |

## Relaciones

1→N `DelegadoFederacionClub`.

## Notas de estudio

1. Persistir string y parsear a enum da flexibilidad (roles nuevos sin migrar enum) a costa de typos.
2. `TryParse` evita excepciones si el string no coincide con `RolTipo`.
3. `[NotMapped]` es ideal para helpers de UI/API sobre datos ya cargados.
