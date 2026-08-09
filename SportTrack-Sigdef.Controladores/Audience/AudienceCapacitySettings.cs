using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Audience
{
    /// <summary>
    /// Techo de referencia para % de saturación. No limita conexiones SignalR.
    /// </summary>
    public sealed class AudienceCapacitySettings : IAudienceCapacitySettings
    {
        private static readonly AudienceCapacityPresetDto[] Presets =
        {
            new()
            {
                Id = "free",
                Label = "Free / pruebas",
                Hint = "API Free o carga muy baja",
                SoftCapacity = 100
            },
            new()
            {
                Id = "starter",
                Label = "API Starter + DB Basic-1gb",
                Hint = "Plan recomendado para pruebas reales (~$26/mes)",
                SoftCapacity = 200
            },
            new()
            {
                Id = "standard",
                Label = "API Standard + DB Pro-4gb",
                Hint = "Eventos medianos / cientos de viewers",
                SoftCapacity = 500
            },
            new()
            {
                Id = "pro",
                Label = "API Pro + DB Pro-4gb",
                Hint = "Meta live grande (~1000)",
                SoftCapacity = 1000
            },
            new()
            {
                Id = "custom",
                Label = "Personalizado",
                Hint = "Definís el techo a mano",
                SoftCapacity = 0,
                IsCustom = true
            }
        };

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly object _gate = new();
        private int _softCapacity = 200;
        private string _presetId = "starter";
        private string _planLabel = "API Starter + DB Basic-1gb";
        private bool _loaded;

        public AudienceCapacitySettings(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;

            var fromConfig = configuration.GetValue<int?>("AudienceMonitoring:SoftCapacity");
            if (fromConfig is > 0)
            {
                _softCapacity = fromConfig.Value;
                var match = Presets.FirstOrDefault(p => !p.IsCustom && p.SoftCapacity == fromConfig.Value);
                if (match != null)
                {
                    _presetId = match.Id;
                    _planLabel = match.Label;
                }
                else
                {
                    _presetId = "custom";
                    _planLabel = $"Personalizado ({fromConfig.Value})";
                }
            }
        }

        public int SoftCapacity
        {
            get { lock (_gate) return _softCapacity; }
        }

        public string PresetId
        {
            get { lock (_gate) return _presetId; }
        }

        public string PlanLabel
        {
            get { lock (_gate) return _planLabel; }
        }

        public IReadOnlyList<AudienceCapacityPresetDto> GetPresets() => Presets;

        public AudienceCapacityConfigDto GetConfig()
        {
            lock (_gate)
            {
                return new AudienceCapacityConfigDto
                {
                    SoftCapacity = _softCapacity,
                    PresetId = _presetId,
                    PlanLabel = _planLabel,
                    Presets = Presets.ToList()
                };
            }
        }

        public async Task EnsureLoadedAsync(CancellationToken ct = default)
        {
            if (_loaded) return;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SportTrackDbContext>();

            try
            {
                var row = await db.AudienceMonitorSettings.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == 1, ct);

                if (row != null && row.SoftCapacity > 0)
                {
                    lock (_gate)
                    {
                        _softCapacity = row.SoftCapacity;
                        _presetId = string.IsNullOrWhiteSpace(row.PresetId) ? "custom" : row.PresetId;
                        _planLabel = string.IsNullOrWhiteSpace(row.PlanLabel)
                            ? $"Techo {row.SoftCapacity}"
                            : row.PlanLabel;
                        _loaded = true;
                    }
                    return;
                }
            }
            catch
            {
                // Tabla aún no migrada: seguimos con appsettings / default.
            }

            lock (_gate) _loaded = true;
        }

        public async Task ApplyAsync(AudienceCapacityUpdateRequest request, CancellationToken ct = default)
        {
            var presetId = (request.PresetId ?? "custom").Trim().ToLowerInvariant();
            var preset = Presets.FirstOrDefault(p => p.Id.Equals(presetId, StringComparison.OrdinalIgnoreCase));

            int capacity;
            string label;
            string resolvedPresetId;

            if (preset != null && !preset.IsCustom)
            {
                capacity = preset.SoftCapacity;
                label = preset.Label;
                resolvedPresetId = preset.Id;
            }
            else
            {
                capacity = request.SoftCapacity ?? SoftCapacity;
                if (capacity < 1) capacity = 1;
                if (capacity > 50000) capacity = 50000;
                label = $"Personalizado ({capacity})";
                resolvedPresetId = "custom";
            }

            lock (_gate)
            {
                _softCapacity = capacity;
                _presetId = resolvedPresetId;
                _planLabel = label;
                _loaded = true;
            }

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SportTrackDbContext>();

            var row = await db.AudienceMonitorSettings.FirstOrDefaultAsync(x => x.Id == 1, ct);
            if (row == null)
            {
                row = new AudienceMonitorSettings { Id = 1 };
                db.AudienceMonitorSettings.Add(row);
            }

            row.SoftCapacity = capacity;
            row.PresetId = resolvedPresetId;
            row.PlanLabel = label;
            row.UpdatedAtUtc = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);
        }
    }
}
