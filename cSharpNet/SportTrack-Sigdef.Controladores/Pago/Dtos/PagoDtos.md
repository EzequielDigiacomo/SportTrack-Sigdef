# PagoDto

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Pago/Dtos/PagoDtos.cs`

## 1. Qué es este archivo

Es un **DTO (objeto de transferencia de datos)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class PagoDto`
- `class RegistrarPagoDto`

## 2. Conceptos C# / .NET que aparecen

- **DTO (Data Transfer Object)**: objeto plano para transferir datos entre capas/API, sin lógica de persistencia.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Pago.Dtos`
- **Usings:**
  - `using System;`

## 4. Detalle del tipo — `class PagoDto`

### Propiedades

- `Id` (`int`)
- `TipoPago` (`string`)
- `ClubId` (`int?`)
- `ClubNombre` (`string?`)
- `ParticipanteId` (`int?`)
- `ParticipanteNombre` (`string?`)
- `InscripcionId` (`int?`)
- `EventoNombre` (`string?`)
- `PruebaNombre` (`string?`)
- `Monto` (`decimal`)
- `FechaPago` (`DateTime`)
- `Referencia` (`string?`)
- `RegistradoPor` (`string?`)
- `Notas` (`string?`)

## 4. Detalle del tipo — `class RegistrarPagoDto`

### Propiedades

- `TipoPago` (`string`)
- `ClubId` (`int?`)
- `ParticipanteId` (`int?`)
- `InscripcionId` (`int?`)
- `Monto` (`decimal`)
- `Referencia` (`string?`)
- `Notas` (`string?`)

## 5. Notas de estudio

- Compará este DTO con la entidad en `SportTrack-Sigdef.Entidades`: verás qué campos se exponen y cuáles se ocultan (hashes, navegaciones, etc.).
- Los `CreateDto`/`UpdateDto` suelen tener menos campos que el DTO de lectura.
- Ruta relativa en el proyecto: `Pago/Dtos/PagoDtos.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
