using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SportTrack_Sigdef.Controladores.Auth;
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

        public TokenService(IConfiguration config)
        {
            // Debe coincidir con la clave usada en Program.cs para JwtBearer
            var tokenKey = config["TokenKey"] ?? "SportTrackSuperSecretKey2026!ForEducationalPurposeOnly_LongEnoughToBeSecure";
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
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
