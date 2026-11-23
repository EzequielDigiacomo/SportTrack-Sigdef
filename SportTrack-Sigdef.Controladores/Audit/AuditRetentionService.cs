using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SportTrack_Sigdef.AccesoDatos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Audit
{
    public class AuditRetentionService : IAuditRetentionService
    {
        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly IHostEnvironment _env;
        private readonly SportTrackDbContext _db;
        private readonly SemaphoreSlim _gate = new(1, 1);

        public AuditRetentionService(IHostEnvironment env, SportTrackDbContext db)
        {
            _env = env;
            _db = db;
        }

        private string ConfigPath =>
            Path.Combine(_env.ContentRootPath, "App_Data", "audit-retention.json");

        public async Task<AuditRetentionOptions> GetAsync(CancellationToken ct = default)
        {
            await _gate.WaitAsync(ct);
            try
            {
                return await ReadUnlockedAsync(ct);
            }
            finally
            {
                _gate.Release();
            }
        }

        public async Task<AuditRetentionOptions> SaveAsync(AuditRetentionOptions options, CancellationToken ct = default)
        {
            var normalized = Normalize(options);
            await _gate.WaitAsync(ct);
            try
            {
                await WriteUnlockedAsync(normalized, ct);
                return normalized;
            }
            finally
            {
                _gate.Release();
            }
        }

        public async Task<FederacionAuditRetentionOptions> UpsertFederacionAsync(
            FederacionAuditRetentionOptions options,
            CancellationToken ct = default)
        {
            if (options == null || options.FederacionId <= 0)
                throw new ArgumentException("FederacionId inválido.");

            await _gate.WaitAsync(ct);
            try
            {
                var all = await ReadUnlockedAsync(ct);
                all.Federaciones ??= new List<FederacionAuditRetentionOptions>();

                var existing = all.Federaciones.FirstOrDefault(f => f.FederacionId == options.FederacionId);
                var normalizedFed = NormalizeFed(options);

                if (existing == null)
                {
                    all.Federaciones.Add(normalizedFed);
                }
                else
                {
                    existing.Nombre = normalizedFed.Nombre ?? existing.Nombre;
                    existing.Enabled = normalizedFed.Enabled;
                    existing.Days = normalizedFed.Days;
                    normalizedFed = existing;
                }

                await WriteUnlockedAsync(Normalize(all), ct);
                return normalizedFed;
            }
            finally
            {
                _gate.Release();
            }
        }

        public async Task<int> PurgeOlderThanAsync(int days, CancellationToken ct = default)
        {
            days = Math.Clamp(days, 1, 3650);
            var cutoff = DateTime.UtcNow.AddDays(-days);

            var old = await _db.Auditoria
                .Where(a => a.Fecha < cutoff)
                .ToListAsync(ct);

            if (old.Count == 0) return 0;

            _db.Auditoria.RemoveRange(old);
            await _db.SaveChangesAsync(ct);
            return old.Count;
        }

        public async Task<int> PurgeFederacionAsync(FederacionAuditPurgeRequest request, CancellationToken ct = default)
        {
            if (request == null || request.FederacionId <= 0)
                throw new ArgumentException("FederacionId inválido.");

            var usernames = await GetFederacionUsernamesAsync(request.FederacionId, ct);
            if (usernames.Count == 0) return 0;

            var query = _db.Auditoria.Where(a => usernames.Contains(a.Usuario));

            if (!request.DeleteAll)
            {
                if (!request.From.HasValue && !request.To.HasValue)
                    throw new ArgumentException("Indicá un rango de fechas o DeleteAll.");

                if (request.From.HasValue)
                {
                    var from = request.From.Value.Date;
                    query = query.Where(a => a.Fecha >= from);
                }

                if (request.To.HasValue)
                {
                    var toExclusive = request.To.Value.Date.AddDays(1);
                    query = query.Where(a => a.Fecha < toExclusive);
                }
            }

            var rows = await query.ToListAsync(ct);
            if (rows.Count == 0) return 0;

            _db.Auditoria.RemoveRange(rows);
            await _db.SaveChangesAsync(ct);
            return rows.Count;
        }

        public async Task RunAutomaticPurgesAsync(CancellationToken ct = default)
        {
            var opts = await GetAsync(ct);

            if (opts.Enabled)
            {
                await PurgeOlderThanAsync(opts.Days, ct);
            }

            foreach (var fed in opts.Federaciones.Where(f => f.Enabled && f.FederacionId > 0))
            {
                var days = Math.Clamp(fed.Days <= 0 ? 90 : fed.Days, 1, 3650);
                var cutoff = DateTime.UtcNow.AddDays(-days).Date;
                // Conservar desde cutoff en adelante → borrar todo lo anterior.
                await PurgeFederacionAsync(new FederacionAuditPurgeRequest
                {
                    FederacionId = fed.FederacionId,
                    DeleteAll = false,
                    From = new DateTime(2000, 1, 1),
                    To = cutoff.AddDays(-1),
                }, ct);
            }
        }

        public async Task TruncateAllAsync(CancellationToken ct = default)
        {
            // TRUNCATE libera el espacio en disco de inmediato (a diferencia de DELETE).
            await _db.Database.ExecuteSqlRawAsync(
                @"TRUNCATE TABLE ""Auditoria"" RESTART IDENTITY;");
            _db.ChangeTracker.Clear();
        }

        public async Task VacuumAsync(CancellationToken ct = default)
        {
            // VACUUM no puede correr dentro de una transacción de EF.
            var connection = _db.Database.GetDbConnection();
            var shouldClose = connection.State != System.Data.ConnectionState.Open;
            if (shouldClose)
                await connection.OpenAsync(ct);

            try
            {
                await using var cmd = connection.CreateCommand();
                // FULL reclama espacio al SO; puede bloquear la tabla unos segundos.
                cmd.CommandText = @"VACUUM (FULL, ANALYZE) ""Auditoria"";";
                cmd.CommandTimeout = 600;
                await cmd.ExecuteNonQueryAsync(ct);
            }
            finally
            {
                if (shouldClose)
                    await connection.CloseAsync();
            }
        }

        private async Task<HashSet<string>> GetFederacionUsernamesAsync(int federacionId, CancellationToken ct)
        {
            var clubIds = await _db.Clubes
                .AsNoTracking()
                .Where(c => c.IdFederacion == federacionId)
                .Select(c => c.IdClub)
                .ToListAsync(ct);

            var usernames = await _db.Usuarios
                .AsNoTracking()
                .Where(u =>
                    u.IdFederacion == federacionId
                    || (u.IdClub.HasValue && clubIds.Contains(u.IdClub.Value)))
                .Select(u => u.Username)
                .Distinct()
                .ToListAsync(ct);

            return usernames
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        private async Task<AuditRetentionOptions> ReadUnlockedAsync(CancellationToken ct)
        {
            var path = ConfigPath;
            if (!File.Exists(path))
            {
                return new AuditRetentionOptions();
            }

            try
            {
                await using var stream = File.OpenRead(path);
                var opts = await JsonSerializer.DeserializeAsync<AuditRetentionOptions>(stream, JsonOpts, ct);
                return Normalize(opts);
            }
            catch
            {
                return new AuditRetentionOptions();
            }
        }

        private async Task WriteUnlockedAsync(AuditRetentionOptions options, CancellationToken ct)
        {
            var dir = Path.GetDirectoryName(ConfigPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            await using var stream = File.Create(ConfigPath);
            await JsonSerializer.SerializeAsync(stream, options, JsonOpts, ct);
        }

        private static AuditRetentionOptions Normalize(AuditRetentionOptions? options)
        {
            var o = options ?? new AuditRetentionOptions();
            o.Days = Math.Clamp(o.Days <= 0 ? 90 : o.Days, 1, 3650);
            o.Federaciones ??= new List<FederacionAuditRetentionOptions>();
            o.Federaciones = o.Federaciones
                .Where(f => f != null && f.FederacionId > 0)
                .GroupBy(f => f.FederacionId)
                .Select(g => NormalizeFed(g.Last()!))
                .ToList();
            return o;
        }

        private static FederacionAuditRetentionOptions NormalizeFed(FederacionAuditRetentionOptions f)
        {
            return new FederacionAuditRetentionOptions
            {
                FederacionId = f.FederacionId,
                Nombre = f.Nombre,
                Enabled = f.Enabled,
                Days = Math.Clamp(f.Days <= 0 ? 90 : f.Days, 1, 3650),
            };
        }
    }
}
