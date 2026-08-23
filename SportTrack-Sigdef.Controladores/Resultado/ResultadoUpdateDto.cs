using System;

namespace SportTrack_Sigdef.Controladores.Resultado
{
    public class ResultadoUpdateDto
    {
        public int Id { get; set; }
        public TimeSpan? TiempoOficial { get; set; }
        public int? Posicion { get; set; }
        public string? Estado { get; set; }
        public int? Carril { get; set; }
        public string? ParticipanteNombre { get; set; }
        public string? ClubSigla { get; set; }
    }
}
