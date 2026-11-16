using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SportTrack_Sigdef.Entidades.Enums;

namespace SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion
{
    public class FichajeRevisionRequestDto
    {
        [MaxLength(1000)]
        public string? Motivo { get; set; }
    }

    public class FichajeHistorialDto
    {
        public int Id { get; set; }
        public int ParticipanteId { get; set; }
        public EstadoFichaje EstadoAnterior { get; set; }
        public EstadoFichaje EstadoNuevo { get; set; }
        public string? Motivo { get; set; }
        public string? RevisadoPorUsername { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class PadronFiltroDto
    {
        public int? ClubId { get; set; }
        public CategoriaEdad? Categoria { get; set; }
        public bool? SoloHabilitados { get; set; } = true;
        public EstadoFichaje? EstadoFichaje { get; set; }
        public string? Busqueda { get; set; }
    }

    public class ReporteAdminResumenDto
    {
        public int TotalAtletas { get; set; }
        public int PendientesFichaje { get; set; }
        public int Observados { get; set; }
        public int Habilitados { get; set; }
        public int NoHabilitados { get; set; }
        public int Deudores { get; set; }
        public int AptosPorVencer { get; set; }
        public int AptosVencidos { get; set; }
        public int AptosFaltantes { get; set; }
        public int DocsIncompletos { get; set; }
    }

    public class PadronAtletaDto
    {
        public int ParticipanteId { get; set; }
        public string NombrePersona { get; set; } = "";
        public string? Documento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int? Edad { get; set; }
        public int? IdClub { get; set; }
        public string? NombreClub { get; set; }
        public CategoriaEdad? Categoria { get; set; }
        public EstadoPago EstadoPago { get; set; }
        public EstadoFichaje EstadoFichaje { get; set; }
        public bool PresentoAptoMedico { get; set; }
        public DateTime? FechaVencimientoApto { get; set; }
        public string EstadoApto { get; set; } = "Faltante";
        public bool EstaHabilitado { get; set; }
        public string EstadoHabilitacion { get; set; } = "NoHabilitado";
        public List<string> MotivosBloqueo { get; set; } = new();
        public List<string> DocsFaltantes { get; set; } = new();
    }
}
