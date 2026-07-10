namespace SportTrack_Sigdef.Controladores.Mensajes.Dtos
{
    public class CrearHiloDto
    {
        public int DestinatarioId { get; set; }
        public string Asunto { get; set; } = string.Empty;
        public string Cuerpo { get; set; } = string.Empty;
    }

    public class ResponderHiloDto
    {
        public string Cuerpo { get; set; } = string.Empty;
    }

    public class UsuarioResumenDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string RolFederacion { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public int? FederacionId { get; set; }
    }

    public class MensajeItemDto
    {
        public int IdMensaje { get; set; }
        public int RemitenteId { get; set; }
        public int DestinatarioId { get; set; }
        public UsuarioResumenDto Remitente { get; set; } = new();
        public string Cuerpo { get; set; } = string.Empty;
        public DateTime EnviadoEn { get; set; }
        public DateTime? LeidoEn { get; set; }
        public bool EsPropio { get; set; }
    }

    public class HiloListItemDto
    {
        public int IdHilo { get; set; }
        public string Asunto { get; set; } = string.Empty;
        public DateTime UltimoMensajeEn { get; set; }
        public UsuarioResumenDto Contraparte { get; set; } = new();
        public string UltimoMensajePreview { get; set; } = string.Empty;
        public int CantidadNoLeidos { get; set; }
    }

    public class HiloDetalleDto
    {
        public int IdHilo { get; set; }
        public string Asunto { get; set; } = string.Empty;
        public DateTime CreadoEn { get; set; }
        public DateTime UltimoMensajeEn { get; set; }
        public int? IdCampana { get; set; }
        public List<MensajeItemDto> Mensajes { get; set; } = new();
    }

    public class EnviarMasivoDto
    {
        public string Asunto { get; set; } = string.Empty;
        public string Cuerpo { get; set; } = string.Empty;
        public List<int> DestinatarioIds { get; set; } = new();
    }

    public class HiloCampanaItemDto
    {
        public int HiloId { get; set; }
        public int DestinatarioId { get; set; }
        public string DestinatarioNombre { get; set; } = string.Empty;
        public string DestinatarioUsername { get; set; } = string.Empty;
        public bool Leido { get; set; }
        public bool Respondido { get; set; }
        public DateTime? UltimoMensajeEn { get; set; }
    }

    public class CampanaListItemDto
    {
        public int IdCampana { get; set; }
        public string Asunto { get; set; } = string.Empty;
        public DateTime EnviadoEn { get; set; }
        public int CantidadDestinatarios { get; set; }
        public string TipoCampana { get; set; } = string.Empty;
        public int CantidadLeidos { get; set; }
        public int CantidadRespondidos { get; set; }
    }

    public class CampanaDetalleDto
    {
        public int IdCampana { get; set; }
        public string Asunto { get; set; } = string.Empty;
        public string Cuerpo { get; set; } = string.Empty;
        public DateTime EnviadoEn { get; set; }
        public int CantidadDestinatarios { get; set; }
        public string TipoCampana { get; set; } = string.Empty;
        public List<HiloCampanaItemDto> Hilos { get; set; } = new();
    }

    public class EnviarMasivoResultDto
    {
        public int CampanaId { get; set; }
        public int CantidadHilos { get; set; }
        public List<HiloCampanaItemDto> Hilos { get; set; } = new();
    }
}
