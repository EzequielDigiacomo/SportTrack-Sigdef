# BadRequestException

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Exceptions/BadRequestException.cs`

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

#### Constructor 1: `BadRequestException(...)`

**Parámetros:**

- `message` (`string`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

## 5. Notas de estudio

- Leé el `.cs` junto a este `.md` y marcá cada `using` e interfaz inyectada hasta entender el grafo de dependencias.
- Ruta relativa en el proyecto: `Exceptions/BadRequestException.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
