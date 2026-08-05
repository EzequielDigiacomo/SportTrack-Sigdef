using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Audit;

namespace SportTrack_Sigdef.Controladores.DbBackup
{
    public class BackupService : IBackupService
    {
        private readonly SportTrackDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuditService _auditService;
        private readonly ILogger<BackupService> _logger;

        public BackupService(
            SportTrackDbContext context,
            IConfiguration configuration,
            IAuditService auditService,
            ILogger<BackupService> logger)
        {
            _context = context;
            _configuration = configuration;
            _auditService = auditService;
            _logger = logger;
        }

        public async Task<BackupFileResult> CreateBackupAsync(
            string scope,
            int? idFederacion,
            string? clientApp,
            CancellationToken ct = default)
        {
            var normalizedScope = (scope ?? "full").Trim().ToLowerInvariant();
            if (normalizedScope is not ("full" or "federacion"))
                throw new ArgumentException("scope debe ser 'full' o 'federacion'.");

            if (normalizedScope == "federacion")
            {
                if (!idFederacion.HasValue || idFederacion.Value <= 0)
                    throw new ArgumentException("idFederacion es obligatorio para scope=federacion.");

                var fed = await _context.Federaciones.AsNoTracking()
                    .FirstOrDefaultAsync(f => f.IdFederacion == idFederacion.Value, ct)
                    ?? throw new InvalidOperationException($"No existe la federación Id={idFederacion.Value}.");

                var bytes = await BuildFederacionSqlAsync(fed.IdFederacion, fed.Nombre, ct);
                var safeName = SanitizeFilePart(fed.Sigla ?? fed.Nombre);
                var fileName = $"backup_federacion_{fed.IdFederacion}_{safeName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.sql";
                var origen = NormalizeClientApp(clientApp);

                await _auditService.RegistrarAccionAsync(
                    "BACKUP_FEDERACION",
                    $"Backup por federación Id={fed.IdFederacion} ('{fed.Nombre}'). Origen={origen}. Archivo={fileName}. Tamaño={bytes.Length} bytes.",
                    modulo: "Backup");

                return new BackupFileResult
                {
                    Content = bytes,
                    FileName = fileName,
                    ContentType = "application/sql"
                };
            }

            // Full dump via pg_dump
            var connectionString = ResolveConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("No hay connection string configurada para generar el backup.");

            var dumpBytes = await RunPgDumpAsync(connectionString, ct);
            var fullName = $"backup_full_{DateTime.UtcNow:yyyyMMdd_HHmmss}.sql";
            var origenFull = NormalizeClientApp(clientApp);

            await _auditService.RegistrarAccionAsync(
                "BACKUP_FULL",
                $"Backup completo de la base de datos. Origen={origenFull}. Archivo={fullName}. Tamaño={dumpBytes.Length} bytes.",
                modulo: "Backup");

            return new BackupFileResult
            {
                Content = dumpBytes,
                FileName = fullName,
                ContentType = "application/sql"
            };
        }

        public async Task<IReadOnlyList<BackupHistoryItemDto>> GetHistoryAsync(int limit = 50, CancellationToken ct = default)
        {
            limit = Math.Clamp(limit, 1, 200);

            var logs = await _context.Auditoria.AsNoTracking()
                .Where(a => a.Modulo == "Backup")
                .OrderByDescending(a => a.Fecha)
                .Take(limit)
                .ToListAsync(ct);

            return logs.Select(a =>
            {
                var origen = ExtractOrigen(a.Detalle);
                return new BackupHistoryItemDto
                {
                    Id = a.Id,
                    Accion = a.Accion,
                    Detalle = a.Detalle,
                    Usuario = a.Usuario,
                    Fecha = a.Fecha,
                    Ip = a.IP,
                    SistemaOrigen = origen
                };
            }).ToList();
        }

