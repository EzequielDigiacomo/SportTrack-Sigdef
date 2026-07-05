# Mapeo de Clase: Federacion

> **Descripción:** La federación deportiva que engloba clubes y atletas.

## Entidad Dominio: `Federacion`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `IdFederacion` | `int` | - |
| `Sigla` | `string?` | - |
| `Activo` | `bool` | - |
| `Nombre` | `string` | - |
| `Cuit` | `string` | - |
| `Email` | `string` | - |
| `Telefono` | `string` | - |
| `Direccion` | `string` | - |
| `BancoNombre` | `string` | - |
| `TipoCuenta` | `string` | - |
| `NumeroCuenta` | `string` | - |
| `TitularCuenta` | `string` | - |
| `EmailCobro` | `string` | - |
| `DelegadosClub` | `ICollection<DelegadoFederacionClub>` | Sí (Navegación) |
| `Clubes` | `ICollection<Club>` | Sí (Navegación) |
| `Usuarios` | `ICollection<Usuario>` | Sí (Navegación) |
| `Eventos` | `ICollection<Evento>` | Sí (Navegación) |
| `AtletasFederados` | `ICollection<AtletaFederacion>` | Sí (Navegación) |
| `Entrenadores` | `ICollection<EntrenadorFederacion>` | Sí (Navegación) |
| `PlanSaaSId` | `int?` | - |
| `PlanSaaS` | `PlanSaaS?` | - |
| `FechaAltaPlan` | `DateTime?` | - |
| `FechaVencimientoPlan` | `DateTime?` | - |
| `FrecuenciaPago` | `string?` | - |
| `BloqueadaPorFaltaDePago` | `bool` | - |

## DTO: `FederacionDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdFederacion` | `int` | `IdFederacion` | **Coincide Exacto** | - |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Cuit` | `string` | `Cuit` | **Coincide Exacto** | - |
| `Email` | `string` | `Email` | **Coincide Exacto** | - |
| `Telefono` | `string` | `Telefono` | **Coincide Exacto** | - |
| `Direccion` | `string` | `Direccion` | **Coincide Exacto** | - |
| `BancoNombre` | `string` | `BancoNombre` | **Coincide Exacto** | - |
| `TipoCuenta` | `string` | `TipoCuenta` | **Coincide Exacto** | - |
| `NumeroCuenta` | `string` | `NumeroCuenta` | **Coincide Exacto** | - |
| `TitularCuenta` | `string` | `TitularCuenta` | **Coincide Exacto** | - |
| `EmailCobro` | `string` | `EmailCobro` | **Coincide Exacto** | - |
| `PlanSaaSId` | `int?` | `PlanSaaSId` | **Coincide Exacto** | - |
| `FechaAltaPlan` | `DateTime?` | `FechaAltaPlan` | **Coincide Exacto** | - |
| `FechaVencimientoPlan` | `DateTime?` | `FechaVencimientoPlan` | **Coincide Exacto** | - |
| `FrecuenciaPago` | `string?` | `FrecuenciaPago` | **Coincide Exacto** | - |
| `BloqueadaPorFaltaDePago` | `bool` | `BloqueadaPorFaltaDePago` | **Coincide Exacto** | - |
| `Activo` | `bool` | `Activo` | **Coincide Exacto** | - |

## DTO: `FederacionCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Cuit` | `string` | `Cuit` | **Coincide Exacto** | - |
| `Email` | `string` | `Email` | **Coincide Exacto** | - |
| `Telefono` | `string` | `Telefono` | **Coincide Exacto** | - |
| `Direccion` | `string` | `Direccion` | **Coincide Exacto** | - |
| `BancoNombre` | `string` | `BancoNombre` | **Coincide Exacto** | - |
| `TipoCuenta` | `string` | `TipoCuenta` | **Coincide Exacto** | - |
| `NumeroCuenta` | `string` | `NumeroCuenta` | **Coincide Exacto** | - |
| `TitularCuenta` | `string` | `TitularCuenta` | **Coincide Exacto** | - |
| `EmailCobro` | `string` | `EmailCobro` | **Coincide Exacto** | - |
| `PlanSaaSId` | `int?` | `PlanSaaSId` | **Coincide Exacto** | - |
| `FechaAltaPlan` | `DateTime?` | `FechaAltaPlan` | **Coincide Exacto** | - |
| `FechaVencimientoPlan` | `DateTime?` | `FechaVencimientoPlan` | **Coincide Exacto** | - |
| `FrecuenciaPago` | `string?` | `FrecuenciaPago` | **Coincide Exacto** | - |
| `BloqueadaPorFaltaDePago` | `bool?` | `BloqueadaPorFaltaDePago` | **Coincide Exacto** | Cambio de tipo: bool -> bool? |