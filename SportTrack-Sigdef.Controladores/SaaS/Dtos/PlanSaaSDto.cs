namespace SportTrack_Sigdef.Controladores.SaaS.Dtos
{
    public class PlanSaaSDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int MaxAtletas { get; set; }
        public int MaxTorneosActivos { get; set; }
        public bool ResultadosTiempoReal { get; set; }
        public bool ExportacionExcel { get; set; }
        public bool SoportePrioritario { get; set; }

        public bool AccesoSigdef { get; set; }
        public bool AccesoSportTrack { get; set; }
        public bool AccesoControlesLive { get; set; }
    }
}
