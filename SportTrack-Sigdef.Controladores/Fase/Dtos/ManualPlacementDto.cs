namespace SportTrack_Sigdef.Controladores.Fase.Dtos
{
    public class ManualPlacementDto
    {
        public int InscripcionId { get; set; }
        public int Serie { get; set; } = 1; // Número de serie (1, 2, 3...)
        public int Carril { get; set; } = 1;
    }
}
