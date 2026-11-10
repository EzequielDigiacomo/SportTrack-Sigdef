using Microsoft.Extensions.Logging;
using SportTrack_Sigdef.Controladores.Evento.Dtos;
using SportTrack_Sigdef.Controladores.Mensajes;
using SportTrack_Sigdef.Entidades.Entidades;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Evento
{
    public interface IEventoNotificacionService
    {
        Task NotificarEventoCreadoAsync(EventoDto evento);
    }

    /// <summary>
    /// Avisa por mensajería interna a todos los clubes de la federación
    /// cuando se crea un evento (no controles).
    /// </summary>
    public class EventoNotificacionService : IEventoNotificacionService
    {
        private const string DefaultTimeZoneId = "America/Argentina/Buenos_Aires";
        private const string WindowsArgentinaTimeZoneId = "Argentina Standard Time";
        private static readonly CultureInfo EsAr = CultureInfo.GetCultureInfo("es-AR");

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
                if (evento.Nombre.Contains("control", StringComparison.OrdinalIgnoreCase)) return;

                var fedId = evento.FederacionId.Value;
                var destinatarios = await _mensajeRepository
                    .GetUsuariosActivosClubesByFederacionAsync(fedId);

                var ids = destinatarios.Select(u => u.IdUsuario).Distinct().ToList();
                if (ids.Count == 0)
                {
                    _logger.LogWarning(
                        "Evento {EventoId} fed {FedId}: no hay usuarios Club activos para mensaje privado.",
                        evento.Id, fedId);
                    return;
                }

                var tz = ResolveTimeZone(evento.TimeZoneId);
                var (asunto, cuerpo) = ConstruirMensaje(evento, tz);

                await _mensajeService.EnviarNotificacionAutomaticaAsync(
                    fedId,
                    ids,
                    asunto,
                    cuerpo,
                    MensajeriaSistemaOrigen.SportTrack);

                _logger.LogInformation(
                    "Evento {EventoId}: mensaje privado enviado a {Count} usuarios Club (fed {FedId}).",
                    evento.Id, ids.Count, fedId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Evento {EventoId}: falló el mensaje privado de creación.", evento?.Id);
            }
        }

        private static (string Asunto, string Cuerpo) ConstruirMensaje(EventoDto evento, TimeZoneInfo tz)
        {
            var nombre = (evento.Nombre ?? string.Empty).Trim();
            var fechaInicio = FormatearFecha(evento.Fecha, tz);
            var fechaFin = evento.FechaFin.HasValue
                ? FormatearFecha(evento.FechaFin.Value, tz)
                : fechaInicio;

            var rangoEvento = string.Equals(fechaInicio, fechaFin, StringComparison.Ordinal)
                ? fechaInicio
                : $"{fechaInicio} al {fechaFin}";

            var finInscripciones = evento.FechaFinInscripciones.HasValue
                ? FormatearFechaHora(evento.FechaFinInscripciones.Value, tz)
                : "sin fecha definida";

            var limiteCambios = evento.FechaLimiteCambiosDelegados.HasValue
                ? FormatearFechaHora(evento.FechaLimiteCambiosDelegados.Value, tz)
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
            sb.Append("Ya podés revisar el evento e inscribir a tus atletas desde el panel del club.");

            return ($"Nuevo evento: {nombre}", sb.ToString());
        }

        private static string FormatearFecha(DateTime utc, TimeZoneInfo tz)
        {
            var local = TimeZoneInfo.ConvertTimeFromUtc(AsUtc(utc), tz);
            return local.ToString("dd/MM/yyyy", EsAr);
        }

        private static string FormatearFechaHora(DateTime utc, TimeZoneInfo tz)
        {
            var local = TimeZoneInfo.ConvertTimeFromUtc(AsUtc(utc), tz);
            return local.ToString("dd/MM/yyyy HH:mm", EsAr);
        }

        private static DateTime AsUtc(DateTime value) =>
            value.Kind == DateTimeKind.Utc
                ? value
                : DateTime.SpecifyKind(value, DateTimeKind.Utc);

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
