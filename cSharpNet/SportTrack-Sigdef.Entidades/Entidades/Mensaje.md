# Mensaje.cs

## Qué es este archivo

Mensaje individual dentro de un hilo: remitente, destinatario, cuerpo, marcas de lectura y soft-delete por cada lado de la conversación.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Soft delete bilateral | Flags `EliminadoPorRemitente` / `EliminadoPorDestinatario`. |
| `DateTime? LeidoEn` | Null = no leído. |
| Dos FKs al mismo tipo | `Remitente` y `Destinatario` son ambos `Usuario`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `IdMensaje` | `int` | PK. |
| `HiloId` / `Hilo` | `int` / `Hilo?` | Contenedor. |
| `RemitenteId` / `Remitente` | `int` / `Usuario?` | Emisor. |
| `DestinatarioId` / `Destinatario` | `int` / `Usuario?` | Receptor. |
| `Cuerpo` | `string` | Texto. |
| `EnviadoEn` | `DateTime` | Envío. |
| `LeidoEn` | `DateTime?` | Lectura. |
| `EliminadoPorRemitente` | `bool` | Oculto para emisor. |
| `EliminadoPorDestinatario` | `bool` | Oculto para receptor. |

## Relaciones

N→1 `Hilo`; N→1 `Usuario` (×2).

## Notas de estudio

1. Soft-delete por lado imita WhatsApp/email: borrar “para mí” no borra la fila.
2. Para contar no leídos: `LeidoEn == null && DestinatarioId == yo`.
3. Configurá en Fluent API los dos roles de `Usuario` para evitar ambigüedad de FK.
