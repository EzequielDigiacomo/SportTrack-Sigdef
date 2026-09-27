# Operación local

> Requiere **.NET 10 SDK** (target `net10.0`).

```powershell
cd SportTrack-Sigdef
dotnet run
```

Swagger (solo Development): `http://localhost:5012/swagger`

> Corrección 2026-09-27: el puerto real es **5012** (no 5029) según `launchSettings.json`.

## Migraciones

```powershell
dotnet ef database update --project ..\SportTrack-Sigdef.AccesoDatos\SportTrack-Sigdef.AccesoDatos.csproj
```

## Config

- Connection string y JWT en `appsettings` / variables de entorno.  
- No commitear secretos. Ver plan en [../seguridad/](../seguridad/).
