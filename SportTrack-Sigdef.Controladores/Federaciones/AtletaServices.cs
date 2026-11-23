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
using System;
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
        private readonly IHabilitacionAtletaService _habilitacionService;
        private readonly SportTrack_Sigdef.Controladores.Audit.IAuditService _auditService;

        public AtletaServices(
            SportTrackDbContext context,
            ITenantProvider tenantProvider,
            IAltaAtletaService altaAtletaService,
            IHabilitacionAtletaService habilitacionService,
            SportTrack_Sigdef.Controladores.Audit.IAuditService auditService)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _altaAtletaService = altaAtletaService;
            _habilitacionService = habilitacionService;
            _auditService = auditService;
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
                        FechaVencimientoApto = a.FechaVencimientoApto,
                        FechaCreacion = a.FechaCreacion,
                        EstadoFichaje = a.EstadoFichaje,
                        FechaEnvioFichaje = a.FechaEnvioFichaje,
                        FechaRevision = a.FechaRevision,
                        MotivoRevision = a.MotivoRevision,
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

                await EnrichDetailHabilitacionAsync(AtletasFederados);
                return new OkObjectResult(AtletasFederados);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

        // -------------------------------------------------
        // GET: Obtener atletas federados de un club
        // -------------------------------------------------
        public async Task<ActionResult<IEnumerable<AtletaDetailDto>>> GetAtletasByClub(int clubId)
        {
            try
            {
                if (clubId <= 0)
                {
                    return new BadRequestObjectResult("clubId inválido.");
                }

                // Un usuario de club solo puede consultar su propio club.
                var tenantClubId = _tenantProvider.GetClubId();
                if (tenantClubId.HasValue && tenantClubId.Value != clubId)
                {
                    return new ObjectResult(new { message = "No autorizado a consultar atletas de otro club." })
                    {
                        StatusCode = 403
                    };
                }

                var query = _context.AtletasFederados
                    .AsNoTracking()
                    .Where(a => a.IdClub == clubId);

                var fedId = _tenantProvider.GetFederacionId();
                if (fedId.HasValue)
                {
                    query = query.Where(a => a.IdFederacion == fedId.Value);
                }

                var atletas = await query
                    .Include(a => a.Participante)
                        .ThenInclude(p => p.Categoria)
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
                        FechaVencimientoApto = a.FechaVencimientoApto,
                        FechaCreacion = a.FechaCreacion,
                        EstadoFichaje = a.EstadoFichaje,
                        FechaEnvioFichaje = a.FechaEnvioFichaje,
                        FechaRevision = a.FechaRevision,
                        MotivoRevision = a.MotivoRevision,
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
                            NombreTutor = (at.TutorFederacion != null && at.TutorFederacion.Participante != null)
                                ? at.TutorFederacion.Participante.Nombre + " " + at.TutorFederacion.Participante.Apellido
                                : "TutorFederacion no encontrado"
                        }).ToList()
                    })
                    .ToListAsync();

                await EnrichDetailHabilitacionAsync(atletas);
                return new OkObjectResult(atletas);
            }
            catch (Exception)
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
                    // Tokens independientes: "acosta facu" debe matchear "Facundo Acosta"
                    // (orden y mayúsculas/minúsculas no importan).
                    var tokens = parameters.SearchTerm
                        .Trim()
                        .ToLower()
                        .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var token in tokens)
                    {
                        var t = token;
                        query = query.Where(a =>
                            a.Participante.Nombre.ToLower().Contains(t) ||
                            a.Participante.Apellido.ToLower().Contains(t) ||
                            a.Participante.Dni.ToLower().Contains(t) ||
                            (a.Club != null && a.Club.Nombre.ToLower().Contains(t))
                        );
                    }
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
                        CantidadDocumentos = a.Participante.Documentacion.Count(),
                        Edad = (DateTime.UtcNow.Year - a.Participante.FechaNacimiento.Year) -
                            (DateTime.UtcNow.DayOfYear < a.Participante.FechaNacimiento.DayOfYear ? 1 : 0),
                        EstadoFichaje = a.EstadoFichaje,
                        PresentoAptoMedico = a.PresentoAptoMedico,
                        FechaVencimientoApto = a.FechaVencimientoApto,
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

                await EnrichListHabilitacionAsync(pagedData);

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
                        FechaVencimientoApto = a.FechaVencimientoApto,
                        FechaCreacion = a.FechaCreacion,
                        EstadoFichaje = a.EstadoFichaje,
                        FechaEnvioFichaje = a.FechaEnvioFichaje,
                        FechaRevision = a.FechaRevision,
                        MotivoRevision = a.MotivoRevision,

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

                await EnrichDetailHabilitacionAsync(new List<AtletaDetailDto> { AtletaFederacion });
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
            try
            {
                // NpgsqlRetryingExecutionStrategy no permite BeginTransaction directo.
                var strategy = _context.Database.CreateExecutionStrategy();
                AltaAtletaResult? altaResult = null;

                await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        await EnsureMaxAtletasAsync(dto.DatosDeportivos.IdClub);

                        var participanteInput = _altaAtletaService.FromPersonaCreateDto(dto.PersonaAtleta, dto.DatosDeportivos.IdClub);
                        var fedInput = _altaAtletaService.FromAtletaCreateDto(dto.DatosDeportivos);
                        altaResult = await _altaAtletaService.AltaAtletaCompletaAsync(participanteInput, fedInput);
                        var AtletaFederacion = altaResult.AtletaFederacion;

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
                                throw new InvalidOperationException("Datos del tutor incompletos para registro de menor.");
                            }

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
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });

                if (altaResult == null)
                    return new ObjectResult(new { error = "Fallo el registro atómico" }) { StatusCode = 500 };

                var nombreAtleta = $"{altaResult.Participante.Nombre} {altaResult.Participante.Apellido}".Trim();
                await _auditService.RegistrarAccionAsync(
                    "CREATE_ATHLETE",
                    $"Atleta creado: {nombreAtleta} (DNI: {altaResult.Participante.Dni})",
                    null,
                    "Atletas");

                var response = await GetAtleta(altaResult.ParticipanteId);

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
                        FechaCreacion = detail.FechaCreacion,
                        EstadoFichaje = detail.EstadoFichaje
                    });
                }

                return response.Result ?? new StatusCodeResult(201);
            }
            catch (InvalidOperationException ex)
            {
                return new BadRequestObjectResult(new { error = ex.Message });
            }
            catch (Exception ex)
            {
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

                var rol = _tenantProvider.GetRol();
                var esRolClub = string.Equals(rol, "Club", StringComparison.OrdinalIgnoreCase);
                var clubAnterior = AtletaFederacion.IdClub;
                var cambiaClub = atletaCreateDto.IdClub != clubAnterior;

                if (esRolClub && cambiaClub)
                {
                    return new BadRequestObjectResult(new
                    {
                        error = "Los traspasos de club deben gestionarse mediante POST /api/Traspaso."
                    });
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
                if (atletaCreateDto.FechaVencimientoApto.HasValue)
                {
                    AtletaFederacion.FechaVencimientoApto = DateTime.SpecifyKind(
                        atletaCreateDto.FechaVencimientoApto.Value, DateTimeKind.Utc);
                }

                if (esRolClub && (AtletaFederacion.EstadoFichaje == EstadoFichaje.Observado
                    || AtletaFederacion.EstadoFichaje == EstadoFichaje.Rechazado))
                {
                    AtletaFederacion.EstadoFichaje = EstadoFichaje.PendienteRevision;
                    AtletaFederacion.FechaEnvioFichaje = DateTime.UtcNow;
                    AtletaFederacion.MotivoRevision = null;
                }

                if (cambiaClub)
                {
                    var participante = await _context.Participantes.FindAsync(id);
                    if (participante != null)
                    {
                        participante.IdClub = atletaCreateDto.IdClub;
                    }

                    await _auditService.RegistrarAccionAsync(
                        "TRASPASO_ADMIN_DIRECTO",
                        $"Admin cambió club atleta {id}: {clubAnterior} → {atletaCreateDto.IdClub?.ToString() ?? "Agente Libre"}",
                        null,
                        "Traspasos");
                }

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
        // POST: Liberar atleta (club → Agente Libre)
        // -------------------------------------------------
        public async Task<IActionResult> LiberarAtleta(int id)
        {
            try
            {
                var clubId = _tenantProvider.GetClubId();
                if (!clubId.HasValue)
                {
                    return new UnauthorizedObjectResult(new
                    {
                        error = "Solo un club puede liberar atletas de su plantel."
                    });
                }

                var atleta = await _context.AtletasFederados
                    .FirstOrDefaultAsync(a => a.ParticipanteId == id);
                if (atleta == null)
                {
                    return new NotFoundObjectResult(new { error = "Atleta no encontrado." });
                }

                var participante = await _context.Participantes.FindAsync(id);
                var clubEfectivo = atleta.IdClub ?? participante?.IdClub;
                if (!clubEfectivo.HasValue || clubEfectivo.Value != clubId.Value)
                {
                    return new BadRequestObjectResult(new
                    {
                        error = "Solo puede liberar atletas que pertenecen a su club."
                    });
                }

                var tieneTraspasoActivo = await _context.SolicitudesTraspaso.AnyAsync(s =>
                    s.ParticipanteId == id
                    && (s.Estado == EstadoSolicitudTraspaso.PendienteOrigen
                        || s.Estado == EstadoSolicitudTraspaso.PendienteFederacion));
                if (tieneTraspasoActivo)
                {
                    return new BadRequestObjectResult(new
                    {
                        error = "El atleta tiene una solicitud de traspaso activa. Cancele o resuelva esa solicitud antes de liberarlo."
                    });
                }

                var clubAnterior = clubEfectivo.Value;
                atleta.IdClub = null;
                if (participante != null)
                    participante.IdClub = null;

                await _context.SaveChangesAsync();

                await _auditService.RegistrarAccionAsync(
                    "LIBERAR_ATLETA",
                    $"Club {clubAnterior} liberó atleta {id} (Agente Libre)",
                    null,
                    "Traspasos");

                return new OkObjectResult(new
                {
                    message = "Atleta liberado. Quedó como Agente Libre; otro club puede solicitar el traspaso.",
                    participanteId = id
                });
            }
            catch (Exception)
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

                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        if (AtletaFederacion.Tutores.Any())
                        {
                            _context.AtletasTutores.RemoveRange(AtletaFederacion.Tutores);
                        }

                        if (AtletaFederacion.Inscripciones.Any())
                        {
                            _context.Inscripciones.RemoveRange(AtletaFederacion.Inscripciones);
                        }

                        _context.AtletasFederados.Remove(AtletaFederacion);

                        var Participante = AtletaFederacion.Participante;
                        var tieneOtrosRoles = await _context.Usuarios.AnyAsync(u => u.ParticipanteId == id) ||
                                             await _context.Entrenadores.AnyAsync(e => e.ParticipanteId == id) ||
                                             await _context.DelegadosClub.AnyAsync(d => d.IdParticipante == id) ||
                                             await _context.Tutores.AnyAsync(t => t.ParticipanteId == id);

                        if (!tieneOtrosRoles)
                        {
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
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });

                return new OkResult();
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
        private async Task EnsureMaxAtletasAsync(int? clubId)
        {
            int? federacionId = _tenantProvider.GetFederacionId();
            PlanSaaS? plan = null;

            if (clubId.HasValue)
            {
                var club = await _context.Clubes
                    .AsNoTracking()
                    .Include(c => c.Federacion!)
                        .ThenInclude(f => f.PlanSaaS)
                    .Include(c => c.PlanSaaS)
                    .FirstOrDefaultAsync(c => c.IdClub == clubId.Value);

                federacionId ??= club?.IdFederacion;
                plan = club?.Federacion?.PlanSaaS ?? club?.PlanSaaS;
            }

            if (plan == null && federacionId.HasValue)
            {
                var fed = await _context.Federaciones
                    .AsNoTracking()
                    .Include(f => f.PlanSaaS)
                    .FirstOrDefaultAsync(f => f.IdFederacion == federacionId.Value);
                plan = fed?.PlanSaaS;
            }

            if (plan == null || plan.MaxAtletas == -1) return;

            int count;
            if (federacionId.HasValue)
            {
                var clubIds = await _context.Clubes
                    .Where(c => c.IdFederacion == federacionId.Value)
                    .Select(c => c.IdClub)
                    .ToListAsync();
                count = await _context.Participantes
                    .CountAsync(p => p.IdClub.HasValue && clubIds.Contains(p.IdClub.Value));
            }
            else if (clubId.HasValue)
            {
                count = await _context.Participantes.CountAsync(p => p.IdClub == clubId.Value);
            }
            else return;

            if (count >= plan.MaxAtletas)
            {
                throw new InvalidOperationException(
                    $"La federación alcanzó el límite de {plan.MaxAtletas} atletas del plan {plan.Nombre}.");
            }
        }

        public async Task<ActionResult<IEnumerable<AtletaDetailDto>>> GetFichajesPendientes(EstadoFichaje? estado = null)
        {
            try
            {
                var query = _context.AtletasFederados.AsQueryable();
                var fedId = _tenantProvider.GetFederacionId();
                if (fedId.HasValue)
                    query = query.Where(a => a.IdFederacion == fedId.Value);

                var clubId = _tenantProvider.GetClubId();
                if (clubId.HasValue)
                    query = query.Where(a => a.IdClub == clubId.Value);

                if (estado.HasValue)
                    query = query.Where(a => a.EstadoFichaje == estado.Value);
                else
                    query = query.Where(a => a.EstadoFichaje == EstadoFichaje.PendienteRevision
                        || a.EstadoFichaje == EstadoFichaje.Observado);

                var list = await query
                    .AsNoTracking()
                    .Include(a => a.Participante)
                    .Include(a => a.Club)
                    .OrderByDescending(a => a.FechaEnvioFichaje ?? a.FechaCreacion)
                    .Select(a => new AtletaDetailDto
                    {
                        ParticipanteId = a.ParticipanteId,
                        IdClub = a.IdClub,
                        EstadoPago = a.EstadoPago,
                        PerteneceSeleccion = a.PerteneceSeleccion,
                        Categoria = a.Categoria,
                        PresentoAptoMedico = a.PresentoAptoMedico,
                        FechaAptoMedico = a.FechaAptoMedico,
                        FechaVencimientoApto = a.FechaVencimientoApto,
                        FechaCreacion = a.FechaCreacion,
                        EstadoFichaje = a.EstadoFichaje,
                        FechaEnvioFichaje = a.FechaEnvioFichaje,
                        FechaRevision = a.FechaRevision,
                        MotivoRevision = a.MotivoRevision,
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
                        } : null
                    })
                    .ToListAsync();

                await EnrichDetailHabilitacionAsync(list);
                return new OkObjectResult(list);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> AprobarFichaje(int id)
        {
            try
            {
                if (!EsRolFederacion())
                    return new UnauthorizedObjectResult(new { error = "Solo la federación puede aprobar fichajes." });

                var atleta = await _context.AtletasFederados
                    .Include(a => a.Participante)
                    .FirstOrDefaultAsync(a => a.ParticipanteId == id);
                if (atleta == null)
                    return new NotFoundObjectResult(new { error = "Atleta no encontrado." });

                // La documentación/apto condicionan la habilitación para competir,
                // no bloquean la aprobación del alta (solicitud de fichaje).
                var docs = await _context.DocumentacionPersonas
                    .Where(d => d.PersonaId == id)
                    .ToListAsync();
                var faltantes = _habilitacionService.ChecklistDocsFaltantes(atleta, docs);
                var estadoApto = _habilitacionService.EvaluarEstadoApto(atleta, docs);

                await CambiarEstadoFichajeAsync(atleta, EstadoFichaje.Aprobado, null);

                var incompleto = faltantes.Count > 0 || estadoApto == "Faltante";
                return new OkObjectResult(new
                {
                    message = incompleto
                        ? "Alta aprobada. Atención: aún faltan requisitos para habilitación."
                        : "Fichaje aprobado.",
                    participanteId = id,
                    estadoFichaje = EstadoFichaje.Aprobado,
                    docsFaltantes = faltantes,
                    estadoApto
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> ObservarFichaje(int id, FichajeRevisionRequestDto dto)
        {
            try
            {
                if (!EsRolFederacion())
                    return new UnauthorizedObjectResult(new { error = "Solo la federación puede observar fichajes." });

                if (string.IsNullOrWhiteSpace(dto?.Motivo))
                    return new BadRequestObjectResult(new { error = "El motivo es obligatorio al observar." });

                var atleta = await _context.AtletasFederados.FindAsync(id);
                if (atleta == null)
                    return new NotFoundObjectResult(new { error = "Atleta no encontrado." });

                await CambiarEstadoFichajeAsync(atleta, EstadoFichaje.Observado, dto.Motivo.Trim());
                return new OkObjectResult(new { message = "Fichaje observado.", participanteId = id, estadoFichaje = EstadoFichaje.Observado });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> RechazarFichaje(int id, FichajeRevisionRequestDto dto)
        {
            try
            {
                if (!EsRolFederacion())
                    return new UnauthorizedObjectResult(new { error = "Solo la federación puede rechazar fichajes." });

                if (string.IsNullOrWhiteSpace(dto?.Motivo))
                    return new BadRequestObjectResult(new { error = "El motivo es obligatorio al rechazar." });

                var atleta = await _context.AtletasFederados.FindAsync(id);
                if (atleta == null)
                    return new NotFoundObjectResult(new { error = "Atleta no encontrado." });

                await CambiarEstadoFichajeAsync(atleta, EstadoFichaje.Rechazado, dto.Motivo.Trim());
                return new OkObjectResult(new { message = "Fichaje rechazado.", participanteId = id, estadoFichaje = EstadoFichaje.Rechazado });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        public async Task<ActionResult<IEnumerable<FichajeHistorialDto>>> GetHistorialFichaje(int id)
        {
            var items = await _context.Set<FichajeRevisionHistorial>()
                .AsNoTracking()
                .Where(h => h.ParticipanteId == id)
                .OrderByDescending(h => h.Fecha)
                .Select(h => new FichajeHistorialDto
                {
                    Id = h.Id,
                    ParticipanteId = h.ParticipanteId,
                    EstadoAnterior = h.EstadoAnterior,
                    EstadoNuevo = h.EstadoNuevo,
                    Motivo = h.Motivo,
                    RevisadoPorUsername = h.RevisadoPorUsername,
                    Fecha = h.Fecha
                })
                .ToListAsync();

            return new OkObjectResult(items);
        }

        public async Task<ActionResult<IEnumerable<PadronAtletaDto>>> GetPadron(PadronFiltroDto filtro)
        {
            try
            {
                filtro ??= new PadronFiltroDto();
                var query = _context.AtletasFederados.AsNoTracking()
                    .Include(a => a.Participante)
                    .Include(a => a.Club)
                    .AsQueryable();

                var fedId = _tenantProvider.GetFederacionId();
                if (fedId.HasValue)
                    query = query.Where(a => a.IdFederacion == fedId.Value);

                var clubScope = _tenantProvider.GetClubId();
                if (clubScope.HasValue)
                    query = query.Where(a => a.IdClub == clubScope.Value);
                else if (filtro.ClubId.HasValue)
                    query = query.Where(a => a.IdClub == filtro.ClubId.Value);

                if (filtro.Categoria.HasValue)
                    query = query.Where(a => a.Categoria == filtro.Categoria.Value);

                if (filtro.EstadoFichaje.HasValue)
                    query = query.Where(a => a.EstadoFichaje == filtro.EstadoFichaje.Value);

                if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
                {
                    var tokens = filtro.Busqueda
                        .Trim()
                        .ToLower()
                        .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var token in tokens)
                    {
                        var t = token;
                        query = query.Where(a =>
                            a.Participante.Nombre.ToLower().Contains(t)
                            || a.Participante.Apellido.ToLower().Contains(t)
                            || a.Participante.Dni.ToLower().Contains(t)
                            || (a.Club != null && a.Club.Nombre.ToLower().Contains(t)));
                    }
                }

                var atletas = await query.OrderBy(a => a.Participante.Apellido).ThenBy(a => a.Participante.Nombre).ToListAsync();
                var ids = atletas.Select(a => a.ParticipanteId).ToList();
                var docs = await _context.DocumentacionPersonas.AsNoTracking()
                    .Where(d => d.PersonaId.HasValue && ids.Contains(d.PersonaId.Value))
                    .ToListAsync();
                var docsByPersona = docs.GroupBy(d => d.PersonaId!.Value).ToDictionary(g => g.Key, g => g.ToList());

                var result = new List<PadronAtletaDto>();
                foreach (var a in atletas)
                {
                    docsByPersona.TryGetValue(a.ParticipanteId, out var docsAtleta);
                    docsAtleta ??= new List<DocumentacionFederacionPersona>();
                    var hab = _habilitacionService.Evaluar(a, docsAtleta);
                    if (filtro.SoloHabilitados == true && !hab.EstaHabilitado)
                        continue;

                    var edad = DateTime.UtcNow.Year - a.Participante.FechaNacimiento.Year;
                    if (DateTime.UtcNow.DayOfYear < a.Participante.FechaNacimiento.DayOfYear) edad--;

                    result.Add(new PadronAtletaDto
                    {
                        ParticipanteId = a.ParticipanteId,
                        NombrePersona = $"{a.Participante.Nombre} {a.Participante.Apellido}".Trim(),
                        Documento = a.Participante.Dni,
                        FechaNacimiento = a.Participante.FechaNacimiento,
                        Edad = edad,
                        IdClub = a.IdClub,
                        NombreClub = a.Club?.Nombre ?? "Agente Libre",
                        Categoria = a.Categoria,
                        EstadoPago = a.EstadoPago,
                        EstadoFichaje = a.EstadoFichaje,
                        PresentoAptoMedico = a.PresentoAptoMedico,
                        FechaVencimientoApto = a.FechaVencimientoApto,
                        EstadoApto = hab.EstadoApto,
                        EstaHabilitado = hab.EstaHabilitado,
                        EstadoHabilitacion = hab.EstadoHabilitacion,
                        MotivosBloqueo = hab.MotivosBloqueo,
                        DocsFaltantes = hab.DocsFaltantes
                    });
                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        public async Task<ActionResult<ReporteAdminResumenDto>> GetReporteResumen()
        {
            try
            {
                var query = _context.AtletasFederados.AsNoTracking()
                    .Include(a => a.Participante)
                    .AsQueryable();

                var fedId = _tenantProvider.GetFederacionId();
                if (fedId.HasValue)
                    query = query.Where(a => a.IdFederacion == fedId.Value);

                var clubId = _tenantProvider.GetClubId();
                if (clubId.HasValue)
                    query = query.Where(a => a.IdClub == clubId.Value);

                var atletas = await query.ToListAsync();
                var ids = atletas.Select(a => a.ParticipanteId).ToList();
                var docs = await _context.DocumentacionPersonas.AsNoTracking()
                    .Where(d => d.PersonaId.HasValue && ids.Contains(d.PersonaId.Value))
                    .ToListAsync();
                var docsByPersona = docs.GroupBy(d => d.PersonaId!.Value).ToDictionary(g => g.Key, g => g.ToList());

                var resumen = new ReporteAdminResumenDto { TotalAtletas = atletas.Count };
                foreach (var a in atletas)
                {
                    if (a.EstadoFichaje == EstadoFichaje.PendienteRevision) resumen.PendientesFichaje++;
                    if (a.EstadoFichaje == EstadoFichaje.Observado) resumen.Observados++;
                    if (a.EstadoPago == EstadoPago.Pendiente || a.EstadoPago == EstadoPago.Vencido || a.EstadoPago == EstadoPago.Parcial)
                        resumen.Deudores++;

                    docsByPersona.TryGetValue(a.ParticipanteId, out var docsAtleta);
                    docsAtleta ??= new List<DocumentacionFederacionPersona>();
                    var hab = _habilitacionService.Evaluar(a, docsAtleta);
                    if (hab.EstaHabilitado) resumen.Habilitados++;
                    else resumen.NoHabilitados++;

                    switch (hab.EstadoApto)
                    {
                        case "PorVencer": resumen.AptosPorVencer++; break;
                        case "Vencido": resumen.AptosVencidos++; break;
                        case "Faltante": resumen.AptosFaltantes++; break;
                    }

                    if (!hab.ChecklistDocsOk) resumen.DocsIncompletos++;
                }

                return new OkObjectResult(resumen);
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { error = ex.Message }) { StatusCode = 500 };
            }
        }

        private async Task CambiarEstadoFichajeAsync(AtletaFederacion atleta, EstadoFichaje nuevo, string? motivo)
        {
            var anterior = atleta.EstadoFichaje;
            atleta.EstadoFichaje = nuevo;
            atleta.FechaRevision = DateTime.UtcNow;
            atleta.MotivoRevision = motivo;
            atleta.RevisadoPorUserId = GetCurrentUserId();

            _context.Set<FichajeRevisionHistorial>().Add(new FichajeRevisionHistorial
            {
                ParticipanteId = atleta.ParticipanteId,
                EstadoAnterior = anterior,
                EstadoNuevo = nuevo,
                Motivo = motivo,
                RevisadoPorUserId = atleta.RevisadoPorUserId,
                RevisadoPorUsername = _tenantProvider.GetUser()?.Identity?.Name,
                Fecha = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            await _auditService.RegistrarAccionAsync(
                $"FICHAJE_{nuevo.ToString().ToUpperInvariant()}",
                $"Fichaje atleta {atleta.ParticipanteId}: {anterior} → {nuevo}" + (string.IsNullOrWhiteSpace(motivo) ? "" : $" ({motivo})"),
                null,
                "Fichajes");
        }

        private async Task EnrichDetailHabilitacionAsync(List<AtletaDetailDto> list)
        {
            if (list.Count == 0) return;
            var ids = list.Select(a => a.ParticipanteId).ToList();
            var docs = await _context.DocumentacionPersonas.AsNoTracking()
                .Where(d => d.PersonaId.HasValue && ids.Contains(d.PersonaId.Value))
                .ToListAsync();
            var byPersona = docs.GroupBy(d => d.PersonaId!.Value).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var dto in list)
            {
                byPersona.TryGetValue(dto.ParticipanteId, out var docsAtleta);
                docsAtleta ??= new List<DocumentacionFederacionPersona>();
                var proxy = new AtletaFederacion
                {
                    ParticipanteId = dto.ParticipanteId,
                    EstadoFichaje = dto.EstadoFichaje,
                    EstadoPago = dto.EstadoPago,
                    PresentoAptoMedico = dto.PresentoAptoMedico,
                    FechaAptoMedico = dto.FechaAptoMedico,
                    FechaVencimientoApto = dto.FechaVencimientoApto,
                    Participante = dto.Participante == null ? null! : new Entidades.Entidades.Participante
                    {
                        ParticipanteId = dto.ParticipanteId,
                        FechaNacimiento = dto.Participante.FechaNacimiento
                    }
                };
                var hab = _habilitacionService.Evaluar(proxy, docsAtleta);
                dto.EstaHabilitado = hab.EstaHabilitado;
                dto.EstadoHabilitacion = hab.EstadoHabilitacion;
                dto.EstadoApto = hab.EstadoApto;
                dto.MotivosBloqueo = hab.MotivosBloqueo;
                dto.DocsFaltantes = hab.DocsFaltantes;
            }
        }

        private async Task EnrichListHabilitacionAsync(List<AtletaListDto> list)
        {
            if (list.Count == 0) return;
            var ids = list.Select(a => a.ParticipanteId).ToList();
            var docs = await _context.DocumentacionPersonas.AsNoTracking()
                .Where(d => d.PersonaId.HasValue && ids.Contains(d.PersonaId.Value))
                .ToListAsync();
            var byPersona = docs.GroupBy(d => d.PersonaId!.Value).ToDictionary(g => g.Key, g => g.ToList());
            var fechas = await _context.AtletasFederados.AsNoTracking()
                .Where(a => ids.Contains(a.ParticipanteId))
                .Select(a => new { a.ParticipanteId, a.Participante.FechaNacimiento })
                .ToListAsync();
            var fechaById = fechas.ToDictionary(x => x.ParticipanteId, x => x.FechaNacimiento);

            foreach (var dto in list)
            {
                byPersona.TryGetValue(dto.ParticipanteId, out var docsAtleta);
                docsAtleta ??= new List<DocumentacionFederacionPersona>();
                fechaById.TryGetValue(dto.ParticipanteId, out var fechaNac);
                var proxy = new AtletaFederacion
                {
                    ParticipanteId = dto.ParticipanteId,
                    EstadoFichaje = dto.EstadoFichaje,
                    EstadoPago = dto.EstadoPago,
                    PresentoAptoMedico = dto.PresentoAptoMedico,
                    FechaVencimientoApto = dto.FechaVencimientoApto,
                    Participante = new Entidades.Entidades.Participante { ParticipanteId = dto.ParticipanteId, FechaNacimiento = fechaNac }
                };
                var hab = _habilitacionService.Evaluar(proxy, docsAtleta);
                dto.EstaHabilitado = hab.EstaHabilitado;
                dto.EstadoHabilitacion = hab.EstadoHabilitacion;
                dto.EstadoApto = hab.EstadoApto;
                dto.MotivosBloqueo = hab.MotivosBloqueo;
            }
        }

        private bool EsRolFederacion()
        {
            var rol = _tenantProvider.GetRol();
            return string.Equals(rol, "Admin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(rol, "Federacion", StringComparison.OrdinalIgnoreCase)
                || string.Equals(rol, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(rol, "soporte_tecnico", StringComparison.OrdinalIgnoreCase);
        }

        private int? GetCurrentUserId()
        {
            var user = _tenantProvider.GetUser();
            var claim = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? user?.FindFirst("sub")?.Value
                ?? user?.FindFirst("UserId")?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }

        private async Task<bool> AtletaExistsAsync(int id)
        {
            return await _context.AtletasFederados.AnyAsync(e => e.ParticipanteId == id);
        }
    }
}


