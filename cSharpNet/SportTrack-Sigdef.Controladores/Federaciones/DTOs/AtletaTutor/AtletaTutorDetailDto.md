# AtletaTutorDetailDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/AtletaTutor/AtletaTutorDetailDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor`
- **Usings:**
  - `using SIGDEF.DTOs;`
  - `using SportTrack_Sigdef.Entidades.DTOs.TutorFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;`
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `ParticipanteId` (`int`)
- `IdTutor` (`int`)
- `Parentesco` (`Parentesco`)
- `AtletaFederacion` (`AtletaDto?`)
- `TutorFederacion` (`TutorDto?`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/AtletaTutor/AtletaTutorDetailDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
