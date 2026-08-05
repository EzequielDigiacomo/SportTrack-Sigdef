# Etapa.cs

## Qué es este archivo

Nivel de competencia dentro de un `EventoPrueba`: eliminatoria, semifinal, final o consuelo (`TipoEtapaEnum`), con orden y colecciones de fases y reglas de progresión (como origen o destino).

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Enum `TipoEtapaEnum` | Tipo de ronda. |
| Dos colecciones al mismo tipo | `ReglasComoOrigen` / `ReglasComoDestino`. |
| `Orden` | Secuencia en el bracket. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- Usa `Enums.TipoEtapaEnum` calificado.

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `Id` | `int` | PK. |
| `EventoPruebaId` / `EventoPrueba` | `int` / `EventoPrueba` | Contexto. |
| `Nombre` | `string` | Etiqueta amigable. |
| `Tipo` | `TipoEtapaEnum` | Clasificación. |
| `Orden` | `int` | Posición en el flujo. |
| `Fases` | colección | Series/finales concretas. |
| `ReglasComoOrigen` | colección | Reglas que salen de aquí. |
| `ReglasComoDestino` | colección | Reglas que llegan aquí. |

## Relaciones

N→1 `EventoPrueba`; 1→N `Fase` y `ReglaProgresion` (dos roles).

## Notas de estudio

1. Nombrar colecciones por rol (`ComoOrigen`/`ComoDestino`) aclara el mapeo EF de dos FKs.
2. `Orden` facilita ordenar UI y algoritmos sin depender del Id.
