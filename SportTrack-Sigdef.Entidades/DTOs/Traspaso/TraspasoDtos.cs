using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;

namespace SportTrack_Sigdef.Entidades.DTOs.Traspaso
{
    public class PeriodoTraspasoDto
    {
        public int Id { get; set; }
        public int IdFederacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; }
        public string? Observaciones { get; set; }
        public bool EsVigente { get; set; }
    }

    public class PeriodoTraspasoCreateDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; } = true;
        public string? Observaciones { get; set; }
    }

    public class PeriodoTraspasoUpdateDto
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Activo { get; set; }
        public string? Observaciones { get; set; }
    }

    public class SolicitudTraspasoDto
    {
        public int Id { get; set; }
        public int IdFederacion { get; set; }
        public int ParticipanteId { get; set; }
        public string ParticipanteNombre { get; set; } = string.Empty;
        public string? ParticipanteDocumento { get; set; }
        public int IdClubOrigen { get; set; }
        public string ClubOrigenNombre { get; set; } = string.Empty;
        public int IdClubDestino { get; set; }
        public string ClubDestinoNombre { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? MotivoSolicitud { get; set; }
        public string? MotivoRechazo { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuestaOrigen { get; set; }
        public DateTime? FechaRespuestaFederacion { get; set; }
        public DateTime? FechaEjecucion { get; set; }
    }

    public class SolicitudTraspasoCreateDto
    {
        public int ParticipanteId { get; set; }
        public int IdClubDestino { get; set; }
        public string? MotivoSolicitud { get; set; }
    }

    public class TraspasoMotivoDto
    {
        public string? Motivo { get; set; }
    }

    public class TraspasoValidacionItemDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Ok { get; set; }
        public bool Bloqueante { get; set; }
        public string? Detalle { get; set; }
    }

    public class TraspasoValidacionDto
    {
        public int SolicitudId { get; set; }
        public bool PuedeAprobar { get; set; }
        public List<TraspasoValidacionItemDto> Items { get; set; } = new();
    }

    public class AtletaTraspasoBusquedaDto
    {
        public int ParticipanteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Documento { get; set; }
        public int IdClub { get; set; }
        public string ClubNombre { get; set; } = string.Empty;
    }

    public class TraspasoAuditoriaDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public string? Usuario { get; set; }
    }
}
