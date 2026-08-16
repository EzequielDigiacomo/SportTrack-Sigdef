namespace SportTrack_Sigdef.Controladores.Auth;

/// <summary>
/// Roles reutilizables para [Authorize(Roles = ...)] (claim ClaimTypes.Role).
/// </summary>
public static class AuthRolePolicies
{
    /// <summary>Roles que pueden operar carrera / hub / mutaciones de fases y resultados.</summary>
    public const string CompetitionOperators =
        "Admin,SuperAdmin,JuezControl,Largador,Cronometrista,ControlTecnico,soporte_tecnico";

    public const string Admins = "Admin,SuperAdmin,soporte_tecnico";

    /// <summary>Roles de alto poder: no se exponen en búsquedas por DNI de altas operativas (delegado, etc.).</summary>
    public static readonly string[] PrivilegedRoles =
    {
        "Admin",
        "SuperAdmin",
        "soporte_tecnico"
    };

    public static bool IsPrivilegedRole(string? rol) =>
        !string.IsNullOrWhiteSpace(rol)
        && PrivilegedRoles.Any(r => string.Equals(r, rol.Trim(), StringComparison.OrdinalIgnoreCase));

    /// <summary>Roles permitidos al registrar vía API (nunca SuperAdmin desde cliente).</summary>
    public static readonly string[] RegisterableRoles =
    {
        "Club",
        "Admin",
        "Largador",
        "Cronometrista",
        "JuezControl",
        "ControlTecnico",
        "soporte_tecnico"
    };
}
