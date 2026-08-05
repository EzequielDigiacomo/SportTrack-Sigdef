# 03 — EF Core y DbContext

Basado en `SportTrack-Sigdef.AccesoDatos`.

## Qué es Entity Framework Core

ORM (Object-Relational Mapper): traduce objetos C# ↔ tablas SQL.

Tú escribes:

```csharp
var eventos = await _db.Eventos.Where(e => e.EstaActivo).ToListAsync();
```

EF genera SQL y materializa filas en objetos `Evento`.

## `DbContext`

`SportTrackDbContext` es la puerta de entrada a la BD.

```csharp
public class SportTrackDbContext : DbContext
{
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Club> Clubes { get; set; }
    // ...
}
```

| Pieza | Significado |
|-------|-------------|
| `DbContext` | Sesión de trabajo con la BD |
| `DbSet<T>` | Colección consultable de la entidad `T` (≈ tabla) |
| `OnModelCreating` | Configuración Fluent API (relaciones, índices, constraints) |

## Ciclo típico

```csharp
_db.Eventos.Add(nuevo);
await _db.SaveChangesAsync(); // aquí se ejecuta el SQL
```

1. Cambios en memoria (Change Tracker)
2. `SaveChangesAsync()` → INSERT/UPDATE/DELETE

## Consultas útiles

```csharp
// Traer relacionados
await _db.Eventos.Include(e => e.Federacion).FirstOrDefaultAsync(e => e.IdEvento == id);

// Proyección a DTO (más eficiente)
await _db.Eventos.Select(e => new EventoDto { Id = e.IdEvento, Nombre = e.Nombre }).ToListAsync();
```

## Migraciones

Las carpetas `Migrations/` (no documentadas una por una aquí) son el historial de cambios del esquema:

```text
Add-Migration Nombre
Update-Database
```

Generan/aplican SQL para alinear BD ↔ modelo.

## PostgreSQL en este proyecto

En `Program.cs` se registra:

```csharp
builder.Services.AddDbContext<SportTrackDbContext>(options =>
    options.UseNpgsql(connectionString, ...));
```

`UseNpgsql` = proveedor EF para PostgreSQL.

## Qué estudiar ahora

1. `SportTrack-Sigdef.AccesoDatos/SportTrackDbContext.md`
2. Relación con entidades: `Entidades/Evento.md`
3. Cómo un Service usa el contexto (capa Controladores)
