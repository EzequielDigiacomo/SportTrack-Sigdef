using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Entidades.Entidades
{
    public class Evento
    {
        public int IdEvento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public DateTime FechaInicio { get => Fecha; set => Fecha = value; }
        public DateTime? FechaFin { get; set; }
        public string? Ubicacion { get; set; }
        public EstadoEventoEnum Estado { get; set; } = EstadoEventoEnum.Programada; // Usando enum
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaFinInscripciones { get; set; }
        
        // Missing fields for SIGDEF compatibility
        public bool EstaActivo { get; set; } = true;
        public string? Descripcion { get; set; }
        public string TipoEvento { get; set; } = string.Empty;
        public DateTime? FechaInicioInscripciones { get; set; }
        public string? Ciudad { get; set; }
        public string? Provincia { get; set; }
        public decimal PrecioBase { get; set; }
        public int CupoMaximo { get; set; }
        public bool TieneCronometraje { get; set; }
        public bool RequiereCertificadoMedico { get; set; }
        public string? Observaciones { get; set; }
        
        // Propiedad de pertenencia
        public int? IdClub { get; set; }
        public Club? Club { get; set; }

        public int? IdFederacion { get; set; }
        public Federacion? Federacion { get; set; }

        public bool InscripcionesHabilitadas { get; set; } = true;

        // Reglas de Competencia
        public bool RestringirSoloCategoriaPropia { get; set; } = false; 
        public bool PermitirSub23EnSenior { get; set; } = false;
        public bool PermitirMasterBajarASenior { get; set; } = false;
        public bool PermitirCompletarK4 { get; set; } = false;
        public bool LimitacionBotesAB { get; set; } = false;
        
        // ConfiguraciÃ³n de Cronograma Inteligente
        public TimeSpan HoraInicioEvento { get; set; } = new TimeSpan(8, 0, 0); // 08:00 default
        public int CarrilesDisponibles { get; set; } = 9;
        public PerfilTiempoEnum PerfilTiempo { get; set; } = PerfilTiempoEnum.Estandar;
        public TimeSpan HoraInicioReceso { get; set; } = new TimeSpan(13, 0, 0); // 13:00 default
        public TimeSpan HoraFinReceso { get; set; } = new TimeSpan(14, 0, 0); // 14:00 default
        public bool SinReceso { get; set; } = false;
        public int GapEntrePruebas { get; set; } = 10;
        public bool PermitirCombinadas { get; set; } = false;
        public bool UsarGapVariable { get; set; } = false;
        public string TimeZoneId { get; set; } = "America/Argentina/Buenos_Aires"; // Default IANA timezone
        
        // Habilitaciones de ConfiguraciÃ³n
        public string? CategoriasHabilitadas { get; set; } // IDs separadas por coma
        public string? BotesHabilitados { get; set; } // IDs separadas por coma
        public string? DistanciasHabilitadas { get; set; } // IDs separadas por coma

        public DateTime? FechaActualizacion { get; set; }

        private ICollection<Inscripcion>? _inscripciones;

        [NotMapped]
        public ICollection<Inscripcion> Inscripciones 
        { 
            get => _inscripciones ?? EventoPruebas?.SelectMany(ep => ep.Inscripciones).ToList() ?? new List<Inscripcion>();
            set => _inscripciones = value;
        }

        public bool PuedeInscribirse()
        {
            return InscripcionesHabilitadas && (FechaFinInscripciones == null || DateTime.UtcNow <= FechaFinInscripciones);
        }

        // Navigation property
        public ICollection<EventoPrueba> EventoPruebas { get; set; } = new List<EventoPrueba>();
    }
}
