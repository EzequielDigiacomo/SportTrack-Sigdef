using Microsoft.Extensions.Logging;
using SportTrack_Sigdef.Controladores.Evento.Dtos;
using SportTrack_Sigdef.Controladores.Mensajes;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Evento
{
    public interface IEventoNotificacionService
    {
        Task NotificarEventoCreadoAsync(EventoDto evento);
        Task NotificarEventoActualizadoAsync(EventoSnapshot before, Entidades.Entidades.Evento after);
    }

    /// <summary>Copia de campos relevantes antes de un update.</summary>
    public sealed class EventoSnapshot
    {
        public int IdEvento { get; init; }
        public int? IdFederacion { get; init; }
        public string Nombre { get; init; } = string.Empty;
        public DateTime Fecha { get; init; }
        public DateTime? FechaFin { get; init; }
        public string? Ubicacion { get; init; }
        public DateTime? FechaFinInscripciones { get; init; }
        public DateTime? FechaLimiteCambiosDelegados { get; init; }
        public string? CategoriasHabilitadas { get; init; }
        public string? BotesHabilitados { get; init; }
        public string? DistanciasHabilitadas { get; init; }
        public EstadoEventoEnum Estado { get; init; }
        public bool InscripcionesHabilitadas { get; init; }
        public string Modalidad { get; init; } = "Velocidad";
        public string TimeZoneId { get; init; } = "America/Argentina/Buenos_Aires";

        public static EventoSnapshot From(Entidades.Entidades.Evento e) => new()
        {
            IdEvento = e.IdEvento,
            IdFederacion = e.IdFederacion,
            Nombre = e.Nombre ?? string.Empty,
            Fecha = e.Fecha,
            FechaFin = e.FechaFin,
            Ubicacion = e.Ubicacion,
            FechaFinInscripciones = e.FechaFinInscripciones,
            FechaLimiteCambiosDelegados = e.FechaLimiteCambiosDelegados,
            CategoriasHabilitadas = e.CategoriasHabilitadas,
            BotesHabilitados = e.BotesHabilitados,
            DistanciasHabilitadas = e.DistanciasHabilitadas,
            Estado = e.Estado,
            InscripcionesHabilitadas = e.InscripcionesHabilitadas,
            Modalidad = e.Modalidad ?? "Velocidad",
            TimeZoneId = e.TimeZoneId ?? "America/Argentina/Buenos_Aires",
        };
    }

    /// <summary>
    /// Avisa por mensajería interna a todos los clubes de la federación
    /// cuando se crea o modifica un evento (no controles).
    /// </summary>
    public class EventoNotificacionService : IEventoNotificacionService
    {
        private const string DefaultTimeZoneId = "America/Argentina/Buenos_Aires";
        private const string WindowsArgentinaTimeZoneId = "Argentina Standard Time";
        private static readonly CultureInfo EsAr = CultureInfo.GetCultureInfo("es-AR");

        private static readonly Dictionary<int, string> CategoriaLabels = new()
        {
            [1] = "Pre-infantil", [2] = "Infantil", [3] = "Menor", [4] = "Cadete",
            [5] = "Junior", [6] = "Sub-23", [7] = "Senior", [8] = "Master A",
            [9] = "Master B", [10] = "Master C", [11] = "Todas las categorías",
        };

        private static readonly Dictionary<int, string> BoteLabels = new()
        {
            [1] = "K1", [2] = "K2", [3] = "K4", [4] = "C1", [5] = "C2", [6] = "C4",
        };

        private static readonly Dictionary<int, string> DistanciaLabels = new()
        {
            [1] = "200m", [2] = "350m", [3] = "400m", [4] = "450m", [5] = "500m",
            [6] = "1000m", [7] = "1500m", [8] = "2000m", [9] = "3000m", [10] = "5000m",
            [11] = "10000m", [12] = "12000m", [13] = "15000m", [14] = "18000m",
            [15] = "22000m", [16] = "30000m",
        };

        private readonly IMensajeRepository _mensajeRepository;
        private readonly IMensajeService _mensajeService;
        private readonly ILogger<EventoNotificacionService> _logger;

        public EventoNotificacionService(
            IMensajeRepository mensajeRepository,
            IMensajeService mensajeService,
            ILogger<EventoNotificacionService> logger)
        {
            _mensajeRepository = mensajeRepository;
            _mensajeService = mensajeService;
            _logger = logger;
        }

        public async Task NotificarEventoCreadoAsync(EventoDto evento)
        {
            try
            {
                if (evento == null) return;
                if (!evento.FederacionId.HasValue || evento.FederacionId.Value <= 0)
                {
                    _logger.LogWarning("Evento {EventoId}: sin FederacionId, no se envía mensaje a clubes.", evento.Id);
                    return;
                }
                if (EsControl(evento.Nombre)) return;

                var (asunto, cuerpo) = ConstruirMensajeCreacion(evento, ResolveTimeZone(evento.TimeZoneId));
                await EnviarAClubesAsync(evento.FederacionId.Value, evento.Id, asunto, cuerpo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Evento {EventoId}: falló el mensaje privado de creación.", evento?.Id);
            }
        }

        public async Task NotificarEventoActualizadoAsync(EventoSnapshot before, Entidades.Entidades.Evento after)
        {
            try
            {
                if (before == null || after == null) return;

                var fedId = after.IdFederacion ?? before.IdFederacion;
                if (!fedId.HasValue || fedId.Value <= 0)
                {
                    _logger.LogWarning("Evento {EventoId}: sin FederacionId en update, no se envía mensaje.", after.IdEvento);
                    return;
                }

                if (EsControl(after.Nombre) || EsControl(before.Nombre)) return;

                var tz = ResolveTimeZone(after.TimeZoneId ?? before.TimeZoneId);
                var cambios = DetectarCambios(before, after, tz);
                if (cambios.Count == 0) return;

                var nombre = (after.Nombre ?? before.Nombre ?? string.Empty).Trim();
                var asunto = $"Evento actualizado: {nombre}";
                var sb = new StringBuilder();
                sb.AppendLine($"Se actualizó el evento \"{nombre}\".");
                sb.AppendLine("Cambios:");
                foreach (var c in cambios)
                    sb.AppendLine($"• {c}");
                sb.Append("Revisá el evento desde el panel del club.");

                await EnviarAClubesAsync(fedId.Value, after.IdEvento, asunto, sb.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Evento {EventoId}: falló el mensaje privado de actualización.", after?.IdEvento);
            }
        }

        private async Task EnviarAClubesAsync(int fedId, int eventoId, string asunto, string cuerpo)
        {
            var destinatarios = await _mensajeRepository.GetUsuariosActivosClubesByFederacionAsync(fedId);
            var ids = destinatarios.Select(u => u.IdUsuario).Distinct().ToList();
            if (ids.Count == 0)
            {
                _logger.LogWarning(
                    "Evento {EventoId} fed {FedId}: no hay usuarios Club activos para mensaje privado.",
                    eventoId, fedId);
                return;
            }

            await _mensajeService.EnviarNotificacionAutomaticaAsync(
                fedId,
                ids,
                asunto,
                cuerpo,
                MensajeriaSistemaOrigen.SportTrack);

            _logger.LogInformation(
                "Evento {EventoId}: mensaje privado enviado a {Count} usuarios Club (fed {FedId}).",
                eventoId, ids.Count, fedId);
        }

        private static bool EsControl(string? nombre) =>
            !string.IsNullOrWhiteSpace(nombre)
            && nombre.Contains("control", StringComparison.OrdinalIgnoreCase);

        private static List<string> DetectarCambios(EventoSnapshot before, Entidades.Entidades.Evento after, TimeZoneInfo tz)
        {
            var cambios = new List<string>();

            if (!StringEq(before.Nombre, after.Nombre))
                cambios.Add($"Nombre: \"{before.Nombre}\" → \"{after.Nombre}\"");

            if (!SameDay(before.Fecha, after.Fecha) || !SameDay(before.FechaFin, after.FechaFin))
            {
                var antes = FormatearRango(before.Fecha, before.FechaFin, tz);
                var despues = FormatearRango(after.Fecha, after.FechaFin, tz);
                cambios.Add($"Fecha del evento: {antes} → {despues}");
            }

            if (!StringEq(before.Ubicacion, after.Ubicacion))
                cambios.Add($"Ubicación: {Txt(before.Ubicacion)} → {Txt(after.Ubicacion)}");

            if (!SameInstant(before.FechaFinInscripciones, after.FechaFinInscripciones))
            {
                cambios.Add(
                    $"Fin de inscripciones: {FormatearFechaHoraOpcional(before.FechaFinInscripciones, tz)} → {FormatearFechaHoraOpcional(after.FechaFinInscripciones, tz)}");
            }

            if (!SameInstant(before.FechaLimiteCambiosDelegados, after.FechaLimiteCambiosDelegados))
            {
                cambios.Add(
                    $"Límite cambios delegados: {FormatearFechaHoraOpcional(before.FechaLimiteCambiosDelegados, tz)} → {FormatearFechaHoraOpcional(after.FechaLimiteCambiosDelegados, tz)}");
            }

            if (!CsvEq(before.CategoriasHabilitadas, after.CategoriasHabilitadas))
                cambios.Add($"Categorías habilitadas: {LabelsCsv(before.CategoriasHabilitadas, CategoriaLabels)} → {LabelsCsv(after.CategoriasHabilitadas, CategoriaLabels)}");

            if (!CsvEq(before.BotesHabilitados, after.BotesHabilitados))
                cambios.Add($"Botes habilitados: {LabelsCsv(before.BotesHabilitados, BoteLabels)} → {LabelsCsv(after.BotesHabilitados, BoteLabels)}");

            if (!CsvEq(before.DistanciasHabilitadas, after.DistanciasHabilitadas))
                cambios.Add($"Distancias habilitadas: {LabelsCsv(before.DistanciasHabilitadas, DistanciaLabels)} → {LabelsCsv(after.DistanciasHabilitadas, DistanciaLabels)}");

            if (before.Estado != after.Estado)
                cambios.Add($"Estado: {before.Estado} → {after.Estado}");

            if (before.InscripcionesHabilitadas != after.InscripcionesHabilitadas)
                cambios.Add($"Inscripciones: {(before.InscripcionesHabilitadas ? "habilitadas" : "cerradas")} → {(after.InscripcionesHabilitadas ? "habilitadas" : "cerradas")}");

            if (!StringEq(before.Modalidad, after.Modalidad))
                cambios.Add($"Modalidad: {before.Modalidad} → {after.Modalidad}");

            return cambios;
        }

        private static (string Asunto, string Cuerpo) ConstruirMensajeCreacion(EventoDto evento, TimeZoneInfo tz)
        {
            var nombre = (evento.Nombre ?? string.Empty).Trim();
            var rangoEvento = FormatearRango(evento.Fecha, evento.FechaFin, tz);
            var finInscripciones = FormatearFechaHoraOpcional(evento.FechaFinInscripciones, tz);
            var limiteCambios = evento.FechaLimiteCambiosDelegados.HasValue
                ? FormatearFechaHoraOpcional(evento.FechaLimiteCambiosDelegados, tz)
                : (evento.FechaFinInscripciones.HasValue
                    ? $"{finInscripciones} (mismo que fin de inscripciones)"
                    : "sin fecha definida");

            var sb = new StringBuilder();
            sb.AppendLine($"Se creó el evento \"{nombre}\".");
            sb.AppendLine($"Fecha del evento: {rangoEvento}.");
            if (!string.IsNullOrWhiteSpace(evento.Ubicacion))
                sb.AppendLine($"Ubicación: {evento.Ubicacion.Trim()}.");
            sb.AppendLine($"Fin de inscripciones: {finInscripciones}.");
            sb.AppendLine($"Fecha límite para cambios de delegados: {limiteCambios}.");
            if (!string.IsNullOrWhiteSpace(evento.CategoriasHabilitadas)
                || !string.IsNullOrWhiteSpace(evento.BotesHabilitados)
                || !string.IsNullOrWhiteSpace(evento.DistanciasHabilitadas))
            {
                sb.AppendLine($"Categorías: {LabelsCsv(evento.CategoriasHabilitadas, CategoriaLabels)}.");
                sb.AppendLine($"Botes: {LabelsCsv(evento.BotesHabilitados, BoteLabels)}.");
                sb.AppendLine($"Distancias: {LabelsCsv(evento.DistanciasHabilitadas, DistanciaLabels)}.");
            }
            sb.Append("Ya podés revisar el evento e inscribir a tus atletas desde el panel del club.");

            return ($"Nuevo evento: {nombre}", sb.ToString());
        }

        private static string FormatearRango(DateTime fecha, DateTime? fechaFin, TimeZoneInfo tz)
        {
            var inicio = FormatearFecha(fecha, tz);
            if (!fechaFin.HasValue) return inicio;
            var fin = FormatearFecha(fechaFin.Value, tz);
            return string.Equals(inicio, fin, StringComparison.Ordinal) ? inicio : $"{inicio} al {fin}";
        }

        private static string FormatearFecha(DateTime utc, TimeZoneInfo tz)
        {
            var local = TimeZoneInfo.ConvertTimeFromUtc(AsUtc(utc), tz);
            return local.ToString("dd/MM/yyyy", EsAr);
        }

        private static string FormatearFechaHoraOpcional(DateTime? utc, TimeZoneInfo tz)
        {
            if (!utc.HasValue) return "sin fecha definida";
            var local = TimeZoneInfo.ConvertTimeFromUtc(AsUtc(utc.Value), tz);
            return local.ToString("dd/MM/yyyy HH:mm", EsAr);
        }

        private static DateTime AsUtc(DateTime value) =>
            value.Kind == DateTimeKind.Utc
                ? value
                : DateTime.SpecifyKind(value, DateTimeKind.Utc);

        private static bool SameDay(DateTime a, DateTime b) => AsUtc(a).Date == AsUtc(b).Date;

        private static bool SameDay(DateTime? a, DateTime? b)
        {
            if (!a.HasValue && !b.HasValue) return true;
            if (!a.HasValue || !b.HasValue) return false;
            return SameDay(a.Value, b.Value);
        }

        private static bool SameInstant(DateTime? a, DateTime? b)
        {
            if (!a.HasValue && !b.HasValue) return true;
            if (!a.HasValue || !b.HasValue) return false;
            return AsUtc(a.Value) == AsUtc(b.Value);
        }

        private static bool StringEq(string? a, string? b) =>
            string.Equals((a ?? string.Empty).Trim(), (b ?? string.Empty).Trim(), StringComparison.Ordinal);

        private static bool CsvEq(string? a, string? b)
        {
            static string Norm(string? s) =>
                string.Join(",",
                    (s ?? string.Empty)
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .OrderBy(x => x, StringComparer.Ordinal));
            return string.Equals(Norm(a), Norm(b), StringComparison.Ordinal);
        }

        private static string LabelsCsv(string? csv, IReadOnlyDictionary<int, string> labels)
        {
            var ids = (csv ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => int.TryParse(s, out var id) ? id : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .OrderBy(id => id)
                .ToList();

            if (ids.Count == 0) return "(ninguna)";
            return string.Join(", ", ids.Select(id =>
                labels.TryGetValue(id, out var name) ? name : $"#{id}"));
        }

        private static string Txt(string? value) =>
            string.IsNullOrWhiteSpace(value) ? "(sin dato)" : value.Trim();

        private static TimeZoneInfo ResolveTimeZone(string? timeZoneId)
        {
            foreach (var id in new[]
                     {
                         timeZoneId,
                         DefaultTimeZoneId,
                         WindowsArgentinaTimeZoneId,
                     })
            {
                if (string.IsNullOrWhiteSpace(id)) continue;
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(id);
                }
                catch (TimeZoneNotFoundException) { }
                catch (InvalidTimeZoneException) { }
            }

            return TimeZoneInfo.CreateCustomTimeZone(
                "SportTrack-ART",
                TimeSpan.FromHours(-3),
                "Argentina",
                "Argentina");
        }
    }
}
