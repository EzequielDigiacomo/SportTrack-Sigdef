using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.DTOs.Inscripcion;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;
using SportTrack_Sigdef.Entidades.DTOs.Evento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportTrack_Sigdef.Controladores.Federaciones;
using Microsoft.AspNetCore.Mvc;

namespace SIGDEF.API.Services
{
    public class InscripcionServices : IInscripcionServices
    {
        private readonly SportTrackDbContext _context;

        public InscripcionServices(SportTrackDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<InscripcionDto>>> GetInscripciones()
        {
            try
            {
                var inscripciones = await _context.Inscripciones
                    .Include(i => i.Participante)
                        .ThenInclude(p => p.AtletaFederacion)
                    .Include(i => i.Participante)
                        .ThenInclude(p => p.Club)
                    .Include(i => i.EventoPrueba)
                        .ThenInclude(ep => ep.Evento)
                    .Include(i => i.EventoPrueba)
                        .ThenInclude(ep => ep.Prueba)
                    .Select(i => new InscripcionDto
                    {
                        IdInscripcion = i.IdInscripcion,
                        ParticipanteId = i.IdParticipante ?? 0,
                        IdEvento = i.EventoPrueba.Evento.IdEvento,
                        IdEventoPrueba = i.IdEventoPrueba,
                        FechaInscripcion = i.FechaInscripcion,
                        NombreAtleta = i.Participante != null ? i.Participante.Nombre + " " + i.Participante.Apellido : "",
                        NombreEvento = i.EventoPrueba.Evento.Nombre,
                        DetallePrueba = $"{i.EventoPrueba.Prueba.Distancia} - {i.EventoPrueba.Prueba.TipoBote} - {i.EventoPrueba.Prueba.CategoriaEdad} - {i.EventoPrueba.Prueba.SexoCompetencia}",
                        NombreClub = i.Participante != null && i.Participante.Club != null ? i.Participante.Club.Nombre : "",
                        FechaInicioEvento = i.EventoPrueba.Evento.Fecha,
                        FechaFinEvento = i.EventoPrueba.Evento.FechaFin
                    })
                    .ToListAsync();

                return new OkObjectResult(inscripciones);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<InscripcionDetailDto>> GetInscripcion(int id)
        {
            try
            {
                var inscripcion = await _context.Inscripciones
                    .Include(i => i.Participante)
                        .ThenInclude(p => p.AtletaFederacion)
                    .Include(i => i.Participante)
                        .ThenInclude(p => p.Club)
                    .Include(i => i.EventoPrueba)
                        .ThenInclude(ep => ep.Evento)
                    .Include(i => i.EventoPrueba)
                        .ThenInclude(ep => ep.Prueba)
                    .Where(i => i.IdInscripcion == id)
                    .Select(i => new InscripcionDetailDto
                    {
                        IdInscripcion = i.IdInscripcion,
                        ParticipanteId = i.IdParticipante ?? 0,
                        IdEvento = i.EventoPrueba.IdEvento,
                        IdEventoPrueba = i.IdEventoPrueba,
                        FechaInscripcion = i.FechaInscripcion,
                        AtletaFederacion = i.Participante != null && i.Participante.AtletaFederacion != null ? new AtletaDto
                        {
                            ParticipanteId = i.Participante.AtletaFederacion.ParticipanteId,
                            IdClub = i.Participante.AtletaFederacion.IdClub,
                            EstadoPago = i.Participante.AtletaFederacion.EstadoPago,
                            PerteneceSeleccion = i.Participante.AtletaFederacion.PerteneceSeleccion,
                            Categoria = i.Participante.AtletaFederacion.Categoria,
                            BecadoEnard = i.Participante.AtletaFederacion.BecadoEnard,
                            BecadoSdn = i.Participante.AtletaFederacion.BecadoSdn,
                            MontoBeca = i.Participante.AtletaFederacion.MontoBeca,
                            PresentoAptoMedico = i.Participante.AtletaFederacion.PresentoAptoMedico,
                            FechaAptoMedico = i.Participante.AtletaFederacion.FechaAptoMedico,
                            NombrePersona = i.Participante.Nombre + " " + i.Participante.Apellido,
                            NombreClub = i.Participante.Club != null ? i.Participante.Club.Nombre : ""
                        } : null,
                        Evento = new EventoDto
                        {
                            IdEvento = i.EventoPrueba.Evento.IdEvento,
                            Nombre = i.EventoPrueba.Evento.Nombre,
                            FechaInicio = i.EventoPrueba.Evento.FechaInicio,
                            FechaFin = i.EventoPrueba.Evento.FechaFin ?? i.EventoPrueba.Evento.FechaInicio,
                            CantidadInscripciones = i.EventoPrueba.Evento.Inscripciones.Count,
                            Estado = GetEstadoEvento(i.EventoPrueba.Evento.FechaInicio, i.EventoPrueba.Evento.FechaFin ?? i.EventoPrueba.Evento.FechaInicio)
                        }
                    })
                    .FirstOrDefaultAsync();

                if (inscripcion == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(inscripcion);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<IEnumerable<InscripcionDto>>> GetInscripcionesPorAtleta(int ParticipanteId)
        {
            try
            {
                var inscripciones = await _context.Inscripciones
                    .Include(i => i.Participante)
                        .ThenInclude(p => p.AtletaFederacion)
                    .Include(i => i.Participante)
                        .ThenInclude(p => p.Club)
                    .Include(i => i.EventoPrueba)
                        .ThenInclude(ep => ep.Evento)
                    .Include(i => i.EventoPrueba)
                        .ThenInclude(ep => ep.Prueba)
                    .Where(i => i.IdParticipante == ParticipanteId)
                    .Select(i => new InscripcionDto
                    {
                        IdInscripcion = i.IdInscripcion,
                        ParticipanteId = i.IdParticipante ?? 0,
                        IdEvento = i.EventoPrueba.IdEvento,
                        IdEventoPrueba = i.IdEventoPrueba,
                        FechaInscripcion = i.FechaInscripcion,
                        NombreAtleta = i.Participante != null ? i.Participante.Nombre + " " + i.Participante.Apellido : "",
                        NombreEvento = i.EventoPrueba.Evento.Nombre,
                        DetallePrueba = $"{i.EventoPrueba.Prueba.Distancia} - {i.EventoPrueba.Prueba.TipoBote} - {i.EventoPrueba.Prueba.CategoriaEdad} - {i.EventoPrueba.Prueba.SexoCompetencia}",
                        NombreClub = i.Participante != null && i.Participante.Club != null ? i.Participante.Club.Nombre : "",
                        FechaInicioEvento = i.EventoPrueba.Evento.Fecha,
                        FechaFinEvento = i.EventoPrueba.Evento.FechaFin
                    })
                    .ToListAsync();

                return new OkObjectResult(inscripciones);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<IEnumerable<InscripcionDto>>> GetInscripcionesPorEvento(int idEvento)
        {
            try
            {
                var inscripciones = await _context.Inscripciones
                    .Include(i => i.Participante)
                        .ThenInclude(p => p.AtletaFederacion)
                    .Include(i => i.Participante)
                        .ThenInclude(p => p.Club)
                    .Include(i => i.EventoPrueba)
                        .ThenInclude(ep => ep.Evento)
                    .Include(i => i.EventoPrueba)
                        .ThenInclude(ep => ep.Prueba)
                    .Where(i => i.EventoPrueba.IdEvento == idEvento)
                    .Select(i => new InscripcionDto
                    {
                        IdInscripcion = i.IdInscripcion,
                        ParticipanteId = i.IdParticipante ?? 0,
                        IdEvento = i.EventoPrueba.IdEvento,
                        IdEventoPrueba = i.IdEventoPrueba,
                        FechaInscripcion = i.FechaInscripcion,
                        NombreAtleta = i.Participante != null ? i.Participante.Nombre + " " + i.Participante.Apellido : "",
                        NombreEvento = i.EventoPrueba.Evento.Nombre,
                        DetallePrueba = $"{i.EventoPrueba.Prueba.Distancia} - {i.EventoPrueba.Prueba.TipoBote} - {i.EventoPrueba.Prueba.CategoriaEdad} - {i.EventoPrueba.Prueba.SexoCompetencia}",
                        NombreClub = i.Participante != null && i.Participante.Club != null ? i.Participante.Club.Nombre : "",
                        FechaInicioEvento = i.EventoPrueba.Evento.Fecha,
                        FechaFinEvento = i.EventoPrueba.Evento.FechaFin
                    })
                    .ToListAsync();

                return new OkObjectResult(inscripciones);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<InscripcionDto>> PostInscripcion(InscripcionCreateDto inscripcionCreateDto)
        {
            try
            {
                var atletaExists = await _context.AtletasFederados.AnyAsync(a => a.ParticipanteId == inscripcionCreateDto.ParticipanteId);
                if (!atletaExists)
                {
                    return new BadRequestResult();
                }

                var eventoPrueba = await _context.EventoPruebas
                    .Include(ep => ep.Evento)
                    .Include(ep => ep.Prueba)
                    .FirstOrDefaultAsync(ep => ep.IdEventoPrueba == inscripcionCreateDto.IdEventoPrueba);

                if (eventoPrueba == null)
                {
                    return new BadRequestResult();
                }

                if (eventoPrueba.Evento.FechaFin < DateTime.UtcNow)
                {
                    return new BadRequestResult();
                }

                var inscripcionExistente = await _context.Inscripciones
                    .AnyAsync(i => i.IdParticipante == inscripcionCreateDto.ParticipanteId &&
                                  i.IdEventoPrueba == inscripcionCreateDto.IdEventoPrueba);

                if (inscripcionExistente)
                {
                    return new BadRequestResult();
                }

                var inscripcion = new SportTrack_Sigdef.Entidades.Entidades.Inscripcion
                {
                    IdParticipante = inscripcionCreateDto.ParticipanteId,
                    IdEventoPrueba = inscripcionCreateDto.IdEventoPrueba,
                    FechaInscripcion = inscripcionCreateDto.FechaInscripcion
                };

                _context.Inscripciones.Add(inscripcion);
                await _context.SaveChangesAsync();

                await _context.Entry(inscripcion)
                    .Reference(i => i.Participante)
                    .LoadAsync();
                if (inscripcion.Participante != null)
                {
                    await _context.Entry(inscripcion.Participante)
                        .Reference(p => p.AtletaFederacion)
                        .LoadAsync();
                    await _context.Entry(inscripcion.Participante)
                        .Reference(p => p.Club)
                        .LoadAsync();
                }
                await _context.Entry(inscripcion)
                    .Reference(i => i.EventoPrueba)
                    .LoadAsync();
                await _context.Entry(inscripcion.EventoPrueba)
                    .Reference(ep => ep.Evento)
                    .LoadAsync();
                await _context.Entry(inscripcion.EventoPrueba)
                    .Reference(ep => ep.Prueba)
                    .LoadAsync();

                var inscripcionDto = new InscripcionDto
                {
                    IdInscripcion = inscripcion.IdInscripcion,
                    ParticipanteId = inscripcion.IdParticipante ?? 0,
                    IdEvento = inscripcion.EventoPrueba.Evento.IdEvento,
                    IdEventoPrueba = inscripcion.IdEventoPrueba,
                    FechaInscripcion = inscripcion.FechaInscripcion,
                    NombreAtleta = inscripcion.Participante != null ? inscripcion.Participante.Nombre + " " + inscripcion.Participante.Apellido : "",
                    NombreEvento = inscripcion.EventoPrueba.Evento.Nombre,
                    DetallePrueba = $"{inscripcion.EventoPrueba.Prueba.Distancia} - {inscripcion.EventoPrueba.Prueba.TipoBote}",
                    NombreClub = inscripcion.Participante != null && inscripcion.Participante.Club != null ? inscripcion.Participante.Club.Nombre : "",
                    FechaInicioEvento = inscripcion.EventoPrueba.Evento.Fecha,
                    FechaFinEvento = inscripcion.EventoPrueba.Evento.FechaFin
                };

                var result = new ObjectResult(inscripcionDto)
                {
                    StatusCode = 201
                };
                return result;
            }
            catch (DbUpdateException dbEx)
            {
                return new StatusCodeResult(500);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> PutInscripcion(int id, InscripcionCreateDto inscripcionCreateDto)
        {
            try
            {
                var inscripcion = await _context.Inscripciones.FindAsync(id);
                if (inscripcion == null)
                {
                    return new NotFoundResult();
                }

                var atletaExists = await _context.AtletasFederados.AnyAsync(a => a.ParticipanteId == inscripcionCreateDto.ParticipanteId);
                if (!atletaExists)
                {
                    return new BadRequestResult();
                }

                var pruebaExists = await _context.EventoPruebas.AnyAsync(ep => ep.IdEventoPrueba == inscripcionCreateDto.IdEventoPrueba);
                if (!pruebaExists)
                {
                    return new BadRequestResult();
                }

                var inscripcionExistente = await _context.Inscripciones
                    .AnyAsync(i => i.IdParticipante == inscripcionCreateDto.ParticipanteId &&
                                  i.IdEventoPrueba == inscripcionCreateDto.IdEventoPrueba &&
                                  i.IdInscripcion != id);

                if (inscripcionExistente)
                {
                    return new BadRequestResult();
                }

                inscripcion.IdParticipante = inscripcionCreateDto.ParticipanteId;
                inscripcion.IdEventoPrueba = inscripcionCreateDto.IdEventoPrueba;
                inscripcion.FechaInscripcion = inscripcionCreateDto.FechaInscripcion;

                await _context.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await InscripcionExistsAsync(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteInscripcion(int id)
        {
            try
            {
                var inscripcion = await _context.Inscripciones.FindAsync(id);
                if (inscripcion == null)
                {
                    return new NotFoundResult();
                }

                _context.Inscripciones.Remove(inscripcion);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (DbUpdateException dbEx)
            {
                return new StatusCodeResult(500);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        private async Task<bool> InscripcionExistsAsync(int id)
        {
            return await _context.Inscripciones.AnyAsync(e => e.IdInscripcion == id);
        }

        private string GetEstadoEvento(DateTime fechaInicio, DateTime fechaFin)
        {
            var ahora = DateTime.UtcNow;

            if (fechaInicio > ahora)
                return "Próximo";
            else if (fechaInicio <= ahora && fechaFin >= ahora)
                return "Activo";
            else
                return "Finalizado";
        }
    }
}
