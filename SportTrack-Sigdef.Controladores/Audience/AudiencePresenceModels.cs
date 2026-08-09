using System;
using System.Collections.Generic;

namespace SportTrack_Sigdef.Controladores.Audience
{
    public sealed class AudienceConnectionState
    {
        public string ConnectionId { get; set; } = string.Empty;
        public string? EventoId { get; set; }
        public string? FaseId { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public bool IsOperator { get; set; }
        public DateTime ConnectedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime LastSeenUtc { get; set; } = DateTime.UtcNow;
    }

    public sealed class AudienceEventBreakdownDto
    {
        public string EventoId { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Live { get; set; }
        public int Operators { get; set; }
    }

    public sealed class AudienceLiveDto
    {
        public DateTime CapturedAtUtc { get; set; }
        public int TotalConnections { get; set; }
        public int LiveConnections { get; set; }
        public int OperatorConnections { get; set; }
        public int SoftCapacity { get; set; }
        public double SaturationPercent { get; set; }
        public string SaturationLevel { get; set; } = "ok";
        public int SessionPeakTotal { get; set; }
        public DateTime? SessionPeakAtUtc { get; set; }
        public string PresetId { get; set; } = "starter";
        public string PlanLabel { get; set; } = string.Empty;
        public List<AudienceEventBreakdownDto> ByEvento { get; set; } = new();
    }

    public sealed class AudiencePeakDto
    {
        public long Id { get; set; }
        public DateTime CapturedAtUtc { get; set; }
        public int TotalConnections { get; set; }
        public int LiveConnections { get; set; }
        public int OperatorConnections { get; set; }
        public int SoftCapacity { get; set; }
        public double SaturationPercent { get; set; }
        public int? TopEventoId { get; set; }
        public string? TopEventoNombre { get; set; }
        public int TopEventoConnections { get; set; }
        public bool IsPeakRecord { get; set; }
    }
}
