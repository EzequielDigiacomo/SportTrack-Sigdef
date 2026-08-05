# AtletaFullCreateDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Atleta/AtletaFullCreateDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class AtletaFullCreateDto`
- `class TutorFullDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion`
- **Usings:**
  - `using SIGDEF.DTOs;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System.ComponentModel.DataAnnotations;`

## 4. Detalle del tipo — `class AtletaFullCreateDto`

### Propiedades

- `PersonaAtleta` (`PersonaCreateDto`) — atributos: `[Required]`
- `DatosDeportivos` (`AtletaCreateDto`) — atributos: `[Required]`
- `EsMenor` (`bool`)
- `TutorFederacion` (`TutorFullDto?`)

## 4. Detalle del tipo — `class TutorFullDto`

### Propiedades

- `IdPersonaTutor` (`int?`)
- `PersonaTutor` (`PersonaCreateDto?`)
- `Parentesco` (`int`) — atributos: `[Required]`

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Atleta/AtletaFullCreateDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
