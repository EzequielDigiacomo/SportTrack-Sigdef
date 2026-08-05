using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.DbBackup
{
    public interface IBackupService
    {
        /// <summary>Genera un dump SQL (full o por federación) y registra auditoría.</summary>
        Task<BackupFileResult> CreateBackupAsync(string scope, int? idFederacion, string? clientApp, CancellationToken ct = default);

        Task<IReadOnlyList<BackupHistoryItemDto>> GetHistoryAsync(int limit = 50, CancellationToken ct = default);
    }

    public sealed class BackupFileResult
    {
        public required byte[] Content { get; init; }
        public required string FileName { get; init; }
        public required string ContentType { get; init; }
    }

    public sealed class BackupHistoryItemDto
    {
        public int Id { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string? Ip { get; set; }
        public string? SistemaOrigen { get; set; }
    }
}
