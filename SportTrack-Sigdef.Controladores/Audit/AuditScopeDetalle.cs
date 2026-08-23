using System.Text.Json;

namespace SportTrack_Sigdef.Controladores.Audit
{
    /// <summary>
    /// Persiste eventoId/eventoPruebaId dentro de Detalle hasta que la columna exista en BD.
    /// </summary>
    public static class AuditScopeDetalle
    {
        public static string Enrich(string detalle, int? eventoId, int? eventoPruebaId)
        {
            if (!eventoId.HasValue && !eventoPruebaId.HasValue) return detalle ?? string.Empty;

            if (TryExtract(detalle).EventoId.HasValue) return detalle ?? string.Empty;

            try
            {
                return JsonSerializer.Serialize(new
                {
                    eventoId,
                    eventoPruebaId,
                    text = detalle ?? string.Empty,
                });
            }
            catch
            {
                return detalle ?? string.Empty;
            }
        }

        public static (int? EventoId, int? EventoPruebaId) TryExtract(string? detalle)
        {
            if (string.IsNullOrWhiteSpace(detalle)) return (null, null);
            var trimmed = detalle.Trim();
            if (!trimmed.StartsWith('{')) return (null, null);

            try
            {
                using var doc = JsonDocument.Parse(trimmed);
                var root = doc.RootElement;
                int? eventoId = null;
                int? eventoPruebaId = null;

                if (root.TryGetProperty("eventoId", out var ev) && ev.TryGetInt32(out var evInt))
                    eventoId = evInt;
                if (root.TryGetProperty("eventoPruebaId", out var ep) && ep.TryGetInt32(out var epInt))
                    eventoPruebaId = epInt;

                if (eventoId.HasValue || eventoPruebaId.HasValue)
                    return (eventoId, eventoPruebaId);
            }
            catch
            {
                /* texto plano o JSON distinto */
            }

            return (null, null);
        }
    }
}
