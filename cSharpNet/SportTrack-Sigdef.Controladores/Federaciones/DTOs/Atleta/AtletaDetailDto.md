# AtletaDetailDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Atleta/AtletaDetailDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Club;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Participante;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Inscripcion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Text;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Campos (dependencias / estado)

- `IdPersona` — tipo `int` (típicamente dependencia inyectada o estado privado)

### Propiedades

- `ParticipanteId` (`int`)
- `IdClub` (`int?`)
- `EstadoPago` (`EstadoPago`)
- `PerteneceSeleccion` (`bool`)
- `Categoria` (`CategoriaEdad?`)
- `CategoriaId` (`int?`)
- `CategoriaNombre` (`string?`)
- `BecadoEnard` (`bool`)
- `BecadoSdn` (`bool`)
- `MontoBeca` (`decimal`)
- `PresentoAptoMedico` (`bool`)
- `FechaAptoMedico` (`DateTime?`)
- `FechaCreacion` (`DateTime`)
- `Participante` (`PersonaDto?`)
- `Club` (`ClubDto?`)
- `Inscripciones` (`List<InscripcionDto>?`)
- `Tutores` (`List<AtletaTutorDto>?`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Atleta/AtletaDetailDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