        private async Task<byte[]> RunPgDumpAsync(string connectionString, CancellationToken ct)
        {
            var csb = new NpgsqlConnectionStringBuilder(NormalizeNpgsqlConnectionString(connectionString));
            var args = BuildPgDumpArgs(csb);

            var psi = new ProcessStartInfo
            {
                FileName = "pg_dump",
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8
            };

            // Evitar que PGPASSWORD quede en process list de args
            if (!string.IsNullOrEmpty(csb.Password))
                psi.Environment["PGPASSWORD"] = csb.Password;

            using var process = new Process { StartInfo = psi };
            var stdout = new MemoryStream();
            var stderr = new StringBuilder();

            try
            {
                if (!process.Start())
                    throw new InvalidOperationException("No se pudo iniciar pg_dump.");
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                _logger.LogError(ex, "pg_dump no disponible en el contenedor");
                throw new InvalidOperationException(
                    "pg_dump no está disponible en el servidor. Verificá que la imagen Docker instale postgresql-client.", ex);
            }

            var stdoutTask = process.StandardOutput.BaseStream.CopyToAsync(stdout, ct);
            var stderrTask = Task.Run(async () =>
            {
                while (!process.StandardError.EndOfStream)
                {
                    var line = await process.StandardError.ReadLineAsync(ct);
                    if (line != null) stderr.AppendLine(line);
                }
            }, ct);

            await Task.WhenAll(stdoutTask, stderrTask);
            await process.WaitForExitAsync(ct);

            if (process.ExitCode != 0)
            {
                var err = stderr.ToString().Trim();
                _logger.LogError("pg_dump falló ({Code}): {Err}", process.ExitCode, err);
                throw new InvalidOperationException(
                    string.IsNullOrWhiteSpace(err)
                        ? $"pg_dump falló con código {process.ExitCode}."
                        : $"pg_dump falló: {err}");
            }

            if (stdout.Length == 0)
                throw new InvalidOperationException("pg_dump generó un archivo vacío.");

            return stdout.ToArray();
        }

        private static string BuildPgDumpArgs(NpgsqlConnectionStringBuilder csb)
        {
            var host = csb.Host ?? "localhost";
            var port = csb.Port > 0 ? csb.Port : 5432;
            var db = csb.Database ?? "postgres";
            var user = csb.Username ?? "postgres";

            // Plain SQL, portable, sin owner/privileges (mismo criterio que BACKUP_GUIDE)
            return $"--no-owner --no-privileges --format=plain --encoding=UTF8 " +
                   $"-h \"{EscapeArg(host)}\" -p {port} -U \"{EscapeArg(user)}\" -d \"{EscapeArg(db)}\"";
        }

