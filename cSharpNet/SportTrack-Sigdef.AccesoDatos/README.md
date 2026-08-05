# SportTrack-Sigdef.AccesoDatos — Persistencia (EF Core)

Proyecto que concentra el acceso a base de datos mediante **Entity Framework Core**.

## Pieza central

- `SportTrackDbContext.md` — `DbSet<>` de todas las entidades + `OnModelCreating` (Fluent API)

## Qué aprender aquí

| Concepto | Dónde verlo |
|----------|-------------|
| `DbContext` / `DbSet<T>` | `SportTrackDbContext` |
| Fluent API | `OnModelCreating` (schemas `federacion`, claves compuestas, deletes) |
| Value converters | DateTime → UTC para PostgreSQL |
| Migraciones | carpeta `Migrations/` del proyecto fuente (historial de esquema) |

## Relación con el resto

```
Entidades  →  AccesoDatos (DbContext)  →  usado por Services / Program.cs
```

En `Program.cs` se registra:

```csharp
builder.Services.AddDbContext<SportTrackDbContext>(options =>
    options.UseNpgsql(connectionString, ...));
```

Continúa en: [`../Fundamentos/03-ef-core-y-dbcontext.md`](../Fundamentos/03-ef-core-y-dbcontext.md)
