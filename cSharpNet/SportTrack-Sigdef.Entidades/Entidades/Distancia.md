# Distancia.cs

## Qué es este archivo

Catálogo de **distancias de regata**, respaldado por el enum `DistanciaRegataEnum`, con gap sugerido entre pruebas y propiedades calculadas (`Metros`, `Descripcion`).

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Enum como valor de dominio | `DistanciaRegata`. |
| Propiedades calculadas | `Metros`, `Descripcion`. |
| Cast a `int` | `(int)DistanciaRegata` — ¡ojo: el valor del enum no son metros reales! |
| Extension method | `GetDisplayName()`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- `SportTrack_Sigdef.Entidades.Enums`.

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `Id` | `int` | PK. |
| `DistanciaRegata` | `DistanciaRegataEnum` | Tipo de distancia. |
| `GapSugerido` | `int` | Minutos (default 10). |
| `Metros` | `int` calc. | Cast del enum (ordinal, no metros físicos). |
| `Descripcion` | `string` calc. | Texto del `[Display]`. |
| `Pruebas` | colección | Pruebas que usan esta distancia. |

## Relaciones

1→N `Prueba`.

## Notas de estudio

1. En `DistanciaRegataEnum`, `Metros500 = 5` — el **5 no es 500 metros**; es el id del valor. Por eso `Metros => (int)DistanciaRegata` es engañoso como “metros reales”.
2. Para metros reales habría que mapear explícitamente (switch/diccionario) o poner el metro como valor del enum (`Metros500 = 500`).
3. `GetDisplayName()` muestra cómo reflexión + atributos enriquecen enums.
