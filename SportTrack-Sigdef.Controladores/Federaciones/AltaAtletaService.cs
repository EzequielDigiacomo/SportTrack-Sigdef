using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.DTOs.Participante;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.Enums;
using SIGDEF.DTOs;
using SportTrack_Sigdef.Controladores.Participante.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public class AltaAtletaService : IAltaAtletaService
    {
        private readonly SportTrackDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public AltaAtletaService(SportTrackDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public string NormalizarDocumento(string? documento)
        {
            if (string.IsNullOrWhiteSpace(documento)) return string.Empty;
            return documento.Replace(".", "").Replace(" ", "").Trim();
        }

        public async Task<Entidades.Entidades.Participante?> BuscarPorDocumentoAsync(string documento)
        {
            var normalizado = NormalizarDocumento(documento);
            if (string.IsNullOrEmpty(normalizado)) return null;

            return await _context.Participantes
                .FirstOrDefaultAsync(p => p.Documento != null && p.Documento.Replace(".", "").Replace(" ", "") == normalizado);
        }

        public AltaAtletaParticipanteInput FromPersonaCreateDto(PersonaCreateDto dto, int? idClub = null)
        {
            return new AltaAtletaParticipanteInput
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Documento = NormalizarDocumento(dto.Documento),
                FechaNacimiento = dto.FechaNacimiento,
                SexoId = dto.SexoId > 0 ? dto.SexoId : ResolverSexoIdDesdePersona(dto),
                IdClub = idClub,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion
            };
        }

        public AltaAtletaParticipanteInput FromParticipanteCreateDto(ParticipanteCreateDto dto)
        {
            return new AltaAtletaParticipanteInput
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Documento = NormalizarDocumento(dto.Dni),
                FechaNacimiento = dto.FechaNacimiento,
                SexoId = dto.SexoId > 0 ? dto.SexoId : 1,
                IdClub = dto.ClubId,
                Email = dto.Email,
                CategoriaId = dto.CategoriaId,
                Pais = dto.Pais,
                PagoAfiliacionAlDia = dto.PagoAfiliacionAlDia
            };
        }

        public AltaAtletaFederacionInput FromAtletaCreateDto(AtletaCreateDto dto)
        {
            return new AltaAtletaFederacionInput
            {
                IdClub = dto.IdClub,
                IdFederacion = dto.IdFederacion,
                EstadoPago = dto.EstadoPago,
                PerteneceSeleccion = dto.PerteneceSeleccion,
                Categoria = dto.Categoria,
                BecadoEnard = dto.BecadoEnard,
                BecadoSdn = dto.BecadoSdn,
                MontoBeca = dto.MontoBeca,
                PresentoAptoMedico = dto.PresentoAptoMedico,
                FechaAptoMedico = dto.FechaAptoMedico
            };
        }

        public AltaAtletaFederacionInput DefaultsFromClub(int? idClub, int? idFederacion = null)
        {
            return new AltaAtletaFederacionInput
            {
                IdClub = idClub,
                IdFederacion = idFederacion,
                EstadoPago = EstadoPago.Pendiente,
                PerteneceSeleccion = false,
                BecadoEnard = false,
                BecadoSdn = false,
                MontoBeca = 0,
                PresentoAptoMedico = false
            };
        }

        public async Task<AltaAtletaResult> UpsertParticipanteAsync(AltaAtletaParticipanteInput input)
        {
            var documento = NormalizarDocumento(input.Documento);
            if (string.IsNullOrEmpty(documento))
                throw new ArgumentException("El documento es obligatorio.");

            var fechaUtc = DateTime.SpecifyKind(input.FechaNacimiento, DateTimeKind.Utc);
            var existente = await BuscarPorDocumentoAsync(documento);
            var creado = false;

            if (existente == null)
            {
                existente = new Entidades.Entidades.Participante
                {
                    Nombre = input.Nombre,
                    Apellido = input.Apellido,
                    Documento = documento,
                    FechaNacimiento = fechaUtc,
                    SexoId = input.SexoId > 0 ? input.SexoId : 1,
                    IdClub = input.IdClub,
                    Email = input.Email ?? string.Empty,
                    Telefono = input.Telefono ?? string.Empty,
                    Direccion = input.Direccion ?? string.Empty,
                    CategoriaId = input.CategoriaId,
                    Pais = input.Pais,
                    PagoAfiliacionAlDia = input.PagoAfiliacionAlDia
                };

                if (!existente.CategoriaId.HasValue)
                {
                    existente.CategoriaId = await ResolverCategoriaPorEdadAsync(fechaUtc);
                }

                _context.Participantes.Add(existente);
                creado = true;
            }
            else
            {
                existente.Nombre = input.Nombre;
                existente.Apellido = input.Apellido;
                existente.FechaNacimiento = fechaUtc;
                existente.SexoId = input.SexoId > 0 ? input.SexoId : existente.SexoId;
                if (input.IdClub.HasValue) existente.IdClub = input.IdClub;
                if (!string.IsNullOrEmpty(input.Email)) existente.Email = input.Email;
                if (!string.IsNullOrEmpty(input.Telefono)) existente.Telefono = input.Telefono;
                if (!string.IsNullOrEmpty(input.Direccion)) existente.Direccion = input.Direccion;
                if (input.CategoriaId.HasValue) existente.CategoriaId = input.CategoriaId;
                if (!string.IsNullOrEmpty(input.Pais)) existente.Pais = input.Pais;
                existente.PagoAfiliacionAlDia = input.PagoAfiliacionAlDia;
            }

            await _context.SaveChangesAsync();

            return new AltaAtletaResult
            {
                ParticipanteId = existente.ParticipanteId,
                Participante = existente,
                ParticipanteCreado = creado
            };
        }

        public async Task<AtletaFederacion> EnsureAtletaFederacionAsync(int participanteId, AltaAtletaFederacionInput input)
        {
            var participante = await _context.Participantes.FindAsync(participanteId)
                ?? throw new InvalidOperationException($"Participante {participanteId} no encontrado.");

            var finalFedId = await ResolverFederacionIdAsync(input.IdFederacion, input.IdClub ?? participante.IdClub);

            DateTime? fechaAptoUtc = input.FechaAptoMedico.HasValue
                ? DateTime.SpecifyKind(input.FechaAptoMedico.Value, DateTimeKind.Utc)
                : null;

            var atleta = await _context.AtletasFederados.FindAsync(participanteId);
            if (atleta == null)
            {
                atleta = new AtletaFederacion
                {
                    ParticipanteId = participanteId,
                    IdClub = input.IdClub ?? participante.IdClub,
                    IdFederacion = finalFedId,
                    EstadoPago = input.EstadoPago,
                    PerteneceSeleccion = input.PerteneceSeleccion,
                    Categoria = input.Categoria,
                    BecadoEnard = input.BecadoEnard,
                    BecadoSdn = input.BecadoSdn,
                    MontoBeca = input.MontoBeca,
                    PresentoAptoMedico = input.PresentoAptoMedico,
                    FechaAptoMedico = fechaAptoUtc
                };
                _context.AtletasFederados.Add(atleta);
            }
            else
            {
                if (input.IdClub.HasValue) atleta.IdClub = input.IdClub;
                else if (participante.IdClub.HasValue) atleta.IdClub = participante.IdClub;

                atleta.IdFederacion = finalFedId;
                atleta.EstadoPago = input.EstadoPago;
                atleta.PerteneceSeleccion = input.PerteneceSeleccion;
                if (input.Categoria.HasValue) atleta.Categoria = input.Categoria;
                atleta.BecadoEnard = input.BecadoEnard;
                atleta.BecadoSdn = input.BecadoSdn;
                atleta.MontoBeca = input.MontoBeca;
                atleta.PresentoAptoMedico = input.PresentoAptoMedico;
                atleta.FechaAptoMedico = fechaAptoUtc;
            }

            // Mantener club sincronizado en Participante
            if (atleta.IdClub.HasValue && participante.IdClub != atleta.IdClub)
            {
                participante.IdClub = atleta.IdClub;
            }

            await _context.SaveChangesAsync();
            return atleta;
        }

        public async Task<AltaAtletaResult> AltaAtletaCompletaAsync(
            AltaAtletaParticipanteInput participanteInput,
            AltaAtletaFederacionInput? federacionInput = null)
        {
            var participanteResult = await UpsertParticipanteAsync(participanteInput);

            var fedInput = federacionInput ?? DefaultsFromClub(
                participanteInput.IdClub ?? participanteResult.Participante.IdClub,
                null);

            if (!fedInput.IdClub.HasValue)
                fedInput.IdClub = participanteResult.Participante.IdClub;

            var atletaExistente = await _context.AtletasFederados.AsNoTracking()
                .AnyAsync(a => a.ParticipanteId == participanteResult.ParticipanteId);
            var atleta = await EnsureAtletaFederacionAsync(participanteResult.ParticipanteId, fedInput);

            participanteResult.AtletaFederacion = atleta;
            participanteResult.AtletaFederacionCreado = !atletaExistente;

            return participanteResult;
        }

        private async Task<int?> ResolverFederacionIdAsync(int? explicitFedId, int? idClub)
        {
            var fedId = _tenantProvider.GetFederacionId() ?? explicitFedId;
            if (fedId.HasValue) return fedId;

            if (idClub.HasValue)
            {
                var club = await _context.Clubes.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.IdClub == idClub.Value);
                return club?.IdFederacion;
            }

            return null;
        }

        private async Task<int?> ResolverCategoriaPorEdadAsync(DateTime fechaNacimiento)
        {
            var edad = DateTime.UtcNow.Year - fechaNacimiento.Year;
            var categoria = await _context.Categorias
                .Where(c => c.Nombre != "Control")
                .FirstOrDefaultAsync(c =>
                    (c.EdadMin == null || edad >= c.EdadMin) &&
                    (c.EdadMax == null || edad <= c.EdadMax));
            return categoria?.Id;
        }

        private static int ResolverSexoIdDesdePersona(PersonaCreateDto dto)
        {
            // El front envía sexo como entero (1=M, 2=F). PersonaCreateDto puede tener Sexo como entidad navegacional.
            try
            {
                if (dto.Sexo != null && dto.Sexo.Id > 0)
                    return dto.Sexo.Id;
            }
            catch
            {
                // ignorar
            }

            return 1;
        }
    }
}
