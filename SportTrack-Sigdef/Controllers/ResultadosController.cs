using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SportTrack_Sigdef.Controladores.Auth;
using SportTrack_Sigdef.Controladores.Caching;
using SportTrack_Sigdef.Controladores.Fase.Dtos;
using SportTrack_Sigdef.Controladores.Resultado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultadosController : ControllerBase
    {
        private static readonly TimeSpan LiveReadTtl = TimeSpan.FromSeconds(30);

        private readonly IResultadoRepository _resultadoRepository;
        private readonly IResultadoBatchUpdateService _batchUpdateService;
        private readonly ILiveCacheService _liveCache;
        private readonly AutoMapper.IMapper _mapper;

        public ResultadosController(
            IResultadoRepository resultadoRepository,
            IResultadoBatchUpdateService batchUpdateService,
            ILiveCacheService liveCache,
            AutoMapper.IMapper mapper)
        {
            _resultadoRepository = resultadoRepository;
            _batchUpdateService = batchUpdateService;
            _liveCache = liveCache;
            _mapper = mapper;
        }

        [HttpGet("Fase/{faseId}")]
        [AllowAnonymous]
        [EnableRateLimiting("live")]
        public async Task<ActionResult<IEnumerable<ResultadoFaseDto>>> GetResultadosPorFase(int faseId)
        {
            var dtos = await _liveCache.GetOrCreateAsync(
                LiveCacheKeys.ResultadosByFase(faseId),
                LiveReadTtl,
                async () =>
                {
                    var resultados = await _resultadoRepository.GetByFaseIdAsync(faseId);
                    return _mapper.Map<IEnumerable<ResultadoFaseDto>>(resultados);
                });

            return Ok(dtos);
        }

        [HttpPut("BatchUpdate")]
        [Authorize(Roles = AuthRolePolicies.CompetitionOperators)]
        public async Task<ActionResult<IEnumerable<ResultadoFaseDto>>> BatchUpdate(List<ResultadoUpdateDto> dto)
        {
            var username = HttpContext.User.Identity?.Name;
            var guardados = await _batchUpdateService.ApplyBatchUpdateAsync(dto, username);
            return Ok(_mapper.Map<IEnumerable<ResultadoFaseDto>>(guardados));
        }
    }
}
