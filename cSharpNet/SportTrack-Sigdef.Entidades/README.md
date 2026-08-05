# SportTrack-Sigdef.Entidades — Capa de dominio

Este proyecto contiene el **modelo de dominio**: las clases que representan el negocio (eventos, clubes, atletas, inscripciones, etc.) y los **enums** (estados, tipos, sexos, etc.).

## Por qué existe esta capa

Separar entidades en su propio proyecto permite que:

- `AccesoDatos` las mapee a tablas
- `Controladores` las use en servicios
- La API no dependa de detalles de UI

Dependencias típicas: **casi ninguna** (solo BCL / DataAnnotations). Así el dominio queda “limpio”.

## Estructura documentada

| Carpeta | Contenido |
|---------|-----------|
| `Entidades/` | Clases de dominio (una ≈ una tabla o agregación) |
| `Enums/` | Valores fijos tipados |
| `DTOs/Traspaso/` | DTOs de traspaso compartidos a nivel entidad |

## Orden de lectura sugerido

1. `Enums/EstadoEventoEnum.md` — qué es un enum
2. `Entidades/Club.md` — entidad simple
3. `Entidades/Federacion.md` — relaciones
4. `Entidades/Evento.md` — entidad rica (reglas + cronograma)
5. `Entidades/Inscripcion.md` — relación muchos-a-uno / tripulantes
6. `Entidades/Usuario.md` — autenticación / roles

## Conceptos C# clave en esta capa

- `class`, propiedades `{ get; set; }`
- Nullable (`string?`, `int?`)
- `enum`
- Data Annotations (`[Key]`, `[Required]`, `[NotMapped]`)
- Navigation properties (`Club?`, `ICollection<T>`)

Continúa en: [`../Fundamentos/01-entidades-y-clases.md`](../Fundamentos/01-entidades-y-clases.md)
