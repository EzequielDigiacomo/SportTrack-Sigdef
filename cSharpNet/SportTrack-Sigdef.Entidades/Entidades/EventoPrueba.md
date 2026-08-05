# EventoPrueba.cs

## Qué es este archivo

Une un **Evento** con una **Prueba** del catálogo: horario, cupo, pista, estado, precio de categoría, plan de progresión, inscripciones, etapas y reglas de avance.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Tabla puente con datos | No es solo dos FKs: tiene horario, precio, etc. |
| Enum | `EstadoEventoEnum`. |
| Varias colecciones | Inscripciones, Etapas, ReglasProgresion. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- `SportTrack_Sigdef.Entidades.Enums`.

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `IdEventoPrueba` | `int` | PK. |
| `IdEvento` / `Evento` | `int` / `Evento` | Evento padre. |
| `IdPrueba` / `Prueba` | `int` / `Prueba` | Definición (bote+cat+dist+sexo). |
| `FechaHora` | `DateTime` | Programación. |
| `MaxParticipantes` | `int` | Cupo (0 puede significar sin tope según lógica app). |
| `Pista` | `string?` | Ubicación física. |
| `Estado` | `EstadoEventoEnum` | Default `Programada`. |
| `PlanProgresionAsignado` | `string?` | Nombre/clave del plan. |
| `PrecioCategoria` | `decimal?` | Precio específico. |
| `Inscripciones` | colección | Inscriptos. |
| `Etapas` | colección | Eliminatoria/semi/final. |
| `ReglasProgresion` | colección | Cómo se avanza entre etapas. |

## Relaciones

N→1 `Evento`, `Prueba`; 1→N `Inscripcion`, `Etapa`, `ReglaProgresion`.

## Notas de estudio

1. Separar `Prueba` (plantilla) de `EventoPrueba` (instancia en un evento) es modelado clásico de competencias.
2. El motor de cronograma trabaja sobre esta entidad y sus etapas/fases.
