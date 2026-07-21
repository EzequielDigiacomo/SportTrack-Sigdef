using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.Mensajes
{
    public interface IMensajeRepository
    {
        Task<Usuario?> GetUsuarioByUsernameAsync(string username);
        Task<Usuario?> GetUsuarioByIdAsync(int id);
        Task<List<Usuario>> GetUsuariosByIdsAsync(IEnumerable<int> ids);
        Task<List<Hilo>> GetHilosVisiblesAsync(int usuarioId, bool esSuperAdmin, string sistemaOrigen, int? campanaId = null);
        Task<Hilo?> GetHiloConMensajesAsync(int hiloId);
        Task<bool> UsuarioParticipaEnHiloAsync(int hiloId, int usuarioId);
        Task AddHiloAsync(Hilo hilo);
        Task AddMensajeAsync(Mensaje mensaje);
        Task AddCampanaAsync(CampanaEnvio campana);
        Task<List<CampanaEnvio>> GetCampanasByRemitenteAsync(int remitenteId, string sistemaOrigen);
        Task<CampanaEnvio?> GetCampanaDetalleAsync(int campanaId);
        Task SaveChangesAsync();
        Task<int> CountNoLeidosAsync(int usuarioId, string sistemaOrigen);
        Task<Usuario?> GetEmisorNotificacionFederacionAsync(int idFederacion);
        Task<List<Usuario>> GetUsuariosActivosByClubAsync(int clubId);
        Task<List<Usuario>> GetUsuariosAdminByFederacionAsync(int idFederacion);
        Task<List<Usuario>> GetUsuariosSuperAdminAsync();
    }
}
