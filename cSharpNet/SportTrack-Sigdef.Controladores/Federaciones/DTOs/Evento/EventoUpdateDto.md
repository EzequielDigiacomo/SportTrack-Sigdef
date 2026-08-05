# EventoUpdateDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Evento/EventoUpdateDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `EventoCreateDTO`.

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SIGDEF.DTOs`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System.ComponentModel.DataAnnotations;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `IdEvento` (`int`) — atributos: `[Required(ErrorMessage = "El ID del evento es requerido")]`
- `EstaActivo` (`bool`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Evento/EventoUpdateDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
