# CategoriasController.cs

## 1. Qué es

CRUD del catálogo **Categorías** etarias de regata, más endpoints auxiliares por tipo de edad y filtrado por edad numérica. Autenticado. Sin namespace de archivo.

## 2. Conceptos C#/.NET ASP.NET Core

| Concepto | Uso |
|----------|-----|
| CRUD + endpoints de consulta | `tipos-edad`, `por-edad/{edad}` |
| `ICategoriaService` | Lógica en capa Controladores |
| `CreatedAtAction` / `NoContent` | Convenciones REST |
| Rutas literales vs `{id}` | Orden importa en el routing |

## 3. Namespace / usings

- Sin namespace
- Authorization, Mvc, Categoria + Dtos

## 4. Detalle

| Método | Ruta | Acción |
|--------|------|--------|
| `GetCategorias` | GET | Listado |
| `GetCategoria` | GET `{id}` | Detalle |
| `CreateCategoria` | POST | Alta |
| `UpdateCategoria` | PUT `{id}` | Update |
| `DeleteCategoria` | DELETE `{id}` | Baja |
| `GetCategoriasEdad` | GET `tipos-edad` | Tipos/edad |
| `GetCategoriasByEdad` | GET `por-edad/{edad}` | Categorías que aplican a esa edad |

## 5. Notas de estudio

- Seed en DbContext (Pre-Infantil … Master C, Control).
- `EdadMin`/`EdadMax` en Fluent API son opcionales.
- Compará con `DistanciasController` (mismo estilo de catálogo).
