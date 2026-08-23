using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Audit
{
    public interface IAuditService
    {
        Task RegistrarAccionAsync(
            string accion,
            string detalle,
            string? usuario = null,
            string modulo = "General",
            int? idEvento = null,
            int? idEventoPrueba = null);

        Task RegistrarErrorAsync(Exception ex, string modulo = "System", int? idEvento = null, int? idEventoPrueba = null);
    }
}
