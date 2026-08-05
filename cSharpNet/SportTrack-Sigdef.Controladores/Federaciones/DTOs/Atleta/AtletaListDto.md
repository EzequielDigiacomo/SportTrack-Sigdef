# AtletaListDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Atleta/AtletaListDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class AtletaListDto`
- `class TutorListDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`

## 4. Detalle del tipo — `class AtletaListDto`

### Campos (dependencias / estado)

- `IdPersona` — tipo `int` (típicamente dependencia inyectada o estado privado)

### Propiedades

- `ParticipanteId` (`int`)
- `NombrePersona` (`string`)
- `Documento` (`string?`)
- `FechaNacimiento` (`DateTime`)
- `Edad` (`int?`)
- `NombreClub` (`string?`)
- `Categoria` (`CategoriaEdad?`)
- `CategoriaId` (`int?`)
- `CategoriaNombre` (`string?`)
- `PerteneceSeleccion` (`bool`)
- `EstadoPago` (`EstadoPago`)
- `FechaCreacion` (`DateTime?`)
- `CantidadDocumentos` (`int?`)
- `TutorInfo` (`TutorListDto?`)

## 4. Detalle del tipo — `class TutorListDto`

### Campos (dependencias / estado)

- `IdPersona` — tipo `int` (típicamente dependencia inyectada o estado privado)

### Propiedades

- `ParticipanteId` (`int`)
- `Nombre` (`string?`)
- `Apellido` (`string?`)
- `Documento` (`string?`)
- `Telefono` (`string?`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Atleta/AtletaListDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
