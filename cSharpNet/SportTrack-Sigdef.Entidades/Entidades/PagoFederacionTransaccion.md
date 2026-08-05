# PagoFederacionTransaccion.cs

## Qué es este archivo

Transacción de **pago federativo** (típicamente Mercado Pago): concepto, monto, estado, fechas, participante, club e id externo de la pasarela.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| `[Key]`, `[Required]`, `[MaxLength]`, `[ForeignKey]` | Anotaciones EF/validación. |
| Enum `EstadoPagoTransaccion` | Ciclo de la transacción. |
| `virtual` | Navegaciones. |
| `DateTime.Now` vs Utc | Aquí usa hora local del servidor. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- Enums + DataAnnotations + Schema.

## Miembros

| Propiedad | Atributos | Tipo | Negocio |
|-----------|-----------|------|---------|
| `IdPago` | `[Key]` | `int` | PK. |
| `Concepto` | `[Required, MaxLength(100)]` | `string` | Qué se paga. |
| `Monto` | — | `decimal` | Importe. |
| `Estado` | — | `EstadoPagoTransaccion` | Pendiente…Fallido. |
| `FechaCreacion` | — | `DateTime` | Default `DateTime.Now`. |
| `FechaAprobacion` | — | `DateTime?` | Cuando se aprobó. |
| `IdParticipante` / `Participante` | `[ForeignKey]` | `int` / `Participante` | Pagador/beneficiario. |
| `IdClub` / `Club` | `[ForeignKey]` | `int` / `Club` | Club asociado. |
| `IdMercadoPago` | `[MaxLength(100)]` | `string` | Id externo. |

## Relaciones

N→1 `Participante` y `Club`. Colección inversa en ambas entidades (`Pagos`).

## Notas de estudio

1. Preferí `UtcNow` de forma consistente en sistemas distribuidos; aquí hay `DateTime.Now`.
2. Guardar `IdMercadoPago` permite reconciliar webhooks con filas locales.
3. Diferenciá este modelo de `Pago` (más genérico / manual).
