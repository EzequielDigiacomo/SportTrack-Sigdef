# 01 · Requerimientos de Usuario

**Última actualización: 2026-09-27**
**Estándares aplicados:** ISO/IEC/IEEE 29148 (ingeniería de requerimientos), ISO/IEC 25010 (calidad de producto), Gherkin (historias de usuario).

---

## Índice

1. [Actores y roles](#1-actores-y-roles)
2. [Requerimientos funcionales (RF)](#2-requerimientos-funcionales-rf)
3. [Requerimientos no funcionales (RNF — ISO 25010)](#3-requerimientos-no-funcionales-rnf--iso-25010)
4. [Historias de usuario (HU — Gherkin)](#4-historias-de-usuario-hu--gherkin)
5. [Reglas de negocio (RN)](#5-reglas-de-negocio-rn)
6. [Matriz de trazabilidad](#6-matriz-de-trazabilidad)

---

## 1. Actores y roles

### 1.1 Actores

| Actor | Descripción | Autenticación |
|-------|-------------|---------------|
| **Visitante / público** | Espectador del Live (resultados en vivo) | Anónimo |
| **Usuario autenticado** | Cualquier portador de JWT válido | JWT |
| **Operador de competencia** | Admin/SuperAdmin/jueces que operan carrera | JWT |
| **Cronometrista** | Toma y envía tiempos en pista (online/offline) | JWT (rol Cronometrista) |
| **Administrador de federación** | Gestiona clubes, atletas, traspasos, mensajería | JWT (rol Admin) |
| **Club** | Gestiona su plantel y consulta su estado | JWT (rol Club) |
| **SuperAdmin / soporte** | Administración SaaS, auditoría, backups | JWT |
| **Procesos internos** | Sincronización de estados, snapshots de audiencia, purgas de outbox | Background services |
| **Sistemas externos** | MercadoPago (webhook), Cloudinary, frontends SPAs | Webhook / CORS |

### 1.2 Roles y políticas de autorización

Los roles viven en el claim `ClaimTypes.Role` del JWT y se agrupan en constantes de `AuthRolePolicies` (proyecto `Controladores`).

| Rol | `RolFederacion` | En `RegisterableRoles` | Pertenece a `CompetitionOperators` | Privilegiado |
|-----|:---:|:---:|:---:|:---:|
| `SuperAdmin` | ✅ | ❌ (nunca desde cliente) | ✅ | ✅ |
| `Admin` | ✅ | ✅ | ✅ | ✅ |
| `soporte_tecnico` | ✅ | ✅ | ✅ | ✅ |
| `Club` | ✅ | ✅ | ❌ | ❌ |
| `Largador` | ✅ | ✅ | ✅ | ❌ |
| `Cronometrista` | ✅ | ✅ | ✅ | ❌ |
| `JuezControl` | ✅ | ✅ | ✅ | ❌ |
| `ControlTecnico` | ✅ | ✅ | ✅ | ❌ |

**Constantes de rol:**

| Constante | Valor literal |
|-----------|---------------|
| `AuthRolePolicies.CompetitionOperators` | `Admin,SuperAdmin,JuezControl,Largador,Cronometrista,ControlTecnico,soporte_tecnico` |
| `AuthRolePolicies.Admins` | `Admin,SuperAdmin,soporte_tecnico` |
| `AuthRolePolicies.RegisterableRoles` | `Club,Admin,Largador,Cronometrista,JuezControl,ControlTecnico,soporte_tecnico` |
| `AuthRolePolicies.PrivilegedRoles` | `Admin,SuperAdmin,soporte_tecnico` |

**Roles de juez** (`PlanSaaSAccessHelper.IsJudgeRole`): `Largador`, `Cronometrista`, `JuezControl`, `ControlTecnico`.

**Política por defecto:** `FallbackPolicy = RequireAuthenticatedUser` — todo endpoint es privado salvo `[AllowAnonymous]` explícito (Live, login, health, webhook, frontend-error).

---

## 2. Requerimientos funcionales (RF)

### 2.1 Autenticación y seguridad

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-01** | Iniciar sesión con usuario/contraseña, emitiendo JWT (HMAC-SHA512) y cookie `X-Access-Token`; lee `X-Client-App` para aislar por sistema de origen | `POST api/Auth/login` | Anónimo (rate `auth`) |
| **RF-02** | Cerrar sesión y limpiar la cookie de acceso | `POST api/Auth/logout` | Anónimo |
| **RF-03** | Solicitar reseteo de contraseña con **respuesta genérica** (no filtra existencia) | `POST api/Auth/solicitar-reset-password` | Anónimo (rate `auth`) |
| **RF-04** | Registrar usuarios, validando rol contra `CanCreateRole` del plan | `POST api/Auth/register` | Admins (rate `auth`) |
| **RF-05** | Listar usuarios del ámbito (federación/sistema) | `GET api/Auth/usuarios` | Autenticado |
| **RF-06** | Cambiar contraseña de usuario / actualizar perfil | `PUT api/Auth/usuarios/{id}/password`<br>`PUT api/Auth/usuarios/{id}/perfil` | Autenticado |
| **RF-07** | Activar/desactivar usuario | `PATCH api/Auth/usuarios/{id}/toggle-activo` | Admin, SuperAdmin, soporte_tecnico |
| **RF-08** | Eliminar usuario (baja de login) | `DELETE api/Auth/usuarios/{id}` | Admin, SuperAdmin, soporte_tecnico |
| **RF-09** | Obtener el perfil del usuario autenticado (con enforcement de plan/estado) | `GET api/Auth/me` | Autenticado |

### 2.2 Eventos

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-10** | Listar eventos y obtener detalle/fases | `GET api/Eventos`, `GET api/Eventos/{id}`, `GET api/Eventos/{id}/fases` | Auth / Live anónimo |
| **RF-11** | Listar próximos eventos | `GET api/Eventos/proximos` | Anónimo |
| **RF-12** | Crear, editar y eliminar eventos; consultar debug | `POST api/Eventos`, `PUT api/Eventos/{id}`, `DELETE api/Eventos/{id}`, `GET api/Eventos/debug` | Autenticado |
| **RF-13** | Gestionar pruebas del evento y largada de maratón | `POST api/Eventos/{id}/pruebas`, `PUT api/Eventos/pruebas/{id}`, `DELETE api/Eventos/pruebas/{id}`, `POST api/Eventos/{id}/pruebas/largada` | Autenticado |

### 2.3 Fases y progresión ICF

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-14** | Generar fases automáticamente por plan ICF (A1–G2), con cabezas de serie y pre-generación de SF/finales | `POST api/Fases/Generar/{eventoPruebaId}` | CompetitionOperators |
| **RF-15** | Generar fases manualmente | `POST api/Fases/GenerarManual/{eventoPruebaId}` | CompetitionOperators |
| **RF-16** | Generar largada única de maratón (una fase `Largada`, carril = número de competidor) | `POST api/Fases/GenerarLargadaMaraton` | CompetitionOperators |
| **RF-17** | Promover etapa (valida resultados completos y arma la siguiente ronda) | `POST api/Fases/Promover/{eventoPruebaId}` | CompetitionOperators |
| **RF-18** | Iniciar / finalizar / reiniciar fase y enviar a revisión | `POST api/Fases/{id}/Iniciar`, `.../Finalizar`, `.../Reiniciar`, `.../EnviarARevision` | CompetitionOperators |
| **RF-19** | Actualización masiva de fases y consulta de auditoría de progresión | `POST api/Fases/BatchUpdate`, `GET api/Fases/ProgresionAudit/{id}` | CompetitionOperators / Live |
| **RF-20** | Eliminar fase | `DELETE api/Fases/{id}` | CompetitionOperators |

### 2.4 Resultados y Live

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-21** | Consultar resultados de una fase (cache 30 s) con posiciones y estados | `GET api/Resultados/Fase/{faseId}` | Live anónimo (rate `live`) |
| **RF-22** | Guardar resultados en lote (limpieza de tiempo/posición en DNS/DNF/DSQ; audita `SAVE_TIMING`; emite `ResultadoActualizado`) | `PUT api/Resultados/BatchUpdate` | CompetitionOperators |

### 2.5 Inscripciones

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-23** | CRUD de inscripciones + filtros por evento/prueba/club y toggle de seeding | `GET api/Inscripciones`, `GET /{id}`, `GET /registro`, `GET /evento-prueba/{id}`, `GET /evento/{id}/club/{id}`, `POST`, `PUT /{id}`, `DELETE /{id}`, `PATCH /{id}/toggle-seeding` | Autenticado |

### 2.6 Catálogos y participantes

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-24** | CRUD de participantes y filtro por club | `api/Participantes`, `GET api/Participantes/club/{clubId}` | Autenticado |
| **RF-25** | CRUD de clubes SportTrack, botes, categorías y distancias | `api/Clubes`, `api/Botes`, `api/Categorias`, `api/Distancias` | Autenticado |
| **RF-26** | Operaciones legacy de pruebas por evento | `api/legacy/eventos/{idEvento}/pruebas` | Autenticado |

### 2.7 SIGDEF (federación)

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-27** | CRUD de atletas federados + alta completa + liberar a Agente Libre | `api/Atleta` (`GET paged`, `POST`, `POST full`, `POST {id}/liberar`) | Autenticado |
| **RF-28** | CRUD de clubes, delegados, entrenadores, tutores, personas, roles y asociaciones atleta-tutor | `api/Club`, `api/DelegadoClub`, `api/Entrenador`, `api/Tutor`, `api/Persona`, `api/Rol`, `api/AtletaTutor` | Autenticado |
| **RF-29** | Gestionar traspasos: periodos, búsqueda, validaciones, aceptación/rechazo por origen, aprobación forzada, cancelación, auditoría y exportación CSV | `api/Traspaso/**` | Autenticado |
| **RF-30** | Subir documentación a Cloudinary (multipart) y listar/eliminar | `POST api/Documentacion/upload`, `GET api/Documentacion/persona/{id}`, `DELETE api/Documentacion/{id}` | Autenticado |
| **RF-31** | CRUD de usuarios SIGDEF | `api/Usuario` | Autenticado |

### 2.8 Pagos y MercadoPago

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-32** | Consultar historial de pagos y registrar pago | `GET api/Pagos/historial`, `POST api/Pagos/registrar` | Autenticado |
| **RF-33** | Togglear/solicitar pago de clubes, togglear pago de atletas e inscripciones | `PUT api/Pagos/clubes/{id}/toggle`, `.../solicitar-pago`, `PUT api/Pagos/atletas/{id}/toggle`, `PUT api/Pagos/inscripciones/{id}/toggle` | Autenticado |
| **RF-34** | Eliminar pago (individual o masivo) | `DELETE api/Pagos/{id:int}`, `DELETE api/Pagos/bulk` | Autenticado |
| **RF-35** | Crear preferencia de pago MercadoPago y CRUD de transacciones | `POST api/PagoTransaccion/preferencia` + CRUD | Autenticado |
| **RF-36** | Recibir webhook de MercadoPago | `POST api/Notificacion/webhook` | Anónimo ⚠️ sin validación de firma (deuda) |

### 2.9 Mensajería

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-37** | Listar/leer hilos, responder, marcar leído y contar no leídos, todo aislado por `X-Client-App` | `api/mensajes/**` | SuperAdmin, Admin, Club |
| **RF-38** | Enviar mensaje masivo y consultar campañas | `POST api/mensajes/hilos/masivo`, `GET api/mensajes/campanas`, `GET api/mensajes/campanas/{id}` | SuperAdmin, Admin |

### 2.10 SaaS

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-39** | Consultar planes (público) y depurar el plan propio | `GET api/SaaS/planes`, `GET api/SaaS/debug-me` | Anónimo / SuperAdmin, soporte |
| **RF-40** | Actualizar plan (calcula `PrecioAnual`; `MaxTorneosActivos = -1`) | `PUT api/SaaS/planes/{id}` | SuperAdmin |
| **RF-41** | Asignar plan a club, listar estado de clubes y toggle de activación | `POST api/SaaS/asignar-plan`, `GET api/SaaS/clubes-status`, `PATCH api/SaaS/clubes/{id}/toggle-activo` | SuperAdmin, Admin, soporte |
| **RF-42** | Crear federación con admin (mapea error único `23505`) | `POST api/SaaS/create-federacion` | SuperAdmin |
| **RF-43** | Consultar métricas globales | `GET api/SaaS/global-metrics` | SuperAdmin, soporte |

### 2.11 Auditoría y soporte

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-44** | Consultar auditoría general y agrupada por eventos; registrar acción de cliente; purgar por evento o por id | `api/Auditoria/**` | Autenticado |
| **RF-45** | Diagnóstico: chequeo de eventos y búsqueda | `GET api/Diagnostic/check-eventos`, `GET api/Diagnostic/search/{query}` | Admin, SuperAdmin, soporte |
| **RF-46** | Soporte: eventos, acciones de cliente, timing-outbox, logs y reporte de error de frontend | `api/Support/**` | Autenticado / Anónimo (`frontend-error`) |

### 2.12 Backups

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-47** | Descargar backup (`full` con `pg_dump` / `federacion` con INSERTs) y consultar historial | `GET api/Backup/download?scope=full\|federacion&idFederacion=N`, `GET api/Backup/history` | SuperAdmin, soporte |

### 2.13 Timing Outbox (cronometraje offline)

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-48** | Encolar, listar pendientes, commit por fase, flush y eliminar envíos de timing | `POST api/timing-outbox`, `GET /pending`, `POST /flush`, `POST /{faseId}/commit`, `DELETE /{faseId}` | CompetitionOperators |
| **RF-49** | Commit de cola desde soporte | `POST api/Support/timing-outbox/{faseId}/commit`, `DELETE api/Support/timing-outbox/{id}` | Autenticado |

### 2.14 Audience (audiencia en vivo)

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-50** | Consultar audiencia live, picos y capacidad, y ajustar capacidad | `GET api/Audience/live`, `/peaks`, `/capacity`, `PUT api/Audience/capacity` | SuperAdmin, soporte |

### 2.15 Notificaciones en tiempo real

| ID | Requerimiento | Mecanismo | Roles |
|----|---------------|-----------|-------|
| **RF-51** | Notificar mensajes nuevos al usuario (`newMessageReceived` → grupo `user_{username}`) | SignalR | Usuario destino |
| **RF-52** | Notificar eventos nuevos a la federación (`newEventCreated` → grupo `fed_{federacionId}`) | SignalR | Miembros de federación |
| **RF-53** | Notificar cambios de estado de resultados y solicitudes de cambio de pago | SignalR | Operadores |

### 2.16 Health

| ID | Requerimiento | Endpoint(s) | Roles |
|----|---------------|-------------|-------|
| **RF-54** | Verificar salud del servicio y de la base de datos | `GET api/Health`, `GET api/Health/db` | Anónimo |

---

## 3. Requerimientos no funcionales (RNF — ISO 25010)

### 3.1 Adecuación funcional

| ID | Requerimiento |
|----|---------------|
| **RNF-01** | El backend debe cubrir los dominios SportTrack, SIGDEF y SaaS en un único despliegue y una única base de datos. |
| **RNF-02** | Toda operación de escritura relevante debe quedar auditada (acción, usuario, detalle, timestamp UTC). |

### 3.2 Eficiencia de desempeño

| ID | Requerimiento |
|----|---------------|
| **RNF-03** | Las lecturas Live deben usar cache en memoria (`ILiveCacheService`) con TTL de 30–45 s. La fase cachea 30 s. |
| **RNF-04** | La base de datos debe usar `EnableRetryOnFailure(3)` y `CommandTimeout = 30 s`. |
| **RNF-05** | El rate limiting debe limitar `auth` a 20 req/min/IP y `live` a 120 req/min/IP (HTTP 429). |

### 3.3 Compatibilidad

| ID | Requerimiento |
|----|---------------|
| **RNF-06** | CORS por lista blanca con `AllowCredentials` (no `AllowAnyOrigin`), incluyendo dominios propios, Vercel, Capacitor/Ionic y localhost de desarrollo. |
| **RNF-07** | Compatibilidad con señalización SignalR vía query string `?access_token` y cookie `X-Access-Token`. |
| **RNF-08** | Debe soportar clientes móviles empaquetados con Capacitor (`capacitor://localhost`, `ionic://localhost`). |

### 3.4 Usabilidad

| ID | Requerimiento |
|----|---------------|
| **RNF-09** | Swagger UI disponible **solo** en Development, con botón Authorize Bearer. |
| **RNF-10** | Los errores deben devolver mensajes sanitizados y códigos HTTP coherentes (400/401/403/404/429/500). |

### 3.5 Fiabilidad

| ID | Requerimiento |
|----|---------------|
| **RNF-11** | Las migraciones EF Core deben aplicarse automáticamente al arranque. |
| **RNF-12** | La sincronización de estados de evento corre como background service cada 15 min (nunca retrocede de estado). |
| **RNF-13** | El outbox de timing debe reintentar/purgar entradas con TTL de 24 h. |

### 3.6 Seguridad

| ID | Requerimiento |
|----|---------------|
| **RNF-14** | Autenticación JWT firmada con HMAC-SHA512; `TokenKey` obligatorio fuera de Development. |
| **RNF-15** | Autorización deny-by-default (`FallbackPolicy`) + roles por endpoint. |
| **RNF-16** | Lockout: 5 intentos fallidos → `EstaActivo = false`. |
| **RNF-17** | Security headers: `X-Content-Type-Options`, `X-Frame-Options: DENY`, `Referrer-Policy`, `Permissions-Policy`. |
| **RNF-18** | Reglas de carga de archivos: máx. 6 MB; `jpg/jpeg/png/webp/gif/pdf`. |
| **RNF-19** | Contraseñas almacenadas con BCrypt. |
| **RNF-20** | Recomendación: agregar validación de firma al webhook de MercadoPago (deuda actual). |

### 3.7 Mantenibilidad

| ID | Requerimiento |
|----|---------------|
| **RNF-21** | Arquitectura en 4 proyectos con separación por capas (Api / Controladores / AccesoDatos / Entidades). |
| **RNF-22** | Codificación UTF-8 forzada por `Directory.Build.props`. |
| **RNF-23** | Desacoplamiento vía interfaces (Services/Repositories) e inyección de dependencias. |

### 3.8 Portabilidad

| ID | Requerimiento |
|----|---------------|
| **RNF-24** | Target `net10.0`; contenedor Docker `mcr.microsoft.com/dotnet/aspnet:10.0-noble` con `EXPOSE 8080`. |
| **RNF-25** | Debe leer la cadena de conexión de `ConnectionStrings:DefaultConnection` o de la variable `DATABASE_URL` (normalizando `postgres://`). |

### 3.9 Capacidad y escalabilidad (SaaS multi-tenant)

| ID | Requerimiento |
|----|---------------|
| **RNF-26** | Aislamiento multi-tenant por federación y por sistema de origen (`X-Client-App`). |
| **RNF-27** | La capacidad de audiencia (`AudienceMonitoring:SoftCapacity`) debe ser configurable y observable (picos guardados). |

---

## 4. Historias de usuario (HU — Gherkin)

### HU-01 — Inicio de sesión (RF-01, RN-01, RN-04)

**Como** usuario registrado, **quiero** iniciar sesión **para** acceder a mi panel según mi rol y sistema.

```gherkin
Feature: Inicio de sesión
  Scenario: Login exitoso
    Given un usuario activo con credenciales válidas
    And la cabecera X-Client-App indica un sistema con acceso habilitado
    When envío POST /api/Auth/login con username y password
    Then recibo HTTP 200 con el JWT y la cookie X-Access-Token
    And el token contiene el claim de rol

  Scenario: Bloqueo por intentos fallidos
    Given un usuario con 4 intentos fallidos previos
    When envío un quinto login con contraseña incorrecta
    Then el usuario queda con EstaActivo = false
    And recibo HTTP 401

  Scenario: Exceso de intentos por IP
    Given una IP que superó 20 requests de auth en un minuto
    When envío POST /api/Auth/login
    Then recibo HTTP 429 Too Many Requests
```

### HU-02 — Alta de federación con administrador (RF-42, RN-03)

**Como** SuperAdmin, **quiero** crear una federación con su admin **para** habilitar un nuevo tenant.

```gherkin
Feature: Alta de federación
  Scenario: Creación exitosa
    Given un SuperAdmin autenticado
    And un nombre de federación no existente
    When envío POST /api/SaaS/create-federacion con datos de federación y admin
    Then recibo HTTP 200/201 con la federación creada
    And existe un usuario Admin asociado a la federación

  Scenario: Federación duplicada
    Given una federación ya existente con el mismo identificador único
    When envío POST /api/SaaS/create-federacion
    Then recibo un error de conflicto mapeado desde el código PostgreSQL 23505
```

### HU-03 — Alta completa de atleta federado (RF-27, RN-05)

**Como** Admin de federación, **quiero** dar de alta un atleta con sus tutores **para** federarlo.

```gherkin
Feature: Alta de atleta
  Scenario: Alta completa
    Given un Admin autenticado
    And un documento normalizado no registrado
    When envío POST /api/Atleta/full con datos del atleta y tutores
    Then el atleta queda asociado a la federación del admin
    And se crean/actualizan las entidades Persona/Tutor correspondientes

  Scenario: Documento duplicado
    Given un atleta ya registrado con el mismo documento
    When envío POST /api/Atleta/full con ese documento
    Then el sistema resuelve el registro existente (upsert) sin duplicar
```

> ⚠️ **Deuda:** el tope `MaxAtletas` del plan **no** se aplica todavía en `AltaAtletaService`.

### HU-04 — Generación automática de fases ICF (RF-14, RN-01)

**Como** operador de competencia, **quiero** generar las series automáticamente **para** programar la regata.

```gherkin
Feature: Generación de fases
  Scenario: Generación por plan automático
    Given un EventoPrueba con N participantes confirmados
    And un plan de progresión resoluble para N
    When envío POST /api/Fases/Generar/{eventoPruebaId}
    Then se crean las fases con carriles asignados en prioridad [5,4,6,3,7,2,8,1,9]
    And los cabezas de serie se distribuyen sin carriles duplicados
    And se registra auditoría GENERATE_HEATS_AUTO

  Scenario: Cantidad inválida
    Given un EventoPrueba sin participantes suficientes
    When envío POST /api/Fases/Generar/{eventoPruebaId}
    Then recibo HTTP 400 con mensaje descriptivo
```

### HU-05 — Promoción de etapa (RF-17, RN-01)

**Como** operador, **quiero** promover los clasificados **para** armar semifinales/finales.

```gherkin
Feature: Promoción de etapa
  Scenario: Promoción con resultados completos
    Given todas las fases de la ronda con resultados completos
    When envío POST /api/Fases/Promover/{eventoPruebaId}
    Then se asignan los clasificados a la siguiente ronda por mejor tiempo
    And se registra auditoría PROMOTE_STAGE

  Scenario: Resultados incompletos
    Given una fase sin todos sus resultados oficiales
    When envío POST /api/Fases/Promover/{eventoPruebaId}
    Then recibo HTTP 400 y no se modifica ninguna fase
```

### HU-06 — Cronometraje offline con outbox (RF-48, RN-06)

**Como** cronometrista, **quiero** registrar tiempos sin conexión **para** no perder datos en pista.

```gherkin
Feature: Outbox de timing
  Scenario: Envío encolado
    Given un cronometrista autenticado sin conectividad al hub
    When encolo tiempos vía POST /api/timing-outbox
    Then cada envío queda persistido con estado pendiente y TTL de 24 h
    And el upsert se realiza por (FaseId, Username)

  Scenario: Commit de la cola
    Given una fase con envíos pendientes y la carrera finalizada
    When envío POST /api/timing-outbox/{faseId}/commit
    Then se aplica BatchUpdate sobre los resultados
    And se ejecuta FinalizarFase o EnviarARevision según corresponda
    And los envíos quedan marcados como procesados
```

### HU-07 — Reseteo de regata (RF-18, RN-07)

**Como** largador, **quiero** reiniciar una regata tras una mala largada **para** repetirla limpiamente.

```gherkin
Feature: Reseteo de regata
  Scenario: Reseteo con motivo
    Given una fase en curso
    When solicito el reseteo con motivo de al menos 5 caracteres
    Then se limpian los tiempos registrados
    And la fase vuelve al estado Programada
    And se emite el evento RaceReset y se audita RESET_RACE

  Scenario: Motivo insuficiente
    Given una fase en curso
    When solicito reseteo con un motivo de menos de 5 caracteres
    Then recibo HTTP 400 y la fase no cambia
```

### HU-08 — Aprobación de traspaso (RF-29, RN-08)

**Como** Admin de federación, **quiero** aprobar un traspaso **para** mover un atleta de club.

```gherkin
Feature: Traspaso de atleta
  Scenario: Flujo completo
    Given una solicitud de traspaso con aceptación del club de origen
    When envío POST /api/Traspaso/{id}/aprobar
    Then el atleta cambia de club
    And se registra la auditoría del traspaso

  Scenario: Rechazo del origen
    Given una solicitud de traspaso pendiente
    When el club de origen envía POST /api/Traspaso/{id}/rechazar-origen
    Then la solicitud queda rechazada y el atleta no cambia de club
```

### HU-09 — Mensajería aislada por sistema (RF-37, RN-02)

**Como** Admin, **quiero** mensajear a mis clubes sin cruzar información **para** respetar el aislamiento SIGDEF/SportTrack.

```gherkin
Feature: Mensajería aislada
  Scenario: Aislamiento por X-Client-App
    Given un Admin en el sistema sigdef
    When consulto GET /api/mensajes/hilos con X-Client-App: sigdef
    Then solo veo hilos de mi federación y sistema de origen
    And no veo hilos de SportTrack ni de otras federaciones

  Scenario: Permiso de escritura
    Given un Admin y un Club de la misma federación
    When el Admin responde un hilo del Club
    Then el mensaje se persiste y se notifica al usuario destino
```

### HU-10 — Backup de base de datos (RF-47)

**Como** SuperAdmin, **quiero** descargar un backup **para** respaldar la información.

```gherkin
Feature: Backup
  Scenario: Backup completo
    Given un SuperAdmin autenticado
    When envío GET /api/Backup/download?scope=full
    Then recibo un archivo generado con pg_dump (PGPASSWORD por variable de entorno)
    And la operación queda en el historial

  Scenario: Backup por federación
    Given un SuperAdmin autenticado y un idFederacion válido
    When envío GET /api/Backup/download?scope=federacion&idFederacion=N
    Then recibo un archivo con INSERTs de esa federación
```

### HU-11 — Live público de resultados (RF-21, RNF-03)

**Como** espectador, **quiero** ver resultados en vivo sin login **para** seguir la regata.

```gherkin
Feature: Live público
  Scenario: Consulta anónima con cache
    Given una fase en curso
    When envío GET /api/Resultados/Fase/{faseId} sin token
    Then recibo HTTP 200 con los resultados (servidos desde cache si está vigente)

  Scenario: Límite de tráfico Live
    Given una IP que superó 120 requests live en un minuto
    When consulto un endpoint Live
    Then recibo HTTP 429 Too Many Requests
```

---

## 5. Reglas de negocio (RN)

| ID | Regla | Implementación |
|----|-------|----------------|
| **RN-01** | **Progresión ICF:** planes A1–G2 con `SlotRule`/`BtSlotRule`; alias `PlanX`; validación de carriles duplicados; `ResolveDefaultPlan` por cantidad (10-18→A, 19-27→B, …, 64-72→G, variante 1/2). Los rankings excluyen `Descalificado`/`DNS`/`DNF` y prefieren `TiempoOficial`. `Assign` lanza excepción si el carril está ocupado. | `Fase/Progression/ProgressionPlanRegistry`, `ProgressionEngine` |
| **RN-02** | **Aislamiento por sistema de origen:** `X-Client-App` (`sporttrack`/`sigdef`) filtra hilos, campañas y no-leídos. `PuedeEscribir` permite SuperAdmin↔Admin y Admin↔Club de la misma federación; masivo solo SuperAdmin/Admin. | `MensajeService`, `MensajeriaSistemaOrigen` |
| **RN-03** | **Multi-tenant:** toda consulta/escritura de federación se acota por `IdFederacion` (tenant). | `TenantScopeHelper`, `TenantProvider` |
| **RN-04** | **Enforcement de planes:** `RegisterAsync` valida `CanCreateRole` (Club ⇐ `AccesoDashboardClub`; jueces ⇐ `AccesoControlesLive`). IDs 1-3 SIGDEF, 4-6 SportTrack, 7-9 Pack Dúo; `AccesoControlesLive` solo ids 6 y 9. `LoginAsync`/`GetMeAsync` bloquean usuarios inactivos/bloqueados por pago/vencidos o con `X-Client-App` sin acceso. | `PlanSaaSAccessHelper`, `AuthService` |
| **RN-05** | **Alta de atleta:** `NormalizarDocumento` + `BuscarPorDocumentoAsync` (3 estrategias) + `UpsertParticipanteAsync` + `EnsureAtletaFederacionAsync` para evitar duplicados. **Tope `MaxAtletas` NO aplicado (deuda).** | `AltaAtletaService` |
| **RN-06** | **TTL outbox 24 h:** upsert por `(FaseId, Username)`; `CommitAsync` aplica `BatchUpdate` + `FinalizarFase`/`EnviarARevision`; `FlushPendingAsync` y `PurgeExpiredAsync`. No existe token de cronometrista separado; su sesión dura 24 h vs. 5 h para otros roles. | `TimingOutboxService`, `SessionLifetimePolicy` |
| **RN-07** | **Reseteo de regata:** motivo ≥ 5 caracteres; limpia tiempos; vuelve a `Programada`; audita `RESET_RACE`; emite `RaceReset`. | `FaseService.ReiniciarFaseAsync`, `TimingHub.RequestResetRace` |
| **RN-08** | **Traspasos:** estados y transiciones con aceptación/rechazo del origen, aprobación forzada, rechazo, cancelación y auditoría. | `TraspasoService` |
| **RN-09** | **Lockout:** 5 intentos fallidos → `EstaActivo = false`. | `AuthService` |
| **RN-10** | **Estados de evento:** `Programada → EnCurso → Finalizado` por día local; **nunca retrocede**; sincronizado por background service cada 15 min y al arranque. | `EventoEstadoSyncService` |
| **RN-11** | **Asignación de carriles:** prioridad `[5,4,6,3,7,2,8,1,9]`; cabezas de serie distribuidos; carril único por fase. | `FaseService.GenerarFasesAutoAsync` |
| **RN-12** | **Maratón:** una única fase `Largada` con `Carril = NumeroCompetidor`; audita `GENERATE_LARGADA_MARATON`. | `FaseService.GenerarLargadaMaratonAsync` |
| **RN-13** | **Resultados DNS/DNF/DSQ:** se limpian tiempo y posición; audita `SAVE_TIMING`; emite `ResultadoActualizado`. | `ResultadoBatchUpdateService` |
| **RN-14** | **Precio anual de plan:** `PrecioAnual = Precio × 12 × (1 − Descuento/100)`, redondeo `AwayFromZero`. `MaxTorneosActivos = -1` (sin límite efectivo). | `SaaSService.UpdatePlanAsync` |
| **RN-15** | **Auditoría de errores:** excepciones inesperadas se loguean y auditan como `ERROR_FATAL` y devuelven mensaje sanitizado. | `ExceptionMiddleware` |

---

## 6. Matriz de trazabilidad

| HU | RF asociados | RN | CU (ver 02) | Endpoints principales |
|----|--------------|----|-------------|------------------------|
| HU-01 | RF-01, RF-09 | RN-04, RN-09 | CU-01 | `POST api/Auth/login`, `GET api/Auth/me` |
| HU-02 | RF-42 | RN-03, RN-04 | CU-02 | `POST api/SaaS/create-federacion` |
| HU-03 | RF-27 | RN-05 | CU-03 | `POST api/Atleta/full` |
| HU-04 | RF-14, RF-15 | RN-01, RN-11 | CU-04 | `POST api/Fases/Generar/{id}` |
| HU-05 | RF-17 | RN-01 | CU-05 | `POST api/Fases/Promover/{id}` |
| HU-06 | RF-48, RF-49 | RN-06 | CU-07, CU-08 | `api/timing-outbox/**`, `api/Support/timing-outbox/**` |
| HU-07 | RF-18 | RN-07 | CU-06 | `POST api/Fases/{id}/Reiniciar` |
| HU-08 | RF-29 | RN-08 | CU-10 | `api/Traspaso/**` |
| HU-09 | RF-37, RF-38 | RN-02 | CU-11 | `api/mensajes/**` |
| HU-10 | RF-47 | — | CU-12 | `api/Backup/**` |
| HU-11 | RF-21, RF-10, RF-13 | RNF-03, RNF-05 | CU-01 (Live) | `GET api/Resultados/Fase/{id}` |

| RNF | Dónde se verifica |
|-----|-------------------|
| RNF-03 | `ResultadosController` (cache 30 s), `LiveCacheService` |
| RNF-04 | `Program.cs` (`AddDbContext`) |
| RNF-05 | `Program.cs` (RateLimiter `auth`/`live`) |
| RNF-11 | `Program.cs` (migraciones al arranque) |
| RNF-12 | `EventoEstadoBackgroundService` |
| RNF-14…RNF-19 | `Program.cs`, `AuthService`, `SecurityHeadersMiddleware`, `FileUploadRules` |

---

## Pie de página

- ➡️ [02 · Casos de Uso](./02-Casos-de-Uso.md)
- ➡️ [03 · Diagramas de Flujo](./03-Diagramas-de-Flujo.md)
- ➡️ [04 · Manual de Uso de la API](./04-Manual-de-Uso-API.md)
- ➡️ [05 · Manual Técnico](./05-Manual-Tecnico.md)
- ⬅️ [Volver al índice del Pack](./README.md)
