using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Federaciones;
using SIGDEF.DTOs;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacionTutor;
using SportTrack_Sigdef.Entidades.DTOs.Base;
using SportTrack_Sigdef.Entidades.DTOs.Club;
using SportTrack_Sigdef.Entidades.DTOs.Inscripcion;
using SportTrack_Sigdef.Entidades.DTOs.Participante;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportTrack_Sigdef.Entidades.Enums;

namespace SportTrack_Sigdef.Controladores.Services
{
    public class AtletaServices : IAtletaServices
    {
        private readonly SportTrackDbContext _context;
        private readonly ITenantProvider _tenantProvider;
        private readonly IAltaAtletaService _altaAtletaService;

        public AtletaServices(SportTrackDbContext context, ITenantProvider tenantProvider, IAltaAtletaService altaAtletaService)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _altaAtletaService = altaAtletaService;
        }

        // -------------------------------------------------
        // GET: Obtener todos los AtletasFederados
        // -------------------------------------------------
        public async Task<ActionResult<IEnumerable<AtletaDetailDto>>> GetAtletas()
        {
            try
            {
                var query = _context.AtletasFederados.AsQueryable();
                var fedId = _tenantProvider.GetFederacionId();
                if (fedId.HasValue)
                {
                    query = query.Where(a => a.IdFederacion == fedId.Value);
                }
                var clubId = _tenantProvider.GetClubId();
                if (clubId.HasValue)
                {
                    query = query.Where(a => a.IdClub == clubId.Value);
                }

                var AtletasFederados = await query
                    .Include(a => a.Participante)
                    .Include(a => a.Club)
                    .Include(a => a.Inscripciones)
                        .ThenInclude(i => i.EventoPrueba)
                    .Include(a => a.Tutores)
                        .ThenInclude(at => at.TutorFederacion)
                        .ThenInclude(t => t.Participante)
                    .Select(a => new AtletaDetailDto
                    {
                        ParticipanteId = a.ParticipanteId,
                        IdClub = a.IdClub,
                        EstadoPago = a.EstadoPago,
                        PerteneceSeleccion = a.PerteneceSeleccion,
                        Categoria = a.Categoria,
                        CategoriaId = a.Participante.CategoriaId,
                        CategoriaNombre = a.Participante.Categoria != null
                            ? a.Participante.Categoria.Nombre
                            : null,
                        BecadoEnard = a.BecadoEnard,
                        BecadoSdn = a.BecadoSdn,
                        MontoBeca = a.MontoBeca,
                        PresentoAptoMedico = a.PresentoAptoMedico,
                        FechaAptoMedico = a.FechaAptoMedico,
                        FechaCreacion = a.FechaCreacion,
                        Participante = new PersonaDto
                        {
                            ParticipanteId = a.Participante.ParticipanteId,
                            Nombre = a.Participante.Nombre,
                            Apellido = a.Participante.Apellido,
                            Documento = a.Participante.Dni,
                            FechaNacimiento = a.Participante.FechaNacimiento,
                            Email = a.Participante.Email,
                            Telefono = a.Participante.Telefono,
                            Direccion = a.Participante.Direccion
                        },
                        Club = a.Club != null ? new ClubDto
                        {
                            IdClub = a.Club.IdClub,
                            Nombre = a.Club.Nombre,
                            Siglas = a.Club.Siglas
                        } : null,
                        Inscripciones = a.Inscripciones.Select(i => new InscripcionDto
                        {
                            IdInscripcion = i.IdInscripcion,
                            IdEvento = i.IdEventoPrueba,
                            FechaInscripcion = i.FechaInscripcion,
                        }).ToList(),
                        Tutores = a.Tutores.Select(at => new AtletaTutorDto
                        {
                            ParticipanteId = at.IdAtleta,
                            IdTutor = at.IdTutor,
                            Parentesco = at.Parentesco,
                            NombreTutor = (at.TutorFederacion != null && at.TutorFederacion.Participante != null) ? at.TutorFederacion.Participante.Nombre + " " + at.TutorFederacion.Participante.Apellido : "TutorFederacion no encontrado"
                        }).ToList()
                    })
                    .ToListAsync();

                return new OkObjectResult(AtletasFederados);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        // -------------------------------------------------
        // GET: Obtener AtletasFederados paginados
        // -------------------------------------------------
        public async Task<ActionResult<PagedResponseDto<AtletaListDto>>> GetAtletasPaginadosAsync(PaginationParamsDto parameters)
        {
            try
            {
                var query = _context.AtletasFederados.AsQueryable();
                var fedId = _tenantProvider.GetFederacionId();
                if (fedId.HasValue)
                {
                    query = query.Where(a => a.IdFederacion == fedId.Value);
                }
                var clubId = _tenantProvider.GetClubId();
                if (clubId.HasValue)
                {
                    query = query.Where(a => a.IdClub == clubId.Value);
                }

                query = query
                    .AsNoTracking()
                    .Include(a => a.Participante)
                    .Include(a => a.Club)
                    .Include(a => a.Tutores)
                        .ThenInclude(at => at.TutorFederacion)
                        .ThenInclude(t => t.Participante)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
                {
                    var search = parameters.SearchTerm.ToLower();
                    query = query.Where(a =>
                        a.Participante.Nombre.ToLower().Contains(search) ||
                        a.Participante.Apellido.ToLower().Contains(search) ||
                        a.Participante.Dni.ToLower().Contains(search) ||
                        (a.Club != null && a.Club.Nombre.ToLower().Contains(search))
                    );
                }

                var totalRecords = await query.CountAsync();

                var pagedData = await query
                    .OrderByDescending(a => a.ParticipanteId)
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .Select(a => new AtletaListDto
                    {
                        ParticipanteId = a.ParticipanteId,
                        NombrePersona = (a.Participante.Nombre + " " + a.Participante.Apellido).Trim(),
                        Documento = a.Participante.Dni,
                        FechaNacimiento = a.Participante.FechaNacimiento,
                        NombreClub = a.Club != null ? a.Club.Nombre : "Agente Libre",
                        Categoria = a.Categoria,
                        CategoriaId = a.Participante.CategoriaId,
                        CategoriaNombre = a.Participante.Categoria != null
                            ? a.Participante.Categoria.Nombre
                            : null,
                        PerteneceSeleccion = a.PerteneceSeleccion,
                        EstadoPago = a.EstadoPago,
                        FechaCreacion = a.FechaCreacion,
                        CantidadDocumentos = 0,
                        Edad = (DateTime.UtcNow.Year - a.Participante.FechaNacimiento.Year) -
                            (DateTime.UtcNow.DayOfYear < a.Participante.FechaNacimiento.DayOfYear ? 1 : 0),
                        TutorInfo = a.Tutores.Select(t => new TutorListDto
                        {
                            ParticipanteId = t.IdTutor,
                            Nombre = t.TutorFederacion.Participante.Nombre,
                            Apellido = t.TutorFederacion.Participante.Apellido,
                            Documento = t.TutorFederacion.Participante.Dni,
                            Telefono = t.TutorFederacion.Participante.Telefono
                        }).FirstOrDefault()
                    })
                    .ToListAsync();

                return new OkObjectResult(new PagedResponseDto<AtletaListDto>
                {
                    Data = pagedData,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize,
                    TotalRecords = totalRecords,
                    TotalPages = (int)Math.Ceiling((double)totalRecords / parameters.PageSize)
                });
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        // -------------------------------------------------
        // GET: Obtener AtletaFederacion por ID
        // -------------------------------------------------
        public async Task<ActionResult<AtletaDetailDto>> GetAtleta(int id)
        {
            try
            {
                var AtletaFederacion = await _context.AtletasFederados
                    .Include(a => a.Participante)
                    .Include(a => a.Club)
                    .Include(a => a.Inscripciones)
                        .ThenInclude(i => i.EventoPrueba)
                    .Include(a => a.Tutores)
                        .ThenInclude(at => at.TutorFederacion)
                        .ThenInclude(t => t.Participante)
                    .Where(a => a.ParticipanteId == id)
                    .Select(a => new AtletaDetailDto
                    {
                        ParticipanteId = a.ParticipanteId,
                        IdClub = a.IdClub,
                        EstadoPago = a.EstadoPago,
                        PerteneceSeleccion = a.PerteneceSeleccion,
                        Categoria = a.Categoria,
                        CategoriaId = a.Participante.CategoriaId,
                        CategoriaNombre = a.Participante.Categoria != null
                            ? a.Participante.Categoria.Nombre
                            : null,
                        BecadoEnard = a.BecadoEnard,
                        BecadoSdn = a.BecadoSdn,
                        MontoBeca = a.MontoBeca,
                        PresentoAptoMedico = a.PresentoAptoMedico,
                        FechaAptoMedico = a.FechaAptoMedico,
                        FechaCreacion = a.FechaCreacion,

                        Participante = new PersonaDto
                        {
                            ParticipanteId = a.Participante.ParticipanteId,
                            Nombre = a.Participante.Nombre,
                            Apellido = a.Participante.Apellido,
                            Documento = a.Participante.Dni,
                            FechaNacimiento = a.Participante.FechaNacimiento,
                            Email = a.Participante.Email,
                            Telefono = a.Participante.Telefono,
                            Direccion = a.Participante.Direccion
                        },
                        Club = a.Club != null ? new ClubDto
                        {
                            IdClub = a.Club.IdClub,
                            Nombre = a.Club.Nombre,
                            Siglas = a.Club.Siglas
                        } : null,

                        Inscripciones = a.Inscripciones.Select(i => new InscripcionDto
                        {
                            IdInscripcion = i.IdInscripcion,
                            IdEvento = i.IdEventoPrueba,
                            FechaInscripcion = i.FechaInscripcion,
                        }).ToList(),
                        Tutores = a.Tutores.Select(at => new AtletaTutorDto
                        {
                            ParticipanteId = at.IdAtleta,
                            IdTutor = at.IdTutor,
                            Parentesco = at.Parentesco,
                            NombreTutor = (at.TutorFederacion != null && at.TutorFederacion.Participante != null) ? at.TutorFederacion.Participante.Nombre + " " + at.TutorFederacion.Participante.Apellido : "TutorFederacion no encontrado"
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (AtletaFederacion == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(AtletaFederacion);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        // -------------------------------------------------
        // POST: Crear nuevo AtletaFederacion
        // -------------------------------------------------
        public async Task<ActionResult<AtletaDto>> PostAtleta(AtletaCreateDto atletaCreateDto)
        {
            try
            {
                // Validar existencia de Participante
                var personaExists = await _context.Participantes.AnyAsync(p => p.ParticipanteId == atletaCreateDto.ParticipanteId);
                if (!personaExists)
                {
                    return new BadRequestResult();
                }

                // Validar existencia de Club
                if (atletaCreateDto.IdClub.HasValue)
                {
                    var clubExists = await _context.Clubes.AnyAsync(c => c.IdClub == atletaCreateDto.IdClub.Value);
                    if (!clubExists)
                    {
                        return new BadRequestResult();
                    }
                }

                // Evitar duplicados: actualizar capa federación si ya existe
                var atletaExists = await _context.AtletasFederados.AnyAsync(a => a.ParticipanteId == atletaCreateDto.ParticipanteId);
                if (atletaExists)
                {
                    var fedInput = _altaAtletaService.FromAtletaCreateDto(atletaCreateDto);
                    await _altaAtletaService.EnsureAtletaFederacionAsync(atletaCreateDto.ParticipanteId, fedInput);

                    var existente = await _context.AtletasFederados
                        .Include(a => a.Participante)
                        .Include(a => a.Club)
                        .FirstAsync(a => a.ParticipanteId == atletaCreateDto.ParticipanteId);

                    return new OkObjectResult(new AtletaDto
                    {
                        ParticipanteId = existente.ParticipanteId,
                        IdClub = existente.IdClub,
                        EstadoPago = existente.EstadoPago,
                        PerteneceSeleccion = existente.PerteneceSeleccion,
                        Categoria = existente.Categoria,
                        BecadoEnard = existente.BecadoEnard,
                        BecadoSdn = existente.BecadoSdn,
                        MontoBeca = existente.MontoBeca,
                        PresentoAptoMedico = existente.PresentoAptoMedico,
                        FechaAptoMedico = existente.FechaAptoMedico,
                        NombrePersona = existente.Participante.Nombre + " " + existente.Participante.Apellido,
                        NombreClub = existente.Club != null ? existente.Club.Nombre : "Agente Libre",
                        FechaCreacion = existente.FechaCreacion
                    });
                }

                var fedInputCreate = _altaAtletaService.FromAtletaCreateDto(atletaCreateDto);
                var atletaFed = await _altaAtletaService.EnsureAtletaFederacionAsync(atletaCreateDto.ParticipanteId, fedInputCreate);
                var AtletaFederacion = atletaFed;

                await _context.Entry(AtletaFederacion).Reference(a => a.Participante).LoadAsync();
                await _context.Entry(AtletaFederacion).Reference(a => a.Club).LoadAsync();

                var atletaDto = new AtletaDto
                {
                    ParticipanteId = AtletaFederacion.ParticipanteId,
                    IdClub = AtletaFederacion.IdClub,
                    EstadoPago = AtletaFederacion.EstadoPago,
                    PerteneceSeleccion = AtletaFederacion.PerteneceSeleccion,
                    Categoria = AtletaFederacion.Categoria,
                    BecadoEnard = AtletaFederacion.BecadoEnard,
                    BecadoSdn = AtletaFederacion.BecadoSdn,
                    MontoBeca = AtletaFederacion.MontoBeca,
                    PresentoAptoMedico = AtletaFederacion.PresentoAptoMedico,
                    FechaAptoMedico = AtletaFederacion.FechaAptoMedico,
                    NombrePersona = AtletaFederacion.Participante.Nombre + " " + AtletaFederacion.Participante.Apellido,
                    NombreClub = AtletaFederacion.Club != null ? AtletaFederacion.Club.Nombre : "Agente Libre",
                    FechaCreacion = AtletaFederacion.FechaCreacion
                };

                var result = new ObjectResult(atletaDto)
                {
                    StatusCode = 201
                };
                return result;
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(500);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        // -------------------------------------------------
        // POST: Registro Atómico de AtletaFederacion (incluye TutorFederacion si es menor)
        // -------------------------------------------------
        public async Task<ActionResult<AtletaDto>> PostAtletaFull(AtletaFullCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var participanteInput = _altaAtletaService.FromPersonaCreateDto(dto.PersonaAtleta, dto.DatosDeportivos.IdClub);
                var fedInput = _altaAtletaService.FromAtletaCreateDto(dto.DatosDeportivos);
                var altaResult = await _altaAtletaService.AltaAtletaCompletaAsync(participanteInput, fedInput);
                var AtletaFederacion = altaResult.AtletaFederacion;

                // Manejar TutorFederacion si es menor
                if (dto.EsMenor && dto.TutorFederacion != null)
                {
                    int idPersonaTutor;
                    if (dto.TutorFederacion.IdPersonaTutor.HasValue)
                    {
                        idPersonaTutor = dto.TutorFederacion.IdPersonaTutor.Value;
                    }
                    else if (dto.TutorFederacion.PersonaTutor != null)
                    {
                        var tutorInput = _altaAtletaService.FromPersonaCreateDto(dto.TutorFederacion.PersonaTutor);
                        var tutorResult = await _altaAtletaService.UpsertParticipanteAsync(tutorInput);
                        idPersonaTutor = tutorResult.ParticipanteId;
                    }
                    else
                    {
                        throw new Exception("Datos del tutor incompletos para registro de menor.");
                    }

                    // Asegurar registro de TutorFederacion
                    var tutor = await _context.Tutores.FindAsync(idPersonaTutor);
                    if (tutor == null)
                    {
                        tutor = new TutorFederacion 
                        { 
                            ParticipanteId = idPersonaTutor, 
                            TipoTutor = "Registrado vía AtletaFederacion" 
                        };
                        _context.Tutores.Add(tutor);
                        await _context.SaveChangesAsync();
                    }

                    // Relación AtletaFederacionTutor
                    var relacion = await _context.AtletasTutores
                        .FirstOrDefaultAsync(at => at.IdAtleta == AtletaFederacion.ParticipanteId && at.IdTutor == idPersonaTutor);
                    
                    if (relacion == null)
                    {
                        relacion = new AtletaFederacionTutor
                        {
                            IdAtleta = AtletaFederacion.ParticipanteId,
                            IdTutor = idPersonaTutor,
                            Parentesco = (SportTrack_Sigdef.Entidades.Enums.Parentesco)dto.TutorFederacion.Parentesco
                        };
                        _context.AtletasTutores.Add(relacion);
                    }
                    else
                    {
                        relacion.Parentesco = (SportTrack_Sigdef.Entidades.Enums.Parentesco)dto.TutorFederacion.Parentesco;
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                // Reutilizar lógica de respuesta de GetAtleta por consistencia
                var response = await GetAtleta(AtletaFederacion.ParticipanteId);
                
                // Mapear AtletaDetailDto a AtletaDto para coincidir con la firma
                if (response.Value is AtletaDetailDto detail)
                {
                    return new OkObjectResult(new AtletaDto
                    {
                        ParticipanteId = detail.ParticipanteId,
                        IdClub = detail.IdClub,
                        EstadoPago = detail.EstadoPago,
                        NombrePersona = detail.Participante.Nombre + " " + detail.Participante.Apellido,
                        NombreClub = detail.Club?.Nombre ?? "Agente Libre",
                        Categoria = detail.Categoria,
                        FechaCreacion = detail.FechaCreacion
                        // ... completar campos si es necesario
                    });
                }

                return response.Result ?? new StatusCodeResult(201);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ObjectResult(new { error = "Fallo el registro atómico", detail = ex.Message }) { StatusCode = 500 };
            }
        }

        // -------------------------------------------------
        // PUT: Actualizar AtletaFederacion
        // -------------------------------------------------
        public async Task<IActionResult> PutAtleta(int id, AtletaCreateDto atletaCreateDto)
        {
            try
            {
                if (id != atletaCreateDto.ParticipanteId)
                {
                    return new BadRequestResult();
                }

                var AtletaFederacion = await _context.AtletasFederados.FindAsync(id);
                if (AtletaFederacion == null)
                {
                    return new NotFoundResult();
                }

                // Validar club
                if (atletaCreateDto.IdClub.HasValue)
                {
                    var clubExists = await _context.Clubes.AnyAsync(c => c.IdClub == atletaCreateDto.IdClub.Value);
                    if (!clubExists)
                    {
                        return new BadRequestResult();
                    }
                }

                // Convertir fecha apto médico a UTC
                DateTime? fechaAptoMedicoUtc = null;
                if (atletaCreateDto.FechaAptoMedico.HasValue)
                {
                    fechaAptoMedicoUtc = DateTime.SpecifyKind(atletaCreateDto.FechaAptoMedico.Value, DateTimeKind.Utc);
                }

                // Actualizar campos (FechaCreacion **no** se modifica)
                AtletaFederacion.IdClub = atletaCreateDto.IdClub;
                AtletaFederacion.EstadoPago = atletaCreateDto.EstadoPago;
                AtletaFederacion.PerteneceSeleccion = atletaCreateDto.PerteneceSeleccion;
                AtletaFederacion.Categoria = atletaCreateDto.Categoria;
                AtletaFederacion.BecadoEnard = atletaCreateDto.BecadoEnard;
                AtletaFederacion.BecadoSdn = atletaCreateDto.BecadoSdn;
                AtletaFederacion.MontoBeca = atletaCreateDto.MontoBeca;
                AtletaFederacion.PresentoAptoMedico = atletaCreateDto.PresentoAptoMedico;
                AtletaFederacion.FechaAptoMedico = fechaAptoMedicoUtc;

                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AtletaExistsAsync(id))
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

        // -------------------------------------------------
        // DELETE: Eliminar AtletaFederacion
        // -------------------------------------------------
        public async Task<IActionResult> DeleteAtleta(int id)
        {
            try
            {
                var AtletaFederacion = await _context.AtletasFederados
                    .Include(a => a.Participante)
                    .Include(a => a.Tutores)
                    .Include(a => a.Inscripciones)
                    .FirstOrDefaultAsync(a => a.ParticipanteId == id);

                if (AtletaFederacion == null)
                {
                    return new NotFoundResult();
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // 1?? Eliminar relaciones AtletaFederacion-TutorFederacion (solo la tabla intermedia)
                    if (AtletaFederacion.Tutores.Any())
                    {
                        _context.AtletasTutores.RemoveRange(AtletaFederacion.Tutores);
                    }

                    // 2?? Eliminar inscripciones del AtletaFederacion
                    if (AtletaFederacion.Inscripciones.Any())
                    {
                        _context.Inscripciones.RemoveRange(AtletaFederacion.Inscripciones);
                    }

                    // 3?? Eliminar el registro de AtletaFederacion
                    _context.AtletasFederados.Remove(AtletaFederacion);

                    // 4?? Verificar si la Participante tiene otros roles antes de borrarla
                    var Participante = AtletaFederacion.Participante;
                    var tieneOtrosRoles = await _context.Usuarios.AnyAsync(u => u.ParticipanteId == id) ||
                                         await _context.Entrenadores.AnyAsync(e => e.ParticipanteId == id) ||
                                         await _context.DelegadosClub.AnyAsync(d => d.IdParticipante == id) ||
                                         await _context.Tutores.AnyAsync(t => t.ParticipanteId == id);

                    // 5?? Eliminar la Participante SOLO si no tiene otros roles
                    if (!tieneOtrosRoles)
                    {
                        // Eliminar pagos asociados a la Participante (si existen)
                        var pagosPersona = await _context.PagosTransacciones
                            .Where(p => p.IdParticipante == id)
                            .ToListAsync();

                        if (pagosPersona.Any())
                        {
                            _context.PagosTransacciones.RemoveRange(pagosPersona);
                        }

                        _context.Participantes.Remove(Participante);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new OkResult();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(500);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        // -------------------------------------------------
        // Métodos auxiliares
        // -------------------------------------------------
        private async Task<bool> AtletaExistsAsync(int id)
        {
            return await _context.AtletasFederados.AnyAsync(e => e.ParticipanteId == id);
        }
    }
}


