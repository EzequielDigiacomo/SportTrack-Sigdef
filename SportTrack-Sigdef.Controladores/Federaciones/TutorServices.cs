using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.DTOs.TutorFederacion;
using SportTrack_Sigdef.Entidades.DTOs.Participante;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportTrack_Sigdef.Controladores.Federaciones;
using Microsoft.AspNetCore.Mvc;

namespace SportTrack_Sigdef.Controladores.Services
{
    public class TutorServices : ITutorServices
    {
        private readonly SportTrackDbContext _context;
        private readonly SportTrack_Sigdef.Controladores.Audit.IAuditService _auditService;

        public TutorServices(
            SportTrackDbContext context,
            SportTrack_Sigdef.Controladores.Audit.IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<ActionResult<IEnumerable<TutorDto>>> GetTutores()
        {
            try
            {
                var tutores = await _context.Tutores
                    .Include(t => t.Participante)
                    .Include(t => t.AtletasTutores)
                    .Select(t => new TutorDto
                    {
                        ParticipanteId = t.ParticipanteId,
                        TipoTutor = t.TipoTutor,
                        NombrePersona = t.Participante.Nombre + " " + t.Participante.Apellido,
                        Documento = t.Participante.Dni,
                        Telefono = t.Participante.Telefono,
                        Email = t.Participante.Email,
                        Direccion = t.Participante.Direccion,
                        CantidadAtletas = t.AtletasTutores.Count
                    })
                    .ToListAsync();

                return new OkObjectResult(tutores);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<TutorDetailDto>> GetTutor(int id)
        {
            try
            {
                var tutor = await _context.Tutores
                    .Include(t => t.Participante)
                    .Include(t => t.AtletasTutores)
                        .ThenInclude(at => at.AtletaFederacion)
                        .ThenInclude(a => a.Participante)
                    .Include(t => t.AtletasTutores)
                        .ThenInclude(at => at.AtletaFederacion)
                        .ThenInclude(a => a.Club)
                    .Where(t => t.ParticipanteId == id)
                    .Select(t => new TutorDetailDto
                    {
                        ParticipanteId = t.ParticipanteId,
                        TipoTutor = t.TipoTutor,
                        Participante = new PersonaDto
                        {
                            ParticipanteId = t.Participante.ParticipanteId,
                            Nombre = t.Participante.Nombre,
                            Apellido = t.Participante.Apellido,
                            Documento = t.Participante.Dni,
                            FechaNacimiento = t.Participante.FechaNacimiento,
                            Email = t.Participante.Email,
                            Telefono = t.Participante.Telefono,
                            Direccion = t.Participante.Direccion
                        },
                        AtletasTutores = t.AtletasTutores.Select(at => new AtletaTutorDto
                        {
                            ParticipanteId = at.IdAtleta,
                            IdTutor = at.IdTutor,
                            Parentesco = at.Parentesco,
                            NombreAtleta = at.AtletaFederacion.Participante.Nombre + " " + at.AtletaFederacion.Participante.Apellido,
                            NombreClub = at.AtletaFederacion.Club.Nombre.ToString()
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (tutor == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(tutor);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<IEnumerable<TutorDto>>> GetTutoresPorTipo(string tipoTutor)
        {
            try
            {
                var tutores = await _context.Tutores
                    .Include(t => t.Participante)
                    .Include(t => t.AtletasTutores)
                    .Where(t => t.TipoTutor.ToLower() == tipoTutor.ToLower())
                    .Select(t => new TutorDto
                    {
                        ParticipanteId = t.ParticipanteId,
                        TipoTutor = t.TipoTutor,
                        NombrePersona = t.Participante.Nombre + " " + t.Participante.Apellido,
                        Documento = t.Participante.Dni,
                        Telefono = t.Participante.Telefono,
                        Email = t.Participante.Email,
                        Direccion = t.Participante.Direccion,
                        CantidadAtletas = t.AtletasTutores.Count
                    })
                    .ToListAsync();

                return new OkObjectResult(tutores);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<IEnumerable<TutorDto>>> GetTutoresSinAtletas()
        {
            try
            {
                var tutores = await _context.Tutores
                    .Include(t => t.Participante)
                    .Include(t => t.AtletasTutores)
                    .Where(t => !t.AtletasTutores.Any())
                    .Select(t => new TutorDto
                    {
                        ParticipanteId = t.ParticipanteId,
                        TipoTutor = t.TipoTutor,
                        NombrePersona = t.Participante.Nombre + " " + t.Participante.Apellido,
                        Documento = t.Participante.Dni,
                        Telefono = t.Participante.Telefono,
                        Email = t.Participante.Email,
                        Direccion = t.Participante.Direccion,
                        CantidadAtletas = 0
                    })
                    .ToListAsync();

                return new OkObjectResult(tutores);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<IEnumerable<TutorDto>>> SearchTutores(string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return new BadRequestResult();
                }

                var tutores = await _context.Tutores
                    .Include(t => t.Participante)
                    .Include(t => t.AtletasTutores)
                    .Where(t => t.Participante.Nombre.Contains(term) ||
                               t.Participante.Apellido.Contains(term) ||
                               t.Participante.Dni.Contains(term) ||
                               t.TipoTutor.Contains(term))
                    .Select(t => new TutorDto
                    {
                        ParticipanteId = t.ParticipanteId,
                        TipoTutor = t.TipoTutor,
                        NombrePersona = t.Participante.Nombre + " " + t.Participante.Apellido,
                        Documento = t.Participante.Dni,
                        Telefono = t.Participante.Telefono,
                        Email = t.Participante.Email,
                        Direccion = t.Participante.Direccion,
                        CantidadAtletas = t.AtletasTutores.Count
                    })
                    .ToListAsync();

                return new OkObjectResult(tutores);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<IEnumerable<string>>> GetTiposTutor()
        {
            try
            {
                var tipos = await _context.Tutores
                    .Select(t => t.TipoTutor)
                    .Distinct()
                    .ToListAsync();

                return new OkObjectResult(tipos);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<TutorDto>> PostTutor(TutorCreateDto tutorCreateDto)
        {
            try
            {
                if (tutorCreateDto.ParticipanteId <= 0)
                {
                    return new BadRequestObjectResult(new { message = "Falta el identificador de la persona del tutor." });
                }

                var personaExists = await _context.Participantes.AnyAsync(p => p.ParticipanteId == tutorCreateDto.ParticipanteId);
                if (!personaExists)
                {
                    return new BadRequestObjectResult(new { message = "No se encontró la persona. Revisá el documento e intentá nuevamente." });
                }

                var tutorExists = await _context.Tutores.AnyAsync(t => t.ParticipanteId == tutorCreateDto.ParticipanteId);
                if (tutorExists)
                {
                    return new BadRequestObjectResult(new { message = "Esta persona ya está registrada como tutor." });
                }

                var tieneOtroRol = await _context.AtletasFederados.AnyAsync(a => a.ParticipanteId == tutorCreateDto.ParticipanteId) ||
                                  await _context.Entrenadores.AnyAsync(e => e.ParticipanteId == tutorCreateDto.ParticipanteId) ||
                                  await _context.DelegadosClub.AnyAsync(d => d.IdParticipante == tutorCreateDto.ParticipanteId);

                if (tieneOtroRol)
                {
                    return new BadRequestObjectResult(new { message = "Esta persona ya tiene otro rol asignado. Una persona no puede tener múltiples roles en el sistema." });
                }

                var tutor = new TutorFederacion
                {
                    ParticipanteId = tutorCreateDto.ParticipanteId,
                    TipoTutor = tutorCreateDto.TipoTutor
                };

                _context.Tutores.Add(tutor);
                await _context.SaveChangesAsync();

                await _context.Entry(tutor)
                    .Reference(t => t.Participante)
                    .LoadAsync();

                var nombreTutor = $"{tutor.Participante.Nombre} {tutor.Participante.Apellido}".Trim();
                await _auditService.RegistrarAccionAsync(
                    "CREATE_TUTOR",
                    $"Tutor creado: {nombreTutor}",
                    null,
                    "Tutores");

                var tutorDto = new TutorDto
                {
                    ParticipanteId = tutor.ParticipanteId,
                    TipoTutor = tutor.TipoTutor,
                    NombrePersona = tutor.Participante.Nombre + " " + tutor.Participante.Apellido,
                    Documento = tutor.Participante.Dni,
                    Telefono = tutor.Participante.Telefono,
                    Email = tutor.Participante.Email,
                    Direccion = tutor.Participante.Direccion,
                    CantidadAtletas = 0
                };

                var result = new ObjectResult(tutorDto)
                {
                    StatusCode = 201
                };
                return result;
            }
            catch (DbUpdateException dbEx)
            {
                return new StatusCodeResult(500);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> PutTutor(int id, TutorCreateDto tutorCreateDto)
        {
            try
            {
                if (tutorCreateDto.ParticipanteId > 0 && id != tutorCreateDto.ParticipanteId)
                {
                    return new BadRequestObjectResult(new { message = "El identificador del tutor no coincide." });
                }

                var tutor = await _context.Tutores.FindAsync(id);
                if (tutor == null)
                {
                    return new NotFoundResult();
                }

                tutor.TipoTutor = tutorCreateDto.TipoTutor;
                await _context.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TutorExistsAsync(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteTutor(int id)
        {
            try
            {
                var tutor = await _context.Tutores
                    .Include(t => t.AtletasTutores)
                    .FirstOrDefaultAsync(t => t.ParticipanteId == id);

                if (tutor == null)
                {
                    return new NotFoundResult();
                }

                if (tutor.AtletasTutores.Any())
                {
                    return new BadRequestResult();
                }

                _context.Tutores.Remove(tutor);
                await _context.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (DbUpdateException dbEx)
            {
                return new StatusCodeResult(500);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        private async Task<bool> TutorExistsAsync(int id)
        {
            return await _context.Tutores.AnyAsync(e => e.ParticipanteId == id);
        }
    }
}
