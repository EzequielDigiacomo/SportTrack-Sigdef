# PruebaDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Prueba/PruebaDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.Prueba`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Extensions;`
  - `using SportTrack_Sigdef.Controladores.Helpers;`
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `DistanciaDisplay` — tipo `string` (típicamente dependencia inyectada o estado privado)
- `CategoriaDisplay` — tipo `string` (típicamente dependencia inyectada o estado privado)
- `SexoDisplay` — tipo `string` (típicamente dependencia inyectada o estado privado)
- `BoteDisplay` — tipo `string` (típicamente dependencia inyectada o estado privado)

### Propiedades

- `IdPrueba` (`int`)
- `Distancia` (`DistanciaRegata`)
- `CategoriaEdad` (`CategoriaEdad`)
- `SexoCompetencia` (`SexoCompetencia`)
- `TipoBote` (`TipoBote`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Prueba/PruebaDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
