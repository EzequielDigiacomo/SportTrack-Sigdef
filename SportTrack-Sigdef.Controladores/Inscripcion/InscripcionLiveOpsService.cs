using SportTrack_Sigdef.Controladores.Exceptions;
using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;
using SportTrack_Sigdef.Controladores.Caching;
using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportTrack_Sigdef.Controladores.Audit;
using EntResultado = SportTrack_Sigdef.Entidades.Entidades.Resultado;
using EntInscripcion = SportTrack_Sigdef.Entidades.Entidades.Inscripcion;

namespace SportTrack_Sigdef.Controladores.Inscripcion
{
    public class InscripcionLiveOpsService : IInscripcionLiveOpsService
    {
        private readonly IInscripcionService _inscripcionService;
        private readonly IInscripcionRepository _repo;
        private readonly IAuditService _auditService;
        private readonly ILiveCacheService _liveCache;

        public InscripcionLiveOpsService(
            IInscripcionService inscripcionService,
            IInscripcionRepository repo,
            IAuditService auditService,
            ILiveCacheService liveCache)
        {
            _inscripcionService = inscripcionService;
            _repo = repo;
            _auditService = auditService;
            _liveCache = liveCache;
        }

        public async Task<InscripcionDto> UpdateTripulacionAdminAsync(int inscripcionId, InscripcionTripulacionUpdateDto dto)
        {
            var result = await _inscripcionService.UpdateTripulacionAsync(
                inscripcionId, dto, clubScope: null, bypassDeadline: true, requireSameClub: true);
            var insc = await _repo.GetByIdAsync(inscripcionId);
            InvalidateScope(insc?.EventoPrueba?.IdEvento, insc?.IdEventoPrueba ?? result.EventoPruebaId);
            return result;
        }

        public async Task PatchParticipanteNombreAsync(int participanteId, ParticipanteNombrePatchDto dto)
        {
            var p = await _repo.GetParticipanteAsync(participanteId)
                ?? throw new NotFoundException($"Participante {participanteId} no encontrado");

            if (!string.IsNullOrWhiteSpace(dto.Nombre))
                p.Nombre = dto.Nombre.Trim();
            if (!string.IsNullOrWhiteSpace(dto.Apellido))
                p.Apellido = dto.Apellido.Trim();

            await _repo.SaveParticipanteAsync(p);
            await _auditService.RegistrarAccionAsync(
                "LIVE_PATCH_NAME",
                $"Participante {participanteId} renombrado a {p.Nombre} {p.Apellido}",
                null,
                "Participantes");
        }

        public async Task<InscripcionDto> UpdateNumeroCompetidorAsync(int inscripcionId, string numeroCompetidor)
        {
            var insc = await _repo.GetByIdAsync(inscripcionId)
                ?? throw new NotFoundException($"Inscripción {inscripcionId} no encontrada");

            EnsureLiveOps(insc);
            insc.NumeroCompetidor = numeroCompetidor?.Trim() ?? string.Empty;
            await _repo.UpdateAsync(insc);

            await _auditService.RegistrarAccionAsync(
                "LIVE_NUMERO",
                $"Inscripción {inscripcionId} Nº → {insc.NumeroCompetidor}",
                null,
                "Inscripciones",
                insc.EventoPrueba?.IdEvento,
                insc.IdEventoPrueba);

            InvalidateScope(insc.EventoPrueba?.IdEvento, insc.IdEventoPrueba);
            return await _inscripcionService.GetInscripcionByIdAsync(inscripcionId);
        }

