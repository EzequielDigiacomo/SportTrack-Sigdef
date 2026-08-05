# CategoriaEdad.cs

## Qué es este archivo

Enum alternativo de categorías etarias (con comentarios de rangos de ejemplo). Usado por `AtletaFederacion.Categoria`.

## Conceptos C# que aparecen

`enum` con comentarios `//`; valores 1–9; incluye `Sub21` y no tiene Master B/C como el otro enum.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums`.

## Miembros

`Preinfantil`, `Infantil`, `Menores`, `Cadete`, `Junior`, `Sub21`, `Sub23`, `Senior`, `MasterA`.

## Relaciones

`AtletaFederacion.Categoria` (`CategoriaEdad?`).

## Notas de estudio

1. `Menores` vs `Menor` en el otro enum: nombres no intercambiables al parsear strings.
2. Unificar ambos enums sería un buen refactor de dominio a mediano plazo.
