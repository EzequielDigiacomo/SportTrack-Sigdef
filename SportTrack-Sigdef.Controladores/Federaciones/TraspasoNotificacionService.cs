using SportTrack_Sigdef.Controladores.Mensajes;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public enum TraspasoNotificacionEvento
    {
        SolicitudCreada,
        OrigenAcepto,
        OrigenRechazo,
        FederacionAprobo,
        FederacionRechazo,
        Cancelado
    }

    public interface ITraspasoNotificacionService
    {
        Task NotificarAsync(SolicitudTraspaso solicitud, TraspasoNotificacionEvento evento);
    }

    public class TraspasoNotificacionService : ITraspasoNotificacionService
    {
        private readonly IMensajeRepository _mensajeRepository;
        private readonly IMensajeService _mensajeService;

        public TraspasoNotificacionService(IMensajeRepository mensajeRepository, IMensajeService mensajeService)
        {
            _mensajeRepository = mensajeRepository;
            _mensajeService = mensajeService;
        }

        public async Task NotificarAsync(SolicitudTraspaso solicitud, TraspasoNotificacionEvento evento)
        {
            try
            {
                var destinatarios = await ResolverDestinatariosAsync(solicitud, evento);
                if (destinatarios.Count == 0) return;

                var atletaNombre = solicitud.Participante != null
                    ? $"{solicitud.Participante.Nombre} {solicitud.Participante.Apellido}".Trim()
                    : $"Atleta #{solicitud.ParticipanteId}";

                var origen = solicitud.ClubOrigen?.Nombre ?? $"Club #{solicitud.IdClubOrigen}";
                var destino = solicitud.ClubDestino?.Nombre ?? $"Club #{solicitud.IdClubDestino}";

                var (asunto, cuerpo) = evento switch
                {
                    TraspasoNotificacionEvento.SolicitudCreada => (
                        $"Nueva solicitud de traspaso #{solicitud.IdSolicitudTraspaso}",
                        $"El club {destino} solicitó el traspaso de {atletaNombre} desde {origen}. " +
                        $"Ingrese a Traspasos → Salidas pendientes para aceptar o rechazar."
                    ),
                    TraspasoNotificacionEvento.OrigenAcepto => (
                        $"Traspaso #{solicitud.IdSolicitudTraspaso} aceptado por club origen",
                        $"El club {origen} aceptó el traspaso de {atletaNombre} hacia {destino}. " +
                        $"La solicitud está pendiente de aprobación federativa."
                    ),
                    TraspasoNotificacionEvento.OrigenRechazo => (
                        $"Traspaso #{solicitud.IdSolicitudTraspaso} rechazado por club origen",
                        $"El club {origen} rechazó el traspaso de {atletaNombre} hacia {destino}." +
                        (string.IsNullOrWhiteSpace(solicitud.MotivoRechazo) ? string.Empty : $" Motivo: {solicitud.MotivoRechazo}")
                    ),
                    TraspasoNotificacionEvento.FederacionAprobo => (
                        $"Traspaso #{solicitud.IdSolicitudTraspaso} aprobado",
                        $"La federación aprobó el traspaso de {atletaNombre} de {origen} a {destino}. El cambio de club ya fue ejecutado."
                    ),
                    TraspasoNotificacionEvento.FederacionRechazo => (
                        $"Traspaso #{solicitud.IdSolicitudTraspaso} rechazado por federación",
                        $"La federación rechazó el traspaso de {atletaNombre} ({origen} → {destino})." +
                        (string.IsNullOrWhiteSpace(solicitud.MotivoRechazo) ? string.Empty : $" Motivo: {solicitud.MotivoRechazo}")
                    ),
                    TraspasoNotificacionEvento.Cancelado => (
                        $"Traspaso #{solicitud.IdSolicitudTraspaso} cancelado",
                        $"El club {destino} canceló la solicitud de traspaso de {atletaNombre} desde {origen}."
                    ),
                    _ => (string.Empty, string.Empty)
                };

                if (string.IsNullOrWhiteSpace(asunto)) return;

                await _mensajeService.EnviarNotificacionAutomaticaAsync(
                    solicitud.IdFederacion,
                    destinatarios,
                    asunto,
                    cuerpo);
            }
            catch
            {
                // No interrumpir el workflow de traspaso si falla la notificación
            }
        }

        private async Task<List<int>> ResolverDestinatariosAsync(
            SolicitudTraspaso solicitud,
            TraspasoNotificacionEvento evento)
        {
            var ids = new HashSet<int>();

            switch (evento)
            {
                case TraspasoNotificacionEvento.SolicitudCreada:
                    AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubOrigen));
                    break;

                case TraspasoNotificacionEvento.OrigenAcepto:
                    AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino));
                    AddUsuarios(ids, await _mensajeRepository.GetUsuariosAdminByFederacionAsync(solicitud.IdFederacion));
                    break;

                case TraspasoNotificacionEvento.OrigenRechazo:
                    AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino));
                    break;

                case TraspasoNotificacionEvento.FederacionAprobo:
                case TraspasoNotificacionEvento.FederacionRechazo:
                    AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubOrigen));
                    AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubDestino));
                    break;

                case TraspasoNotificacionEvento.Cancelado:
                    AddUsuarios(ids, await _mensajeRepository.GetUsuariosActivosByClubAsync(solicitud.IdClubOrigen));
                    if (solicitud.Estado == EstadoSolicitudTraspaso.Cancelado)
                    {
                        AddUsuarios(ids, await _mensajeRepository.GetUsuariosAdminByFederacionAsync(solicitud.IdFederacion));
                    }
                    break;
            }

            return ids.ToList();
        }

        private static void AddUsuarios(ISet<int> ids, IEnumerable<Usuario> usuarios)
        {
            foreach (var u in usuarios)
                ids.Add(u.IdUsuario);
        }
    }
}
