# EnumExtensions.cs

## Qué es este archivo

Clase estática de **métodos de extensión** para cualquier `Enum`: obtener el nombre de `[Display]` y el valor entero.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| `static class` | Solo miembros estáticos. |
| Extension method | `this Enum enumValue` — se llama como instancia. |
| Reflexión | `GetType().GetMember`, `GetCustomAttribute`. |
| `DisplayAttribute` | Fuente del texto amigable. |
| `Convert.ToInt32` | Valor subyacente. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Enums`
- `System.Reflection`, `System.ComponentModel.DataAnnotations`, Linq.

## Miembros

### `GetDisplayName(this Enum enumValue)`
1. Obtiene el `MemberInfo` del valor.
2. Busca `[Display]`.
3. Devuelve `Name` o, si no hay, `ToString()`.

### `GetValue(this Enum enumValue)`
Devuelve el entero del enum (`Convert.ToInt32`).

## Relaciones

Usado p. ej. por `Distancia.Descripcion => DistanciaRegata.GetDisplayName()`.

## Notas de estudio

1. Las extensions deben estar “en scope” (mismo namespace o `using`).
2. Reflexión tiene costo: en hot paths se puede cachear.
3. Si `Display.Name` es null, el código usa `?? enumValue.ToString()`.
4. `GetValue` asume underlying type convertible a int (válido aquí).