        private async Task<byte[]> BuildFederacionSqlAsync(int idFederacion, string nombreFed, CancellationToken ct)
        {
            var connectionString = ResolveConnectionString()
                ?? throw new InvalidOperationException("No hay connection string configurada.");

            await using var conn = new NpgsqlConnection(NormalizeNpgsqlConnectionString(connectionString));
            await conn.OpenAsync(ct);

            var sb = new StringBuilder();
            sb.AppendLine("-- ============================================================");
            sb.AppendLine($"-- SportTrack / SIGDEF — Backup por federación");
            sb.AppendLine($"-- Federación: {nombreFed} (Id={idFederacion})");
            sb.AppendLine($"-- Generado (UTC): {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine("--");
            sb.AppendLine("-- Restaurar:");
            sb.AppendLine("--  1) Crear DB vacía y aplicar migraciones EF (mismo esquema).");
            sb.AppendLine("--  2) Ejecutar este archivo con psql o Query Tool.");
            sb.AppendLine("--  3) Los catálogos globales (sexos, botes, categorías, planes) NO se");
            sb.AppendLine("--     incluyen aquí: deben existir en la DB destino (seed/migraciones).");
            sb.AppendLine("--  4) Archivos Cloudinary no van en este dump (solo URLs en BD).");
            sb.AppendLine("-- ============================================================");
            sb.AppendLine();
            sb.AppendLine("BEGIN;");
            sb.AppendLine();

            // Orden respetando FKs aproximado
            await AppendInsertsAsync(conn, sb,
                @"SELECT * FROM federacion.""Federaciones"" WHERE ""IdFederacion"" = @id",
                "federacion.\"Federaciones\"", idFederacion, ct);

            await AppendInsertsAsync(conn, sb,
                @"SELECT * FROM catalogos.""Clubes"" WHERE ""IdFederacion"" = @id",
                "catalogos.\"Clubes\"", idFederacion, ct);

            await AppendInsertsAsync(conn, sb,
                @"SELECT * FROM seguridad.""Usuarios"" WHERE ""IdFederacion"" = @id
                  OR ""IdClub"" IN (SELECT ""IdClub"" FROM catalogos.""Clubes"" WHERE ""IdFederacion"" = @id)",
                "seguridad.\"Usuarios\"", idFederacion, ct);

            await AppendInsertsAsync(conn, sb,
                @"SELECT p.* FROM regatas.""Participantes"" p
                  INNER JOIN catalogos.""Clubes"" c ON c.""IdClub"" = p.""IdClub""
                  WHERE c.""IdFederacion"" = @id",
                "regatas.\"Participantes\"", idFederacion, ct);

            await AppendInsertsAsync(conn, sb,
                @"SELECT * FROM federacion.""AtletasFederados"" WHERE ""IdFederacion"" = @id",
                "federacion.\"AtletasFederados\"", idFederacion, ct);

            await AppendInsertsAsync(conn, sb,
                @"SELECT * FROM federacion.""Entrenadores"" WHERE ""IdFederacion"" = @id",
                "federacion.\"Entrenadores\"", idFederacion, ct);

            await AppendInsertsAsync(conn, sb,
                @"SELECT * FROM federacion.""DelegadosClub"" WHERE ""IdFederacion"" = @id",
                "federacion.\"DelegadosClub\"", idFederacion, ct);

            await AppendInsertsAsync(conn, sb,
                @"SELECT * FROM regatas.""Eventos"" WHERE ""IdFederacion"" = @id
                  OR ""IdClub"" IN (SELECT ""IdClub"" FROM catalogos.""Clubes"" WHERE ""IdFederacion"" = @id)",
                "regatas.\"Eventos\"", idFederacion, ct);

            await AppendInsertsAsync(conn, sb,
                @"SELECT ep.* FROM regatas.""EventoPruebas"" ep
                  INNER JOIN regatas.""Eventos"" e ON e.""IdEvento"" = ep.""IdEvento""
                  WHERE e.""IdFederacion"" = @id
                     OR e.""IdClub"" IN (SELECT ""IdClub"" FROM catalogos.""Clubes"" WHERE ""IdFederacion"" = @id)",
                "regatas.\"EventoPruebas\"", idFederacion, ct);

            await AppendInsertsAsync(conn, sb,
                @"SELECT i.* FROM regatas.""Inscripciones"" i
                  INNER JOIN regatas.""EventoPruebas"" ep ON ep.""IdEventoPrueba"" = i.""IdEventoPrueba""
                  INNER JOIN regatas.""Eventos"" e ON e.""IdEvento"" = ep.""IdEvento""
                  WHERE e.""IdFederacion"" = @id
                     OR e.""IdClub"" IN (SELECT ""IdClub"" FROM catalogos.""Clubes"" WHERE ""IdFederacion"" = @id)",
                "regatas.\"Inscripciones\"", idFederacion, ct);

            sb.AppendLine("COMMIT;");
            sb.AppendLine();
            sb.AppendLine("-- Fin backup por federación");

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private static async Task AppendInsertsAsync(
            NpgsqlConnection conn,
            StringBuilder sb,
            string selectSql,
            string tableName,
            int idFederacion,
            CancellationToken ct)
        {
            await using var cmd = new NpgsqlCommand(selectSql, conn);
            cmd.Parameters.AddWithValue("id", idFederacion);

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            if (!reader.HasRows)
            {
                sb.AppendLine($"-- (sin filas) {tableName}");
                sb.AppendLine();
                return;
            }

            var columns = Enumerable.Range(0, reader.FieldCount)
                .Select(i => $"\"{reader.GetName(i)}\"")
                .ToArray();
            var colList = string.Join(", ", columns);

            sb.AppendLine($"-- {tableName}");
            var count = 0;
            while (await reader.ReadAsync(ct))
            {
                var values = new string[reader.FieldCount];
                for (var i = 0; i < reader.FieldCount; i++)
                    values[i] = FormatSqlLiteral(reader.GetValue(i));

                sb.AppendLine($"INSERT INTO {tableName} ({colList}) VALUES ({string.Join(", ", values)});");
                count++;
            }

            sb.AppendLine($"-- {count} fila(s) en {tableName}");
            sb.AppendLine();
        }

        private static string FormatSqlLiteral(object? value)
        {
            if (value is null or DBNull)
                return "NULL";

            return value switch
            {
                bool b => b ? "TRUE" : "FALSE",
                byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal
                    => Convert.ToString(value, CultureInfo.InvariantCulture)!,
                DateTime dt => $"'{dt.ToUniversalTime():yyyy-MM-dd HH:mm:ss.ffffff}+00'",
                DateTimeOffset dto => $"'{dto.UtcDateTime:yyyy-MM-dd HH:mm:ss.ffffff}+00'",
                Guid g => $"'{g}'",
                byte[] bytes => $"'\\x{Convert.ToHexString(bytes)}'",
                _ => $"'{EscapeSqlString(Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty)}'"
            };
        }

        private static string EscapeSqlString(string s) => s.Replace("'", "''");

        private string? ResolveConnectionString()
        {
            var fromConfig = _configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrWhiteSpace(fromConfig))
                return fromConfig;

            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (string.IsNullOrWhiteSpace(databaseUrl))
                return null;

            if (databaseUrl.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
                databaseUrl = "postgresql://" + databaseUrl["postgres://".Length..];

            return databaseUrl;
        }

        private static string NormalizeNpgsqlConnectionString(string connectionString)
        {
            // Npgsql acepta URI postgresql://... directamente en versiones recientes;
            // si viene como key=value ya está bien.
            if (connectionString.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase)
                || connectionString.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
            {
                var uri = connectionString.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
                    ? "postgresql://" + connectionString["postgres://".Length..]
                    : connectionString;
                return uri;
            }

            return connectionString;
        }

        private static string NormalizeClientApp(string? clientApp)
        {
            if (string.Equals(clientApp, "sporttrack", StringComparison.OrdinalIgnoreCase))
                return "SportTrack";
            if (string.Equals(clientApp, "sigdef", StringComparison.OrdinalIgnoreCase))
                return "SIGDEF";
            return string.IsNullOrWhiteSpace(clientApp) ? "desconocido" : clientApp.Trim();
        }

        private static string? ExtractOrigen(string? detalle)
        {
            if (string.IsNullOrWhiteSpace(detalle)) return null;
            var m = Regex.Match(detalle, @"Origen=([^\s.]+)", RegexOptions.IgnoreCase);
            return m.Success ? m.Groups[1].Value : null;
        }

        private static string SanitizeFilePart(string value)
        {
            var cleaned = Regex.Replace(value.Trim(), @"[^\w\-]+", "_");
            return string.IsNullOrWhiteSpace(cleaned) ? "fed" : cleaned[..Math.Min(cleaned.Length, 40)];
        }

        private static string EscapeArg(string value) => value.Replace("\"", "\\\"");
    }
}
