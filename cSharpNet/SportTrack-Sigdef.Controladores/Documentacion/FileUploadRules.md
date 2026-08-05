# FileUploadRules

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Documentacion/FileUploadRules.cs`

## 1. Qué es este archivo

Es un **Tipo `class` de la capa Controladores** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Excepciones de dominio**: errores controlados (`NotFound`, `BadRequest`, `Unauthorized`) que la API traduce a HTTP.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Documentacion`
- **Usings:**
  - `using Microsoft.AspNetCore.Http;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.IO;`
  - `using System.Linq;`

## 4. Detalle del tipo — tipo principal

### Constantes

- `MaxBytes` (`long`)

### Métodos

#### `Validate`

- **Firma:** `void Validate(IFormFile file)`
- **Retorno:** `void`
- **Parámetros:**

- `file` (`IFormFile`)

- **Qué hace:** Valida reglas de negocio. ordena resultados.

## 5. Notas de estudio

- Leé el `.cs` junto a este `.md` y marcá cada `using` e interfaz inyectada hasta entender el grafo de dependencias.
- Ruta relativa en el proyecto: `Documentacion/FileUploadRules.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
