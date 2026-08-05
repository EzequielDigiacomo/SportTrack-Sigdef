# Bote.cs

## Qué es este archivo

Catálogo de **tipos de bote** (K1, K2, C1…) como entidad con nombre textual. Las pruebas referencian un bote.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Entidad mínima | Id + Nombre + colección. |
| Dualidad enum/tabla | Existen `TipoBote` y `TipoBoteEnum`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `Id` | `int` | PK. |
| `Tipo` | `string` | Nombre del bote. |
| `Pruebas` | colección | Pruebas asociadas. |

## Relaciones

1→N `Prueba`.

## Notas de estudio

1. Propiedad llamada `Tipo` (string) vs clase `Bote`: el “tipo” es el dato de negocio.
2. Para cupos de tripulación (1/2/4), el número suele derivarse del código K1/K2/K4 en servicios.
