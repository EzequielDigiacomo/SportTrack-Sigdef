# SportTrack-Sigdef

Backend unificado para **SportTrack** (competencias/regatas) y **SIGDEF** (administración federativa).

- ASP.NET Core 8 + PostgreSQL
- BD compartida: `Participante`, `Club`, `Federacion`, `AtletaFederacion`
- APIs: `api/Participantes`, `api/Clubes`, `api/Atleta`, `api/Eventos`, etc.

## Desarrollo local

```powershell
cd SportTrack-Sigdef
dotnet run
```

Swagger: `http://localhost:5029/swagger`

## Migraciones

```powershell
dotnet ef database update --project ..\SportTrack-Sigdef.AccesoDatos\SportTrack-Sigdef.AccesoDatos.csproj
```
