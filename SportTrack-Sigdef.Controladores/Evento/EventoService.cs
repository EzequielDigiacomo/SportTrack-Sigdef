using AutoMapper;
using SportTrack_Sigdef.Controladores.Caching;
using SportTrack_Sigdef.Controladores.Evento.Dtos;
using SportTrack_Sigdef.Controladores.Exceptions;
using SportTrack_Sigdef.Controladores.Notifications;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Evento
{
    public class EventoService : IEventoService
    {
        private static readonly TimeSpan LiveReadTtl = TimeSpan.FromSeconds(45);

        private readonly IEventoRepository _eventoRepository;
        private readonly IMapper _mapper;
        private readonly Audit.IAuditService _auditService;
        private readonly IEventoEstadoSyncService _estadoSyncService;
        private readonly ILiveCacheService _liveCache;
        private readonly INotificationBroadcastService _notificationBroadcast;

        public EventoService(
            IEventoRepository eventoRepository,
            IMapper mapper,
            Audit.IAuditService auditService,
            IEventoEstadoSyncService estadoSyncService,
            ILiveCacheService liveCache,
            INotificationBroadcastService notificationBroadcast)
        {
            _eventoRepository = eventoRepository;
            _mapper = mapper;
            _auditService = auditService;
            _estadoSyncService = estadoSyncService;
            _liveCache = liveCache;
            _notificationBroadcast = notificationBroadcast;
        }

        public async Task<IEnumerable<EventoDto>> GetAllEventosAsync(
            int? clubId = null,
            string? rol = null,
            int? federacionId = null)
        {
            await _estadoSyncService.SyncAllAsync();
            var eventos = await _eventoRepository.GetAllAsync(clubId, rol, federacionId);
            return _mapper.Map<IEnumerable<EventoDto>>(eventos);
        }

        public async Task<EventoDto> GetEventoByIdAsync(int id)
        {
            return await _liveCache.GetOrCreateAsync(
                LiveCacheKeys.Evento(id),
                LiveReadTtl,
                async () =>
                {
                    await _estadoSyncService.SyncEventoAsync(id);
                    var evento = await _eventoRepository.GetByIdAsync(id);
                    if (evento == null) throw new NotFoundException($"Evento con ID {id} no encontrado");
                    return _mapper.Map<EventoDto>(evento);
                });
        }

        public async Task<EventoDto> CreateEventoAsync(EventoCreateDto eventoDto)
        {
            var evento = _mapper.Map<Entidades.Entidades.Evento>(eventoDto);
            
            // Forzar fecha a UTC para evitar error de Npgsql (timestamp with time zone)
            evento.Fecha = DateTime.SpecifyKind(evento.Fecha, DateTimeKind.Utc);
            if (evento.FechaFin.HasValue)
            {
                evento.FechaFin = DateTime.SpecifyKind(evento.FechaFin.Value, DateTimeKind.Utc);
            }
            if (evento.FechaFinInscripciones.HasValue)
            {
                evento.FechaFinInscripciones = DateTime.SpecifyKind(evento.FechaFinInscripciones.Value, DateTimeKind.Utc);
            }
            
            // Scope de tenant seteado por el Controller desde Claims / GetMe
            evento.IdClub = eventoDto.ClubId;
            evento.IdFederacion = eventoDto.FederacionId;
            evento.Modalidad = string.Equals(eventoDto.Modalidad, "Maraton", StringComparison.OrdinalIgnoreCase)
                || string.Equals(eventoDto.Modalidad, "Maratón", StringComparison.OrdinalIgnoreCase)
                ? "Maraton"
                : "Velocidad";
            
            var result = await _eventoRepository.CreateAsync(evento);
            
            // Recargar con Club para que el DTO tenga el nombre
            var fullEvento = await _eventoRepository.GetByIdAsync(result.IdEvento);

            // Auditoria
            await _auditService.RegistrarAccionAsync("CREATE_EVENT", 
                $"Evento creado: {result.Nombre} (Ubicación: {result.Ubicacion}, Fecha: {result.Fecha:dd/MM/yyyy})", null, "Eventos");

            _liveCache.InvalidateEvento(result.IdEvento);

            var createdEventoDto = _mapper.Map<EventoDto>(fullEvento);
            await TryNotifyNewEventAsync(createdEventoDto);

            return createdEventoDto;
        }

        private async Task TryNotifyNewEventAsync(EventoDto createdEventoDto)
        {
            if (!createdEventoDto.FederacionId.HasValue || createdEventoDto.FederacionId.Value <= 0) return;
            if (createdEventoDto.Nombre.Contains("control", StringComparison.OrdinalIgnoreCase)) return;

            await _notificationBroadcast.NotifyNewEventAsync(createdEventoDto.FederacionId.Value, new
            {
                eventoId = createdEventoDto.Id,
                nombre = createdEventoDto.Nombre,
                fecha = createdEventoDto.Fecha,
                ubicacion = createdEventoDto.Ubicacion,
                inscripcionesAbiertas = createdEventoDto.InscripcionesAbiertas,
            });
        }

        public async Task<EventoDto> UpdateEventoAsync(int id, EventoUpdateDto eventoDto, int? clubId = null)
        {
            var existing = await _eventoRepository.GetByIdAsync(id);
            if (existing == null) throw new NotFoundException($"Evento con ID {id} no encontrado");
            
            // Verificación de propiedad (si es un Club)
            if (clubId.HasValue && existing.IdClub != clubId.Value)
            {
                throw new UnauthorizedAccessException("No tenés permisos para modificar un evento de otro club.");
            }
            
            _mapper.Map(eventoDto, existing);

            if (!string.IsNullOrWhiteSpace(eventoDto.Modalidad))
            {
                existing.Modalidad = string.Equals(eventoDto.Modalidad, "Maraton", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(eventoDto.Modalidad, "Maratón", StringComparison.OrdinalIgnoreCase)
                    ? "Maraton"
                    : "Velocidad";
            }

            existing.Fecha = DateTime.SpecifyKind(existing.Fecha, DateTimeKind.Utc);
            if (existing.FechaFin.HasValue)
            {
                existing.FechaFin = DateTime.SpecifyKind(existing.FechaFin.Value, DateTimeKind.Utc);
            }
            if (existing.FechaFinInscripciones.HasValue)
            {
                existing.FechaFinInscripciones = DateTime.SpecifyKind(existing.FechaFinInscripciones.Value, DateTimeKind.Utc);
            }

            var result = await _eventoRepository.UpdateAsync(existing);
            
            // Recargar con Club para que el DTO tenga el nombre
            var fullEvento = await _eventoRepository.GetByIdAsync(result.IdEvento);

            // Auditoria
            await _auditService.RegistrarAccionAsync("UPDATE_EVENT", 
                $"Evento actualizado: {result.Nombre} (ID: {id})", null, "Eventos");

            _liveCache.InvalidateEvento(id);
            return _mapper.Map<EventoDto>(fullEvento);
        }

        public async Task<bool> DeleteEventoAsync(int id, int? clubId = null)
        {
            var existing = await _eventoRepository.GetByIdAsync(id);
            if (existing == null) throw new NotFoundException($"Evento con ID {id} no encontrado");
            
            // Verificación de propiedad (si es un Club)
            if (clubId.HasValue && existing.IdClub != clubId.Value)
            {
                throw new UnauthorizedAccessException("No tenés permisos para eliminar un evento de otro club.");
            }
            
            var res = await _eventoRepository.DeleteAsync(id);
            if (res)
            {
                _liveCache.InvalidateEvento(id);
                await _auditService.RegistrarAccionAsync("DELETE_EVENT", 
                    $"Evento eliminado: {existing.Nombre} (ID: {id})", null, "Eventos");
            }

            return res;
        }
        public async Task<IEnumerable<EventoDto>> GetProximosEventosAsync(
            int? clubId = null,
            string? rol = null,
            int? federacionId = null)
        {
            await _estadoSyncService.SyncAllAsync();
            var eventos = await _eventoRepository.GetProximosAsync(clubId, rol, federacionId);
            return _mapper.Map<IEnumerable<EventoDto>>(eventos);
        }

        public async Task<IEnumerable<EventoPruebaDto>> GetPruebasByEventoAsync(int eventoId)
        {
            return await _liveCache.GetOrCreateAsync(
                LiveCacheKeys.PruebasByEvento(eventoId),
                LiveReadTtl,
                async () =>
                {
                    var pruebas = await _eventoRepository.GetPruebasByEventoIdAsync(eventoId);
                    return _mapper.Map<IEnumerable<EventoPruebaDto>>(pruebas);
                });
        }

        public async Task<EventoPruebaDto> AssignPruebaToEventoAsync(int eventoId, EventoPruebaCreateDto assignDto)
        {
            // 1. Buscar si la prueba técnica ya existe por sus IDs
            var prueba = await _eventoRepository.GetPruebaAsync(assignDto.CategoriaId, assignDto.BoteId, assignDto.DistanciaId, assignDto.SexoId);

            if (prueba == null)
            {
                // 2. Si no existe, crearla. Consultamos maestros para el nombre.
                // Por ahora usamos IDs directamente en el nombre o consultamos repositorios si fuera necesario.
                // Simplificado: Creamos la prueba con los IDs enviados.
                prueba = new Prueba
                {
                    CategoriaEdad = assignDto.CategoriaId,
                    TipoBote = assignDto.BoteId,
                    DistanciaId = assignDto.DistanciaId,
                    SexoCompetencia = assignDto.SexoId,
                    Nombre = $"Prueba {assignDto.CategoriaId}-{assignDto.BoteId}-{assignDto.DistanciaId}"
                };
                prueba = await _eventoRepository.CreatePruebaAsync(prueba);
            }

            // 3. Vincular al evento
            var eventoPrueba = new EventoPrueba
            {
                IdEvento = eventoId,
                IdPrueba = prueba.IdPrueba,
                FechaHora = assignDto.FechaHora ?? DateTime.UtcNow,
                Estado = EstadoEventoEnum.Programada,
                GrupoLargadaId = assignDto.GrupoLargadaId,
                MaxParticipantes = 0
            };

            // Asegurar UTC para la fecha de la prueba
            eventoPrueba.FechaHora = DateTime.SpecifyKind(eventoPrueba.FechaHora, DateTimeKind.Utc);

            var result = await _eventoRepository.AssignPruebaAsync(eventoPrueba);
            _liveCache.InvalidateEvento(eventoId);
            return _mapper.Map<EventoPruebaDto>(result);
        }

        public async Task<IEnumerable<EventoPruebaDto>> AssignLargadaMaratonAsync(int eventoId, EventoLargadaCreateDto largadaDto)
        {
            if (largadaDto.CategoriaIds == null || largadaDto.CategoriaIds.Count == 0)
                throw new ArgumentException("Debe seleccionar al menos una categoría.");
            if (largadaDto.BoteIds == null || largadaDto.BoteIds.Count == 0)
                throw new ArgumentException("Debe seleccionar al menos un bote.");
            if (largadaDto.SexoIds == null || largadaDto.SexoIds.Count == 0)
                throw new ArgumentException("Debe seleccionar al menos una rama (sexo).");
            if (largadaDto.DistanciaId <= 0)
                throw new ArgumentException("Debe seleccionar una distancia.");

            var grupoId = largadaDto.GrupoLargadaId ?? Guid.NewGuid();
            var fechaHora = DateTime.SpecifyKind(
                largadaDto.FechaHora ?? DateTime.UtcNow,
                DateTimeKind.Utc);

            // Edición: reemplazar el grupo completo
            if (largadaDto.GrupoLargadaId.HasValue)
            {
                await _eventoRepository.UnassignByGrupoLargadaAsync(largadaDto.GrupoLargadaId.Value);
            }

            foreach (var catId in largadaDto.CategoriaIds.Distinct())
            {
                foreach (var boteId in largadaDto.BoteIds.Distinct())
                {
                    foreach (var sexoId in largadaDto.SexoIds.Distinct())
                    {
                        await AssignPruebaToEventoAsync(eventoId, new EventoPruebaCreateDto
                        {
                            CategoriaId = catId,
                            BoteId = boteId,
                            DistanciaId = largadaDto.DistanciaId,
                            SexoId = sexoId,
                            FechaHora = fechaHora,
                            GrupoLargadaId = grupoId
                        });
                    }
                }
            }

            var grupo = await _eventoRepository.GetPruebasByGrupoLargadaAsync(grupoId);
            _liveCache.InvalidateEvento(eventoId);
            return _mapper.Map<IEnumerable<EventoPruebaDto>>(grupo);
        }

        public async Task<EventoPruebaDto> UpdateEventoPruebaAsync(int eventoPruebaId, EventoPruebaCreateDto updateDto)
        {
            var existing = await _eventoRepository.GetEventoPruebaByIdAsync(eventoPruebaId);
            if (existing == null) throw new NotFoundException($"Asignación {eventoPruebaId} no encontrada");

            // 1. Buscar/Crear la prueba técnica si cambiaron los parámetros
            var prueba = await _eventoRepository.GetPruebaAsync(updateDto.CategoriaId, updateDto.BoteId, updateDto.DistanciaId, updateDto.SexoId);
            if (prueba == null)
            {
                prueba = new Prueba
                {
                    CategoriaEdad = updateDto.CategoriaId,
                    TipoBote = updateDto.BoteId,
                    DistanciaId = updateDto.DistanciaId,
                    SexoCompetencia = updateDto.SexoId,
                    Nombre = $"Prueba {updateDto.CategoriaId}-{updateDto.BoteId}-{updateDto.DistanciaId}"
                };
                prueba = await _eventoRepository.CreatePruebaAsync(prueba);
            }

            // 2. Actualizar la asignación
            existing.IdPrueba = prueba.IdPrueba;
            existing.FechaHora = updateDto.FechaHora ?? existing.FechaHora;
            existing.FechaHora = DateTime.SpecifyKind(existing.FechaHora, DateTimeKind.Utc);

            var result = await _eventoRepository.UpdateEventoPruebaAsync(existing);
            _liveCache.InvalidateEventoPrueba(eventoPruebaId, existing.IdEvento);
            return _mapper.Map<EventoPruebaDto>(result);
        }

        public async Task<bool> DeleteEventoPruebaAsync(int eventoPruebaId)
        {
            var existing = await _eventoRepository.GetEventoPruebaByIdAsync(eventoPruebaId);
            if (existing == null) return false;

            // Si forma parte de una largada Maratón, eliminar todo el grupo
            if (existing.GrupoLargadaId.HasValue)
            {
                var removed = await _eventoRepository.UnassignByGrupoLargadaAsync(existing.GrupoLargadaId.Value);
                if (removed > 0)
                    _liveCache.InvalidateEvento(existing.IdEvento);
                return removed > 0;
            }

            var ok = await _eventoRepository.UnassignPruebaAsync(eventoPruebaId);
            if (ok)
                _liveCache.InvalidateEventoPrueba(eventoPruebaId, existing.IdEvento);
            return ok;
        }

        private static DistanciaRegata MapDistanciaToEnum(int distanciaId)
        {
            return distanciaId switch
            {
                5 => DistanciaRegata.QuinientosMetros,
                6 => DistanciaRegata.MilMetros,
                8 => DistanciaRegata.DosKilometros,
                9 => DistanciaRegata.TresKilometros,
                10 => DistanciaRegata.CincoKilometros,
                11 => DistanciaRegata.DiezKilometros,
                _ => (DistanciaRegata)distanciaId
            };
        }
    }
}

