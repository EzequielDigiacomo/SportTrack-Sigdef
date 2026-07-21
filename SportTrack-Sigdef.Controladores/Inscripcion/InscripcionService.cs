using AutoMapper;
using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;
using SportTrack_Sigdef.Controladores.Inscripcion;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportTrack_Sigdef.Controladores.Exceptions;

namespace SportTrack_Sigdef.Controladores.Inscripcion
{
    public class InscripcionService : IInscripcionService
    {
        private readonly IInscripcionRepository _inscripcionRepository;
        private readonly IMapper _mapper;
        private readonly Audit.IAuditService _auditService;

        public InscripcionService(IInscripcionRepository inscripcionRepository, IMapper mapper, Audit.IAuditService auditService)
        {
            _inscripcionRepository = inscripcionRepository;
            _mapper = mapper;
            _auditService = auditService;
        }

        public async Task<IEnumerable<InscripcionDto>> GetAllInscripcionesAsync()
        {
            var inscripciones = await _inscripcionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InscripcionDto>>(inscripciones);
        }

        public async Task<InscripcionDto> GetInscripcionByIdAsync(int id)
        {
            var inscripcion = await _inscripcionRepository.GetByIdAsync(id);
            if (inscripcion == null)
                throw new NotFoundException($"Inscripción con ID {id} no encontrada");

            return _mapper.Map<InscripcionDto>(inscripcion);
        }

        public async Task<InscripcionDto> CreateInscripcionAsync(InscripcionCreateDto inscripcionDto)
        {
            if (inscripcionDto.EventoPruebaId <= 0)
                throw new BadRequestException("Debe indicar la prueba del evento (eventoPruebaId).");

            if (!inscripcionDto.ParticipanteId.HasValue || inscripcionDto.ParticipanteId <= 0)
                throw new BadRequestException("Debe indicar el participante titular de la inscripción.");

            var inscripcion = _mapper.Map<Entidades.Entidades.Inscripcion>(inscripcionDto);
            var createdInscripcion = await _inscripcionRepository.CreateAsync(inscripcion);
            
            // Recargamos para incluir los datos relacionados (Participante) en el DTO de salida
            var result = await _inscripcionRepository.GetByIdAsync(createdInscripcion.IdInscripcion);
            
            // Auditoria
            await _auditService.RegistrarAccionAsync("CREATE_INSCRIPTION", 
                $"Nueva inscripción: {result.Participante?.Nombre} {result.Participante?.Apellido} (ID: {result.IdInscripcion})", null, "Inscripciones");

            return _mapper.Map<InscripcionDto>(result);
        }

        public async Task<InscripcionDto> UpdateInscripcionAsync(int id, InscripcionUpdateDto inscripcionDto)
        {
            var existingInscripcion = await _inscripcionRepository.GetByIdAsync(id);
            if (existingInscripcion == null)
                throw new NotFoundException($"Inscripción con ID {id} no encontrada");

            // Patch manual: solo sobrescribir los campos que vienen con valor
            if (inscripcionDto.EventoPruebaId.HasValue)
                existingInscripcion.IdEventoPrueba = inscripcionDto.EventoPruebaId.Value;

            if (inscripcionDto.NumeroCompetidor != null)
                existingInscripcion.NumeroCompetidor = inscripcionDto.NumeroCompetidor;

            var updatedInscripcion = await _inscripcionRepository.UpdateAsync(existingInscripcion);
            
            // Auditoria
            await _auditService.RegistrarAccionAsync("UPDATE_INSCRIPTION", 
                $"Inscripción actualizada (ID: {id}, Carril: {updatedInscripcion.NumeroCompetidor})", null, "Inscripciones");

            return _mapper.Map<InscripcionDto>(updatedInscripcion);
        }

        public async Task<bool> DeleteInscripcionAsync(int id, bool allowWhenClosed = false)
        {
            var inscripcion = await _inscripcionRepository.GetByIdAsync(id);
            if (inscripcion == null)
                throw new NotFoundException($"Inscripción con ID {id} no encontrada");

            if (!allowWhenClosed)
            {
                var evento = inscripcion.EventoPrueba?.Evento;
                if (evento != null && !EventoPermiteModificarInscripciones(evento))
                    throw new BadRequestException("Las inscripciones están cerradas para este evento. No se puede eliminar la inscripción.");
            }

            var res = await _inscripcionRepository.DeleteAsync(id);
            
            // Auditoria
            if (res) {
                await _auditService.RegistrarAccionAsync("DELETE_INSCRIPTION", 
                    $"Inscripción eliminada (ID: {id})", null, "Inscripciones");
            }

            return res;
        }

