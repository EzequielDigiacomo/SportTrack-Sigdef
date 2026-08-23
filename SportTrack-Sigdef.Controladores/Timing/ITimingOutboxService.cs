using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Timing
{
    public interface ITimingOutboxService
    {
        Task<TimingOutboxDto> UpsertAsync(string username, TimingOutboxUpsertDto dto);
        Task<IReadOnlyList<TimingOutboxDto>> GetPendingAsync(string username);
        Task<TimingOutboxCommitResultDto> CommitAsync(string username, int faseId);
        Task<IReadOnlyList<TimingOutboxCommitResultDto>> FlushPendingAsync(string username);
        Task RemoveAsync(string username, int faseId);
        Task PurgeExpiredAsync();
    }

    public class TimingOutboxUpsertDto
    {
        public int FaseId { get; set; }
        public int? EventoId { get; set; }
        public string? EventoNombre { get; set; }
        public string? FaseNombre { get; set; }
        public bool SoloMode { get; set; }
        public List<TimingOutboxResultadoDto> Resultados { get; set; } = new();
    }

    public class TimingOutboxResultadoDto
    {
        public int Id { get; set; }
        public int? Carril { get; set; }
        public string? ParticipanteNombre { get; set; }
        public string? TiempoOficial { get; set; }
        public int? MsLlegada { get; set; }
        public string? EstadoCanto { get; set; }
    }

    public class TimingOutboxDto
    {
        public int Id { get; set; }
        public int FaseId { get; set; }
        public int? EventoId { get; set; }
        public string? EventoNombre { get; set; }
        public string? FaseNombre { get; set; }
        public bool SoloMode { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
        public int AttemptCount { get; set; }
        public List<TimingOutboxResultadoDto> Resultados { get; set; } = new();
    }

    public class TimingOutboxCommitResultDto
    {
        public int FaseId { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
