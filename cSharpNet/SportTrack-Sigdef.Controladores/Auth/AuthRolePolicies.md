# AuthRolePolicies

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Auth/AuthRolePolicies.cs`

## 1. Qué es este archivo

Es una clase **`static`** con **constantes y listas de roles** reutilizables para autorización ASP.NET (`[Authorize(Roles = ...)]`).

Centraliza los nombres de roles (claim `ClaimTypes.Role`) para no hardcodear strings mágicos en cada Controller.

## 2. Conceptos C# / .NET que aparecen

- **`static class`**: no se instancia; solo agrupa miembros estáticos.
- **`const`**: valor constante conocido en compile-time (debe ser tipo primitivo/string).
- **`static readonly`**: valor fijo en runtime (permite arrays/objetos inicializados una vez).
- **Authorization roles**: strings que coinciden con roles asignados al usuario (JWT claims).
- **File-scoped namespace** (`namespace X;`): sintaxis moderna sin llaves envolviendo todo el archivo.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Auth`
- **Usings:** _(ninguno explícito)_

## 4. Detalle del tipo — `static class AuthRolePolicies`

### Constantes

#### `CompetitionOperators` (`const string`)

Roles que pueden operar carrera / hub / mutaciones de fases y resultados:

`Admin,SuperAdmin,JuezControl,Largador,Cronometrista,soporte_tecnico`

Uso típico: `[Authorize(Roles = AuthRolePolicies.CompetitionOperators)]`.

#### `Admins` (`const string`)

Roles administrativos: `Admin,SuperAdmin,soporte_tecnico`.

### Campos estáticos

#### `RegisterableRoles` (`static readonly string[]`)

Roles permitidos al registrar vía API. **Nunca** incluye `SuperAdmin` desde el cliente.

Valores:

- `Club`
- `Admin`
- `Largador`
- `Cronometrista`
- `JuezControl`
- `soporte_tecnico`

## 5. Notas de estudio

- Un solo lugar para roles evita typos (`"Admin"` vs `"admin"`).
- `const string` con roles separados por coma es el formato que espera el atributo `[Authorize(Roles = "...")]`.
- Compará con el registro de usuarios en `AuthService`: debería validar contra `RegisterableRoles`.
- Ruta: `Auth/AuthRolePolicies.cs`.

---

*Documentación educativa C#/.NET a partir del código fuente real.*
