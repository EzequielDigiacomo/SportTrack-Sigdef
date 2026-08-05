# PerfilTiempoEnum.cs

## Qué es este archivo

Perfiles de **timing** para el cronograma inteligente del evento (estándar, rápido, casos predefinidos, personalizado).

## Conceptos C# que aparecen

`enum` base 0; sin Display.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums`.

## Miembros

| Valor | Int | Uso típico |
|-------|-----|------------|
| `Estandar` | 0 | Default en `Evento`. |
| `Rapido` | 1 | Gaps más cortos. |
| `Caso1`…`Caso3` | 2–4 | Presets de negocio. |
| `Personalizado` | 5 | Usa gaps manuales del evento. |

## Relaciones

`Evento.PerfilTiempo`.

## Notas de estudio

Los “CasoN” deberían documentarse en la capa de servicios que interpreta cada perfil.
