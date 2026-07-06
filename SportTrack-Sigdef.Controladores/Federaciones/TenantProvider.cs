using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? GetUser()
        {
            return _httpContextAccessor.HttpContext?.User;
        }

        public int? GetFederacionId()
        {
            var user = GetUser();
            if (user == null) return null;

            var fromQuery = ParseQueryInt("idFederacion", "federacionId");
            if (IsGlobalAdmin() && fromQuery.HasValue)
            {
                return fromQuery;
            }

            var federacionIdClaim = user.FindFirst("FederacionId")?.Value;
            if (int.TryParse(federacionIdClaim, out int federacionId) && federacionId > 0)
            {
                return federacionId;
            }

            // SuperAdmin / soporte sin scope explícito: sin filtro de federación
            if (IsGlobalAdmin()) return null;

            return null;
        }

        public int? GetClubId()
        {
            var user = GetUser();
            if (user == null) return null;

            var fromQuery = ParseQueryInt("idClub", "clubId");
            if (IsGlobalAdmin() && fromQuery.HasValue)
            {
                return fromQuery;
            }

            var clubIdClaim = user.FindFirst("ClubId")?.Value;
            if (int.TryParse(clubIdClaim, out int clubId) && clubId > 0)
            {
                return clubId;
            }

            if (IsGlobalAdmin()) return null;

            return null;
        }

        public string GetRol()
        {
            var user = GetUser();
            return user?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        private bool IsGlobalAdmin()
        {
            var rol = GetRol();
            return string.Equals(rol, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(rol, "soporte_tecnico", StringComparison.OrdinalIgnoreCase);
        }

        private int? ParseQueryInt(params string[] keys)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return null;

            foreach (var key in keys)
            {
                if (request.Query.TryGetValue(key, out var value)
                    && int.TryParse(value.ToString(), out int id)
                    && id > 0)
                {
                    return id;
                }
            }

            return null;
        }
    }
}
