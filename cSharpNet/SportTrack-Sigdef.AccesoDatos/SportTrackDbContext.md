# SportTrackDbContext.cs

## 1. Qué es

El **DbContext** central de SportTrack/SIGDEF. Hereda de `Microsoft.EntityFrameworkCore.DbContext` y concentra:

- Todos los `DbSet<T>` (tablas).
- La configuración Fluent API en `OnModelCreating`.
- Seed data de catálogos, planes SaaS y usuario admin.
- Value converters para enums y DateTime UTC.
- Un override de `SaveChangesAsync` para auditar fechas en `Resultado`.

Es el corazón de la capa `AccesoDatos`.

## 2. Conceptos C#/.NET / EF Core

| Concepto | Dónde aparece |
|----------|----------------|
| Herencia | `SportTrackDbContext : DbContext` |
| Constructor + DI | `DbContextOptions<SportTrackDbContext>` |
| `DbSet<T>` | Propiedades públicas por entidad |
| `OnModelCreating` | Configuración del modelo |
| Fluent API | `modelBuilder.Entity<T>(...)` |
| `ValueConverter` | DateTime UTC y enums → string |
| Clases anidadas | Converters de estado |
| `override` / `async` | `SaveChangesAsync` |
| LINQ sobre ChangeTracker | Filtrar entidades `Modified` |
| Seed | `HasData(...)` |
| Esquemas PostgreSQL | `ToTable(name, schema)` |

## 3. Namespace / usings

**Namespace:** `SportTrack_Sigdef.AccesoDatos`

**Usings principales:**

- `Microsoft.EntityFrameworkCore` — DbContext, ModelBuilder, DeleteBehavior, etc.
- `Microsoft.EntityFrameworkCore.Storage.ValueConversion` — ValueConverter.
- `SportTrack_Sigdef.Entidades.Entidades` — clases de dominio.
- `SportTrack_Sigdef.Entidades.Enums` — enums de estado/distancia.
- `System.Collections.Generic`, `System.Reflection.Emit` (este último no se usa de forma significativa; típico residuo).

## 4. Detalle por secciones

### 4.1 Constructor

```csharp
public SportTrackDbContext(DbContextOptions<SportTrackDbContext> options) : base(options)
```

Recibe opciones (connection string, proveedor Npgsql) desde `AddDbContext` en la API. Patrón estándar: **no** hardcodear la connection string en el contexto.

### 4.2 DbSets (tablas)

Agrupados por dominio:

**Maestras / catálogos:** `Sexos`, `Botes`, `Categorias`, `Distancias`, `Clubes`, `Usuarios`, `PlanesSaaS`.

**Federación (SIGDEF):** `Federaciones`, `DelegadosClub`, `Entrenadores`, `Tutores`, `AtletasFederados`, `AtletasTutores`, `Roles`, `DocumentacionPersonas`, `PagosTransacciones`, `PeriodosTraspaso`, `SolicitudesTraspaso`.

**Regatas:** `Eventos`, `Pruebas`, `EventoPruebas`, `Participantes`, `Inscripciones`, `InscripcionTripulantes`, `Etapas`, `Fases`, `ReglasProgresion`, `Resultados`, `Penalizaciones`, `Auditoria`, `Pagos`.

**Mensajería:** `Hilos`, `Mensajes`, `CampanasEnvio`.

Cada `DbSet` es el punto de entrada LINQ para esa entidad.

### 4.3 OnModelCreating — DateTime UTC global

Recorre todas las propiedades `DateTime` / `DateTime?` y aplica un `ValueConverter` que fuerza `DateTimeKind.Utc` al leer y escribir. Evita errores típicos de PostgreSQL con kind `Unspecified`/`Local`.

### 4.4 Fluent API — Federaciones y traspasos

Ejemplos clave:

