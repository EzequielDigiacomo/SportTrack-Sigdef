# UsuarioController.cs (SIGDEF)

## 1. Qué es

CRUD SIGDEF de **usuarios** del sistema federativo, más cambio de contraseña dedicado. Complementa (y a veces solapa) endpoints de `AuthController`.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| DTO específico de update | `UsuarioUpdateDto` |
| Acción anidada | POST `{id}/change-password` |
| Separación Auth vs Usuarios | Login en Auth; ABM aquí |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, DTOs Usuario, Federaciones

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetUsuarios` | GET | Listado |
| `GetUsuario` | GET `{id}` | Detalle |
| `PostUsuario` | POST | Alta |
| `PutUsuario` | PUT `{id}` | Update |
| `DeleteUsuario` | DELETE `{id}` | Baja |
| `ChangePassword` | POST `{id}/change-password` | Cambio password |

## 5. Notas de estudio

1. Tabla `seguridad.Usuarios`; índices únicos Username/Email.
2. Compará políticas: `AuthController.Register` limita roles; este controller depende de la lógica de `IUsuarioServices`.
3. PasswordHash se guarda hasheado (BCrypt en el ecosistema del proyecto).
