namespace SportTrack_Sigdef.Controladores.Fase.Dtos
{
    /// <summary>Motivo obligatorio para reiniciar una fase (serie / largada).</summary>
    public class ReiniciarFaseDto
    {
        public string Motivo { get; set; } = string.Empty;
        /// <summary>Categoría operativa: mala_largada, postergacion, problema_tecnico, problema_externo, otro.</summary>
        public string? Categoria { get; set; }
    }
}
