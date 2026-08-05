# Categoria.cs

## Qué es este archivo

Catálogo de **categoría etaria** (nombre + rango de edades opcional) usado por participantes y pruebas.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| `int?` para rangos | Sin mínimo/máximo = abierto. |
| Catálogo + colecciones | Patrón lookup table. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `Id` | `int` | PK. |
| `Nombre` | `string` | Ej. "Junior". |
| `EdadMin` / `EdadMax` | `int?` | Rango. |
| `Pruebas` | colección | Pruebas de la categoría. |
| `Participantes` | colección | Personas asignadas. |

## Relaciones

1→N `Prueba` y `Participante`. Convive con enums `CategoriaEdad` / `CategoriaEdadEnum`.

## Notas de estudio

1. Tener tabla **y** enums permite UI editable vs lógica tipada; mantené sincronizados los seeds.
2. Validar `EdadMin <= EdadMax` es responsabilidad de la capa de aplicación.
