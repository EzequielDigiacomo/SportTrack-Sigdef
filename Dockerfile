# Etapa 1: Build y Publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["SportTrack-Sigdef/SportTrack-Sigdef.csproj", "SportTrack-Sigdef/"]
COPY ["SportTrack-Sigdef.AccesoDatos/SportTrack-Sigdef.AccesoDatos.csproj", "SportTrack-Sigdef.AccesoDatos/"]
COPY ["SportTrack-Sigdef.Controladores/SportTrack-Sigdef.Controladores.csproj", "SportTrack-Sigdef.Controladores/"]
COPY ["SportTrack-Sigdef.Entidades/SportTrack-Sigdef.Entidades.csproj", "SportTrack-Sigdef.Entidades/"]

RUN dotnet restore "SportTrack-Sigdef/SportTrack-Sigdef.csproj"

# Copiar el resto del código y compilar
COPY . .
WORKDIR "/src/SportTrack-Sigdef"
RUN dotnet build "SportTrack-Sigdef.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SportTrack-Sigdef.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Instalar postgresql-client para disponer de pg_dump (necesario para respaldos de base de datos)
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

# Exponer el puerto predeterminado de Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Desactivar file watchers (fix para Render free tier - límite de inotify)
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENTRYPOINT ["dotnet", "SportTrack-Sigdef.dll"]
