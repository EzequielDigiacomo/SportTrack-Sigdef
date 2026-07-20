using System;
using System.Collections.Generic;

namespace SportTrack_Sigdef.Controladores.Inscripcion.Dtos
{
    public class RegistroInscripcionDto
    {
        public int Id { get; set; }
        public int ParticipanteId { get; set; }
        public string ParticipanteNombre { get; set; } = string.Empty;
        public string? ParticipanteDocumento { get; set; }
        public int? ClubId { get; set; }
        public string? ClubNombre { get; set; }
        public int EventoId { get; set; }
        public string EventoNombre { get; set; } = string.Empty;
        public int EventoPruebaId { get; set; }
        public string PruebaNombre { get; set; } = string.Empty;
        public DateTime FechaInscripcion { get; set; }
        public DateTime? FechaInicioEvento { get; set; }
        public DateTime? FechaFinEvento { get; set; }
        public string Estado { get; set; } = string.Empty;
        public bool Pagado { get; set; }
        public List<string> TripulantesNombres { get; set; } = new();
    }
}
