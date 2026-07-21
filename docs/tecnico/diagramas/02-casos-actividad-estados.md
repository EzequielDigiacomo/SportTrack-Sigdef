# 02 — Casos de uso, actividad y estados (API)

## 1. Casos de uso (capacidades expuestas)

```mermaid
flowchart TB
    ST((SportTrack-Front))
    SF((FrontSigdef))
    LIVE((Live anónimo))

    ST --> A1[Auth JWT]
    ST --> A2[Eventos / fases / resultados]
    ST --> A3[TimingHub write+read]
    ST --> A4[Mensajes origen sporttrack]
    ST --> A5[Pagos / SaaS admin]

    SF --> B1[Auth JWT]
    SF --> B2[CRUD personas fed/club]
    SF --> B3[Mensajes origen sigdef]
    SF --> B4[Documentacion Cloudinary]
    SF --> B5[Pagos afiliación]

    LIVE --> C1[GET Live públicos]
    LIVE --> C2[SignalR Join* + receive]
```

---

## 2. Actividad

### Request autenticado típico

```mermaid
flowchart TD
    A[HTTP + Bearer] --> B{JWT válido?}
    B -->|No| C[401]
    B -->|Sí| D[Controller]
    D --> E[Service]
    E --> F[EF / Repo]
    F --> G[JSON response]
```

### Crear hilo con aislamiento

```mermaid
flowchart TD
    A[POST /mensajes/hilos] --> B[Leer X-Client-App]
    B --> C{Normalizar origen}
    C -->|inválido| D[400]
    C -->|ok| E[Set SistemaOrigen]
    E --> F[INSERT Hilo + Mensaje]
    F --> G[201 detalle]
```

### Start race vía Hub

```mermaid
flowchart TD
    A[Client Invoke RequestStartRace] --> B[TimingHub]
    B --> C[IFaseService]
    C --> D[Update Fase + broadcast grupo]
```

### Migración al arrancar

```mermaid
flowchart TD
    A[Program startup] --> B[MigrateAsync]
    B --> C{OK?}
    C -->|Sí| D[App ready]
    C -->|No| E[Log / fail host]
```

---

## 3. Estados (persistidos / runtime)

Documentación completa SportTrack (evento, fases, resultados, inscripciones):  
[../estados-eventos-fases-sporttrack.md](../estados-eventos-fases-sporttrack.md)

### Usuario

```mermaid
stateDiagram-v2
    [*] --> Activo
    Activo --> Inactivo: toggle / lock intentos
    Inactivo --> Activo
```

### Evento (SportTrack)

Sync **automático** por fechas + override **manual** admin. `Cancelado` no se toca.

```mermaid
stateDiagram-v2
    [*] --> Programada
    Programada --> EnCurso: fecha/hora inicio (auto o manual)
    EnCurso --> Finalizado: fin del día FechaFin (auto o manual)
    Programada --> Finalizado: evento ya terminó
    Programada --> Cancelado: admin
    EnCurso --> Cancelado: admin
```

### Fase (serie / carrera)

Sync **manual** vía jueces (API `/api/fases/{id}/...`).

```mermaid
stateDiagram-v2
    [*] --> Programada
    Programada --> EnCarrera: Iniciar (largador)
    EnCarrera --> Finalizada: Finalizar (cronometrista)
    EnCarrera --> PendienteValidacion: EnviarARevision
    Programada --> Programada: Reiniciar
    EnCarrera --> Programada: Reiniciar
    Finalizada --> Programada: Reiniciar
    PendienteValidacion --> Programada: Reiniciar
```

### Resultado (por carril)

```mermaid
stateDiagram-v2
    [*] --> Pendiente
    Pendiente --> Preliminar: EnviarARevision
    Preliminar --> Oficial: Finalizar fase
    Pendiente --> Oficial: Finalizar fase con tiempo
    Pendiente --> DSQ: carga manual
    Pendiente --> DNS: carga manual
    Pendiente --> DNF: carga manual
```

### Hilo/Mensaje

```mermaid
stateDiagram-v2
    [*] --> Creado: SistemaOrigen set
    Creado --> ConMensajes
    ConMensajes --> ParcialLeido
    ParcialLeido --> Respondido
```

### PagoFederacionTransaccion

```mermaid
stateDiagram-v2
    [*] --> Pendiente
    Pendiente --> Aprobado
    Pendiente --> Rechazado
    Aprobado --> [*]
```
