# 06 · Manual de Instalación y Despliegue

**Última actualización: 2026-09-27**
**Audiencia:** desarrolladores, DevOps y responsables de despliegue.

---

## Índice

1. [Requisitos](#1-requisitos)
2. [Obtención del código](#2-obtención-del-código)
3. [Configuración (appsettings / variables)](#3-configuración-appsettings--variables)
4. [Ejecución local](#4-ejecución-local)
5. [Migraciones](#5-migraciones)
6. [Build y publicación](#6-build-y-publicación)
7. [Docker](#7-docker)
8. [Despliegue en Render](#8-despliegue-en-render)
9. [Verificación](#9-verificación)
10. [Checklist de despliegue](#10-checklist-de-despliegue)
11. [Troubleshooting](#11-troubleshooting)

---

## 1. Requisitos

| Requisito | Versión / notas |
|-----------|-----------------|
| **.NET SDK** | **10.0** (target `net10.0`) |
| **PostgreSQL** | 14+ (Render usa PG 18.x) |
| **`pg_dump`** | Debe ser **≥ versión del servidor** (para backups) |
| **Docker** | Opcional (imagen multi-stage) |
| **EF Core CLI** | `dotnet tool install --global dotnet-ef` (para migraciones manuales) |
| **Git** | Para clonar el repositorio |

---

## 2. Obtención del código

```powershell
git clone <url-del-repositorio> SportTrack-Sigdef
cd SportTrack-Sigdef
dotnet restore SportTrack-Sigdef.sln
```

La solución `SportTrack-Sigdef.sln` contiene 4 proyectos: `SportTrack-Sigdef` (Api), `SportTrack-Sigdef.Controladores`, `SportTrack-Sigdef.AccesoDatos` y `SportTrack-Sigdef.Entidades`.

---

## 3. Configuración (appsettings / variables)

### 3.1 `appsettings.json` (base)

```json
{
  "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } },
  "AllowedHosts": "*",
  "AudienceMonitoring": { "SoftCapacity": 200 },
  "CloudinarySettings": { "CloudName": "", "ApiKey": "", "ApiSecret": "" }
}
```

### 3.2 Variables de entorno

| Variable | Obligatoria | Descripción |
|----------|:-----------:|-------------|
| `ConnectionStrings__DefaultConnection` | Sí (o `DATABASE_URL`) | Cadena Npgsql de PostgreSQL |
| `DATABASE_URL` | Alternativa | Render la provee; acepta `postgres://` (se normaliza a `postgresql://`) |
| `TokenKey` | **Sí en producción** | Clave de firma JWT (en Development hay fallback) |
| `AllowedOrigins` | No | Lista CSV adicional de orígenes CORS |
| `CloudinarySettings__CloudName` | No | Cloudinary (o `CLOUDINARY_CLOUD_NAME`) |
| `CloudinarySettings__ApiKey` | No | Cloudinary (o `CLOUDINARY_API_KEY`) |
| `CloudinarySettings__ApiSecret` | No | Cloudinary (o `CLOUDINARY_API_SECRET`) |
| `AudienceMonitoring__SoftCapacity` | No | Capacidad de audiencia (default 200) |
| `ASPNETCORE_ENVIRONMENT` | No | `Development` habilita Swagger; en producción dejarlo sin Swagger |
| `PGPASSWORD` | Para backups | Contraseña de PostgreSQL para `pg_dump` |

> ⚠️ **`TokenKey` es obligatorio fuera de Development.** Si falta, el arranque debe fallar/no emitir tokens válidos.

### 3.3 `launchSettings.json` (perfiles locales)

| Perfil | URL |
|--------|-----|
| `http` | `http://localhost:5012` |
| `https` | `https://localhost:7232;http://localhost:5012` |
| `IIS Express` | `http://localhost:10202` / `https://localhost:44390` |

> **Corrección:** documentos previos indicaban el puerto **5029**; el puerto real es **5012**.

---

## 4. Ejecución local

```powershell
cd SportTrack-Sigdef
dotnet run
```

- Con el perfil por defecto: API en `http://localhost:5012`.
- Swagger (solo Development): **`http://localhost:5012/swagger`**.
- Health: `http://localhost:5012/api/Health` y `http://localhost:5012/api/Health/db`.
- Hub SignalR: `http://localhost:5012/hubs/timing`.

Al arrancar, el servicio:
1. Verifica conexión a PostgreSQL.
2. Aplica migraciones pendientes (`Database.MigrateAsync`).
3. Ejecuta `EventoEstadoSyncService.SyncAllAsync`.
4. Levanta el background service de estados (cada 15 min) y el de snapshots de audiencia.

---

## 5. Migraciones

### 5.1 Automáticas (recomendado)

Se aplican solas al iniciar si hay pendientes. No requiere intervención.

### 5.2 Manuales

```powershell
dotnet ef migrations add <NombreMigracion> --project SportTrack-Sigdef.AccesoDatos --startup-project SportTrack-Sigdef
dotnet ef database update --project SportTrack-Sigdef.AccesoDatos --startup-project SportTrack-Sigdef
```

Última migración: `AddPermitirMezclarCategorias` (2026-09-16).

---

## 6. Build y publicación

```powershell
dotnet build SportTrack-Sigdef.sln -c Release
dotnet publish SportTrack-Sigdef/SportTrack-Sigdef.csproj -c Release -o ./publish /p:UseAppHost=false
```

---

## 7. Docker

### 7.1 Construir y ejecutar

```bash
docker build -t sporttrack-sigdef .
docker run -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__DefaultConnection="Host=...;Database=...;Username=...;Password=..." \
  -e TokenKey="<clave-larga-y-segura>" \
  sporttrack-sigdef
```

### 7.2 Características de la imagen

| Aspecto | Detalle |
|---------|---------|
| Build | `mcr.microsoft.com/dotnet/sdk:10.0-noble` |
| Runtime | `mcr.microsoft.com/dotnet/aspnet:10.0-noble` |
| PostgreSQL client | `postgresql-client-18` + symlinks a `/usr/local/bin` |
| Puerto | `ASPNETCORE_URLS=http://+:8080`, `EXPOSE 8080` |
| Watchers | `DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false`, `DOTNET_USE_POLLING_FILE_WATCHER=true` (fix Render free tier) |

> No hay `docker-compose` ni pipeline CI en el repositorio.

---

## 8. Despliegue en Render

1. **Repositorio conectado a Render** con auto-deploy en push a `main`.
2. Render construye con el `Dockerfile` (build + publish + runtime).
3. Configurar **variables de entorno** en el panel de Render:

| Variable | Valor |
|----------|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `TokenKey` | Clave segura y larga |
| `ConnectionStrings__DefaultConnection` | (o dejar que use `DATABASE_URL` de Render) |
| `AllowedOrigins` | CSVs de dominios adicionales si aplica |
| `CloudinarySettings__CloudName` / `__ApiKey` / `__ApiSecret` | Credenciales Cloudinary |
| `AudienceMonitoring__SoftCapacity` | (opcional) |
| `PGPASSWORD` | Contraseña PG para `pg_dump` |

4. El servicio escucha en `8080` (Render lo enruta por HTTPS).
5. `UseForwardedHeaders` lee la IP/esquema real del proxy.
6. Las migraciones se aplican al primer arranque tras el deploy.

---

## 9. Verificación

| Chequeo | Cómo |
|---------|------|
| Servicio arriba | `GET /api/Health` → 200 |
| Base de datos | `GET /api/Health/db` → 200 |
| Migraciones al día | Logs de arranque: "Base de datos al día" |
| Login | `POST /api/Auth/login` con `admin/admin123` (cambiar en prod) |
| Live público | `GET /api/Eventos/proximos` sin token → 200 |
| Auth por defecto | `GET /api/Auth/me` sin token → 401 |
| CORS | Probar desde el dominio del frontend con credenciales |
| Backups | `GET /api/Backup/download?scope=full` con SuperAdmin |
| SignalR | Conectar a `/hubs/timing` y llamar `GetServerTime` |

---

## 10. Checklist de despliegue

- [ ] `TokenKey` configurado (obligatorio en prod).
- [ ] Cadena de conexión / `DATABASE_URL` configurada.
- [ ] `ASPNETCORE_ENVIRONMENT=Production` (Swagger deshabilitado).
- [ ] `AllowedOrigins` con los dominios del frontend en producción.
- [ ] Credenciales Cloudinary configuradas (subida de documentación).
- [ ] `PGPASSWORD` configurado (backups).
- [ ] Imagen Docker con `postgresql-client-18`.
- [ ] Verificar `/api/Health` y `/api/Health/db` tras el deploy.
- [ ] Cambiar credenciales seed `admin/admin123`.
- [ ] Confirmar migraciones aplicadas en los logs.

---

## 11. Troubleshooting

| Síntoma | Causa probable | Solución |
|---------|----------------|----------|
| `ERROR: No se pudo conectar a PostgreSQL` | Cadena de conexión mal formada o ausente | Revisar `ConnectionStrings__DefaultConnection` o `DATABASE_URL` (formato `postgresql://`) |
| Arranque falla por **TokenKey** | Falta `TokenKey` en producción | Definir `TokenKey` en variables de entorno |
| **401** en todas las llamadas | Falta token válido / usuario inactivo / plan vencido | Revisar login y estado del usuario |
| **403** en Live o consolas de juez | Plan sin `AccesoControlesLive` / origen sin acceso | Verificar plan de la federación (IDs 6 y 9) |
| **429** frecuente | Rate limit `auth` (20/min) o `live` (120/min) por IP | Reducir frecuencia; revisar IP detrás del proxy |
| **CORS bloqueado** | Dominio no incluido en la whitelist | Agregar el dominio a `AllowedOrigins` |
| Falla el backup | `pg_dump` ausente o versión menor al server | Verificar `postgresql-client-18` en la imagen |
| Error de **SSL** local | Certificado HTTPS de desarrollo | `dotnet dev-certs https --trust` |
| Swagger no aparece | `ASPNETCORE_ENVIRONMENT` distinto de `Development` | Usar el perfil `http`/`https` local |
| Migraciones no aplican | Error en el arranque silenciado en log | Revisar logs de arranque; aplicar manual con `dotnet ef database update` |

---

## Pie de página

- ➡️ [05 · Manual Técnico](./05-Manual-Tecnico.md)
- ➡️ [04 · Manual de Uso de la API](./04-Manual-de-Uso-API.md)
- ⬅️ [Volver al índice del Pack](./README.md) · [Documentación histórica](../README.md)
