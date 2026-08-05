# Fase.cs

## Qué es este archivo

Una **fase** concreta dentro de una etapa (ej. “Serie 1”, “Final A”): número, horarios programados/reales, estado textual y resultados.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Estado como `string` | `"Programada"` — menos tipado que el enum de evento. |
| Fechas nullable | Inicio/fin reales opcionales. |
| Colección | `Resultados`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `Id` | `int` | PK. |
| `EtapaId` / `Etapa` | `int` / `Etapa` | Padre. |
| `NombreFase` | `string` | Etiqueta. |
| `NumeroFase` | `int` | Orden/número (default 1). |
| `FechaHoraProgramada` | `DateTime?` | Plan. |
| `Estado` | `string` | Default `"Programada"`. |
| `FechaHoraInicioReal` / `FechaHoraFinReal` | `DateTime?` | Ejecución. |
| `Resultados` | colección | Tiempos/posiciones. |

## Relaciones

N→1 `Etapa`; 1→N `Resultado`. Cadena: `EventoPrueba` → `Etapa` → `Fase` → `Resultado`.

## Notas de estudio

1. Ideal candidato a usar `EstadoEventoEnum` u otro enum en lugar de string.
2. Diferenciar programado vs real permite medir delays del cronograma.
