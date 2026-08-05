# BotesController.cs

## 1. Qué es

CRUD de catálogo **Botes** (tipos de embarcación) + listado de tipos. Requiere autenticación. Sin namespace explícito (clase en global namespace del archivo).

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| CRUD REST clásico | GET/POST/PUT/DELETE |
| `CreatedAtAction` | 201 con enlace a GET by id |
| `NoContent` | DELETE |
| Service abstraction | `IBoteService` |
| File without namespace | Válido en C#; vive en global ns |

## 3. Namespace / usings

- Sin `namespace` declarado
- Usings: Authorization, Mvc, Bote + Dtos

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetBotes` | GET | Todos |
| `GetBote` | GET `{id}` | Por id |
| `CreateBote` | POST | Alta |
| `UpdateBote` | PUT `{id}` | Update |
| `DeleteBote` | DELETE `{id}` | Baja |
| `GetTiposBote` | GET `tipos` | Catálogo de tipos |

## 5. Notas de estudio

1. Orden de rutas: `tipos` es estático y no choca con `{id}` porque es segmento literal distinto… en realidad `GET tipos` vs `GET {id}` — ASP.NET distingue por plantilla; si `id` es int y “tipos” no parsea, puede haber conflicto según constraints. Aquí `id` no tiene `:int`; estudiá el orden de registro.
2. Seed inicial de botes está en DbContext.
3. Misma forma que Categorias/Distancias (patrón catálogo).
