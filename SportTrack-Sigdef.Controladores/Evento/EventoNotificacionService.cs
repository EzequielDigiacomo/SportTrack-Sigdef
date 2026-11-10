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
        private static readonly CultureInfo EsAr = CultureInfo.GetCultureInfo("es-AR");

        private readonly IMensajeRepository _mensajeRepository;
        private readonly IMensajeService _mensajeService;

        public EventoNotificacionService(IMensajeRepository mensajeRepository, IMensajeService mensajeService)
        {
            _mensajeRepository = mensajeRepository;
            _mensajeService = mensajeService;
        }

        public async Task NotificarEventoCreadoAsync(EventoDto evento)
        {
            try
            {
                if (evento == null) return;
                if (!evento.FederacionId.HasValue || evento.FederacionId.Value <= 0) return;
                if (evento.Nombre.Contains("control", StringComparison.OrdinalIgnoreCase)) return;

                var destinatarios = await _mensajeRepository
                    .GetUsuariosActivosClubesByFederacionAsync(evento.FederacionId.Value);

                var ids = destinatarios.Select(u => u.IdUsuario).Distinct().ToList();
                if (ids.Count == 0) return;

                var tz = ResolveTimeZone(evento.TimeZoneId);
                var (asunto, cuerpo) = ConstruirMensaje(evento, tz);

                await _mensajeService.EnviarNotificacionAutomaticaAsync(
                    evento.FederacionId.Value,
                    ids,
                    asunto,
                    cuerpo,
                    MensajeriaSistemaOrigen.SportTrack);
            }
            catch
            {
                // No interrumpir la creación del evento si falla el aviso.
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
            var local = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), tz);
            return local.ToString("dd/MM/yyyy", EsAr);
        }

        private static string FormatearFechaHora(DateTime utc, TimeZoneInfo tz)
        {
            var local = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), tz);
            return local.ToString("dd/MM/yyyy HH:mm", EsAr);
        }

        private static TimeZoneInfo ResolveTimeZone(string? timeZoneId)
        {
            if (string.IsNullOrWhiteSpace(timeZoneId))
                timeZoneId = DefaultTimeZoneId;

            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(DefaultTimeZoneId);
                }
                catch
                {
                    return TimeZoneInfo.Utc;
                }
            }
            catch (InvalidTimeZoneException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById(DefaultTimeZoneId);
            }
        }
    }
}