        private static bool EventoPermiteModificarInscripciones(SportTrack_Sigdef.Entidades.Entidades.Evento evento)
        {
            if (!evento.InscripcionesHabilitadas)
                return false;

            if (evento.Estado != EstadoEventoEnum.Programada)
                return false;

            if (evento.FechaFinInscripciones.HasValue && DateTime.UtcNow > evento.FechaFinInscripciones.Value)
                return false;

            return true;
        }

        public async Task<int> GetCountByEventoPruebaIdAsync(int eventoPruebaId)
        {
            return await _inscripcionRepository.CountByEventoPruebaIdAsync(eventoPruebaId);
        }

        public async Task<IEnumerable<InscripcionDto>> GetInscripcionesByEventoPruebaIdAsync(int eventoPruebaId)
        {
            var inscripciones = await _inscripcionRepository.GetByEventoPruebaIdAsync(eventoPruebaId);
            return _mapper.Map<IEnumerable<InscripcionDto>>(inscripciones);
        }
        public async Task<IEnumerable<InscripcionDto>> GetInscripcionesByEventoAndClubAsync(int eventoId, int clubId)
        {
            var inscripciones = await _inscripcionRepository.GetByEventoAndClubAsync(eventoId, clubId);
            return _mapper.Map<IEnumerable<InscripcionDto>>(inscripciones);
        }

        public async Task<bool> ToggleEsCabezaDeSerieAsync(int id)
        {
            var inscripcion = await _inscripcionRepository.GetByIdAsync(id);
            if (inscripcion == null) throw new NotFoundException($"Inscripción {id} no encontrada");

            // Si se intenta activar (pasar de false a true)
            if (!inscripcion.EsCabezaDeSerie)
            {
                var inscripcionesEnPrueba = await _inscripcionRepository.GetByEventoPruebaIdAsync(inscripcion.IdEventoPrueba);
                var totalInscritos = inscripcionesEnPrueba.Count();
                var actualSeeds = inscripcionesEnPrueba.Count(i => i.EsCabezaDeSerie);

                var maxSeedsAllowed = (int)Math.Ceiling(totalInscritos / 9.0);

                if (actualSeeds >= maxSeedsAllowed)
                {
                    throw new BadRequestException($"Límite de cabezas de serie alcanzado. Máximo permitido: {maxSeedsAllowed} para {totalInscritos} atletas.");
                }
            }

            inscripcion.EsCabezaDeSerie = !inscripcion.EsCabezaDeSerie;
            await _inscripcionRepository.UpdateAsync(inscripcion);
            
            // Auditoria
            await _auditService.RegistrarAccionAsync("TOGGLE_SEED", 
                $"Cabeza de serie {(inscripcion.EsCabezaDeSerie ? "activado" : "desactivado")} para Inscripción {id}", null, "Inscripciones");

            return true;
        }

        public async Task<IEnumerable<RegistroInscripcionDto>> GetRegistroInscripcionesAsync(
            int? clubScope,
            int? federacionScope,
            int? eventoId,
            int? clubIdFilter,
            int? participanteId,
            string? busqueda)
        {
            var inscripciones = await _inscripcionRepository.GetRegistroAsync(
                clubScope,
                federacionScope,
                eventoId,
                clubIdFilter,
                participanteId,
                busqueda);

            return inscripciones.Select(MapToRegistroDto);
        }

        private static RegistroInscripcionDto MapToRegistroDto(Entidades.Entidades.Inscripcion i)
        {
            var prueba = i.EventoPrueba?.Prueba;
            var pruebaNombre = prueba != null
                ? $"{prueba.Categoria?.Nombre} {prueba.Bote?.Tipo} {prueba.Distancia?.Descripcion} {prueba.Sexo?.Nombre}".Trim()
                : string.Empty;

            return new RegistroInscripcionDto
            {
                Id = i.IdInscripcion,
                ParticipanteId = i.IdParticipante ?? 0,
                ParticipanteNombre = i.Participante != null
                    ? $"{i.Participante.Nombre} {i.Participante.Apellido}".Trim()
                    : string.Empty,
                ParticipanteDocumento = i.Participante?.Documento,
                ClubId = i.Participante?.IdClub,
                ClubNombre = i.Participante?.Club?.Nombre,
                EventoId = i.EventoPrueba?.IdEvento ?? 0,
                EventoNombre = i.EventoPrueba?.Evento?.Nombre ?? string.Empty,
                EventoPruebaId = i.IdEventoPrueba,
                PruebaNombre = pruebaNombre,
                FechaInscripcion = i.FechaInscripcion,
                FechaInicioEvento = i.EventoPrueba?.Evento?.FechaInicio,
                FechaFinEvento = i.EventoPrueba?.Evento?.FechaFin,
                Estado = i.Estado.ToString(),
                Pagado = i.Pagado,
                TripulantesNombres = i.Tripulantes
                    .Where(t => t.Participante != null)
                    .Select(t => $"{t.Participante!.Nombre} {t.Participante.Apellido}".Trim())
                    .ToList()
            };
        }
    }
}
