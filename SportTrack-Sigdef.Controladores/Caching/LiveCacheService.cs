using Microsoft.Extensions.Caching.Memory;

namespace SportTrack_Sigdef.Controladores.Caching
{
    public class LiveCacheService : ILiveCacheService
    {
        private readonly IMemoryCache _cache;

        public LiveCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, TimeSpan ttl, Func<Task<T>> factory)
        {
            if (_cache.TryGetValue(key, out T? cached) && cached is not null)
                return cached;

            var value = await factory();
            _cache.Set(key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl,
                Size = 1
            });
            return value;
        }

        public void Remove(string key) => _cache.Remove(key);

        public void InvalidateEvento(int eventoId)
        {
            _cache.Remove(LiveCacheKeys.Evento(eventoId));
            _cache.Remove(LiveCacheKeys.FasesByEvento(eventoId));
            _cache.Remove(LiveCacheKeys.PruebasByEvento(eventoId));
        }

        public void InvalidateEventoPrueba(int eventoPruebaId, int? eventoId = null)
        {
            _cache.Remove(LiveCacheKeys.FasesByEventoPrueba(eventoPruebaId));
            if (eventoId.HasValue)
                InvalidateEvento(eventoId.Value);
        }

        public void InvalidateFase(int faseId, int? eventoId = null, int? eventoPruebaId = null)
        {
            _cache.Remove(LiveCacheKeys.ResultadosByFase(faseId));
            if (eventoPruebaId.HasValue)
                _cache.Remove(LiveCacheKeys.FasesByEventoPrueba(eventoPruebaId.Value));
            if (eventoId.HasValue)
                InvalidateEvento(eventoId.Value);
        }
    }
}
