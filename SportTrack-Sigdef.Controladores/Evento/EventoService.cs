using AutoMapper;
using SportTrack_Sigdef.Controladores.Evento.Dtos;
using SportTrack_Sigdef.Controladores.Exceptions;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.Enums;
using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Evento
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IMapper _mapper;
        private readonly Audit.IAuditService _auditService;
        private readonly IEventoEstadoSyncService _estadoSyncService;

        public EventoService(
            IEventoRepository eventoRepository,
            IMapper mapper,
            Audit.IAuditService auditService,
            IEventoEstadoSyncService estadoSyncService)
        {
            _eventoRepository = eventoRepository;
            _mapper = mapper;
            _auditService = auditService;
            _estadoSyncService = estadoSyncService;
        }

        public async Task<IEnumerable<EventoDto>> GetAllEventosAsync(int? clubId = null, string? rol = null)
        {
            await _estadoSyncService.SyncAllAsync();
            var eventos = await _eventoRepository.GetAllAsync(clubId, rol);
            return _mapper.Map<IEnumerable<EventoDto>>(eventos);
        }

        public async Task<EventoDto> GetEventoByIdAsync(int id)
        {
            await _estadoSyncService.SyncEventoAsync(id);
            var evento = await _eventoRepository.GetByIdAsync(id);
            if (evento == null) throw new NotFoundException($"Evento con ID {id} no encontrado");
            return _mapper.Map<EventoDto>(evento);
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
            
            // Asignar el ClubId si viene en el DTO (seteado por el Controller desde Claims)
            evento.IdClub = eventoDto.ClubId;
            
            var result = await _eventoRepository.CreateAsync(evento);
            
            // Recargar con Club para que el DTO tenga el nombre
            var fullEvento = await _eventoRepository.GetByIdAsync(result.IdEvento);

            // Auditoria
            await _auditService.RegistrarAccionAsync("CREATE_EVENT", 
                $"Evento creado: {result.Nombre} (Ubicación: {result.Ubicacion}, Fecha: {result.Fecha:dd/MM/yyyy})", null, "Eventos");

            return _mapper.Map<EventoDto>(fullEvento);
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

            // Auditoria
            if (res) {
                await _auditService.RegistrarAccionAsync("DELETE_EVENT", 
                    $"Evento eliminado: {existing.Nombre} (ID: {id})", null, "Eventos");
            }

            return res;
        }
        public async Task<IEnumerable<EventoDto>> GetProximosEventosAsync(int? clubId = null, string? rol = null)
        {
            await _estadoSyncService.SyncAllAsync();
            var eventos = await _eventoRepository.GetProximosAsync(clubId, rol);
            return _mapper.Map<IEnumerable<EventoDto>>(eventos);
        }

        public async Task<IEnumerable<EventoPruebaDto>> GetPruebasByEventoAsync(int eventoId)
        {
            var pruebas = await _eventoRepository.GetPruebasByEventoIdAsync(eventoId);
            return _mapper.Map<IEnumerable<EventoPruebaDto>>(pruebas);
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
                Estado = EstadoEventoEnum.Programada
            };

            // Asegurar UTC para la fecha de la prueba
            eventoPrueba.FechaHora = DateTime.SpecifyKind(eventoPrueba.FechaHora, DateTimeKind.Utc);

            var result = await _eventoRepository.AssignPruebaAsync(eventoPrueba);
            
            // Recargamos para traer las navegaciones (Categoria, Bote, Distancia) si el Repo lo permite
            // o mapeamos lo que tenemos.
            // Recargamos para traer las navegaciones (Categoria, Bote, Distancia) si el Repo lo permite
            // o mapeamos lo que tenemos.
            return _mapper.Map<EventoPruebaDto>(result);
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
            return _mapper.Map<EventoPruebaDto>(result);
        }

        public async Task<bool> DeleteEventoPruebaAsync(int eventoPruebaId)
        {
            return await _eventoRepository.UnassignPruebaAsync(eventoPruebaId);
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

