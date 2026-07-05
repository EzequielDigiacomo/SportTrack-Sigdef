# Mapeo de Clase: Usuario

> **Descripción:** Usuarios con acceso al sistema y roles asignados.

## Entidad Dominio: `Usuario`
| Propiedad | Tipo | Notas / Relación |
| --- | --- | --- |
| `IdUsuario` | `int` | - |
| `Username` | `string` | - |
| `PasswordHash` | `string` | - |
| `Email` | `string` | - |
| `RolFederacion` | `string` | - |
| `IdClub` | `int?` | Sí (FK) |
| `Club` | `Club?` | - |
| `IdFederacion` | `int?` | Sí (FK) |
| `Federacion` | `Federacion?` | - |
| `FechaCreacion` | `DateTime` | - |
| `EstaActivo` | `bool` | - |
| `IntentosFallidos` | `int` | - |
| `UltimoAcceso` | `DateTime?` | - |
| `Nombre` | `string?` | - |
| `Apellido` | `string?` | - |
| `Dni` | `string?` | - |
| `Telefono` | `string?` | - |
| `ParticipanteId` | `int?` | - |
| `Participante` | `Participante?` | - |

## DTO: `UsuarioDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdUsuario` | `int` | `IdUsuario` | **Coincide Exacto** | - |
| `ParticipanteId` | `int?` | `ParticipanteId` | **Coincide Exacto** | - |
| `IdFederacion` | `int?` | `IdFederacion` | **Coincide Exacto** | - |
| `Username` | `string` | `Username` | **Coincide Exacto** | - |
| `EstaActivo` | `bool` | `EstaActivo` | **Coincide Exacto** | - |
| `FechaCreacion` | `DateTime` | `FechaCreacion` | **Coincide Exacto** | - |
| `UltimoAcceso` | `DateTime` | `UltimoAcceso` | **Coincide Exacto** | Cambio de tipo: DateTime? -> DateTime |
| `NombrePersona` | `string?` | `-` | *No coincide* | - |
| `Email` | `string?` | `Email` | **Coincide Exacto** | Cambio de tipo: string -> string? |
| `RolFederacion` | `string?` | `RolFederacion` | **Coincide Exacto** | Cambio de tipo: string -> string? |
| `IdClub` | `int?` | `IdClub` | **Coincide Exacto** | - |

## DTO: `UsuarioCreateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `ParticipanteId` | `int?` | `ParticipanteId` | **Coincide Exacto** | - |
| `IdClub` | `int?` | `IdClub` | **Coincide Exacto** | - |
| `IdFederacion` | `int?` | `IdFederacion` | **Coincide Exacto** | - |
| `Username` | `string` | `Username` | **Coincide Exacto** | - |
| `Password` | `string` | `-` | *No coincide* | - |
| `ConfirmPassword` | `string` | `-` | *No coincide* | - |
| `EstaActivo` | `bool` | `EstaActivo` | **Coincide Exacto** | - |
| `RolFederacion` | `string` | `RolFederacion` | **Coincide Exacto** | - |

## DTO: `UsuarioUpdateDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Username` | `string?` | `Username` | **Coincide Exacto** | Cambio de tipo: string -> string? |
| `EstaActivo` | `bool?` | `EstaActivo` | **Coincide Exacto** | Cambio de tipo: bool -> bool? |
| `RolFederacion` | `RolTipo?` | `RolFederacion` | **Coincide Exacto** | Cambio de tipo: string -> RolTipo? |
| `IdClub` | `int?` | `IdClub` | **Coincide Exacto** | - |

