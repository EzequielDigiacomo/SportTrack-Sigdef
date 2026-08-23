using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Auth;
using SportTrack_Sigdef.Controladores.Fase;
using SportTrack_Sigdef.Controladores.Resultado;
using SportTrack_Sigdef.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Timing
{
    public class TimingOutboxService : ITimingOutboxService
    {
        private static readonly TimeSpan OutboxTtl = TimeSpan.FromHours(SessionLifetimePolicy.CronometristaHours);

        private readonly SportTrackDbContext _context;
        private readonly IResultadoBatchUpdateService _batchUpdateService;
        private readonly IFaseService _faseService;

        public TimingOutboxService(
            SportTrackDbContext context,
            IResultadoBatchUpdateService batchUpdateService,
            IFaseService faseService)
        {
            _context = context;
            _batchUpdateService = batchUpdateService;
            _faseService = faseService;
        }

        public async Task<TimingOutboxDto> UpsertAsync(string username, TimingOutboxUpsertDto dto)
        {
            await PurgeExpiredAsync();

            var now = DateTime.UtcNow;
            var payloadJson = JsonSerializer.Serialize(dto);

            var existing = await _context.TimingSubmissionOutbox
                .FirstOrDefaultAsync(x => x.FaseId == dto.FaseId && x.Username == username);

            if (existing == null)
            {
                existing = new TimingSubmissionOutbox
                {
                    FaseId = dto.FaseId,
                    Username = username,
                    CreatedAtUtc = now,
                };
                _context.TimingSubmissionOutbox.Add(existing);
            }

            existing.EventoId = dto.EventoId;
            existing.PayloadJson = payloadJson;
            existing.SoloMode = dto.SoloMode;
            existing.ExpiresAtUtc = now.Add(OutboxTtl);
            existing.LastAttemptAtUtc = null;

            await _context.SaveChangesAsync();
            return MapToDto(existing, dto);
        }

        public async Task<IReadOnlyList<TimingOutboxDto>> GetPendingAsync(string username)
        {
            await PurgeExpiredAsync();

            var rows = await _context.TimingSubmissionOutbox
                .AsNoTracking()
                .Where(x => x.Username == username && x.ExpiresAtUtc > DateTime.UtcNow)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToListAsync();

            return rows.Select(r => MapToDto(r, DeserializePayload(r.PayloadJson))).ToList();
        }

        public async Task<TimingOutboxCommitResultDto> CommitAsync(string username, int faseId)
        {
            var row = await _context.TimingSubmissionOutbox
                .FirstOrDefaultAsync(x => x.FaseId == faseId && x.Username == username);

            if (row == null)
            {
                return new TimingOutboxCommitResultDto
                {
                    FaseId = faseId,
                    Success = true,
                    Message = "Sin pendientes en servidor.",
                };
            }

            if (row.ExpiresAtUtc <= DateTime.UtcNow)
            {
                _context.TimingSubmissionOutbox.Remove(row);
                await _context.SaveChangesAsync();
                return new TimingOutboxCommitResultDto
                {
                    FaseId = faseId,
                    Success = false,
                    Message = "El respaldo en servidor expiró.",
                };
            }

            row.AttemptCount += 1;
            row.LastAttemptAtUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            try
            {
                var payload = DeserializePayload(row.PayloadJson);
                var updates = BuildBatchUpdates(payload);
                if (updates.Count == 0)
                    throw new InvalidOperationException("No hay tiempos para confirmar.");

                await _batchUpdateService.ApplyBatchUpdateAsync(updates, username);

                if (row.SoloMode)
                    await _faseService.FinalizarFaseAsync(faseId);
                else
                    await _faseService.EnviarARevisionAsync(faseId);

                _context.TimingSubmissionOutbox.Remove(row);
                await _context.SaveChangesAsync();

                return new TimingOutboxCommitResultDto
                {
                    FaseId = faseId,
                    Success = true,
                    Message = "Tiempos confirmados desde cola temporal.",
                };
            }
            catch (Exception ex)
            {
                return new TimingOutboxCommitResultDto
                {
                    FaseId = faseId,
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<IReadOnlyList<TimingOutboxCommitResultDto>> FlushPendingAsync(string username)
        {
            var pending = await _context.TimingSubmissionOutbox
                .Where(x => x.Username == username && x.ExpiresAtUtc > DateTime.UtcNow)
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => x.FaseId)
                .ToListAsync();

            var results = new List<TimingOutboxCommitResultDto>();
            foreach (var faseId in pending)
                results.Add(await CommitAsync(username, faseId));

            return results;
        }

        public async Task RemoveAsync(string username, int faseId)
        {
            var row = await _context.TimingSubmissionOutbox
                .FirstOrDefaultAsync(x => x.FaseId == faseId && x.Username == username);
            if (row == null) return;

            _context.TimingSubmissionOutbox.Remove(row);
            await _context.SaveChangesAsync();
        }

        public async Task PurgeExpiredAsync()
        {
            var expired = await _context.TimingSubmissionOutbox
                .Where(x => x.ExpiresAtUtc <= DateTime.UtcNow)
                .ToListAsync();
            if (expired.Count == 0) return;

            _context.TimingSubmissionOutbox.RemoveRange(expired);
            await _context.SaveChangesAsync();
        }

        private static TimingOutboxUpsertDto DeserializePayload(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<TimingOutboxUpsertDto>(json)
                       ?? new TimingOutboxUpsertDto();
            }
            catch
            {
                return new TimingOutboxUpsertDto();
            }
        }

        private static TimingOutboxDto MapToDto(TimingSubmissionOutbox row, TimingOutboxUpsertDto payload) =>
            new()
            {
                Id = row.Id,
                FaseId = row.FaseId,
                EventoId = payload.EventoId ?? row.EventoId,
                EventoNombre = payload.EventoNombre,
                FaseNombre = payload.FaseNombre,
                SoloMode = row.SoloMode,
                CreatedAtUtc = row.CreatedAtUtc,
                ExpiresAtUtc = row.ExpiresAtUtc,
                AttemptCount = row.AttemptCount,
                Resultados = payload.Resultados ?? new List<TimingOutboxResultadoDto>(),
            };

        private static List<ResultadoUpdateDto> BuildBatchUpdates(TimingOutboxUpsertDto payload)
        {
            var updates = new List<ResultadoUpdateDto>();
            foreach (var r in payload.Resultados ?? Enumerable.Empty<TimingOutboxResultadoDto>())
            {
                var hasTime = !string.IsNullOrWhiteSpace(r.TiempoOficial);
                var hasEstado = !string.IsNullOrWhiteSpace(r.EstadoCanto)
                    && !string.Equals(r.EstadoCanto, "Pendiente", StringComparison.OrdinalIgnoreCase);
                if (!hasTime && !hasEstado) continue;

                updates.Add(new ResultadoUpdateDto
                {
                    Id = r.Id,
                    TiempoOficial = hasTime ? ParseTimeSpan(r.TiempoOficial!) : null,
                    Estado = hasEstado ? MapEstadoCanto(r.EstadoCanto!) : null,
                });
            }

            return updates;
        }

        private static TimeSpan? ParseTimeSpan(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            var normalized = value.Trim().Replace(',', '.');
            if (TimeSpan.TryParse(normalized, CultureInfo.InvariantCulture, out var ts))
                return ts;

            var parts = normalized.Split(':');
            if (parts.Length >= 3
                && int.TryParse(parts[0], out var h)
                && int.TryParse(parts[1], out var m)
                && double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var s))
            {
                return TimeSpan.FromHours(h) + TimeSpan.FromMinutes(m) + TimeSpan.FromSeconds(s);
            }

            return null;
        }

        private static string MapEstadoCanto(string estadoCanto) =>
            estadoCanto.Trim().ToUpperInvariant() switch
            {
                "DNS" => "DNS",
                "DNF" => "DNF",
                "DSQ" or "DESCALIFICADO" => "Descalificado",
                _ => estadoCanto,
            };
    }
}
