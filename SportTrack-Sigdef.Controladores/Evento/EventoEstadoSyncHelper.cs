using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.Enums;
using System;

namespace SportTrack_Sigdef.Controladores.Evento
{
    /// <summary>
    /// Calcula el estado del evento según fechas y zona horaria configurada.
    /// </summary>
    public static class EventoEstadoSyncHelper
    {
        private const string DefaultTimeZoneId = "America/Argentina/Buenos_Aires";

        public static EstadoEventoEnum ComputeEstado(SportTrack_Sigdef.Entidades.Entidades.Evento evento, DateTime utcNow)
        {
            if (evento.Estado == EstadoEventoEnum.Cancelado)
                return EstadoEventoEnum.Cancelado;

            var tz = ResolveTimeZone(evento.TimeZoneId);
            var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(utcNow, tz);

            var startLocal = ToLocalDateTime(evento.Fecha, evento.HoraInicioEvento, tz);
            var endSource = evento.FechaFin ?? evento.Fecha;
            var endLocal = ToLocalEndOfDay(endSource, tz);

            if (nowLocal >= endLocal)
                return EstadoEventoEnum.Finalizado;

            if (nowLocal >= startLocal)
                return EstadoEventoEnum.EnCurso;

            return EstadoEventoEnum.Programada;
        }

        public static bool ShouldAutoSync(EstadoEventoEnum estado) =>
            estado is EstadoEventoEnum.Programada or EstadoEventoEnum.EnCurso;

        private static DateTime ToLocalDateTime(DateTime utcDate, TimeSpan timeOfDay, TimeZoneInfo tz)
        {
            var utc = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);
            var localDate = TimeZoneInfo.ConvertTimeFromUtc(utc, tz).Date;
            return localDate.Add(timeOfDay);
        }

        private static DateTime ToLocalEndOfDay(DateTime utcDate, TimeZoneInfo tz)
        {
            var utc = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);
            var localDate = TimeZoneInfo.ConvertTimeFromUtc(utc, tz).Date;
            return localDate.AddHours(23).AddMinutes(59).AddSeconds(59);
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
