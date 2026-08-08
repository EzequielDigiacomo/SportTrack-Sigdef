using System.Collections.Generic;

namespace SportTrack_Sigdef.Controladores.Fase.Dtos
{
    /// <summary>
    /// Genera una única fase de cronometraje para una largada de Maratón
    /// (varias EventoPrueba que comparten salida).
    /// </summary>
    public class GenerarLargadaMaratonDto
    {
        public List<int> EventoPruebaIds { get; set; } = new();
    }
}
