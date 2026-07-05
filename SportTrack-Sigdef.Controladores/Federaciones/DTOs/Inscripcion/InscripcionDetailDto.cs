using SIGDEF.DTOs;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;
using SportTrack_Sigdef.Entidades.DTOs.Evento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.DTOs.Inscripcion
{
    public class InscripcionDetailDto
    {
        public int IdInscripcion { get; set; }
        public int ParticipanteId { get; set; }
        public int IdEvento { get; set; }
        public int IdEventoPrueba { get; set; }
        public DateTime FechaInscripcion { get; set; }

        public AtletaDto? AtletaFederacion { get; set; }
        public EventoDto? Evento { get; set; }
    }
}
