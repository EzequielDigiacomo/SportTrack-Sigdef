# EstadoPago.cs

## Qué es este archivo

Estado de **pago/matrícula** a nivel dominio (club o atleta federado): pendiente, pagado, vencido, parcial.

## Conceptos C# que aparecen

`enum` implícito sin Display.

## Namespace y usings

`SportTrack_Sigdef.Entidades.Enums`.

## Miembros

`Pendiente`, `Pagado`, `Vencido`, `Parcial`.

## Relaciones

`Club.EstadoMatricula`, `AtletaFederacion.EstadoPago`.

## Notas de estudio

No confundir con `EstadoPagoTransaccion` (una operación concreta vs estado agregado de afiliación).
