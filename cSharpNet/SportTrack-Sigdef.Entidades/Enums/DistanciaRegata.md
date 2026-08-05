# DistanciaRegata.cs

## Qué es este archivo

Enum alternativo/legacy de distancias con nombres en español y un conjunto **distinto** al de `DistanciaRegataEnum` (faltan algunas, otras no coinciden).

## Conceptos C# que aparecen

`enum` con typos ortográficos en identificadores (`Trecientos`, `Quatro`).

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums`.

## Miembros

Desde `DoscientosMetros = 1` hasta `TreintaDosKilometros = 13` (lista incompleta vs el enum Display).

## Relaciones

Legacy; el código nuevo tiende a `DistanciaRegataEnum`.

## Notas de estudio

1. Los typos en nombres de enum son costosos de corregir si ya hay datos.
2. Antes de borrar este archivo, buscá referencias en toda la solución.
