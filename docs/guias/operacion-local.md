# Operación local

```powershell
cd SportTrack-Sigdef
dotnet run
```

Swagger: `http://localhost:5029/swagger`

## Migraciones

```powershell
dotnet ef database update --project ..\SportTrack-Sigdef.AccesoDatos\SportTrack-Sigdef.AccesoDatos.csproj
```

## Config

- Connection string y JWT en `appsettings` / variables de entorno.  
- No commitear secretos. Ver plan en [../seguridad/](../seguridad/).