- `Federacion` → tabla `Federaciones` en esquema `federacion`; FK opcional a `PlanSaaS` con `SetNull` al borrar plan.
- `AtletaFederacionTutor` → clave **compuesta** `{ IdAtleta, IdTutor }`.
- `PeriodoTraspaso` → índice compuesto por federación/activo/fechas.
- `SolicitudTraspaso` → enum `Estado` como string vía converter; FKs a federación, participante, club origen/destino; `Cascade` vs `Restrict` según criticidad.

### 4.5 Catálogos (Sexos, Botes, Categorias, Distancias, Clubes, PlanSaaS, Usuario)

Patrones repetidos:

- `ToTable(..., "catalogos")` o `"seguridad"` para usuarios.
- `HasKey`, `IsRequired`, `HasMaxLength`.
- Índices únicos (`Nombre`, `Tipo`, `Username`, `Email`).
- `Distancia`: enum `DistanciaRegata` como `int`; `Ignore` de `Metros` y `Descripcion` (calculados).
- `Club` / `Usuario`: relaciones a federación y plan.

### 4.6 Tablas de regatas

Destacan:

| Entidad | Detalles didácticos |
|---------|---------------------|
| `Evento` | Estado enum→string; default `Programada`; default SQL `NOW()` en creación; FKs club/federación. |
| `Prueba` | FKs a Bote/Categoria/Distancia/Sexo; índice único compuesto de combinación de prueba. |
| `EventoPrueba` | Cascade desde Evento; índice único Evento+Prueba+FechaHora (varias series). |
| `Participante` | `date` en nacimiento; `Ignore(Edad)`; índices únicos filtrados en Email y Documento (PostgreSQL). |
| `Inscripcion` | Estado enum→string; tripulación vía `InscripcionTripulante`. |
| `Fase` / `Etapa` / `ReglaProgresion` | Jerarquía de competencia y promoción. |
| `Resultado` | `interval` para tiempo; precisión decimal; estado enum. |
| `Penalizacion` | Enums como string genérico `HasConversion<string>()`. |
| `Pago` | Montos con `HasPrecision(18,2)`; FKs opcionales. |

### 4.7 Mensajería (`comunicacion`)

- `Hilo`, `Mensaje`, `CampanaEnvio` con `SistemaOrigen` (SportTrack vs SIGDEF).
- Cascadas: borrar hilo borra mensajes; campaña con `SetNull` en hilos.

### 4.8 Seed data (`HasData`)

Inserta en migraciones:

- 3 sexos, 6 tipos de bote, 11 categorías etarias, 16 distancias.
- 9 planes SaaS (SIGDEF / SportTrack / Pack Dúo × S/M/L).
- Usuario `admin` (hash BCrypt; comentario indica password de ejemplo).

**Nota de seguridad:** el hash en seed es para bootstrap; en producción rotá credenciales.

### 4.9 Clases anidadas (converters)

```csharp
public class EstadoEventoEnumConverter : ValueConverter<EstadoEventoEnum, string>
```

Convierte con `ToString()` / `Enum.Parse`. Igual patrón para inscripción, resultado y solicitud de traspaso.

### 4.10 SaveChangesAsync

Antes de guardar:

1. Obtiene entradas del `ChangeTracker` donde la entidad es `Resultado` y el estado es `Modified`.
2. Asigna `FechaActualizacion = DateTime.UtcNow`.
3. Llama a `base.SaveChangesAsync`.

## 5. Notas de estudio

1. Leé primero la lista de `DbSet`: es el vocabulario del dominio.
2. Compará `DeleteBehavior.Cascade` (Evento→EventoPrueba) vs `Restrict` (Participante en inscripción): ¿qué pasa si borrás el padre?
3. Los índices con `HasFilter` son específicos de PostgreSQL; en SQL Server la sintaxis de filtro difiere.
4. `HasSentinel` + `HasDefaultValue` en enums evita que EF confunda el valor default del enum CLR con “no seteado”.
5. Relacioná este archivo con las migraciones y con `AddDbContext` en `Program.cs`.
6. Guía conceptual: [03-ef-core-y-dbcontext.md](../Fundamentos/03-ef-core-y-dbcontext.md).
