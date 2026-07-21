using SportTrack_Sigdef.Entidades.Enums;
using System;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class SolicitudTraspaso
    {
        public int IdSolicitudTraspaso { get; set; }
        public int IdFederacion { get; set; }
        public Federacion Federacion { get; set; } = null!;

        public int ParticipanteId { get; set; }
        public Participante Participante { get; set; } = null!;

        public int IdClubOrigen { get; set; }
        public Club ClubOrigen { get; set; } = null!;

        public int IdClubDestino { get; set; }
        public Club ClubDestino { get; set; } = null!;

        public EstadoSolicitudTraspaso Estado { get; set; } = EstadoSolicitudTraspaso.PendienteOrigen;

        public string? MotivoSolicitud { get; set; }
        public string? MotivoRechazo { get; set; }

        public int? SolicitadoPorUsuarioId { get; set; }
        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
        public DateTime? FechaRespuestaOrigen { get; set; }
        public DateTime? FechaRespuestaFederacion { get; set; }
        public DateTime? FechaEjecucion { get; set; }
        public int? AprobadoPorUsuarioId { get; set; }
    }
}
