using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.DTOs.Participante;
using SportTrack_Sigdef.Entidades.DTOs.Usuario;
using SportTrack_Sigdef.Entidades.DTOs.DelegadoFederacionClub;
using SportTrack_Sigdef.Entidades.DTOs.EntrenadorFederacion;
using SportTrack_Sigdef.Entidades.DTOs.TutorFederacion;
using SportTrack_Sigdef.Entidades.DTOs.AtletaFederacion;
using SportTrack_Sigdef.Entidades.DTOs.PagoFederacionTransaccion;
using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportTrack_Sigdef.Controladores.Federaciones;
using Microsoft.AspNetCore.Mvc;
using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.Services
{
    public class PersonaServices : IPersonaServices
    {
        private readonly SportTrackDbContext _context;
        private readonly IAltaAtletaService _altaAtletaService;

        public PersonaServices(SportTrackDbContext context, IAltaAtletaService altaAtletaService)
        {
            _context = context;
            _altaAtletaService = altaAtletaService;
        }

        public async Task<ActionResult<IEnumerable<PersonaDto>>> GetPersonas()
        {
            try
            {
                var Participantes = await _context.Participantes
                    .Select(p => new PersonaDto
                    {
                        ParticipanteId = p.ParticipanteId,
                        Nombre = p.Nombre,
                        Apellido = p.Apellido,
                        Documento = p.Dni,
                        FechaNacimiento = p.FechaNacimiento,
                        Email = p.Email,
                        Telefono = p.Telefono,
                        Direccion = p.Direccion,
                        Sexo = p.Sexo,
                        SexoDisplay = p.Sexo.ToString(),
                        Edad = CalcularEdad(p.FechaNacimiento),
                        NombreCompleto = p.Nombre + " " + p.Apellido,
                        TipoPersona = GetTipoPersona(p)
                    })
                    .ToListAsync();

                return new OkObjectResult(Participantes);
            }
            catch (Exception ex)
            {
                return new ObjectResult(new
                {
                    error = ex.Message,
                    innerError = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                })
                {
                    StatusCode = 500
                };
            }
        }

        public async Task<ActionResult<PersonaDetailDto>> GetPersona(int id)
        {
            try
            {
                var participante = await _context.Participantes
                    .Include(p => p.Usuario)
                    .Include(p => p.DelegadoFederacionClub)
                    .Include(p => p.EntrenadorFederacion)
                    .Include(p => p.TutorFederacion)
                    .Include(p => p.AtletaFederacion)
                    .Include(p => p.Pagos)
                    .Where(p => p.ParticipanteId == id)
                    .Select(p => new PersonaDetailDto
                    {
                        ParticipanteId = p.ParticipanteId,
                        Nombre = p.Nombre,
                        Apellido = p.Apellido,
                        Documento = p.Dni,
                        FechaNacimiento = p.FechaNacimiento,
                        Email = p.Email,
                        Telefono = p.Telefono,
                        Direccion = p.Direccion,
                        Sexo = p.Sexo,
                        SexoDisplay = p.Sexo.ToString(),
                        Usuario = p.Usuario != null ? new UsuarioDto
                        {
                            ParticipanteId = p.Usuario.ParticipanteId,
                            Username = p.Usuario.Username,
                            EstaActivo = p.Usuario.EstaActivo,
                            FechaCreacion = p.Usuario.FechaCreacion,
                            UltimoAcceso = p.Usuario.FechaCreacion
                        } : null,
                        DelegadoFederacionClub = p.DelegadoFederacionClub != null ? new DelegadoClubDto
                        {
                            ParticipanteId = p.DelegadoFederacionClub.IdParticipante ?? 0,
                            IdRol = p.DelegadoFederacionClub.IdRol,
                            IdFederacion = p.DelegadoFederacionClub.IdFederacion
                        } : null,
                        EntrenadorFederacion = p.EntrenadorFederacion != null ? new EntrenadorDto
                        {
                            ParticipanteId = p.EntrenadorFederacion.ParticipanteId,
                            IdClub = p.EntrenadorFederacion.IdClub ?? 0,
                            PerteneceSeleccion = p.EntrenadorFederacion.PerteneceSeleccion == true,
                            CategoriaSeleccion = p.EntrenadorFederacion.CategoriaSeleccion,
                            BecadoEnard = p.EntrenadorFederacion.BecadoEnard == true,
                            BecadoSdn = p.EntrenadorFederacion.BecadoSdn == true,
                            MontoBeca = p.EntrenadorFederacion.MontoBeca ?? 0,
                            PresentoAptoMedico = p.EntrenadorFederacion.PresentoAptoMedico == true
                        } : null,
                        TutorFederacion = p.TutorFederacion != null ? new TutorDto
                        {
                            ParticipanteId = p.TutorFederacion.ParticipanteId,
                            TipoTutor = p.TutorFederacion.TipoTutor
                        } : null,
                        AtletaFederacion = p.AtletaFederacion != null ? new AtletaDto
                        {
                            ParticipanteId = p.AtletaFederacion.ParticipanteId,
                            IdClub = p.AtletaFederacion.IdClub,
                            EstadoPago = p.AtletaFederacion.EstadoPago,
                            PerteneceSeleccion = p.AtletaFederacion.PerteneceSeleccion,
                            Categoria = p.AtletaFederacion.Categoria,
                            BecadoEnard = p.AtletaFederacion.BecadoEnard,
                            BecadoSdn = p.AtletaFederacion.BecadoSdn,
                            MontoBeca = p.AtletaFederacion.MontoBeca,
                            PresentoAptoMedico = p.AtletaFederacion.PresentoAptoMedico,
                            FechaAptoMedico = p.AtletaFederacion.FechaAptoMedico
                        } : null,
                        Pagos = p.Pagos.Select(pa => new PagoTransaccionDto
                        {
                            IdPago = pa.IdPago,
                            Concepto = pa.Concepto,
                            Monto = pa.Monto,
                            Estado = pa.Estado,
                            FechaCreacion = pa.FechaCreacion,
                            FechaAprobacion = pa.FechaAprobacion,
                            ParticipanteId = pa.IdParticipante,
                            IdClub = pa.IdClub,
                            IdMercadoPago = pa.IdMercadoPago
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (participante == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(participante);
            }
            catch (Exception ex) { return new ObjectResult(new { error = ex.Message, inner = ex.InnerException?.Message }) { StatusCode = 500 }; }
        }

        public async Task<ActionResult<PersonaDto>> GetPersonaByDocumento(string documento)
        {
            try
            {
                var p = await _context.Participantes
                    .Include(x => x.Usuario)
                    .Include(x => x.DelegadoFederacionClub)
                    .Include(x => x.EntrenadorFederacion)
                    .Include(x => x.TutorFederacion)
                    .Include(x => x.AtletaFederacion)
                    .Where(x => x.Dni == documento)
                    .FirstOrDefaultAsync();

                if (p == null)
                {
                    return new NotFoundResult();
                }

                var personaDto = new PersonaDto
                {
                    ParticipanteId = p.ParticipanteId,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Documento = p.Dni,
                    FechaNacimiento = p.FechaNacimiento,
                    Email = p.Email,
                    Telefono = p.Telefono,
                    Direccion = p.Direccion,
                    Sexo = p.Sexo,
                    SexoDisplay = p.Sexo.ToString(),
                    Edad = CalcularEdad(p.FechaNacimiento),
                    NombreCompleto = p.Nombre + " " + p.Apellido,
                    TipoPersona = GetTipoPersona(p)
                };

                return new OkObjectResult(personaDto);
            }
            catch (Exception ex) { return new ObjectResult(new { error = ex.Message, inner = ex.InnerException?.Message }) { StatusCode = 500 }; }
        }

        public async Task<ActionResult<PersonaDto>> PostPersona(PersonaCreateDto personaCreateDto)
        {
            try
            {
                var fechaNacimientoUtc = DateTime.SpecifyKind(personaCreateDto.FechaNacimiento, DateTimeKind.Utc);

                if (fechaNacimientoUtc > DateTime.UtcNow)
                {
                    return new BadRequestResult();
                }

                var edadMinima = DateTime.UtcNow.AddYears(-5);
                if (fechaNacimientoUtc > edadMinima)
                {
                    return new BadRequestResult();
                }

                var input = _altaAtletaService.FromPersonaCreateDto(personaCreateDto);
                var upsert = await _altaAtletaService.UpsertParticipanteAsync(input);
                var participante = await _context.Participantes
                    .Include(p => p.Sexo)
                    .FirstAsync(p => p.ParticipanteId == upsert.ParticipanteId);

                var personaDto = new PersonaDto
                {
                    ParticipanteId = participante.ParticipanteId,
                    Nombre = participante.Nombre,
                    Apellido = participante.Apellido,
                    Documento = participante.Dni,
                    FechaNacimiento = participante.FechaNacimiento,
                    Email = participante.Email,
                    Telefono = participante.Telefono,
                    Direccion = participante.Direccion,
                    Sexo = participante.Sexo,
                    SexoDisplay = participante.Sexo?.ToString() ?? string.Empty,
                    Edad = CalcularEdad(participante.FechaNacimiento),
                    NombreCompleto = participante.Nombre + " " + participante.Apellido,
                    TipoPersona = "Participante Base"
                };

                var result = new ObjectResult(personaDto)
                {
                    StatusCode = upsert.ParticipanteCreado ? 201 : 200
                };
                return result;
            }
            catch (DbUpdateException dbEx) { return new ObjectResult(new { error = "Error de base de datos", detail = dbEx.Message, inner = dbEx.InnerException?.Message }) { StatusCode = 500 }; }
            catch (Exception ex) { return new ObjectResult(new { error = ex.Message, inner = ex.InnerException?.Message }) { StatusCode = 500 }; }
        }

        public async Task<IActionResult> PutPersona(int id, PersonaCreateDto personaCreateDto)
        {
            try
            {
                var participante = await _context.Participantes.FindAsync(id);
                if (participante == null)
                {
                    return new NotFoundResult();
                }

                var documentoExists = await _context.Participantes
                    .AnyAsync(p => p.Dni == personaCreateDto.Documento && p.ParticipanteId != id);
                if (documentoExists)
                {
                    return new BadRequestResult();
                }

                var fechaNacimientoUtc = DateTime.SpecifyKind(personaCreateDto.FechaNacimiento, DateTimeKind.Utc);

                if (fechaNacimientoUtc > DateTime.UtcNow)
                {
                    return new BadRequestResult();
                }

                participante.Nombre = personaCreateDto.Nombre;
                participante.Apellido = personaCreateDto.Apellido;
                participante.Dni = _altaAtletaService.NormalizarDocumento(personaCreateDto.Documento);
                participante.FechaNacimiento = fechaNacimientoUtc;
                participante.Email = personaCreateDto.Email ?? string.Empty;
                participante.Telefono = personaCreateDto.Telefono ?? string.Empty;
                participante.Direccion = personaCreateDto.Direccion ?? string.Empty;
                participante.SexoId = personaCreateDto.SexoId > 0 ? personaCreateDto.SexoId : participante.SexoId;

                await _context.SaveChangesAsync();
                return new NoContentResult();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PersonaExistsAsync(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex) { return new ObjectResult(new { error = ex.Message, inner = ex.InnerException?.Message }) { StatusCode = 500 }; }
        }

        public async Task<IActionResult> DeletePersona(int id)
        {
            try
            {
                var participante = await _context.Participantes.FindAsync(id);
                if (participante == null)
                {
                    return new NotFoundResult();
                }

                _context.Participantes.Remove(participante);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (DbUpdateException dbEx) { return new ObjectResult(new { error = "Error de base de datos", detail = dbEx.Message, inner = dbEx.InnerException?.Message }) { StatusCode = 500 }; }
            catch (Exception ex) { return new ObjectResult(new { error = ex.Message, inner = ex.InnerException?.Message }) { StatusCode = 500 }; }
        }

        private async Task<bool> PersonaExistsAsync(int id)
        {
            return await _context.Participantes.AnyAsync(e => e.ParticipanteId == id);
        }

        private static int CalcularEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
            return edad;
        }

        private static string GetTipoPersona(SportTrack_Sigdef.Entidades.Entidades.Participante Participante)
        {
            if (Participante.AtletaFederacion != null) return "AtletaFederacion";
            if (Participante.EntrenadorFederacion != null) return "EntrenadorFederacion";
            if (Participante.TutorFederacion != null) return "TutorFederacion";
            if (Participante.DelegadoFederacionClub != null) return "DelegadoFederacionClub";
            return "Participante Base";
        }
    }
}


