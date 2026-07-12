# 05 — Secuencias y contratos (API)

## Red / clientes

```mermaid
flowchart LR
    ST[SportTrack-Front<br/>sporttrack] --> API
    SF[FrontSigdef<br/>sigdef] --> API
    LIVE[Live] --> API
    API --> DB[(PostgreSQL)]
```

### Contratos clave

| Área | Prefijo | Notas |
|------|---------|-------|
| Auth | `/api/Auth` | Login lee `X-Client-App` |
| Mensajes | `/api/mensajes` | Origen obligatorio |
| SIGDEF | `/api/Atleta`, `/Tutor`, `/Club`, … | JWT + roles |
| Regatas | `/api/Eventos`, `/Fases`, `/Resultados` | |
| Hub | `/hubs/timing` | Grupos race_/event_ |
| Docs | `/api/Documentacion` | Cloudinary opcional |
| Health | `/api/Health` | |

---

## 1. Login

```mermaid
sequenceDiagram
    participant C as Cliente
    participant Auth as AuthController
    participant S as AuthService
    participant DB as Usuarios
    C->>Auth: POST login + X-Client-App
    Auth->>S: Validate
    S->>DB: query
    S-->>C: JWT claims rol fed club
```

---

## 2. Mensaje con SistemaOrigen

```mermaid
sequenceDiagram
    participant C as Front
    participant M as MensajesController
    participant S as MensajeService
    participant DB as Hilos
    C->>M: POST hilos + header
    M->>M: MensajeriaSistemaOrigen.Normalizar
    M->>S: CrearHilo(origen)
    S->>DB: INSERT SistemaOrigen
    S-->>C: detalle
```

---

## 3. Filtro listado hilos

```mermaid
sequenceDiagram
    participant C as Front
    participant S as MensajeService
    participant R as MensajeRepository
    C->>S: GetHilos(user, origen)
    S->>R: Where SistemaOrigen == origen AND participante
    R-->>C: lista filtrada
```

---

## 4. TimingHub start + broadcast

```mermaid
sequenceDiagram
    participant J as Juez client
    participant H as TimingHub
    participant F as IFaseService
    participant G as Group race_id
    J->>H: RequestStartRace
    H->>F: start
    F-->>H: ok
    H->>G: RaceStarted
```

---

## 5. Upload documentación

```mermaid
sequenceDiagram
    participant C as FrontSigdef
    participant D as DocumentacionController
    participant S as DocumentacionService
    participant CL as Cloudinary
    participant DB as DocumentacionPersonas
    C->>D: POST upload
    alt con credenciales
        S->>CL: Upload
        CL-->>S: url publicId
    else fallback
        S->>S: data URL
    end
    S->>DB: INSERT
    S-->>C: DTO
```

---

## 6. Masivo campañas

```mermaid
sequenceDiagram
    participant C as Front
    participant S as MensajeService
    participant DB as DB
    C->>S: EnviarMasivo(ids, origen)
    S->>DB: CampanaEnvio
    loop destinatarios
        S->>DB: Hilo+Mensaje mismo origen
    end
    S-->>C: campanaId cantidad
```

---

## 7. Error header inválido

```mermaid
sequenceDiagram
    participant C as Cliente
    participant M as MensajesController
    C->>M: request sin X-Client-App válido
    M-->>C: 400 ArgumentException Normalizar
```
