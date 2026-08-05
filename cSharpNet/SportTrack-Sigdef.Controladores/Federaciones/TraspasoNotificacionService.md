# TraspasoNotificacionEvento

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/TraspasoNotificacionService.cs`

## 1. Qué es este archivo

Es un **Tipo `enum` de la capa Controladores** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `enum TraspasoNotificacionEvento`
- `interface ITraspasoNotificacionService`
- `class TraspasoNotificacionService` : `ITraspasoNotificacionService`

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Service layer**: concentra reglas de negocio, orquesta repositorios/DbContext y mapea a DTOs.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **IEnumerable<T>**: secuencia de elementos; común como retorno de listados.
- **enum**: conjunto fijo de constantes con nombre.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Federaciones`
- **Usings:**
  - `using SportTrack_Sigdef.Controladores.Mensajes;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`
  - `using System.Collections.Generic;`
  - `using System.Linq;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — `enum TraspasoNotificacionEvento`

### Valores del enum

- `SolicitudCreada`
- `FederacionHabilito`
- `FederacionHabilitoForzado`
- `OrigenAcepto`
- `OrigenRechazo`
- `FederacionRechazo`
- `Cancelado`

## 4. Detalle del tipo — `interface ITraspasoNotificacionService`

### Métodos

#### `NotificarAsync`

- **Firma:** `Task NotificarAsync(SolicitudTraspaso solicitud, TraspasoNotificacionEvento evento)`
- **Retorno:** `Task`
- **Parámetros:**

- `solicitud` (`SolicitudTraspaso`)
- `evento` (`TraspasoNotificacionEvento`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

## 4. Detalle del tipo — `class TraspasoNotificacionService`

### Campos (dependencias / estado)

- `_mensajeRepository` — tipo `IMensajeRepository` (típicamente dependencia inyectada o estado privado)
- `_mensajeService` — tipo `IMensajeService` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `TraspasoNotificacionService(...)`

**Parámetros:**

- `mensajeRepository` (`IMensajeRepository`)
- `mensajeService` (`IMensajeService`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `NotificarAsync`

- **Firma:** `async Task NotificarAsync(SolicitudTraspaso solicitud, TraspasoNotificacionEvento evento)`
- **Retorno:** `Task`
- **Parámetros:**

- `solicitud` (`SolicitudTraspaso`)
- `evento` (`TraspasoNotificacionEvento`)

- **Qué hace:** Envía notificaciones. operación asíncrona (`await`).

#### `AddUsuarios`

- **Firma:** ` AddUsuarios(ids, await _mensajeRepository.GetUsuariosAdminByFederacionAsync(solicitud.IdFederacion)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`ids`)
- `?` (`await _mensajeRepository.GetUsuariosAdminByFederacionAsync(solicitud.IdFederacion`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddUsuarios`

- **Firma:** ` AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`ids`)
- `?` (`await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddUsuarios`

- **Firma:** ` AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubOrigen)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`ids`)
- `?` (`await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubOrigen`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddUsuarios`

- **Firma:** ` AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`ids`)
- `?` (`await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddUsuarios`

- **Firma:** ` AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`ids`)
- `?` (`await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddUsuarios`

- **Firma:** ` AddUsuarios(ids, await _mensajeRepository.GetUsuariosAdminByFederacionAsync(solicitud.IdFederacion)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`ids`)
- `?` (`await _mensajeRepository.GetUsuariosAdminByFederacionAsync(solicitud.IdFederacion`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddUsuarios`

- **Firma:** ` AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`ids`)
- `?` (`await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddUsuarios`

- **Firma:** ` AddUsuarios(ids, await _mensajeRepository.GetUsuariosAdminByFederacionAsync(solicitud.IdFederacion)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`ids`)
- `?` (`await _mensajeRepository.GetUsuariosAdminByFederacionAsync(solicitud.IdFederacion`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddUsuarios`

- **Firma:** ` AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`ids`)
- `?` (`await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddUsuarios`

- **Firma:** ` AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubOrigen)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`ids`)
- `?` (`await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubOrigen`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddUsuarios`

- **Firma:** ` AddUsuarios(ids, await _mensajeRepository.GetUsuariosAdminByFederacionAsync(solicitud.IdFederacion)`
- **Retorno:** ``
- **Parámetros:**

- `?` (`ids`)
- `?` (`await _mensajeRepository.GetUsuariosAdminByFederacionAsync(solicitud.IdFederacion`)

- **Qué hace:** declara el contrato; la implementación concreta está en la clase que implementa la interfaz.

#### `AddUsuarios`

- **Firma:** `void AddUsuarios(ISet<int> ids, IEnumerable<Usuario> usuarios)`
- **Retorno:** `void`
- **Parámetros:**

- `ids` (`ISet<int>`)
- `usuarios` (`IEnumerable<Usuario>`)

- **Qué hace:** Crea/registra un nuevo recurso.

## 5. Notas de estudio

- Seguí el flujo: Controller → Service → Repository/DbContext → Entidad → mapeo a DTO.
- Prestá atención a las excepciones de dominio: son la forma de comunicar errores al middleware/API.
- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/TraspasoNotificacionService.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
