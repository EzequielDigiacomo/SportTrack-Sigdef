using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Controladores.Club;
using SportTrack_Sigdef.Controladores.Club.Dtos;
using SportTrack_Sigdef.Controladores.Federaciones;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers.Clubes
{
    /// <summary>
    /// API de clubes para SportTrack (competencias). SIGDEF usa api/Club con el mismo modelo de datos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClubesController : ControllerBase
    {
        private readonly IClubService _clubService;
        private readonly ITenantProvider _tenantProvider;

        public ClubesController(IClubService clubService, ITenantProvider tenantProvider)
        {
            _clubService = clubService;
            _tenantProvider = tenantProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClubDto>>> GetClubes()
        {
            var result = (await _clubService.GetAllClubesAsync()).ToList();
            var fedId = _tenantProvider.GetFederacionId();

            if (fedId.HasValue)
            {
                result = result.Where(c => c.FederacionId == fedId.Value).ToList();
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClubDto>> GetClub(int id)
        {
            var result = await _clubService.GetClubByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ClubDto>> CreateClub(ClubCreateDto clubDto)
        {
            var fedId = _tenantProvider.GetFederacionId();
            if (fedId.HasValue && !clubDto.FederacionId.HasValue)
            {
                clubDto.FederacionId = fedId;
            }

            var result = await _clubService.CreateClubAsync(clubDto);
            return CreatedAtAction(nameof(GetClub), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ClubDto>> UpdateClub(int id, ClubUpdateDto clubDto)
        {
            var result = await _clubService.UpdateClubAsync(id, clubDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteClub(int id)
        {
            await _clubService.DeleteClubAsync(id);
            return NoContent();
        }
    }
}
