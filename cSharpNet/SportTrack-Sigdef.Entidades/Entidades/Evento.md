# Evento.cs

## Qué es este archivo

Define la entidad **Evento**: una competencia o actividad deportiva (regata, campeonato, etc.) con fechas, cupos, reglas de inscripción, configuración de cronograma inteligente y pertenencia a club o federación.

## Conceptos C# que aparecen

| Concepto | Explicación breve |
|----------|-------------------|
| `class` | Tipo de referencia que agrupa datos y comportamiento del evento. |
| Auto-property | `public string Nombre { get; set; }` — propiedad con getter/setter. |
| Valor por defecto | `= string.Empty`, `= true`, `= DateTime.UtcNow`. |
| Nullable | `DateTime?`, `string?`, `Club?` — pueden ser null. |
| Enum como propiedad | `EstadoEventoEnum Estado` — estado tipado. |
| Propiedad con cuerpo | `FechaInicio` redirige get/set a `Fecha`. |
| `[NotMapped]` | Propiedad que EF Core **no** persiste como columna. |
| Navigation property | `Club`, `Federacion`, `EventoPruebas`. |
| `ICollection<T>` | Colección del lado “muchos”. |
| Método de instancia | `PuedeInscribirse()` — lógica de dominio. |
| `TimeSpan` | Duración/hora del día (inicio, receso). |
| XML doc (`///`) | Comentario de documentación en `GapRecuperacionMinutos`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- `using SportTrack_Sigdef.Entidades.Enums` — estados y perfiles de tiempo.
- `System.ComponentModel.DataAnnotations.Schema` — atributo `[NotMapped]`.
- Usings de `Linq`/`Tasks` vienen del template; aquí se usa Linq en el getter de `Inscripciones`.

## Miembros

### Propiedades de identidad y datos básicos

| Miembro | Tipo | Notas |
|---------|------|-------|
| `IdEvento` | `int` | Clave primaria (convención EF por nombre/`Id`). |
| `Nombre` | `string` | Nombre visible del evento. |
| `Fecha` | `DateTime` | Fecha principal. |
| `FechaInicio` | `DateTime` | Alias de `Fecha` (compatibilidad). |
| `FechaFin` | `DateTime?` | Fin opcional (eventos de varios días). |
| `Ubicacion` | `string?` | Lugar genérico. |
| `Estado` | `EstadoEventoEnum` | Default `Programada`. |
| `FechaCreacion` | `DateTime` | Default `UtcNow`. |
| `FechaFinInscripciones` | `DateTime?` | Cierre de inscripciones. |
| `EstaActivo` | `bool` | Soft-flag de actividad. |
| `Descripcion` | `string?` | Texto libre. |
| `TipoEvento` | `string` | Tipo como texto (también existe enum `TipoEvento`). |
| `FechaInicioInscripciones` | `DateTime?` | Apertura de inscripciones. |
| `Ciudad` / `Provincia` | `string?` | Ubicación administrativa. |
| `PrecioBase` | `decimal` | Precio base de inscripción. |
| `CupoMaximo` | `int` | Límite de participantes. |
| `TieneCronometraje` | `bool` | Si hay cronometraje oficial. |
| `RequiereCertificadoMedico` | `bool` | Regla de negocio. |
| `Observaciones` | `string?` | Notas. |

### Pertenencia

| Miembro | Tipo | Significado |
|---------|------|-------------|
| `IdClub` / `Club` | `int?` / `Club?` | Evento organizado por un club. |
| `IdFederacion` / `Federacion` | `int?` / `Federacion?` | O por una federación. |
| `InscripcionesHabilitadas` | `bool` | Interruptor global de altas. |

### Reglas de competencia (flags)

Controlan excepciones de categorías/botes: `RestringirSoloCategoriaPropia`, `PermitirSub23EnSenior`, `PermitirMasterBajarASenior`, `PermitirCompletarK4`, `LimitacionBotesAB`.

### Cronograma inteligente

| Miembro | Default | Rol |
|---------|---------|-----|
| `HoraInicioEvento` | 08:00 | Arranque del día. |
| `CarrilesDisponibles` | 9 | Carriles de pista. |
| `PerfilTiempo` | `Estandar` | Perfil de gaps. |
| `HoraInicioReceso` / `HoraFinReceso` | 13:00–14:00 | Almuerzo. |
| `SinReceso` | `false` | Omite receso. |
| `GapEntrePruebas` | 10 | Minutos entre pruebas. |
| `GapRecuperacionMinutos` | 40 | Descanso mínimo por atleta (misma cat/sexo). |
| `PermitirCombinadas` | `false` | Pruebas combinadas. |
| `UsarGapVariable` | `false` | Gaps dinámicos. |
| `TimeZoneId` | IANA AR | Zona horaria. |
| `CategoriasHabilitadas` etc. | `string?` | IDs separados por coma (diseño denormalizado). |
| `FechaActualizacion` | `DateTime?` | Última modificación. |

### Colecciones y método

| Miembro | Detalle |
|---------|---------|
| `Inscripciones` | `[NotMapped]`. Si no hay backing field cargado, deriva inscripciones desde `EventoPruebas`. |
| `EventoPruebas` | Navegación 1→N a pruebas del evento. |
| `PuedeInscribirse()` | `true` si están habilitadas y no venció `FechaFinInscripciones`. |

## Relaciones

- **N:1** opcional con `Club` y `Federacion`.
- **1:N** con `EventoPrueba`.
- Indirectamente con `Inscripcion` vía `EventoPrueba` (y el atajo `[NotMapped]`).

## Notas de estudio

1. Mezcla de `TipoEvento` como `string` y enum `TipoEvento` en otro archivo: al integrar APIs, unificá criterios.
2. Listas de IDs en un `string` (“1,2,3”) son simples pero frágiles; en modelos más maduros serían tablas puente.
3. `PuedeInscribirse()` es un buen ejemplo de **rich domain model** liviano: la entidad sabe una regla suya.
4. Usá `DateTime.UtcNow` al comparar con fechas guardadas en UTC (como hace el método).
