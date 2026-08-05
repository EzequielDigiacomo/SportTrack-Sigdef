# LoginDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Auth/Dtos/AuthDtos.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class LoginDto`
- `class AuthResponseDto`
- `class RegisterDto`
- `class UsuarioDto`
- `class UpdatePerfilDto`
- `class SolicitarResetPasswordDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Auth.Dtos`
- **Usings:**
  - _(ninguno explícito)_

## 4. Detalle del tipo — `class LoginDto`

### Propiedades

- `Username` (`string`)
- `Password` (`string`)

## 4. Detalle del tipo — `class AuthResponseDto`

### Propiedades

- `Token` (`string`)
- `Username` (`string`)
- `RolFederacion` (`string`)
- `ClubId` (`int?`)
- `FederacionId` (`int?`)
- `ClubNombre` (`string?`)
- `Nombre` (`string?`)
- `Apellido` (`string?`)
- `FrecuenciaPago` (`string?`)
- `FechaVencimientoPlan` (`System.DateTime?`)
- `Plan` (`SportTrack_Sigdef.Controladores.SaaS.Dtos.PlanSaaSDto?`)

## 4. Detalle del tipo — `class RegisterDto`

### Propiedades

- `Username` (`string`)
- `Password` (`string`)
- `Email` (`string`)
- `RolFederacion` (`string`)
- `ClubId` (`int?`)
- `FederacionId` (`int?`)
- `Nombre` (`string?`)
- `Apellido` (`string?`)
- `Dni` (`string?`)
- `Telefono` (`string?`)

## 4. Detalle del tipo — `class UsuarioDto`

### Propiedades

- `Id` (`int`)
- `Username` (`string`)
- `Email` (`string`)
- `RolFederacion` (`string`)
- `ClubId` (`int?`)
- `FederacionId` (`int?`)
- `ClubNombre` (`string?`)
- `Activo` (`bool`)
- `Nombre` (`string?`)
- `Apellido` (`string?`)
- `Dni` (`string?`)
- `FrecuenciaPago` (`string?`)
- `FechaVencimientoPlan` (`System.DateTime?`)
- `Plan` (`SportTrack_Sigdef.Controladores.SaaS.Dtos.PlanSaaSDto?`)

## 4. Detalle del tipo — `class UpdatePerfilDto`

### Propiedades

- `Nombre` (`string?`)
- `Apellido` (`string?`)
- `Dni` (`string?`)
- `Telefono` (`string?`)
- `Email` (`string?`)

## 4. Detalle del tipo — `class SolicitarResetPasswordDto`

### Propiedades

- `Username` (`string`)
- `Nota` (`string?`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Auth combina verificación de password (BCrypt), emisión de JWT y auditoría de intentos.
- Ruta relativa en el proyecto: `Auth/Dtos/AuthDtos.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
