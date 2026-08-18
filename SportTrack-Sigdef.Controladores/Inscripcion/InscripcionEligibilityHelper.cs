using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.Inscripcion
{
    public static class InscripcionEligibilityHelper
    {
        public const int Sub23CategoriaId = 6;
        public const int SeniorCategoriaId = 7;

        public const int Sub23EdadMin = 19;
        public const int Sub23EdadMax = 23;

        public static int CalcularEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.UtcNow.Date;
            var edad = hoy.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
            return edad;
        }

        public static bool EsAtletaSub23(Entidades.Entidades.Participante participante)
        {
            if (participante.CategoriaId == Sub23CategoriaId) return true;
            var edad = CalcularEdad(participante.FechaNacimiento);
            return edad >= Sub23EdadMin && edad <= Sub23EdadMax;
        }

        public static bool EsPruebaSenior(int categoriaPruebaId) => categoriaPruebaId == SeniorCategoriaId;

        /// <summary>
        /// Valida Sub-23 en prueba Senior según regla del evento.
        /// </summary>
        public static void ValidarSub23EnSenior(Entidades.Entidades.Evento evento, int categoriaPruebaId, Entidades.Entidades.Participante participante)
        {
            if (!EsPruebaSenior(categoriaPruebaId)) return;
            if (!EsAtletaSub23(participante)) return;
            if (evento.PermitirSub23EnSenior) return;

            throw new Exceptions.BadRequestException(
                "Este evento no permite inscribir atletas Sub-23 en pruebas Senior.");
        }
    }
}
