namespace SportTrack_Sigdef.Controladores.Caching
{
    public interface ILiveCacheService
    {
        Task<T> GetOrCreateAsync<T>(string key, TimeSpan ttl, Func<Task<T>> factory);
        void Remove(string key);
        void InvalidateEvento(int eventoId);
        void InvalidateEventoPrueba(int eventoPruebaId, int? eventoId = null);
        void InvalidateFase(int faseId, int? eventoId = null, int? eventoPruebaId = null);
    }
}