        public async Task ReasignarCarrilesAsync(int faseId, LiveCarrilesBatchDto dto)
        {
            if (dto?.Asignaciones == null || dto.Asignaciones.Count == 0)
                throw new BadRequestException("Sin asignaciones de carril.");

            var fase = await _repo.GetFaseByIdAsync(faseId)
                ?? throw new NotFoundException($"Fase {faseId} no encontrada");

            var duplicados = dto.Asignaciones
                .GroupBy(a => a.Carril)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (duplicados.Count > 0)
                throw new BadRequestException($"Carriles duplicados en el pedido: {string.Join(", ", duplicados)}");

            var resultados = (await _repo.GetResultadosByFaseAsync(faseId)).ToList();
            var byId = resultados.ToDictionary(r => r.Id);

            foreach (var a in dto.Asignaciones)
            {
                if (!byId.TryGetValue(a.ResultadoId, out var res))
                    throw new NotFoundException($"Resultado {a.ResultadoId} no pertenece a la fase {faseId}");
                if (a.Carril < 1)
                    throw new BadRequestException("Carril inválido.");
            }

            // Liberar primero para evitar choques de unique-like constraints en memoria
            foreach (var a in dto.Asignaciones)
                byId[a.ResultadoId].Carril = null;
            await _repo.SaveResultadosAsync(dto.Asignaciones.Select(a => byId[a.ResultadoId]));

            foreach (var a in dto.Asignaciones)
                byId[a.ResultadoId].Carril = a.Carril;
            await _repo.SaveResultadosAsync(dto.Asignaciones.Select(a => byId[a.ResultadoId]));

            var epId = fase.Etapa?.EventoPruebaId;
            await _auditService.RegistrarAccionAsync(
                "LIVE_LANES",
                $"Reasignación de {dto.Asignaciones.Count} carriles en fase {faseId} ({fase.NombreFase})",
                null,
                "Fases",
                null,
                epId);

            _liveCache.InvalidateFase(faseId, eventoPruebaId: epId);
            if (epId.HasValue)
            {
                var ep = await _repo.GetEventoPruebaAsync(epId.Value);
                if (ep != null)
                    _liveCache.InvalidateEvento(ep.IdEvento);
            }
        }

        public async Task BajaBoteAsync(int inscripcionId, bool force)
        {
            var insc = await _repo.GetByIdAsync(inscripcionId)
                ?? throw new NotFoundException($"Inscripción {inscripcionId} no encontrada");

            EnsureLiveOps(insc);

            var eventoId = insc.EventoPrueba?.IdEvento;
            var epId = insc.IdEventoPrueba;

            // Quita resultados sin tocar FechaHora de otras fases/pruebas
            await _repo.RemoveResultadosByInscripcionAsync(inscripcionId, force);
            await _inscripcionService.DeleteInscripcionAsync(inscripcionId, allowWhenClosed: true);

            await _auditService.RegistrarAccionAsync(
                "LIVE_BAJA",
                $"Baja en vivo de inscripción {inscripcionId} (force={force})",
                null,
                "Inscripciones",
                eventoId,
                epId);

            InvalidateScope(eventoId, epId);
        }

        public async Task<InscripcionDto> AgregarBoteAsync(LiveAgregarBoteDto dto)
        {
            if (dto == null || dto.ParticipanteId <= 0 || dto.EventoPruebaId <= 0)
                throw new BadRequestException("Datos incompletos para agregar bote.");

            var ep = await _repo.GetEventoPruebaAsync(dto.EventoPruebaId)
                ?? throw new NotFoundException($"EventoPrueba {dto.EventoPruebaId} no encontrada");

            if (ep.Evento == null || !EventoCambiosPolicy.PermiteLiveOpsAdmin(ep.Evento))
                throw new BadRequestException("El evento no admite altas en vivo.");

            var created = await _inscripcionService.CreateInscripcionAsync(new InscripcionCreateDto
            {
                EventoPruebaId = dto.EventoPruebaId,
                ParticipanteId = dto.ParticipanteId,
                NumeroCompetidor = dto.NumeroCompetidor ?? string.Empty,
                Tripulantes = dto.Tripulantes ?? new List<InscripcionTripulanteCreateDto>()
            });

            // Asegurar tripulación K2/K4 (CreateInscripcion puede no persistir remadores anidados).
            if (dto.Tripulantes != null && dto.Tripulantes.Count > 0)
            {
                await _inscripcionService.UpdateTripulacionAsync(
                    created.Id,
                    new InscripcionTripulacionUpdateDto
                    {
                        ParticipanteId = dto.ParticipanteId,
                        Tripulantes = dto.Tripulantes,
                    },
                    clubScope: null,
                    bypassDeadline: true,
                    requireSameClub: true);
            }

            var fase = dto.FaseId.HasValue
                ? await _repo.GetFaseByIdAsync(dto.FaseId.Value)
                : await _repo.GetPrimeraFaseByEventoPruebaAsync(dto.EventoPruebaId);

            if (fase == null)
                throw new BadRequestException("No hay fase destino para agregar el bote. Generá heats primero.");

            if (fase.Etapa != null && fase.Etapa.EventoPruebaId != dto.EventoPruebaId)
                throw new BadRequestException("La fase seleccionada no pertenece a la prueba del evento.");

            var ocupados = (await _repo.GetResultadosByFaseAsync(fase.Id))
                .Where(r => r.Carril.HasValue)
                .Select(r => r.Carril!.Value)
                .ToHashSet();

            var carril = dto.Carril;
            if (!carril.HasValue || carril < 1 || ocupados.Contains(carril.Value))
            {
                carril = 1;
                while (ocupados.Contains(carril.Value)) carril++;
            }

            await _repo.AddResultadoAsync(new EntResultado
            {
                FaseId = fase.Id,
                InscripcionId = created.Id,
                Carril = carril,
                Estado = EstadoResultadoEnum.Pendiente,
                FechaRegistro = DateTime.UtcNow
            });

            await _auditService.RegistrarAccionAsync(
                "LIVE_ALTA",
                $"Alta en vivo inscripción {created.Id} en EP {dto.EventoPruebaId} / fase {fase.Id}",
                null,
                "Inscripciones",
                ep.IdEvento,
                dto.EventoPruebaId);

            InvalidateScope(ep.IdEvento, dto.EventoPruebaId, fase.Id);
            return await _inscripcionService.GetInscripcionByIdAsync(created.Id);
        }

