# CategoriaEdadEnum.cs

## Qué es este archivo

Categorías etarias “oficiales” con Display que incluye rangos de edad (Pre-infantil … Master C).

## Conceptos C# que aparecen

`enum` + `[Display]` descriptivo; valores 1–10.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums` + DataAnnotations.

## Miembros

| Valor | Int | Display (resumen) |
|-------|-----|-------------------|
| `Preinfantil` | 1 | 8–10 |
| `Infantil` | 2 | 11–12 |
| `Menor` | 3 | 13–14 |
| `Cadete` | 4 | 14–15 |
| `Junior` | 5 | 16–17 |
| `Sub23` | 6 | 18–22 |
| `Senior` | 7 | 18–35 |
| `MasterA` | 8 | 40–45 |
| `MasterB` | 9 | 46–50 |
| `MasterC` | 10 | 50+ |

## Relaciones

Paralelo a tabla `Categoria` y enum `CategoriaEdad` (usado en `AtletaFederacion`).

## Notas de estudio

Los rangos del Display pueden solaparse (Cadete/Menor); la regla fina vive en servicios/`Evento` flags (`PermitirSub23EnSenior`, etc.).
