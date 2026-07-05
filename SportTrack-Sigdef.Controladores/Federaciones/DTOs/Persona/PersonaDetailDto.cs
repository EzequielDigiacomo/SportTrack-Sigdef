using SportTrack_Sigdef.Entidades.Entidades;
using SIGDEF.DTOs;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;
using SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub;
using SportTrack_Sigdef.Entidades.DTOs.EntrenadorFederacion;
using SportTrack_Sigdef.Entidades.DTOs.PagoFederacionTransaccion;
using SportTrack_Sigdef.Entidades.DTOs.TutorFederacion;
using SportTrack_Sigdef.Entidades.DTOs.Usuario;
using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.DTOs.Participante
{
    public class PersonaDetailDto
    {
        public int ParticipanteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public Sexo Sexo { get; set; } // NUEVO
        public string SexoDisplay { get; set; } = string.Empty; // Para mostrar en UI
        // Información relacionada
        public UsuarioDto? Usuario { get; set; }
        public DelegadoClubDto? DelegadoFederacionClub { get; set; }
        public EntrenadorDto? EntrenadorFederacion { get; set; }
        public TutorDto? TutorFederacion { get; set; }
        public AtletaDto? AtletaFederacion { get; set; }
        public List<PagoTransaccionDto>? Pagos { get; set; }
    }
}
