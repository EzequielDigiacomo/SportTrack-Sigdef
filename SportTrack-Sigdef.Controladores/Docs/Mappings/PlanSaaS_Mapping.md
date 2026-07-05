# Mapeo de Clase: PlanSaaS

> **Descripción:** Planes de suscripción SaaS para clubes.

## Entidad Dominio: `PlanSaaS`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `Id` | `int` | Sí (FK) |
| `Nombre` | `string` | - |
| `Precio` | `decimal` | - |
| `MaxAtletas` | `int` | - |
| `MaxTorneosActivos` | `int` | - |
| `ResultadosTiempoReal` | `bool` | - |
| `ExportacionExcel` | `bool` | - |
| `SoportePrioritario` | `bool` | - |
| `Clubes` | `ICollection<Club>` | Sí (Navegación) |
| `Federaciones` | `ICollection<Federacion>` | Sí (Navegación) |

## DTO: `PlanSaaSDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Id` | `int` | `Id` | **Coincide Exacto** | - |
| `Nombre` | `string` | `Nombre` | **Coincide Exacto** | - |
| `Precio` | `decimal` | `Precio` | **Coincide Exacto** | - |
| `MaxAtletas` | `int` | `MaxAtletas` | **Coincide Exacto** | - |
| `MaxTorneosActivos` | `int` | `MaxTorneosActivos` | **Coincide Exacto** | - |
| `ResultadosTiempoReal` | `bool` | `ResultadosTiempoReal` | **Coincide Exacto** | - |
| `ExportacionExcel` | `bool` | `ExportacionExcel` | **Coincide Exacto** | - |
| `SoportePrioritario` | `bool` | `SoportePrioritario` | **Coincide Exacto** | - |