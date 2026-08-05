# 01 — Entidades, clases, propiedades y enums

Basado en `SportTrack-Sigdef.Entidades`.

## Qué es una entidad

En este proyecto, una **entidad** es una clase C# que representa una tabla (o concepto de dominio) y se persiste con Entity Framework Core.

Ejemplo mental: `Evento` ≈ tabla `Eventos` en la base de datos.

## Anatomía de una clase

```csharp
namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class Evento
    {
        public int IdEvento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime? FechaFin { get; set; }
        public Federacion? Federacion { get; set; }
    }
}
```

| Pieza | Qué significa |
|-------|----------------|
| `namespace` | Agrupa tipos para evitar colisiones de nombres |
| `public class` | Tipo visible desde otros proyectos |
| `public int IdEvento { get; set; }` | Propiedad auto-implementada (campo + getter/setter) |
| `= string.Empty` | Valor inicial por defecto |
| `DateTime?` | Nullable: puede ser `null` |
| `Federacion?` | **Propiedad de navegación**: relación con otra entidad |

## Tipos de datos frecuentes

| Tipo C# | Uso típico |
|---------|------------|
| `int` | IDs, cupos, contadores |
| `string` / `string?` | Textos |
| `bool` | Flags (activo, habilitado) |
| `DateTime` / `DateTime?` | Fechas |
| `TimeSpan` | Horas del día / duraciones |
| `decimal` | Dinero / precios (evita errores de `float`) |
| `enum` | Estados y categorías fijas |

## Enums

Un `enum` es un conjunto cerrado de valores:

```csharp
public enum EstadoEventoEnum
{
    Programada,
    EnCurso,
    Finalizada,
    Cancelada
}
```

Ventajas frente a `string`:

- El compilador evita typos
- IntelliSense te sugiere valores
- Se puede mapear a `int` en BD

## Atributos (Data Annotations)

```csharp
[Key]
public int Id { get; set; }

[Required]
[MaxLength(100)]
public string Nombre { get; set; }

[NotMapped]
public ICollection<Inscripcion> Inscripciones { get; set; }
```

| Atributo | Efecto |
|----------|--------|
| `[Key]` | Clave primaria |
| `[Required]` | Obligatorio |
| `[MaxLength(n)]` | Límite de caracteres |
| `[ForeignKey]` | Relación FK |
| `[NotMapped]` | No se guarda en BD (solo en memoria / lógica) |

## Relaciones (navegación)

```csharp
public int? IdFederacion { get; set; }   // FK (columna)
public Federacion? Federacion { get; set; } // navegación (objeto)
```

- **Uno a muchos**: un `Evento` tiene muchas `Inscripcion`
- **Muchos a uno**: muchas entidades apuntan a un `Club` / `Federacion`

EF Core usa estas propiedades para armar `JOIN`s cuando haces `.Include(...)`.

## Qué estudiar ahora

1. Lee `Entidades/Evento.md` y abre `Evento.cs`
2. Identifica: PK, FKs, enums, nullables, `[NotMapped]`
3. Compara con `Enums/EstadoEventoEnum.md`
4. Sigue a `AccesoDatos` para ver cómo el `DbContext` registra estas entidades
