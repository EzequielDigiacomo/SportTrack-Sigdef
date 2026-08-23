using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using SportTrack_Sigdef.Controladores.Auth;
using SportTrack_Sigdef.Controladores.Caching;
using SportTrack_Sigdef.Controladores.Fase.Dtos;
using SportTrack_Sigdef.Controladores.Hubs;
using SportTrack_Sigdef.Controladores.Resultado;
using SportTrack_Sigdef.Controladores.Audit;
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
        private readonly IHubContext<TimingHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly ILiveCacheService _liveCache;
        private readonly IAuditService _auditService;

        public ResultadosController(
            IResultadoRepository resultadoRepository,
            IHubContext<TimingHub> hubContext,
            IMapper mapper,
            ILiveCacheService liveCache,
            IAuditService auditService)
        {
            _resultadoRepository = resultadoRepository;
            _hubContext = hubContext;
            _mapper = mapper;
            _liveCache = liveCache;
            _auditService = auditService;
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
            var aActualizar = new List<Entidades.Entidades.Resultado>();
            foreach(var item in dto)
            {
                var original = await _resultadoRepository.GetByIdAsync(item.Id);
                if(original != null)
                {
                    var isFullUpdate = item.Carril.HasValue
                        || !string.IsNullOrEmpty(item.ParticipanteNombre)
                        || !string.IsNullOrEmpty(item.ClubSigla);

                    if (!string.IsNullOrEmpty(item.Estado))
                    {
                        original.Estado = (SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum)Enum.Parse(typeof(SportTrack_Sigdef.Entidades.Enums.EstadoResultadoEnum), item.Estado);

                        if (original.Estado == Entidades.Enums.EstadoResultadoEnum.DNS
                            || original.Estado == Entidades.Enums.EstadoResultadoEnum.DNF
                            || original.Estado == Entidades.Enums.EstadoResultadoEnum.Descalificado)
                        {
                            original.TiempoOficial = null;
                            original.Posicion = null;
                        }
                    }

                    if (isFullUpdate)
                    {
                        original.TiempoOficial = item.TiempoOficial;
                        original.Posicion = item.Posicion;
                        if (item.Carril.HasValue) original.Carril = item.Carril;
                    }
                    else
                    {
                        if (item.TiempoOficial.HasValue) original.TiempoOficial = item.TiempoOficial;
                        if (item.Posicion.HasValue) original.Posicion = item.Posicion;
                        if (item.Carril.HasValue) original.Carril = item.Carril;
                    }
                    
                    if (original.Inscripcion?.Participante != null && !string.IsNullOrEmpty(item.ParticipanteNombre))
                    {
                        var nameParts = item.ParticipanteNombre.Trim().Split(' ');
                        if (nameParts.Length > 0)
                        {
                            original.Inscripcion.Participante.Nombre = nameParts[0];
                            original.Inscripcion.Participante.Apellido = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";
                        }
                    }

                    if (original.Inscripcion?.Participante?.Club != null && !string.IsNullOrEmpty(item.ClubSigla))
                    {
                        original.Inscripcion.Participante.Club.Siglas = item.ClubSigla.Trim();
                    }

                    original.FechaActualizacion = DateTime.UtcNow;
                    original.UsuarioActualizacion = HttpContext.User.Identity?.Name ?? "Sistema";
                    
                    aActualizar.Add(original);
                }
            }
            var guardados = await _resultadoRepository.UpdateManyAsync(aActualizar);

            if (guardados.Any())
            {
                var first = guardados.First();
                var eventoId = first.Fase?.Etapa?.EventoPrueba?.IdEvento;
                var eventoPruebaId = first.Fase?.Etapa?.EventoPruebaId;
                var faseId = first.FaseId;
                var conTiempo = guardados.Count(r => r.TiempoOficial.HasValue);
                await _auditService.RegistrarAccionAsync(
                    "SAVE_TIMING",
                    $"Tiempos guardados: fase {faseId}, {guardados.Count()} filas ({conTiempo} con tiempo).",
                    null,
                    "Competencia",
                    eventoId,
                    eventoPruebaId);

                foreach (var r in guardados)
                {
                    var evId = r.Fase?.Etapa?.EventoPrueba?.IdEvento;
                    var evPruebaId = r.Fase?.Etapa?.EventoPruebaId;
                    _liveCache.InvalidateFase(r.FaseId, evId, evPruebaId);

                    if (evId.HasValue && r.Fase?.Etapa != null)
                    {
                        await _hubContext.Clients.Group(TimingGroups.Event(evId.Value)).SendAsync(
                            "ResultadoActualizado",
                            r.Fase.Etapa.EventoPruebaId,
                            _mapper.Map<ResultadoFaseDto>(r));
                    }
                }
            }

            return Ok(_mapper.Map<IEnumerable<ResultadoFaseDto>>(guardados));
        }
    }

    public class ResultadoUpdateDto
    {
        public int Id { get; set; }
        public TimeSpan? TiempoOficial { get; set; }
        public int? Posicion { get; set; }
        public string? Estado { get; set; }
        public int? Carril { get; set; }
        public string? ParticipanteNombre { get; set; }
        public string? ClubSigla { get; set; }
    }
}
