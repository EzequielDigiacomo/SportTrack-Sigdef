# EventoCreateDTO

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Evento/EventoCreateDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.
- **Data Annotations**: atributos de validación (`[Required]`, etc.) usados en modelos/DTOs.

## 3. Namespace y usings

- **Namespace:** `SIGDEF.DTOs`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.DTOs.Evento;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.ComponentModel.DataAnnotations;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Nombre` (`string`) — atributos: `[Required(ErrorMessage = "El nombre es requerido")]`, `[MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]`
- `Descripcion` (`string?`) — atributos: `[MaxLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]`
- `TipoEvento` (`TipoEvento`) — atributos: `[Required(ErrorMessage = "El tipo de evento es requerido")]`, `[EnumDataType(typeof(TipoEvento), ErrorMessage = "Tipo de evento no válido")]`
- `FechaInicio` (`DateTime`) — atributos: `[Required(ErrorMessage = "La fecha de inicio es requerida")]`
- `FechaFin` (`DateTime`) — atributos: `[Required(ErrorMessage = "La fecha de fin es requerida")]`
- `FechaInicioInscripciones` (`DateTime?`)
- `FechaFinInscripciones` (`DateTime?`)
- `Ubicacion` (`string?`) — atributos: `[MaxLength(200, ErrorMessage = "La ubicación no puede exceder 200 caracteres")]`
- `Ciudad` (`string?`) — atributos: `[MaxLength(100, ErrorMessage = "La ciudad no puede exceder 100 caracteres")]`
- `Provincia` (`string?`) — atributos: `[MaxLength(100, ErrorMessage = "La provincia no puede exceder 100 caracteres")]`
- `Distancias` (`List<DistanciaDTO>`) — atributos: `[Required(ErrorMessage = "Debe especificar al menos una distancia")]`, `[MinLength(1, ErrorMessage = "Debe especificar al menos una distancia")]`
- `PrecioBase` (`decimal`) — atributos: `[Range(0, 100000, ErrorMessage = "El precio debe estar entre 0 y 100,000")]`
- `CupoMaximo` (`int`) — atributos: `[Range(1, 10000, ErrorMessage = "El cupo debe estar entre 1 y 10,000")]`
- `TieneCronometraje` (`bool`)
- `RequiereCertificadoMedico` (`bool`)
- `Observaciones` (`string?`) — atributos: `[MaxLength(1000, ErrorMessage = "Las observaciones no pueden exceder 1000 caracteres")]`

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Evento/EventoCreateDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
