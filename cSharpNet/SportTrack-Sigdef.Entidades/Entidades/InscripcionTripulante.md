# InscripcionTripulante.cs

## Qué es este archivo

Tabla de unión entre una **inscripción** (bote) y un **participante** (tripulante), con posición opcional en el bote (proa/popa en K2/K4, etc.).

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Entidad de join | Relación N:N Inscripcion↔Participante con datos extra. |
| XML docs | Explica `PosicionEnBote`. |
| `null!` | Navegaciones obligatorias. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `Id` | `int` | PK. |
| `InscripcionId` / `Inscripcion` | `int` / `Inscripcion` | Bote/inscripción. |
| `ParticipanteId` / `Participante` | `int` / `Participante` | Remero. |
| `PosicionEnBote` | `int?` | 1=proa … N=popa. |

## Relaciones

N→1 `Inscripcion` y `Participante`. En K1 a veces basta `Inscripcion.IdParticipante`; en K2/K4 esta tabla es esencial.

## Notas de estudio

1. Cuando una relación N:N necesita **atributos propios** (posición), no alcanza con EF many-to-many implícita: necesitás entidad intermedia.
2. Leé el comentario XML: el dominio de canotaje queda claro sin mirar servicios.
