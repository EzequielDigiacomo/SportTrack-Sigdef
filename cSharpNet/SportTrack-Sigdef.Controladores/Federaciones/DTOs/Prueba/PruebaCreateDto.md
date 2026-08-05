# PruebaCreateDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Prueba/PruebaCreateDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.Prueba`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System.ComponentModel.DataAnnotations;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Distancia` (`DistanciaRegata`) — atributos: `[Required]`
- `CategoriaEdad` (`CategoriaEdad`) — atributos: `[Required]`
- `SexoCompetencia` (`SexoCompetencia`) — atributos: `[Required]`
- `TipoBote` (`TipoBote`) — atributos: `[Required]`

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Prueba/PruebaCreateDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
