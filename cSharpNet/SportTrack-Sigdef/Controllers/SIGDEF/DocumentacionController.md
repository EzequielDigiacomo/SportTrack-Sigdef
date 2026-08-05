# DocumentacionController.cs (SIGDEF)

## 1. Qué es

Gestión de **documentación de personas** (upload de archivos, listado por persona, delete). Usa multipart/form-data y límite de tamaño de request. Integra Cloudinary (configurado en `Program.cs`) con posible fallback local en el service.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| `[FromForm]` | Binding multipart |
| `[RequestSizeLimit]` | 6 MB |
| try/catch local | Mapeo a 400/404/500 (además del middleware) |
| `IFormFile` (vía DTO) | Archivo subido |
| Validación manual | File null, PersonaId |

## 3. Namespace / usings

- `SIGDEF.API.Controllers`
- Authorization, Mvc, Documentacion service, DTOs, etc.

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `Upload` | POST `upload` | Sube archivo; valida; llama `UploadAsync` |
| `GetByPersona` | GET `persona/{id}` | Lista docs |
| `Delete` | DELETE `{id}` | Soft/hard delete según service; 404 si no existe |

### Manejo de errores en Upload

- `KeyNotFoundException` → 404
- `ArgumentException` → 400
- Otros → 500 con mensaje

## 5. Notas de estudio

1. `[RequestSizeLimit]` es defensa en profundidad (también hay límites de Kestrel/IIS).
2. try/catch en controller + ExceptionMiddleware: aquí se elige formato `{ success, error }` específico.
3. Relacioná con `Configure<CloudinarySettings>` en Program.
4. Entidad EF: `DocumentacionPersonas` en esquema `federacion`.
