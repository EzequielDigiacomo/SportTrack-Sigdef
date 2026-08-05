# RaceUserPresence

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Hubs/TimingHub.cs`

## 1. Qué es este archivo

Es un **Tipo `class` de la capa Controladores** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class RaceUserPresence`
- `class TimingHub` : `Hub`

## 2. Conceptos C# / .NET que aparecen

- **async/await y Task**: permiten I/O no bloqueante; el método retorna `Task`/`Task<T>` y se espera con `await`.
- **Dependency Injection (DI)**: el constructor recibe dependencias (`IRepository`, `IMapper`, etc.) inyectadas por el contenedor.
- **SignalR Hub**: canal en tiempo real hacia clientes conectados.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Hubs`
- **Usings:**
  - `using Microsoft.AspNetCore.Authorization;`
  - `using Microsoft.AspNetCore.SignalR;`
  - `using SportTrack_Sigdef.Controladores.Auth;`
  - `using SportTrack_Sigdef.Controladores.Fase;`
  - `using System;`
  - `using System.Threading.Tasks;`

## 4. Detalle del tipo — `class RaceUserPresence`

### Propiedades

#### `ConnectionId`

- **Tipo:** `string`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `UserName`

- **Tipo:** `string`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `Role`

- **Tipo:** `string`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración


## 4. Detalle del tipo — `class TimingHub`

### Campos (dependencias / estado)

- `_faseService` — tipo `IFaseService` (típicamente dependencia inyectada o estado privado)

### Constructores

#### Constructor 1: `TimingHub(...)`

**Parámetros:**

- `faseService` (`IFaseService`)

**Qué hace:** recibe dependencias (DI) y las asigna a campos `readonly` para usarlas en los métodos.

### Métodos

#### `JoinRaceGroup`

- **Firma:** `async Task JoinRaceGroup(string faseId, string userName, string role)`
- **Retorno:** `Task`
- **Atributos:** `[AllowAnonymous]`
- **Parámetros:**

- `faseId` (`string`)
- `userName` (`string`)
- `role` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `Groups.AddToGroupAsync(...)`, `Clients.Group(...)`

#### `JoinEventGroup`

- **Firma:** `async Task JoinEventGroup(string eventoId, string userName, string role)`
- **Retorno:** `Task`
- **Atributos:** `[AllowAnonymous]`
- **Parámetros:**

- `eventoId` (`string`)
- `userName` (`string`)
- `role` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `Groups.AddToGroupAsync(...)`, `Clients.Group(...)`

#### `JoinOperatorsGroup`

- **Firma:** `async Task JoinOperatorsGroup()`
- **Retorno:** `Task`
- **Atributos:** `[Authorize(Roles = AuthRolePolicies.CompetitionOperators + ",Club")]`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `Groups.AddToGroupAsync(...)`

#### `LeaveRaceGroup`

- **Firma:** `async Task LeaveRaceGroup(string faseId)`
- **Retorno:** `Task`
- **Atributos:** `[AllowAnonymous]`
- **Parámetros:**

- `faseId` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `Groups.RemoveFromGroupAsync(...)`, `Clients.Group(...)`

#### `OnDisconnectedAsync`

- **Firma:** `async Task OnDisconnectedAsync(Exception? exception)`
- **Retorno:** `Task`
- **Parámetros:**

- `exception` (`Exception?`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `Clients.Group(...)`, `base.OnDisconnectedAsync(...)`

#### `RequestStartRace`

- **Firma:** `async Task RequestStartRace(int faseId, DateTime startTime)`
- **Retorno:** `Task`
- **Atributos:** `[Authorize(Roles = AuthRolePolicies.CompetitionOperators)]`
- **Parámetros:**

- `faseId` (`int`)
- `startTime` (`DateTime`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseService.IniciarFaseAsync(...)`

#### `RequestResetRace`

- **Firma:** `async Task RequestResetRace(int faseId)`
- **Retorno:** `Task`
- **Atributos:** `[Authorize(Roles = AuthRolePolicies.CompetitionOperators)]`
- **Parámetros:**

- `faseId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseService.ReiniciarFaseAsync(...)`

#### `GetServerTime`

- **Firma:** `DateTime GetServerTime()`
- **Retorno:** `DateTime`
- **Atributos:** `[AllowAnonymous]`
- **Parámetros:**

_sin parámetros_

- **Qué hace:** Obtiene/consulta datos.

#### `RecordLap`

- **Firma:** `async Task RecordLap(int faseId, int resultadoId, string time)`
- **Retorno:** `Task`
- **Atributos:** `[Authorize(Roles = AuthRolePolicies.CompetitionOperators)]`
- **Parámetros:**

- `faseId` (`int`)
- `resultadoId` (`int`)
- `time` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `Clients.Group(...)`

#### `FinishRace`

- **Firma:** `async Task FinishRace(int faseId)`
- **Retorno:** `Task`
- **Atributos:** `[Authorize(Roles = AuthRolePolicies.CompetitionOperators)]`
- **Parámetros:**

- `faseId` (`int`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `Clients.Group(...)`

#### `SendTime`

- **Firma:** `async Task SendTime(string faseId, string resultadoId, string timeStr, long ms)`
- **Retorno:** `Task`
- **Atributos:** `[Authorize(Roles = AuthRolePolicies.CompetitionOperators)]`
- **Parámetros:**

- `faseId` (`string`)
- `resultadoId` (`string`)
- `timeStr` (`string`)
- `ms` (`long`)

- **Qué hace:** Envía notificaciones. notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `Clients.Group(...)`, `_faseService.GetEventoIdByFaseIdAsync(...)`

#### `UpdateResultStatus`

- **Firma:** `async Task UpdateResultStatus(string faseId, string resultadoId, string status)`
- **Retorno:** `Task`
- **Atributos:** `[Authorize(Roles = AuthRolePolicies.CompetitionOperators)]`
- **Parámetros:**

- `faseId` (`string`)
- `resultadoId` (`string`)
- `status` (`string`)

- **Qué hace:** Actualiza un recurso existente. operación asíncrona (`await`).
- **Llamadas await destacadas:** `_faseService.UpdateResultadoStatusAsync(...)`

#### `RequestPaymentStatusChange`

- **Firma:** `async Task RequestPaymentStatusChange(string clubNombre, string clubId)`
- **Retorno:** `Task`
- **Atributos:** `[Authorize(Roles = "Admin,SuperAdmin,Club,soporte_tecnico")]`
- **Parámetros:**

- `clubNombre` (`string`)
- `clubId` (`string`)

- **Qué hace:** Ejecuta lógica de negocio asociada al nombre del método. notifica clientes vía SignalR; operación asíncrona (`await`).
- **Llamadas await destacadas:** `Clients.Group(...)`

## 5. Notas de estudio

- No uses `.Result` ni `.Wait()` sobre Tasks en ASP.NET: preferí `await` en toda la cadena.
- Ruta relativa en el proyecto: `Hubs/TimingHub.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
