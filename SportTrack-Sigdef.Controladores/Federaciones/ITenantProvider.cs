using System.Security.Claims;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public interface ITenantProvider
    {
        int? GetFederacionId();
        int? GetClubId();
        string GetRol();
        ClaimsPrincipal GetUser();
    }
}
