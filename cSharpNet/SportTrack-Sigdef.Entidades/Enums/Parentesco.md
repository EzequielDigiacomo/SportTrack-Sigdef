# Parentesco.cs

## Qué es este archivo

Parentesco entre tutor y atleta (Padre, Madre, TutorLegal, etc.).

## Conceptos C# que aparecen

`enum` **sin valores explícitos**: C# asigna 0, 1, 2… en orden de declaración.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums`.

## Miembros

`Padre`, `Madre`, `TutorLegal`, `Hermano`, `Abuelo`, `Abuela`, `Otro` (0…6 implícitos).

## Relaciones

`AtletaFederacionTutor.Parentesco`.

## Notas de estudio

1. Insertar un valor **en el medio** cambia los números siguientes y puede romper datos ya guardados → preferí valores explícitos en enums persistidos.
2. `TutorLegal` cubre tutores no parentales.