        public async Task<InscripcionDto> MoverPruebaAsync(int inscripcionId, LiveMoverPruebaDto dto)
        {
            var insc = await _repo.GetByIdAsync(inscripcionId)
                ?? throw new NotFoundException($"Inscripción {inscripcionId} no encontrada");

            EnsureLiveOps(insc);

            var destino = await _repo.GetEventoPruebaAsync(dto.EventoPruebaIdDestino)
                ?? throw new NotFoundException($"EventoPrueba destino {dto.EventoPruebaIdDestino} no encontrada");

            if (destino.IdEvento != insc.EventoPrueba.IdEvento)
                throw new BadRequestException("Solo se puede mover dentro del mismo evento.");

            var origenEventoId = insc.EventoPrueba?.IdEvento;
            var origenEpId = insc.IdEventoPrueba;

            await _repo.RemoveResultadosByInscripcionAsync(inscripcionId, dto.Force);

            insc.IdEventoPrueba = dto.EventoPruebaIdDestino;
            await _repo.UpdateAsync(insc);

            var fase = dto.FaseIdDestino.HasValue
                ? await _repo.GetFaseByIdAsync(dto.FaseIdDestino.Value)
                : await _repo.GetPrimeraFaseByEventoPruebaAsync(dto.EventoPruebaIdDestino);

            if (fase != null)
            {
                var ocupados = (await _repo.GetResultadosByFaseAsync(fase.Id))
                    .Where(r => r.Carril.HasValue)
                    .Select(r => r.Carril!.Value)
                    .ToHashSet();

                var carril = dto.Carril;
                if (!carril.HasValue || carril < 1 || ocupados.Contains(carril.Value))
                {
                    carril = 1;
                    while (ocupados.Contains(carril.Value)) carril++;
                }

                await _repo.AddResultadoAsync(new EntResultado
                {
                    FaseId = fase.Id,
                    InscripcionId = inscripcionId,
                    Carril = carril,
                    Estado = EstadoResultadoEnum.Pendiente,
                    FechaRegistro = DateTime.UtcNow
                });
            }

            await _auditService.RegistrarAccionAsync(
                "LIVE_MOVE",
                $"Inscripción {inscripcionId} movida a EP {dto.EventoPruebaIdDestino}",
                null,
                "Inscripciones",
                destino.IdEvento,
                dto.EventoPruebaIdDestino);

            InvalidateScope(origenEventoId, origenEpId);
            InvalidateScope(destino.IdEvento, dto.EventoPruebaIdDestino, fase?.Id);
            return await _inscripcionService.GetInscripcionByIdAsync(inscripcionId);
        }

        private void InvalidateScope(int? eventoId, int? eventoPruebaId, int? faseId = null)
        {
            if (faseId.HasValue)
                _liveCache.InvalidateFase(faseId.Value, eventoId, eventoPruebaId);
            else if (eventoPruebaId.HasValue)
                _liveCache.InvalidateEventoPrueba(eventoPruebaId.Value, eventoId);
            else if (eventoId.HasValue)
                _liveCache.InvalidateEvento(eventoId.Value);
        }

        private static void EnsureLiveOps(EntInscripcion insc)
        {
            var evento = insc.EventoPrueba?.Evento;
            if (evento == null || !EventoCambiosPolicy.PermiteLiveOpsAdmin(evento))
                throw new BadRequestException("El evento no admite operaciones en vivo.");
        }
    }
}
