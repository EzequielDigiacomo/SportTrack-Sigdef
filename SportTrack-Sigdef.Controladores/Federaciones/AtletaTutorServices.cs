using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;
using SportTrack_Sigdef.Entidades.DTOs.TutorFederacion;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportTrack_Sigdef.Controladores.Federaciones;

namespace SportTrack_Sigdef.Controladores.Services
{
    public class AtletaTutorServices : IAtletaTutorServices
    {
        private readonly SportTrackDbContext _context;

        public AtletaTutorServices(SportTrackDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetAtletasTutores()
        {
            try
            {
                var atletasTutores = await _context.AtletasTutores
                    .Include(at => at.AtletaFederacion)
                        .ThenInclude(a => a.Participante)
                    .Include(at => at.AtletaFederacion)
                        .ThenInclude(a => a.Club)
                    .Include(at => at.TutorFederacion)
                        .ThenInclude(t => t.Participante)
                    .Select(at => new AtletaTutorDto
                    {
                        ParticipanteId = at.IdAtleta,
                        IdTutor = at.IdTutor,
                        Parentesco = at.Parentesco,
                        NombreAtleta = at.AtletaFederacion.Participante.Nombre + " " + at.AtletaFederacion.Participante.Apellido,
                        NombreTutor = at.TutorFederacion.Participante.Nombre + " " + at.TutorFederacion.Participante.Apellido,
                        NombreClub = at.AtletaFederacion.Club.Nombre
                    })
                    .ToListAsync();

                return new OkObjectResult(atletasTutores);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetTutoresPorAtleta(int ParticipanteId)
        {
            try
            {
                var tutores = await _context.AtletasTutores
                    .Include(at => at.AtletaFederacion)
                        .ThenInclude(a => a.Participante)
                    .Include(at => at.TutorFederacion)
                        .ThenInclude(t => t.Participante)
                    .Where(at => at.IdAtleta == ParticipanteId)
                    .Select(at => new AtletaTutorDto
                    {
                        ParticipanteId = at.IdAtleta,
                        IdTutor = at.IdTutor,
                        Parentesco = at.Parentesco,
                        NombreAtleta = at.AtletaFederacion.Participante.Nombre + " " + at.AtletaFederacion.Participante.Apellido,
                        NombreTutor = at.TutorFederacion.Participante.Nombre + " " + at.TutorFederacion.Participante.Apellido
                    })
                    .ToListAsync();

                return new OkObjectResult(tutores);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<IEnumerable<AtletaTutorDto>>> GetAtletasPorTutor(int idTutor)
        {
            try
            {
                var AtletasFederados = await _context.AtletasTutores
                    .Include(at => at.AtletaFederacion)
                        .ThenInclude(a => a.Participante)
                    .Include(at => at.AtletaFederacion)
                        .ThenInclude(a => a.Club)
                    .Include(at => at.TutorFederacion)
                        .ThenInclude(t => t.Participante)
                    .Where(at => at.IdTutor == idTutor)
                    .Select(at => new AtletaTutorDto
                    {
                        ParticipanteId = at.IdAtleta,
                        IdTutor = at.IdTutor,
                        Parentesco = at.Parentesco,
                        NombreAtleta = at.AtletaFederacion.Participante.Nombre + " " + at.AtletaFederacion.Participante.Apellido,
                        NombreTutor = at.TutorFederacion.Participante.Nombre + " " + at.TutorFederacion.Participante.Apellido,
                        NombreClub = at.AtletaFederacion.Club.Nombre
                    })
                    .ToListAsync();

                return new OkObjectResult(AtletasFederados);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<AtletaTutorDetailDto>> GetAtletaTutor(int ParticipanteId, int idTutor)
        {
            try
            {
                var atletaTutor = await _context.AtletasTutores
                    .Include(at => at.AtletaFederacion)
                        .ThenInclude(a => a.Participante)
                    .Include(at => at.AtletaFederacion)
                        .ThenInclude(a => a.Club)
                    .Include(at => at.TutorFederacion)
                        .ThenInclude(t => t.Participante)
                    .Where(at => at.IdAtleta == ParticipanteId && at.IdTutor == idTutor)
                    .Select(at => new AtletaTutorDetailDto
                    {
                        ParticipanteId = at.IdAtleta,
                        IdTutor = at.IdTutor,
                        Parentesco = at.Parentesco,
                        AtletaFederacion = new AtletaDto
                        {
                            ParticipanteId = at.AtletaFederacion.ParticipanteId,
                            IdClub = at.AtletaFederacion.IdClub,
                            EstadoPago = at.AtletaFederacion.EstadoPago,
                            PerteneceSeleccion = at.AtletaFederacion.PerteneceSeleccion,
                            Categoria = at.AtletaFederacion.Categoria,
                            NombrePersona = at.AtletaFederacion.Participante.Nombre + " " + at.AtletaFederacion.Participante.Apellido,
                            NombreClub = at.AtletaFederacion.Club.Nombre
                        },
                        TutorFederacion = new TutorDto
                        {
                            ParticipanteId = at.TutorFederacion.ParticipanteId,
                            TipoTutor = at.TutorFederacion.TipoTutor,
                            NombrePersona = at.TutorFederacion.Participante.Nombre + " " + at.TutorFederacion.Participante.Apellido
                        }
                    })
                    .FirstOrDefaultAsync();

                if (atletaTutor == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(atletaTutor);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<AtletaTutorDto>> PostAtletaTutor(AtletaTutorCreateDto atletaTutorCreateDto)
        {
            try
            {
                var atletaExists = await _context.AtletasFederados.AnyAsync(a => a.ParticipanteId == atletaTutorCreateDto.ParticipanteId);
                if (!atletaExists)
                {
                    return new BadRequestResult();
                }

                var tutorExists = await _context.Tutores.AnyAsync(t => t.ParticipanteId == atletaTutorCreateDto.IdTutor);
                if (!tutorExists)
                {
                    return new BadRequestResult();
                }

                var relationExists = await _context.AtletasTutores
                    .AnyAsync(at => at.IdAtleta == atletaTutorCreateDto.ParticipanteId && at.IdTutor == atletaTutorCreateDto.IdTutor);

                if (relationExists)
                {
                    return new BadRequestResult();
                }

                var atletaTutor = new AtletaFederacionTutor
                {
                    IdAtleta = atletaTutorCreateDto.ParticipanteId,
                    IdTutor = atletaTutorCreateDto.IdTutor,
                    Parentesco = atletaTutorCreateDto.Parentesco
                };

                _context.AtletasTutores.Add(atletaTutor);
                await _context.SaveChangesAsync();

                await _context.Entry(atletaTutor)
                    .Reference(at => at.AtletaFederacion)
                    .LoadAsync();
                await _context.Entry(atletaTutor.AtletaFederacion)
                    .Reference(a => a.Participante)
                    .LoadAsync();
                await _context.Entry(atletaTutor.AtletaFederacion)
                    .Reference(a => a.Club)
                    .LoadAsync();
                await _context.Entry(atletaTutor)
                    .Reference(at => at.TutorFederacion)
                    .LoadAsync();
                await _context.Entry(atletaTutor.TutorFederacion)
                    .Reference(t => t.Participante)
                    .LoadAsync();

                var atletaTutorDto = new AtletaTutorDto
                {
                    ParticipanteId = atletaTutor.IdAtleta,
                    IdTutor = atletaTutor.IdTutor,
                    Parentesco = atletaTutor.Parentesco,
                    NombreAtleta = atletaTutor.AtletaFederacion.Participante.Nombre + " " + atletaTutor.AtletaFederacion.Participante.Apellido,
                    NombreTutor = atletaTutor.TutorFederacion.Participante.Nombre + " " + atletaTutor.TutorFederacion.Participante.Apellido,
                    NombreClub = atletaTutor.AtletaFederacion.Club.Nombre
                };

                var result = new ObjectResult(atletaTutorDto)
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

        public async Task<IActionResult> PutAtletaTutor(int ParticipanteId, int idTutor, AtletaTutorCreateDto atletaTutorCreateDto)
        {
            try
            {
                if (ParticipanteId != atletaTutorCreateDto.ParticipanteId || idTutor != atletaTutorCreateDto.IdTutor)
                {
                    return new BadRequestResult();
                }

                var atletaTutor = await _context.AtletasTutores
                    .FirstOrDefaultAsync(at => at.IdAtleta == ParticipanteId && at.IdTutor == idTutor);

                if (atletaTutor == null)
                {
                    return new NotFoundResult();
                }

                atletaTutor.Parentesco = atletaTutorCreateDto.Parentesco;

                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AtletaTutorExistsAsync(ParticipanteId, idTutor))
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

        public async Task<IActionResult> DeleteAtletaTutor(int ParticipanteId, int idTutor)
        {
            try
            {
                var atletaTutor = await _context.AtletasTutores
                    .FirstOrDefaultAsync(at => at.IdAtleta == ParticipanteId && at.IdTutor == idTutor);

                if (atletaTutor == null)
                {
                    return new NotFoundResult();
                }

                _context.AtletasTutores.Remove(atletaTutor);
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

        private async Task<bool> AtletaTutorExistsAsync(int ParticipanteId, int idTutor)
        {
            return await _context.AtletasTutores.AnyAsync(e => e.IdAtleta == ParticipanteId && e.IdTutor == idTutor);
        }
    }
}
