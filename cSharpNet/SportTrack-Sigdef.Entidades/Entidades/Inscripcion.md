# Inscripcion.cs

## Qué es este archivo

Una **inscripción** a una prueba concreta de un evento (`EventoPrueba`): número de competidor, estado, pago, participante principal, tripulantes y historial de resultados.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Enum de estado | `EstadoInscripcionEnum`. |
| Colecciones | `Tripulantes`, `Resultados`. |
| Participante nullable | Útil en tripulaciones donde el “dueño” puede modelarse distinto. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- `SportTrack_Sigdef.Entidades.Enums` y también `Enums.EstadoInscripcionEnum` calificado en la propiedad.

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `IdInscripcion` | `int` | PK. |
| `IdEventoPrueba` / `EventoPrueba` | `int` / `EventoPrueba` | Prueba del evento. |
| `IdParticipante` / `Participante` | `int?` / `Participante?` | Titular (K1 u organizador del bote). |
| `FechaInscripcion` | `DateTime` | Alta. |
| `NumeroCompetidor` | `string` | Dorsal/número. |
| `EsCabezaDeSerie` | `bool` | Seeding a nivel inscripción. |
| `Estado` | `EstadoInscripcionEnum` | Default `Inscrito`. |
| `Pagado` | `bool` | Flag de pago. |
| `Tripulantes` | colección | Miembros del bote. |
| `Resultados` | colección | Historial por fases. |

## Relaciones

N→1 `EventoPrueba` y opcionalmente `Participante`; 1→N `InscripcionTripulante` y `Resultado`.

## Notas de estudio

1. Jerarquía: `Evento` → `EventoPrueba` → `Inscripcion` → `Resultado`.
2. `Pagado` aquí es un bool simple; puede coexistir con entidad `Pago`.
3. Para K4, mirá siempre `Tripulantes`, no solo `IdParticipante`.
