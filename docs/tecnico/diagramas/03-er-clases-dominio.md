# 03 — ER y clases de dominio (API canónica)

Schemas: `seguridad`, `federacion`, `catalogos`, `regatas`, `comunicacion` (+ `Auditoria`).

Mappings detallados por entidad: `SportTrack-Sigdef.Entidades/Docs/Mappings/`.

---

## 1. Mapa de schemas

```mermaid
flowchart LR
    seguridad --> federacion
    seguridad --> catalogos
    federacion --> catalogos
    federacion --> regatas
    regatas --> catalogos
    comunicacion --> seguridad
```

---

## 2. ER — seguridad

```mermaid
erDiagram
    USUARIO ||--o| CLUB : ClubId
    USUARIO ||--o| FEDERACION : FederacionId
    USUARIO ||--o| PARTICIPANTE : ParticipanteId
    USUARIO {
        int IdUsuario PK
        string Username
        string PasswordHash
        string RolFederacion
        bool EstaActivo
    }
```

---

## 3. ER — federacion / catalogos

```mermaid
erDiagram
    FEDERACION ||--o{ CLUB : tiene
    FEDERACION }o--o| PLANSAAS : plan
    CLUB }o--o| PLANSAAS : plan
    FEDERACION {
        int Id PK
        string Nombre
        string Sigla
    }
    CLUB {
        int IdClub PK
        string Nombre
        bool Activo
    }
    PLANSAAS {
        int Id PK
        string Nombre
        decimal Precio
    }
    BOTE {
        int Id PK
        string Tipo
    }
    CATEGORIA {
        int Id PK
        int EdadMin
        int EdadMax
    }
    DISTANCIA {
        int Id PK
        string DistanciaRegata
    }
    SEXO {
        int Id PK
        string Nombre
    }
```

---

## 4. ER — personas

```mermaid
erDiagram
    PARTICIPANTE ||--o| ATLETA : "1:1"
    PARTICIPANTE ||--o| TUTOR : "1:1"
    PARTICIPANTE ||--o| ENTRENADOR : "1:1"
    PARTICIPANTE ||--o| DELEGADO : "1:1"
    ATLETA ||--o{ ATLETA_TUTOR : vinculos
    TUTOR ||--o{ ATLETA_TUTOR : vinculos
    PARTICIPANTE ||--o{ DOCUMENTACION : docs
    PARTICIPANTE {
        int IdParticipante PK
        date FechaNacimiento
        string Documento
    }
    ATLETA_TUTOR {
        int ParticipanteId FK
        int IdTutor FK
        string Parentesco
    }
    DOCUMENTACION {
        int Id PK
        string UrlArchivo
        string PublicId
    }
```

---

## 5. ER — regatas

```mermaid
erDiagram
    EVENTO ||--o{ EVENTO_PRUEBA : incluye
    PRUEBA ||--o{ EVENTO_PRUEBA : instancia
    EVENTO_PRUEBA ||--o{ INSCRIPCION : insc
    INSCRIPCION ||--o{ TRIPULANTE : crew
    EVENTO_PRUEBA ||--o{ ETAPA : etapas
    ETAPA ||--o{ FASE : fases
    FASE ||--o{ RESULTADO : resultados
    RESULTADO ||--o{ PENALIZACION : penas
    INSCRIPCION ||--o{ RESULTADO : corre
    EVENTO_PRUEBA ||--o{ REGLA_PROG : progresion
```

---

## 6. ER — comunicacion

```mermaid
erDiagram
    CAMPANA ||--o{ HILO : genera
    HILO ||--|{ MENSAJE : contiene
    HILO {
        int IdHilo PK
        string SistemaOrigen
        int IdCampana FK
    }
    CAMPANA {
        int IdCampana PK
        string SistemaOrigen
    }
    MENSAJE {
        int IdMensaje PK
        int RemitenteId FK
        int DestinatarioId FK
        datetime LeidoEn
    }
```

---

## 7. ER — pagos

```mermaid
erDiagram
    PAGO }o--o| CLUB : opcional
    PAGO }o--o| PARTICIPANTE : opcional
    PAGO }o--o| INSCRIPCION : opcional
    PAGO_TX }o--|| CLUB : club
    PAGO_TX }o--|| PARTICIPANTE : persona
```

---

## 8. Clases dominio — núcleo

```mermaid
classDiagram
    class Usuario
    class Federacion
    class Club
    class PlanSaaS
    class Participante
    class AtletaFederacion
    class TutorFederacion
    class AtletaFederacionTutor
    class Evento
    class EventoPrueba
    class Fase
    class Resultado
    class Hilo {
        +string SistemaOrigen
    }
    class Mensaje
    class CampanaEnvio {
        +string SistemaOrigen
    }
    class MensajeriaSistemaOrigen {
        <<static>>
        +Normalizar()
    }
    Federacion "1" --> "*" Club
    Participante "1" --> "0..1" AtletaFederacion
    AtletaFederacion "*" --> "*" TutorFederacion : AtletaFederacionTutor
    Evento "1" --> "*" EventoPrueba
    EventoPrueba "1" --> "*" Fase
    Fase "1" --> "*" Resultado
    CampanaEnvio "1" --> "*" Hilo
    Hilo "1" --> "*" Mensaje
    Hilo ..> MensajeriaSistemaOrigen
```

Ver también guía [../../guias/mensajeria-aislamiento.md](../../guias/mensajeria-aislamiento.md).
