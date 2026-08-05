# Federacion.cs

## Qué es este archivo

Representa una **federación deportiva** (organización paraguas): datos fiscales, contacto, cuenta bancaria para cobros, clubes afiliados, usuarios, eventos y plan SaaS.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| Atributos `[Key]`, `[Required]`, `[MaxLength]` | Metadatos de PK, obligatoriedad y tamaño. |
| `virtual ICollection<T>` | Colecciones navegables (proxies EF). |
| Nullable de valor y referencia | `string? Sigla`, `DateTime? FechaAltaPlan`. |
| Defaults | `Activo = true`, strings `= string.Empty`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`
- `System.ComponentModel.DataAnnotations` — validación/mapeo.
- Colecciones vía `System.Collections.Generic`.

## Miembros

### Identidad y datos institucionales

| Propiedad | Atributos | Tipo | Negocio |
|-----------|-----------|------|---------|
| `IdFederacion` | `[Key]` | `int` | PK. |
| `Sigla` | — | `string?` | Abreviatura (ej. FAC). |
| `Activo` | — | `bool` | Soft-delete lógico. |
| `Nombre` | `[Required, MaxLength(100)]` | `string` | Nombre oficial. |
| `Cuit` | `[Required, MaxLength(20)]` | `string` | Identificación fiscal AR. |
| `Email` | `[MaxLength(100)]` | `string` | Contacto. |
| `Telefono` | `[MaxLength(20)]` | `string` | Contacto. |
| `Direccion` | `[MaxLength(200)]` | `string` | Domicilio. |

### Datos bancarios / cobro

`BancoNombre`, `TipoCuenta`, `NumeroCuenta`, `TitularCuenta`, `EmailCobro` — todos con `[MaxLength]` apropiado. Sirven para transferencias y facturación de afiliaciones.

### Navegaciones

| Propiedad | Relación |
|-----------|----------|
| `DelegadosClub` | 1→N `DelegadoFederacionClub` |
| `Clubes` | 1→N `Club` |
| `Usuarios` | 1→N `Usuario` |
| `Eventos` | 1→N `Evento` |
| `AtletasFederados` | 1→N `AtletaFederacion` |
| `Entrenadores` | 1→N `EntrenadorFederacion` |
| `PlanSaaSId` / `PlanSaaS` | N→1 opcional al plan comercial |

### Suscripción SaaS

`FechaAltaPlan`, `FechaVencimientoPlan`, `FrecuenciaPago` (`"Mensual"`/`"Anual"`), `BloqueadaPorFaltaDePago`.

## Relaciones

Es un **agregado raíz** del lado federación: agrupa clubes, personas federadas, eventos y usuarios administrativos.

También se relaciona con `PeriodoTraspaso` y `SolicitudTraspaso` (desde esas entidades hacia aquí).

## Notas de estudio

1. Varios strings no nullable con `= string.Empty` evitan null pero **no** equivalen a `[Required]` en validación de API; el atributo sí fuerza validación.
2. Compará con `Club`: ambos tienen plan SaaS y flags de bloqueo por falta de pago — patrón repetido a dos niveles (tenant federación vs tenant club).
3. `[MaxLength]` influye en el DDL que genera EF (tamaño de columna `nvarchar`).
