# ServiceExtensions

**Archivo fuente:** `SportTrack-Sigdef.Controladores/PagosSIGDEF/Services/ServiceExtensions.cs`

## 1. Qué es este archivo

Es un **Servicio (lógica de negocio)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

## 2. Conceptos C# / .NET que aparecen

- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Extension methods**: métodos estáticos con `this` que añaden API a tipos existentes.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.PagosSIGDEF.Extensions`
- **Usings:**
  - `using Microsoft.Extensions.DependencyInjection;`
  - `using SportTrack_Sigdef.Controladores.PagosSIGDEF;`
  - `using SportTrack_Sigdef.Controladores.PagosSIGDEF.Services;`

## 4. Detalle del tipo — tipo principal

### Métodos

#### `AddMercadoPagoServices`

- **Firma:** `IServiceCollection AddMercadoPagoServices(this IServiceCollection services)`
- **Retorno:** `IServiceCollection`
- **Parámetros:**

- `services` (`this IServiceCollection`)

- **Qué hace:** Crea/registra un nuevo recurso.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- Integración con Mercado Pago: separá configuración (`Config`), modelos/DTOs y servicios HTTP.
- Ruta relativa en el proyecto: `PagosSIGDEF/Services/ServiceExtensions.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
