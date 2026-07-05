using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.Auth
{
    public interface ITokenService
    {
        string CreateToken(Usuario usuario);
    }
}
