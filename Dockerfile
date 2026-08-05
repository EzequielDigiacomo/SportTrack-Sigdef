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

# pg_dump debe ser >= versión del server (Render PG 18.x).
# El metapaquete debian "postgresql-client" trae v15 y falla con mismatch.
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl ca-certificates gnupg \
    && install -d /usr/share/postgresql-common/pgdg \
    && curl -fsSL -o /usr/share/postgresql-common/pgdg/apt.postgresql.org.asc \
         https://www.postgresql.org/media/keys/ACCC4CF8.asc \
    && echo "deb [signed-by=/usr/share/postgresql-common/pgdg/apt.postgresql.org.asc] https://apt.postgresql.org/pub/repos/apt bookworm-pgdg main" \
         > /etc/apt/sources.list.d/pgdg.list \
    && apt-get update \
    && apt-get install -y --no-install-recommends postgresql-client-18 \
    && ln -sf /usr/lib/postgresql/18/bin/pg_dump /usr/local/bin/pg_dump \
    && ln -sf /usr/lib/postgresql/18/bin/pg_restore /usr/local/bin/pg_restore \
    && ln -sf /usr/lib/postgresql/18/bin/psql /usr/local/bin/psql \
    && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

# Exponer el puerto predeterminado de Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Desactivar file watchers (fix para Render free tier - límite de inotify)
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENTRYPOINT ["dotnet", "SportTrack-Sigdef.dll"]
