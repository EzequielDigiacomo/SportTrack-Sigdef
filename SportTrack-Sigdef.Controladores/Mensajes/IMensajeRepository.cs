using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.Mensajes
{
    public interface IMensajeRepository
    {
        Task<Usuario?> GetUsuarioByUsernameAsync(string username);
        Task<Usuario?> GetUsuarioByIdAsync(int id);
        Task<List<Hilo>> GetHilosVisiblesAsync(int usuarioId, bool esSuperAdmin);
        Task<Hilo?> GetHiloConMensajesAsync(int hiloId);
        Task<bool> UsuarioParticipaEnHiloAsync(int hiloId, int usuarioId);
        Task AddHiloAsync(Hilo hilo);
        Task AddMensajeAsync(Mensaje mensaje);
        Task SaveChangesAsync();
    }
}
