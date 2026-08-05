# Resultado.cs

## Qué es este archivo

Registra el **resultado de una inscripción en una fase** (serie/final): carril, tiempo, posición, puntos, estado (DNS/DNF/DSQ…), trazabilidad de progresión y auditoría de carga.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| `TimeSpan?` | Tiempo de carrera opcional. |
| Enum anidado por namespace | `Enums.EstadoResultadoEnum`. |
| Defaults | `EsCabezaDeSerie = false`, `Estado = Pendiente`. |
| Colección hija | `Penalizaciones`. |
| `null!` | Navegaciones requeridas. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- Usa `Enums.EstadoResultadoEnum` con nombre calificado (sin `using` del enum).

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `Id` | `int` | PK. |
| `FaseId` / `Fase` | `int` / `Fase` | Heat/serie. |
| `InscripcionId` / `Inscripcion` | `int` / `Inscripcion` | Quién compitió. |
| `Carril` | `int?` | Asignación de pista. |
| `EsCabezaDeSerie` | `bool` | Seeding. |
| `TiempoOficial` | `TimeSpan?` | Crono. |
| `Posicion` | `int?` | Lugar. |
| `Puntos` / `VelocidadMedia` | `decimal?` | Métricas. |
| `Estado` | `EstadoResultadoEnum` | Pendiente…DNF. |
| `Observaciones` | `string?` | Notas. |
| `FaseOrigenId` | `int?` | De qué fase clasificó. |
| `ReglaClasificacionAplicada` | `string?` | Trazabilidad. |
| `FechaRegistro` / `FechaActualizacion` | fechas | Auditoría. |
| `UsuarioRegistro` / `UsuarioActualizacion` | `string?` | Quién cargó. |
| `Penalizaciones` | colección | Sanciones ligadas. |

## Relaciones

N→1 `Fase`, `Inscripcion`; 1→N `Penalizacion`.

## Notas de estudio

1. Un mismo `Inscripcion` puede tener muchos `Resultado` (una por fase/heat).
2. DNS/DNF/DSQ viven en el **estado**, no solo en tiempo null — modelado limpio.
3. Guardar la regla aplicada ayuda a auditar el motor de progresión.
