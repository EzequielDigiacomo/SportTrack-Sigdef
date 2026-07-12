namespace SportTrack_Sigdef.Entidades.Entidades
{
    /// <summary>
    /// Aísla la mensajería interna entre productos que comparten la misma API/BD.
    /// Valores alineados con el header X-Client-App de cada front.
    /// </summary>
    public static class MensajeriaSistemaOrigen
    {
        public const string SportTrack = "sporttrack";
        public const string Sigdef = "sigdef";

        public static bool EsValido(string? valor) =>
            string.Equals(valor, SportTrack, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(valor, Sigdef, StringComparison.OrdinalIgnoreCase);

        public static string Normalizar(string? clientApp)
        {
            if (string.Equals(clientApp, Sigdef, StringComparison.OrdinalIgnoreCase))
                return Sigdef;

            if (string.Equals(clientApp, SportTrack, StringComparison.OrdinalIgnoreCase))
                return SportTrack;

            throw new ArgumentException(
                "Header X-Client-App inválido o ausente. Debe ser 'sporttrack' o 'sigdef'.",
                nameof(clientApp));
        }
    }
}
