using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SportTrack_Sigdef.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SportTrack_Sigdef.Controladores.Auth
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config, IHostEnvironment env)
        {
            // Misma resolución que Program.cs (sin fallback en producción)
            var tokenKey = ResolveTokenKey(config, env);
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        }

        /// <summary>
        /// Duplicado intencional liviano para no referenciar el proyecto Web desde Controladores.
        /// Debe mantenerse alineado con SportTrack_Sigdef.Security.TokenKeyResolver.
        /// </summary>
        internal static string ResolveTokenKey(IConfiguration config, IHostEnvironment env)
        {
            const string developmentFallback =
                "SportTrackSuperSecretKey2026!ForEducationalPurposeOnly_LongEnoughToBeSecure";

            var key = config["TokenKey"];
            if (!string.IsNullOrWhiteSpace(key))
                return key.Trim();

            if (env.IsDevelopment())
            {
                Console.WriteLine("⚠️ TokenKey no configurado: usando fallback solo para Development.");
                return developmentFallback;
            }

            throw new InvalidOperationException(
                "TokenKey no configurado. Definí la variable de entorno TokenKey antes de iniciar en producción.");
        }

        public string CreateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Username),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Role, usuario.RolFederacion),
                new Claim("ClubId", usuario.IdClub?.ToString() ?? "0"),
                new Claim("FederacionId", usuario.IdFederacion?.ToString() ?? "0")
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(5),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
