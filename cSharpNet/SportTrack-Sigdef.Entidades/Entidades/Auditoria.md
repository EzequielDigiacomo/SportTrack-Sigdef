# Auditoria.cs

## Qué es este archivo

Registro de **auditoría de acciones** del sistema (login, altas, bajas…): quién, qué, cuándo, desde qué IP/módulo/user-agent. Mapeada explícitamente a la tabla `Auditoria`.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| `[Table("Auditoria")]` | Nombre de tabla fijo. |
| `[Key]`, `[Required]`, `[MaxLength]` | Metadatos. |
| Entidad append-only | Típicamente solo se inserta. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- DataAnnotations + Schema.

## Miembros

| Propiedad | Atributos | Tipo | Negocio |
|-----------|-----------|------|---------|
| `Id` | `[Key]` | `int` | PK. |
| `Fecha` | — | `DateTime` | Default `DateTime.Now`. |
| `Accion` | `[Required]`, `[MaxLength(100)]` | `string` | Código (LOGIN, CREATE_ATHLETE…). |
| `Detalle` | — | `string` | JSON o texto. |
| `Usuario` | — | `string` | Quién. |
| `IP` | `[MaxLength(50)]` | `string` | Origen. |
| `Modulo` | — | `string` | Auth, Atletas… |
| `UserAgent` | — | `string` | Cliente HTTP. |

## Relaciones

Ninguna navegación: es un log independiente (a veces se relaciona lógicamente por nombre de usuario).

## Notas de estudio

1. Auditoría no debería actualizarse ni borrarse en flujos normales.
2. `Detalle` como string flexible permite evolucionar el payload sin migrar esquema.
3. Considerá UTC y retención/GDPR al diseñar el volumen de logs.
