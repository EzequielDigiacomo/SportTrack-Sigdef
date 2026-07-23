namespace SportTrack_Sigdef.Controladores.Caching
{
    /// <summary>
    /// Claves de cache del hot path Live (lecturas públicas).
    /// IMemoryCache hoy; mismas claves sirven si más adelante se usa Redis.
    /// </summary>
    public static class LiveCacheKeys
    {
        public static string Evento(int eventoId) => $"live:evento:{eventoId}";
        public static string FasesByEvento(int eventoId) => $"live:fases:evento:{eventoId}";
        public static string FasesByEventoPrueba(int eventoPruebaId) => $"live:fases:ep:{eventoPruebaId}";
        public static string ResultadosByFase(int faseId) => $"live:resultados:fase:{faseId}";
        public static string PruebasByEvento(int eventoId) => $"live:pruebas:evento:{eventoId}";
    }
}
