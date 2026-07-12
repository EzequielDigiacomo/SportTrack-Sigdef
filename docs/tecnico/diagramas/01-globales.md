# 01 — Globales (API)

## 1. Contexto

```mermaid
flowchart TB
    subgraph Clientes
        ST[SportTrack-Front]
        SF[FrontSigdef]
        LIVE[Browser Live anónimo]
    end

    subgraph EsteRepo["SportTrack-Sigdef"]
        API[ASP.NET Core 8]
    end

    subgraph Ext
        PG[(PostgreSQL)]
        CLD[Cloudinary]
        MP[MercadoPago]
    end

    ST -->|JWT + X-Client-App:sporttrack| API
    SF -->|JWT + X-Client-App:sigdef| API
    LIVE -->|AllowAnonymous + SignalR| API
    API --> PG
    API --> CLD
    API --> MP
```

---

## 2. Contenedores

```mermaid
flowchart TB
    REST[Controllers REST /api]
    HUB[TimingHub /hubs/timing]
    SVC[Services]
    REPO[Repositories]
    EF[SportTrackDbContext]
    DB[(PostgreSQL schemas)]

    REST --> SVC
    HUB --> SVC
    SVC --> REPO
    REPO --> EF
    SVC --> EF
    EF --> DB
```

---

## 3. Capas

```mermaid
flowchart TB
    subgraph Host["SportTrack-Sigdef"]
        CTRL[Controllers]
        PROG[Program.cs DI JWT CORS SignalR]
    end
    subgraph App["Controladores + AccesoDatos"]
        SERV[I*Service implementations]
        REP[I*Repository]
        HUBC[TimingHub]
    end
    subgraph Dom["Entidades"]
        ENT[Entidades + MensajeriaSistemaOrigen]
    end
    subgraph Infra
        EF[EF Core + Migraciones]
        EXT[Cloudinary / MP]
    end

    CTRL --> SERV
    HUBC --> SERV
    SERV --> REP
    SERV --> ENT
    REP --> EF
    SERV --> EXT
    PROG --> CTRL
```

---

## 4. Despliegue

```mermaid
flowchart LR
    Clients[Fronts / Live] --> Kestrel[API Kestrel]
    Kestrel --> PG[(PostgreSQL)]
    Kestrel --> CLD[Cloudinary]
    Kestrel --> MP[MercadoPago]
```

---

## 5. Despliegue detallado

```mermaid
flowchart TB
    subgraph Local
        SW[Swagger :5029]
        MIG[MigrateAsync on startup]
        PGL[(PG local/remoto)]
    end
    subgraph Prod
        RENDER[Render / IIS]
        ENV[Env: JWT CS CLOUDINARY MP]
        PGP[(PG managed)]
    end
    SW --> PGL
    MIG --> PGL
    RENDER --> ENV
    RENDER --> PGP
```

Ver [../../guias/operacion-local.md](../../guias/operacion-local.md).

---

## 6. Paquetes de la solución

```mermaid
flowchart LR
    HOST[SportTrack-Sigdef] --> CTRL[Controladores]
    HOST --> AD[AccesoDatos]
    CTRL --> ENT[Entidades]
    AD --> ENT
    HOST --> ENT
```

---

## 7. Componentes API (módulos)

```mermaid
flowchart TB
    subgraph AuthMod[Auth]
        AuthC[AuthController]
        AuthS[AuthService TokenService]
    end
    subgraph SigdefMod[SIGDEF]
        Atl[Atleta Tutor Club Usuario…]
        Doc[DocumentacionService]
    end
    subgraph RegatasMod[Regatas]
        Ev[Eventos Inscripciones]
        Fas[FaseService Resultados]
        Hub[TimingHub]
    end
    subgraph MsgMod[Mensajes]
        MsgC[MensajesController]
        MsgS[MensajeService]
    end
    subgraph PayMod[Pagos SaaS]
        Pay[PagoService]
        SaaS[SaaSService]
    end
    AuthC --> AuthS
    MsgC --> MsgS
    Hub --> Fas
```
