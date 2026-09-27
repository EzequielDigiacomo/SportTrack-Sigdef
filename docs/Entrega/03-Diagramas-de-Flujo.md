# 03 · Diagramas de Flujo

**Última actualización: 2026-09-27**
**Notación:** Mermaid (flowchart, stateDiagram-v2, sequenceDiagram). Todos los diagramas verificados contra el código.

---

## Índice

1. [Pipeline HTTP y autenticación/autorización](#1-pipeline-http-y-autenticaciónautorización)
2. [Ciclo de vida evento / fase / resultado](#2-ciclo-de-vida-evento--fase--resultado)
3. [Motor de progresión ICF](#3-motor-de-progresión-icf)
4. [Cronometraje SignalR + outbox](#4-cronometraje-signalr--outbox)
5. [Reseteo de regata](#5-reseteo-de-regata)
6. [Enforcement de planes SaaS](#6-enforcement-de-planes-saas)
7. [Mensajería aislada por sistema de origen](#7-mensajería-aislada-por-sistema-de-origen)
8. [Secuencia de login](#8-secuencia-de-login)
9. [Backup](#9-backup)

---

## 1. Pipeline HTTP y autenticación/autorización

```mermaid
flowchart TD
    A[Request HTTP / SignalR negotiate] --> B[UseForwardedHeaders]
    B --> C[ExceptionMiddleware]
    C --> D[SecurityHeadersMiddleware]
    D --> E{¿Development?}
    E -- No --> F[UseHsts + UseHttpsRedirection]
    E -- Sí --> G[UseSwagger + SwaggerUI]
    F --> H[UseCors CorsPolicy]
    G --> H
    H --> I[UseRateLimiter]
    I --> J[UseAuthentication JWT]
    J --> K[UseAuthorization FallbackPolicy]
    K --> L{¿Endpoint AllowAnonymous?}
    L -- Sí --> M[Ejecuta Controller / Hub]
    L -- No --> N{¿Token válido?}
    N -- No --> O[401 Unauthorized]
    N -- Sí --> P{¿Rol autorizado?}
    P -- No --> Q[403 Forbidden]
    P -- Sí --> M
    M --> R[MapControllers / MapHub timing]
    C -. excepción .-> S[404 / 401 / 400 / 500 sanitizado + auditoría ERROR_FATAL]

    style A fill:#e8f4ff
    style O fill:#ffe0e0
    style Q fill:#ffe0e0
    style S fill:#fff3cd
```

**Orden real del pipeline (`Program.cs`):** `ExceptionMiddleware → SecurityHeadersMiddleware → HSTS/HTTPS (no Dev) → Swagger (Dev) → CORS → RateLimiter → Authentication → Authorization → MapControllers → MapHub<TimingHub>('/hubs/timing').AllowAnonymous()`.

**Resolución del token (`OnMessageReceived`):** `Authorization: Bearer` → `?access_token` (SignalR) → cookie `X-Access-Token`.

---

## 2. Ciclo de vida evento / fase / resultado

### 2.1 Estado del evento

```mermaid
stateDiagram-v2
    [*] --> Programada
    Programada --> EnCurso: día local del evento
    EnCurso --> Finalizado: cierre por día local
    Finalizado --> [*]
    note right of Programada
      EventoEstadoSyncService
      (background 15 min + arranque)
      Nunca retrocede
    end note
```

### 2.2 Estado de la fase

```mermaid
stateDiagram-v2
    [*] --> Programada
    Programada --> EnCurso: Iniciar / RequestStartRace
    EnCurso --> Finalizada: Finalizar
    EnCurso --> Programada: Reiniciar (motivo >= 5, limpia tiempos)
    Finalizada --> EnRevision: EnviarARevision
    EnRevision --> Finalizada: (revisión cerrada)
    Programada --> Eliminada: DELETE
    Finalizada --> Eliminada: DELETE
    Eliminada --> [*]
```

### 2.3 Estado del resultado

```mermaid
stateDiagram-v2
    [*] --> Pendiente
    Pendiente --> EnCurso: largada
    EnCurso --> Finalizado: tiempo oficial
    EnCurso --> DNS: no largó
    EnCurso --> DNF: no finalizó
    EnCurso --> Descalificado: DSQ
    Finalizado --> [*]
    DNS --> [*]
    DNF --> [*]
    Descalificado --> [*]
    note right of DNS
      DNS/DNF/DSQ limpian
      tiempo y posición
    end note
```

---

## 3. Motor de progresión ICF

```mermaid
flowchart TD
    A[POST api/Fases/Generar/eventoPruebaId] --> B[ProgressionPlanRegistry.ResolveDefaultPlan]
    B --> C{Cantidad de participantes}
    C -- 10-18 --> D[Plan A]
    C -- 19-27 --> E[Plan B]
    C -- ... --> F[Planes C..F]
    C -- 64-72 --> G[Plan G]
    D --> H[Variante 1 o 2]
    E --> H
    F --> H
    G --> H
    H --> I[ProgressionEngine]
    I --> J[Ranking: excluye Descalificado/DNS/DNF<br/>prefiere TiempoOficial]
    J --> K[Asignar carriles<br/>prioridad 5,4,6,3,7,2,8,1,9]
    K --> L{¿Carril ocupado?}
    L -- Sí --> M[Lanza excepción 400]
    L -- No --> N[Pre-generar SF y finales A/B/C]
    N --> O[Auditoría GENERATE_HEATS_AUTO]
    O --> P[Fases persistidas]

    subgraph Promoción
      Q[POST api/Fases/Promover/eventoPruebaId] --> R{¿Resultados completos?}
      R -- No --> S[400 sin cambios]
      R -- Sí --> T[Clasificados por mejor tiempo]
      T --> U[Auditoría PROMOTE_STAGE]
    end

    style M fill:#ffe0e0
    style S fill:#ffe0e0
```

**Alias:** `PlanX` (comodín). **Maratón:** `GenerarLargadaMaraton` crea una única fase `Largada` con `Carril = NumeroCompetidor` y audita `GENERATE_LARGADA_MARATON`.

---

## 4. Cronometraje SignalR + outbox

```mermaid
sequenceDiagram
    autonumber
    participant C as Cronometrista
    participant H as TimingHub (/hubs/timing)
    participant FS as FaseService
    participant OB as TimingOutboxService
    participant DB as PostgreSQL
    participant LIVE as Espectadores (grupo event_)

    C->>H: JoinRaceGroup(faseId, user, role)
    H-->>C: RacePresenceUpdated
    C->>H: GetServerTime()
    H-->>C: hora UTC

    alt Online
        C->>H: SendTime(faseId, resultadoId, timeStr, ms)
        H-->>C: TimeReceived
        H-->>LIVE: globalTimeReceived
        C->>H: UpdateResultStatus(faseId, resultadoId, status)
        H->>FS: UpdateResultadoStatusAsync
        C->>H: FinishRace(faseId)
        H-->>C: RaceFinished
    else Offline
        C->>OB: POST api/timing-outbox (upsert FaseId+Username)
        OB->>DB: Persistir envío pendiente (TTL 24h)
        C->>OB: GET api/timing-outbox/pending
        C->>OB: POST api/timing-outbox/flush
    end
```

---

## 5. Reseteo de regata

```mermaid
sequenceDiagram
    autonumber
    participant L as Largador
    participant H as TimingHub
    participant FS as FaseService
    participant DB as PostgreSQL
    participant AUD as Auditoría
    participant R as Grupo race_ + event_

    L->>H: RequestResetRace(faseId, motivo, categoria)
    H->>FS: ReiniciarFaseAsync(faseId, motivo, categoria)
    alt motivo < 5 caracteres
        FS-->>L: 400 Bad Request
    else motivo válido
        FS->>DB: Limpiar tiempos de la fase
        FS->>DB: Fase -> Programada
        FS->>AUD: Registrar RESET_RACE
        FS->>R: Emitir RaceReset
        R-->>L: RaceReset
    end
```

---

## 6. Enforcement de planes SaaS

```mermaid
flowchart TD
    A[Usuario / Club] --> B[LoginAsync o GetMeAsync]
    B --> C[PlanSaaSAccessHelper.FromEntity]
    C --> D{ResolveByPlanId}
    D -- 1-3 SIGDEF --> E[AccesoSigdef=true, Live=false]
    D -- 4-6 SportTrack --> F[AccesoSportTrack=true, Live solo id 6]
    D -- 7-9 Pack Dúo --> G[Ambos=true, Live solo id 9]

    B --> H{Usuario activo?}
    H -- No --> I[401/403]
    H -- Sí --> J{PlanAlDia?}
    J -- No --> I
    J -- Sí --> K{X-Client-App con acceso?}
    K -- No --> I
    K -- Sí --> L[Acceso concedido]

    subgraph Alta de usuarios
      M[POST api/Auth/register] --> N[PlanSaaSAccessHelper.CanCreateRole]
      N --> O{Club? -> AccesoDashboardClub}
      N --> P{Juez? -> AccesoControlesLive}
      N -- No permitido --> Q[403]
      N -- Permitido --> R[Usuario creado]
    end

    style I fill:#ffe0e0
    style Q fill:#ffe0e0
```

> ⚠️ **Deuda:** `MaxAtletas` no se aplica aún en el alta de atletas; `MaxTorneosActivos` quedó en `-1` (sin enforcement efectivo).

---

## 7. Mensajería aislada por sistema de origen

```mermaid
flowchart LR
    A[Request api/mensajes] --> B[X-Client-App header]
    B --> C{MensajeriaSistemaOrigen}
    C -- sporttrack --> D[Filtra origen sporttrack]
    C -- sigdef --> E[Filtra origen sigdef]
    D --> F[Aplica tenant IdFederacion]
    E --> F
    F --> G{Tipo de operación}
    G -- Leer/listar --> H[Hilos y no-leídos filtrados]
    G -- Escribir --> I[PuedeEscribir]
    I --> J{SuperAdmin-Admin o Admin-Club misma fed?}
    J -- No --> K[403]
    J -- Sí --> L[Mensaje persistido]
    G -- Masivo --> M{Solo SuperAdmin/Admin}
    M -- Sí --> N[CampanaEnvio + notificación SignalR]
    M -- No --> K

    style K fill:#ffe0e0
```

**Notificación:** `newMessageReceived` → grupo `user_{username}`.

---

## 8. Secuencia de login

```mermaid
sequenceDiagram
    autonumber
    participant Cli as Cliente
    participant AC as AuthController
    participant AS as AuthService
    participant DB as PostgreSQL
    participant TS as TokenService

    Cli->>AC: POST api/Auth/login (X-Client-App)
    Note over AC: Rate limit auth 20/min/IP
    AC->>AS: LoginAsync(credentials, clientApp)
    AS->>DB: Buscar usuario por username
    DB-->>AS: Usuario
    alt Credenciales válidas + activo + plan al día + acceso por origen
        AS->>TS: Generar JWT (HMAC-SHA512)
        TS-->>AS: Token
        AS-->>AC: Token + perfil
        AC-->>Cli: 200 + Set-Cookie X-Access-Token
    else Credenciales inválidas
        AS->>DB: Incrementar intentos
        alt 5 intentos fallidos
            AS->>DB: EstaActivo = false
        end
        AS-->>AC: Error
        AC-->>Cli: 401
    else Bloqueado por pago/vencimiento/origen
        AC-->>Cli: 403
    end
```

---

## 9. Backup

```mermaid
sequenceDiagram
    autonumber
    participant SA as SuperAdmin
    participant BC as BackupController
    participant BS as BackupService
    participant PG as PostgreSQL (pg_dump)
    participant AUD as Auditoría

    SA->>BC: GET api/Backup/download?scope=full
    BC->>BS: Generar backup
    BS->>PG: pg_dump (PGPASSWORD por env)
    PG-->>BS: Volcado SQL
    BS->>AUD: Registrar en historial
    BS-->>BC: Archivo
    BC-->>SA: Descarga

    SA->>BC: GET api/Backup/download?scope=federacion&idFederacion=N
    BC->>BS: Generar backup por federación
    BS->>PG: SELECT + generar INSERTs
    PG-->>BS: Datos
    BS-->>BC: Archivo
    BC-->>SA: Descarga
```

---

## Pie de página

- ➡️ [04 · Manual de Uso de la API](./04-Manual-de-Uso-API.md)
- ➡️ [05 · Manual Técnico](./05-Manual-Tecnico.md)
- ⬅️ [02 · Casos de Uso](./02-Casos-de-Uso.md) · [Volver al índice del Pack](./README.md)
