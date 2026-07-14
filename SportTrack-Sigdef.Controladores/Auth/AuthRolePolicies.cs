namespace SportTrack_Sigdef.Controladores.Auth;

/// <summary>
/// Roles reutilizables para [Authorize(Roles = ...)] (claim ClaimTypes.Role).
/// </summary>
public static class AuthRolePolicies
{
    /// <summary>Roles que pueden operar carrera / hub / mutaciones de fases y resultados.</summary>
    public const string CompetitionOperators =
        "Admin,SuperAdmin,JuezControl,Largador,Cronometrista,soporte_tecnico";

    public const string Admins = "Admin,SuperAdmin,soporte_tecnico";

    /// <summary>Roles permitidos al registrar vía API (nunca SuperAdmin desde cliente).</summary>
    public static readonly string[] RegisterableRoles =
    {
        "Club",
        "Admin",
        "Largador",
        "Cronometrista",
        "JuezControl",
        "soporte_tecnico"
    };
}
