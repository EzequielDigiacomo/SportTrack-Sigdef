# PersonaDetailDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Persona/PersonaDetailDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.Participante`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SIGDEF.DTOs;`
  - `using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub;`
  - `using SportTrack_Sigdef.Entidades.DTOs.EntrenadorFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.PagoFederacionTransaccion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.TutorFederacion;`
  - `using SportTrack_Sigdef.Entidades.DTOs.Usuario;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Text;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `ParticipanteId` (`int`)
- `Nombre` (`string`)
- `Apellido` (`string`)
- `Documento` (`string`)
- `FechaNacimiento` (`DateTime`)
- `Email` (`string`)
- `Telefono` (`string`)
- `Direccion` (`string`)
- `Sexo` (`Sexo`)
- `SexoDisplay` (`string`)
- `Usuario` (`UsuarioDto?`)
- `DelegadoFederacionClub` (`DelegadoClubDto?`)
- `EntrenadorFederacion` (`EntrenadorDto?`)
- `TutorFederacion` (`TutorDto?`)
- `AtletaFederacion` (`AtletaDto?`)
- `Pagos` (`List<PagoTransaccionDto>?`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Persona/PersonaDetailDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
