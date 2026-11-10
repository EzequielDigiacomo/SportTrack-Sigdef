using System.Collections.Generic;

namespace SportTrack_Sigdef.Controladores.Inscripcion.Dtos
{
    public class InscripcionTripulacionUpdateDto
    {
        /// <summary>Nuevo titular del bote (posición 1 / proa).</summary>
        public int ParticipanteId { get; set; }

        /// <summary>Resto de la tripulación (sin incluir al titular, o con posiciones explícitas).</summary>
        public List<InscripcionTripulanteCreateDto> Tripulantes { get; set; } = new();
    }

    public class ParticipanteNombrePatchDto
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
    }

    public class LiveCarrilUpdateDto
    {
        public int ResultadoId { get; set; }
        public int Carril { get; set; }
    }

    public class LiveCarrilesBatchDto
    {
        public List<LiveCarrilUpdateDto> Asignaciones { get; set; } = new();
    }

    public class LiveAgregarBoteDto
    {
        public int EventoPruebaId { get; set; }
        public int? FaseId { get; set; }
        public int? Carril { get; set; }
        public string? NumeroCompetidor { get; set; }
        public int ParticipanteId { get; set; }
        public List<InscripcionTripulanteCreateDto> Tripulantes { get; set; } = new();
    }

    public class LiveMoverPruebaDto
    {
        public int EventoPruebaIdDestino { get; set; }
        public int? FaseIdDestino { get; set; }
        public int? Carril { get; set; }
        public bool Force { get; set; }
    }

    public class TokenAccesoCambiosCreateDto
    {
        /// <summary>Si null, genera un token por cada club con inscripciones en el evento.</summary>
        public int? ClubId { get; set; }
        public DateTime? ExpiraEn { get; set; }
    }

    public class TokenAccesoCambiosDto
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public int EventoId { get; set; }
        public string? EventoNombre { get; set; }
        public int ClubId { get; set; }
        public string? ClubNombre { get; set; }
        public DateTime ExpiraEn { get; set; }
        public bool Revocado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Usos { get; set; }
        public string? UrlRelativa { get; set; }
    }

    public class CambiosAccesoResumenDto
    {
        public string Token { get; set; } = string.Empty;
        public int EventoId { get; set; }
        public string EventoNombre { get; set; } = string.Empty;
        public int ClubId { get; set; }
        public string ClubNombre { get; set; } = string.Empty;
        public DateTime ExpiraEn { get; set; }
        public bool CambiosAbiertos { get; set; }
        public IEnumerable<InscripcionDto> Inscripciones { get; set; } = new List<InscripcionDto>();
        public IEnumerable<AtletaCambioDto> AtletasClub { get; set; } = new List<AtletaCambioDto>();
    }

    public class AtletaCambioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
    }
}
