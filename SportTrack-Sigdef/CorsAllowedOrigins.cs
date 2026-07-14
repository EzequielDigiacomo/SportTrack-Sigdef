using System;
using System.Collections.Generic;

namespace SportTrack_Sigdef;

/// <summary>
/// Orígenes CORS permitidos (compartido entre policy y ExceptionMiddleware).
/// </summary>
public sealed class CorsAllowedOrigins
{
    private readonly HashSet<string> _origins;

    public CorsAllowedOrigins(IEnumerable<string> origins)
    {
        _origins = new HashSet<string>(
            origins.Where(o => !string.IsNullOrWhiteSpace(o)).Select(o => o.Trim()),
            StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyCollection<string> Origins => _origins;

    public bool IsAllowed(string? origin) =>
        !string.IsNullOrWhiteSpace(origin) && _origins.Contains(origin.Trim());
}
