# Guía — Auth y usuarios

## Endpoints relevantes

| Método | Ruta | Uso |
|--------|------|-----|
| POST | `/api/Auth/login` | Login |
| GET | `/api/Auth/usuarios` | Listado (autorizado) |
| PUT | `/api/Auth/usuarios/{id}/password` | **Setear** nueva contraseña (body: string JSON) |
| PUT | `/api/Auth/usuarios/{id}/perfil` | Perfil |
| PATCH | `/api/Auth/usuarios/{id}/toggle-activo` | Activar/desactivar (Admin) |
| GET | `/api/Auth/me` | Sesión actual |
| GET | `/api/Usuario` | Listado DTO con `IdUsuario`, `ParticipanteId`, rol, club |
| POST | `/api/Usuario/{id}/change-password` | Requiere contraseña **actual** |
| DELETE | `/api/Usuario/{id}` | Borrado de usuario |

## Importante

1. **`FindAsync` / PK** de `Usuario` es **`IdUsuario`**.  
2. No existe `DELETE /api/Auth/usuarios/{id}`.  
3. No está expuesto `POST /api/Usuario/{id}/reset-password` en el controller (el servicio puede tener lógica interna, pero el front Admin debe usar Auth PUT password).  
4. Login valida hash **BCrypt** (`AuthService`). Preferir Auth para setear claves de acceso.

## Paso a paso — Admin cambia clave de un club

1. Front lista `/api/Usuario` y toma `idUsuario`.  
2. `PUT /api/Auth/usuarios/{idUsuario}/password` con body `"nuevaClave"`.  
3. Usuario prueba login.
