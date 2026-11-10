using SportTrack_Sigdef.Entidades.Enums;
using System;

namespace SportTrack_Sigdef.Controladores.Inscripcion
{
    public static class EventoCambiosPolicy
    {
        public static DateTime? ResolveDeadline(Entidades.Entidades.Evento evento)
            => evento.FechaLimiteCambiosDelegados ?? evento.FechaFinInscripciones;

        public static bool PermiteCambiosDelegados(Entidades.Entidades.Evento evento)
        {
            if (!evento.CambiosDelegadosHabilitados)
                return false;

            if (evento.Estado == EstadoEventoEnum.Finalizado || evento.Estado == EstadoEventoEnum.Cancelado)
                return false;

            var deadline = ResolveDeadline(evento);
            if (deadline.HasValue && DateTime.UtcNow > deadline.Value)
                return false;

            return true;
        }

        public static bool PermiteLiveOpsAdmin(Entidades.Entidades.Evento evento)
        {
            return evento.Estado != EstadoEventoEnum.Cancelado;
        }
    }
}
