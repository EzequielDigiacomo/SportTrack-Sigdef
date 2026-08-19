using SportTrack_Sigdef.Controladores.Auth.Dtos;
using SportTrack_Sigdef.Controladores.Exceptions;
using SportTrack_Sigdef.Controladores.SaaS;
using System;

namespace SportTrack_Sigdef.Controladores.Auth;

/// <summary>Resuelve el tenant (federación/club) efectivo para listados y accesos.</summary>
public static class TenantScopeHelper
{
    public record EventListScope(string Role, int? ClubId, int? FederacionId);

    public static bool IsSuperAdmin(string? rol) =>
        string.Equals(rol?.Trim(), "SuperAdmin", StringComparison.OrdinalIgnoreCase);

    public static bool RequiresFederation(string? rol) =>
        PlanSaaSAccessHelper.IsJudgeRole(rol)
        || string.Equals(rol?.Trim(), "Admin", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// SuperAdmin: respeta filtros de query. Resto: fuerza club/federación del usuario.
    /// Jueces y Admin sin federación resuelta → scope vacío (sin eventos).
    /// </summary>
    public static EventListScope ResolveEventListScope(
        UsuarioDto user,
        int? queryClubId = null,
        int? queryFederacionId = null)
    {
        var role = user.RolFederacion ?? string.Empty;

        if (IsSuperAdmin(role))
            return new EventListScope(role, queryClubId, queryFederacionId);

        if (user.FederacionId is > 0)
            return new EventListScope(role, null, user.FederacionId);

        if (user.ClubId is > 0)
            return new EventListScope(role, user.ClubId, null);

        return new EventListScope(role, null, null);
    }

    public static EventListScope ResolveEventListScopeFromClaims(
        string? role,
        int? claimClubId,
        int? claimFederacionId,
        int? queryClubId = null,
        int? queryFederacionId = null)
    {
        role ??= string.Empty;

        if (IsSuperAdmin(role))
            return new EventListScope(role, queryClubId, queryFederacionId);

        if (claimFederacionId is > 0)
            return new EventListScope(role, null, claimFederacionId);

        if (claimClubId is > 0)
            return new EventListScope(role, claimClubId, null);

        return new EventListScope(role, null, null);
    }

    public static void EnsureJudgeHasFederation(string? rol, int? federacionId)
    {
        if (!PlanSaaSAccessHelper.IsJudgeRole(rol))
            return;

        if (!federacionId.HasValue || federacionId.Value <= 0)
            throw new UnauthorizedException(
                "Tu cuenta de juez no está vinculada a una federación. Pedí al administrador que cree un nuevo login con federación asignada.");
    }
}