## DTO: `UsuarioResponseDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdUsuario` | `int` | `IdUsuario` | **Coincide Exacto** | - |
| `ParticipanteId` | `int?` | `ParticipanteId` | **Coincide Exacto** | - |
| `IdFederacion` | `int?` | `IdFederacion` | **Coincide Exacto** | - |
| `Username` | `string` | `Username` | **Coincide Exacto** | - |
| `EstaActivo` | `bool` | `EstaActivo` | **Coincide Exacto** | - |
| `UltimoAcceso` | `DateTime` | `UltimoAcceso` | **Coincide Exacto** | Cambio de tipo: DateTime? -> DateTime |
| `Token` | `string` | `-` | *No coincide* | - |
| `TokenExpira` | `DateTime` | `-` | *No coincide* | - |
| `NombreCompleto` | `string?` | `Nombre.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `Email` | `string?` | `Email` | **Coincide Exacto** | Cambio de tipo: string -> string? |
| `RolFederacion` | `string?` | `RolFederacion` | **Coincide Exacto** | Cambio de tipo: string -> string? |
| `IdClub` | `int?` | `IdClub` | **Coincide Exacto** | - |

## DTO: `UsuarioLoginDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Username` | `string` | `Username` | **Coincide Exacto** | - |
| `Password` | `string` | `-` | *No coincide* | - |

## DTO: `UsuarioDetailDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `IdUsuario` | `int` | `IdUsuario` | **Coincide Exacto** | - |
| `ParticipanteId` | `int?` | `ParticipanteId` | **Coincide Exacto** | - |
| `IdFederacion` | `int?` | `IdFederacion` | **Coincide Exacto** | - |
| `Username` | `string` | `Username` | **Coincide Exacto** | - |
| `EstaActivo` | `bool` | `EstaActivo` | **Coincide Exacto** | - |
| `FechaCreacion` | `DateTime` | `FechaCreacion` | **Coincide Exacto** | - |
| `UltimoAcceso` | `DateTime` | `UltimoAcceso` | **Coincide Exacto** | Cambio de tipo: DateTime? -> DateTime |
| `IdClub` | `int?` | `IdClub` | **Coincide Exacto** | - |
| `RolFederacion` | `string?` | `RolFederacion` | **Coincide Exacto** | Cambio de tipo: string -> string? |
| `NombreClub` | `string` | `-` | *No coincide* | - |
| `Emailclub` | `string` | `-` | *No coincide* | - |
| `Participante` | `PersonaDto?` | `Participante` | **Coincide Exacto** | Cambio de tipo: Participante? -> PersonaDto? |
| `Club` | `ClubDetailDto?` | `Club` | **Coincide Exacto** | Cambio de tipo: Club? -> ClubDetailDto? |

## DTO: `AuthResponseDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Token` | `string` | `-` | *No coincide* | - |
| `Username` | `string` | `Username` | **Coincide Exacto** | - |
| `RolFederacion` | `string` | `RolFederacion` | **Coincide Exacto** | - |
| `ClubId` | `int?` | `Club.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `FederacionId` | `int?` | `Federacion.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `ClubNombre` | `string?` | `Club.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `Nombre` | `string?` | `Nombre` | **Coincide Exacto** | - |
| `Apellido` | `string?` | `Apellido` | **Coincide Exacto** | - |
| `FrecuenciaPago` | `string?` | `-` | *No coincide* | - |
| `FechaVencimientoPlan` | `System.DateTime?` | `-` | *No coincide* | - |
| `Plan` | `SportTrack_Sigdef.Controladores.SaaS.Dtos.PlanSaaSDto?` | `-` | *No coincide* | - |

## DTO: `RegisterDto`
| Atributo DTO | Tipo DTO | Atributo Entidad Equiv. | Estado de Coincidencia | Notas |
| --- | --- | --- | --- | --- |
| `Username` | `string` | `Username` | **Coincide Exacto** | - |
| `Password` | `string` | `-` | *No coincide* | - |
| `Email` | `string` | `Email` | **Coincide Exacto** | - |
| `RolFederacion` | `string` | `RolFederacion` | **Coincide Exacto** | - |
| `ClubId` | `int?` | `Club.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `FederacionId` | `int?` | `Federacion.Propiedad` | Propiedad de Navegación | Mapeado desde objeto relacionado en AutoMapper |
| `Nombre` | `string?` | `Nombre` | **Coincide Exacto** | - |
| `Apellido` | `string?` | `Apellido` | **Coincide Exacto** | - |
| `Dni` | `string?` | `Dni` | **Coincide Exacto** | - |
| `Telefono` | `string?` | `Telefono` | **Coincide Exacto** | - |