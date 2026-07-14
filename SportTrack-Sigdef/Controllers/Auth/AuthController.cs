using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Auth;
using SportTrack_Sigdef.Controladores.Auth.Dtos;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var clientApp = Request.Headers["X-Client-App"].FirstOrDefault();
            var result = await _authService.LoginAsync(loginDto, clientApp);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = Request.IsHttps ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddHours(5)
            };

            Response.Cookies.Append("X-Access-Token", result.Token, cookieOptions);
            return Ok(result);
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("X-Access-Token", new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = Request.IsHttps ? SameSiteMode.None : SameSiteMode.Lax
            });
            return Ok(new { message = "Sesión cerrada correctamente" });
        }

        /// <summary>Alta de usuarios solo desde panel admin (JWT). Nunca SuperAdmin vía API.</summary>
        [HttpPost("register")]
        [Authorize(Roles = AuthRolePolicies.Admins)]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            await _authService.RegisterAsync(registerDto);
            return Ok(new { message = "Usuario registrado con éxito" });
        }

        [HttpGet("usuarios")]
        [Authorize]
        public async Task<ActionResult> GetUsuarios()
        {
            var username = User.Identity?.Name;
            var result = await _authService.GetUsuariosAsync(username);
            return Ok(result);
        }

        [HttpPut("usuarios/{id}/password")]
        [Authorize]
        public async Task<ActionResult> UpdatePassword(int id, [FromBody] string newPassword)
        {
            if (!await CanManageUserAsync(id))
                return Forbid();

            await _authService.UpdatePasswordAsync(id, newPassword);
            return Ok(new { message = "Contraseña actualizada con éxito" });
        }

        [HttpPut("usuarios/{id}/perfil")]
        [Authorize]
        public async Task<ActionResult> UpdatePerfil(int id, [FromBody] UpdatePerfilDto dto)
        {
            if (!await CanManageUserAsync(id))
                return Forbid();

            await _authService.UpdatePerfilAsync(id, dto);
            return Ok(new { message = "Perfil actualizado con éxito" });
        }

        [HttpPatch("usuarios/{id}/toggle-activo")]
        [Authorize(Roles = "Admin,SuperAdmin,soporte_tecnico")]
        public async Task<ActionResult> ToggleActivo(int id)
        {
            await _authService.ToggleActivoAsync(id);
            return Ok(new { message = "Estado de cuenta actualizado correctamente" });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UsuarioDto>> GetMe()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var clientApp = Request.Headers["X-Client-App"].FirstOrDefault();
            var result = await _authService.GetMeAsync(username, clientApp);
            return Ok(result);
        }

        /// <summary>Self o Admin/SuperAdmin/soporte.</summary>
        private async Task<bool> CanManageUserAsync(int targetUserId)
        {
            var username = User.FindFirstValue(ClaimTypes.Name)
                           ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
                           ?? User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return false;

            var me = await _authService.GetMeAsync(username);
            if (me.Id == targetUserId) return true;

            var role = (me.RolFederacion ?? "").Trim();
            return role is "Admin" or "SuperAdmin" or "soporte_tecnico";
        }
    }
}
