# ReglaProgresion.cs

## Qué es este archivo

Define **cómo clasifican** los competidores de una etapa origen a una etapa destino dentro de un `EventoPrueba` (por posiciones o por mejores tiempos).

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Rango inclusivo | `PosicionDesde`–`PosicionHasta`. |
| Flag de modo | `PorTiempo`. |
| Dos navegaciones al mismo tipo | `EtapaOrigen` y `EtapaDestino`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `Id` | `int` | PK. |
| `EventoPruebaId` / `EventoPrueba` | `int` / `EventoPrueba` | Contexto. |
| `EtapaOrigenId` / `EtapaOrigen` | `int` / `Etapa` | Desde. |
| `EtapaDestinoId` / `EtapaDestino` | `int` / `Etapa` | Hacia. |
| `PosicionDesde` / `PosicionHasta` | `int` | Ej. puestos 1–2 pasan. |
| `PorTiempo` | `bool` | Si true, mejores tiempos globales. |
| `CantidadATomar` | `int?` | Cupo de clasificados por tiempo/posición especial. |

## Relaciones

N→1 `EventoPrueba`; N→1 `Etapa` (×2). En `Etapa` existen colecciones `ReglasComoOrigen` / `ReglasComoDestino`.

## Notas de estudio

1. Es la configuración del **motor de brackets/heats**.
2. Dos FKs a `Etapa` requieren Fluent API o atributos claros (como en traspasos con dos clubes).
3. Combiná lectura con `Resultado.ReglaClasificacionAplicada` (trazabilidad en runtime).
