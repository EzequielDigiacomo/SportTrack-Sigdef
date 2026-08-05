# Pago.cs

## Qué es este archivo

Registro genérico de **pago** (afiliación de club/atleta o inscripción a evento): monto, referencia, vínculos opcionales polimórficos vía FKs nullables y metadatos de registro.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Discriminador textual | `TipoPago` como `string`. |
| FKs opcionales mutuamente | Solo una de Club/Participante/Inscripcion suele aplicar. |
| `decimal` | Importe. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `Id` | `int` | PK. |
| `TipoPago` | `string` | `ClubAfiliacion`, `AtletaAfiliacion`, `InscripcionEvento`. |
| `ClubId` / `Club` | `int?` / `Club?` | Si aplica. |
| `ParticipanteId` / `Participante` | `int?` / `Participante?` | Si aplica. |
| `InscripcionId` / `Inscripcion` | `int?` / `Inscripcion?` | Si aplica. |
| `Monto` | `decimal` | Importe. |
| `FechaPago` | `DateTime` | Default UTC. |
| `Referencia` | `string?` | Transferencia/recibo. |
| `RegistradoPor` | `string?` | Admin. |
| `Notas` | `string?` | Extra. |

## Relaciones

N→1 opcionales con `Club`, `Participante`, `Inscripcion`. Distinto de `PagoFederacionTransaccion` (flujo Mercado Pago / federación).

## Notas de estudio

1. Este diseño es un **polimorfismo pobre** con FKs nullables; funciona pero la validación de “exactamente un target” debe vivir en servicio.
2. Compará con `PagoFederacionTransaccion`: más atributos EF y estado tipado.
