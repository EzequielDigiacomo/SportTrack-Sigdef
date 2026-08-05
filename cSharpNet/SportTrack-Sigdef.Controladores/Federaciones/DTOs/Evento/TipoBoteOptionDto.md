# TipoBoteOptionDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/DTOs/Evento/TipoBoteOptionDto.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.DTOs.Evento`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`

## 4. Detalle del tipo — tipo principal

### Propiedades

- `Id` (`int`)
- `Nombre` (`string`)
- `Codigo` (`string`)

### Métodos

#### `FromEnum`

- **Firma:** `TipoBoteOptionDto FromEnum(TipoBote bote)`
- **Retorno:** `TipoBoteOptionDto`
- **Parámetros:**

- `bote` (`TipoBote`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método.

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/DTOs/Evento/TipoBoteOptionDto.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
