using System;

namespace SportTrack_Sigdef.Controladores.Auth;

/// <summary>
/// Duración de sesión JWT/cookie según rol.
/// Jueces de campo en evento largo necesitan ventana extendida para reenviar tiempos.
/// </summary>
public static class SessionLifetimePolicy
{
    public const int DefaultHours = 5;
    public const int CronometristaHours = 24;

    public static TimeSpan ForRole(string? rolFederacion)
    {
        if (string.Equals(rolFederacion?.Trim(), "Cronometrista", StringComparison.OrdinalIgnoreCase))
            return TimeSpan.FromHours(CronometristaHours);

        return TimeSpan.FromHours(DefaultHours);
    }
}
