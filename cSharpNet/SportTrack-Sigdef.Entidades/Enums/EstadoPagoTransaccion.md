# EstadoPagoTransaccion.cs

## Qué es este archivo

Estados de una **transacción de pago** federativa (pasarela).

## Conceptos C# que aparecen

`enum` implícito 0…n sin Display.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums`.

## Miembros

`Pendiente`, `Aprobado`, `Rechazado`, `Cancelado`, `Fallido`.

## Relaciones

`PagoFederacionTransaccion.Estado`.

## Notas de estudio

Distinguí `Rechazado` (negocio/pasarela negó) de `Fallido` (error técnico) y `Cancelado` (usuario/admin).
