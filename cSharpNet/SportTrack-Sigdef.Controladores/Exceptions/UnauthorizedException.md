# UnauthorizedException

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Exceptions/UnauthorizedException.cs`

## 1. Qué es este archivo

Es un **Excepción personalizada de la capa de negocio** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `Exception`.

## 2. Conceptos C# / .NET que aparecen

- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Exceptions`
- **Usings:**
  - `using System;`

## 4. Detalle del tipo — tipo principal

### Constructores

#### Constructor 1: `UnauthorizedException(...)`

**Parámetros:**

- `message` (`string`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

## 5. Notas de estudio

- Auth combina verificación de password (BCrypt), emisión de JWT y auditoría de intentos.
- Ruta relativa en el proyecto: `Exceptions/UnauthorizedException.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
