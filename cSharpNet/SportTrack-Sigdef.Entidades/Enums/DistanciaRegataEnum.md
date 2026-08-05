# DistanciaRegataEnum.cs

## Qué es este archivo

Enum principal de **distancias de regata** con Display en metros (200m…30000m). Los valores enteros son **ids ordinales (1..16), no los metros**.

## Conceptos C# que aparecen

`enum` + `[Display]`; comentario visual en el fuente; using `System.Xml.Linq` no usado (ruido de template).

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums` + DataAnnotations.

## Miembros (resumen)

| Valor | Int | Display |
|-------|-----|---------|
| `Metros200` … `Metros500` | 1–5 | 200m…500m |
| `Metros1000` … `Metros5000` | 6–10 | 1000m…5000m |
| `Metros10000` … `Metros30000` | 11–16 | 10000m…30000m |

## Relaciones

`Distancia.DistanciaRegata`.

## Notas de estudio

1. **No** hagas `(int)Metros500` esperando 500 — obtendrás `5`.
2. Para metros reales: parsear el Display, usar un diccionario, o redefinir valores (`Metros500 = 500`) sabiendo el impacto en BD.
3. Convive con `DistanciaRegata` (otro enum, otro conjunto de distancias).
