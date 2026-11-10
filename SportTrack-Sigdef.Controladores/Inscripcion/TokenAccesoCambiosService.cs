using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Audit;
using SportTrack_Sigdef.Controladores.Exceptions;
using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;
using SportTrack_Sigdef.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Inscripcion
{
    public class TokenAccesoCambiosService : ITokenAccesoCambiosService
    {
        private readonly SportTrackDbContext _context;
        private readonly IInscripcionService _inscripcionService;
        private readonly IAuditService _auditService;

        public TokenAccesoCambiosService(
            SportTrackDbContext context,
            IInscripcionService inscripcionService,
            IAuditService auditService)
        {
            _context = context;
            _inscripcionService = inscripcionService;
            _auditService = auditService;
        }

        public async Task<IEnumerable<TokenAccesoCambiosDto>> GenerarAsync(
            int eventoId,
            TokenAccesoCambiosCreateDto dto,
            string? creadoPor)
        {
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.IdEvento == eventoId)
                ?? throw new NotFoundException($"Evento {eventoId} no encontrado");

            var expira = dto.ExpiraEn
                ?? EventoCambiosPolicy.ResolveDeadline(evento)
                ?? evento.Fecha.AddDays(1);
            if (expira.Kind != DateTimeKind.Utc)
                expira = DateTime.SpecifyKind(expira, DateTimeKind.Utc);

            List<int> clubIds;
            if (dto.ClubId.HasValue && dto.ClubId.Value > 0)
            {
                clubIds = new List<int> { dto.ClubId.Value };
            }
            else
            {
                clubIds = await _context.Inscripciones
                    .Where(i => i.EventoPrueba.IdEvento == eventoId && i.Participante != null && i.Participante.IdClub != null)
                    .Select(i => i.Participante!.IdClub!.Value)
                    .Distinct()
                    .ToListAsync();
            }

            if (clubIds.Count == 0)
                throw new BadRequestException("No hay clubes con inscripciones para generar links.");

            var creados = new List<TokenAccesoCambios>();
            foreach (var clubId in clubIds)
            {
                // Revocar tokens activos previos del mismo club/evento
                var previos = await _context.TokensAccesoCambios
                    .Where(t => t.IdEvento == eventoId && t.IdClub == clubId && !t.Revocado)
                    .ToListAsync();
                foreach (var p in previos) p.Revocado = true;

                var entity = new TokenAccesoCambios
                {
                    Token = Guid.NewGuid().ToString("N"),
                    IdEvento = eventoId,
                    IdClub = clubId,
                    ExpiraEn = expira,
                    Revocado = false,
                    FechaCreacion = DateTime.UtcNow,
                    CreadoPor = creadoPor,
                    Usos = 0
                };
                _context.TokensAccesoCambios.Add(entity);
                creados.Add(entity);
            }

            await _context.SaveChangesAsync();

            await _auditService.RegistrarAccionAsync(
                "TOKEN_CAMBIOS_GEN",
                $"Generados {creados.Count} links de cambios para evento {eventoId}",
                null,
                "Eventos",
                eventoId);

            return await MapTokensAsync(creados.Select(c => c.Id).ToList());
        }

        public async Task<IEnumerable<TokenAccesoCambiosDto>> ListarPorEventoAsync(int eventoId)
        {
            var ids = await _context.TokensAccesoCambios
                .Where(t => t.IdEvento == eventoId)
                .OrderByDescending(t => t.FechaCreacion)
                .Select(t => t.Id)
                .ToListAsync();
            return await MapTokensAsync(ids);
        }

        public async Task RevocarAsync(int eventoId, int tokenId)
        {
            var token = await _context.TokensAccesoCambios
                .FirstOrDefaultAsync(t => t.Id == tokenId && t.IdEvento == eventoId)
                ?? throw new NotFoundException("Token no encontrado");

            token.Revocado = true;
            await _context.SaveChangesAsync();

            await _auditService.RegistrarAccionAsync(
                "TOKEN_CAMBIOS_REV",
                $"Token {tokenId} revocado (evento {eventoId})",
                null,
                "Eventos",
                eventoId);
        }

        public async Task<CambiosAccesoResumenDto> GetResumenByTokenAsync(string token)
        {
            var entity = await LoadValidTokenAsync(token);
            var insc = await _inscripcionService.GetInscripcionesByEventoAndClubAsync(entity.IdEvento, entity.IdClub);
            var evento = entity.Evento;

            var atletas = await _context.Participantes
                .AsNoTracking()
                .Where(p => p.IdClub == entity.IdClub)
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .Select(p => new AtletaCambioDto
                {
                    Id = p.ParticipanteId,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    CategoriaId = p.CategoriaId,
                    SexoId = p.SexoId
                })
                .ToListAsync();

            return new CambiosAccesoResumenDto
            {
                Token = entity.Token,
                EventoId = entity.IdEvento,
                EventoNombre = evento.Nombre,
                ClubId = entity.IdClub,
                ClubNombre = entity.Club?.Nombre ?? string.Empty,
                ExpiraEn = entity.ExpiraEn,
                CambiosAbiertos = EventoCambiosPolicy.PermiteCambiosDelegados(evento) && !entity.Revocado && entity.ExpiraEn >= DateTime.UtcNow,
                Inscripciones = insc,
                AtletasClub = atletas
            };
        }

        public async Task<InscripcionDto> UpdateTripulacionByTokenAsync(
            string token,
            int inscripcionId,
            InscripcionTripulacionUpdateDto dto)
        {
            var entity = await LoadValidTokenAsync(token);
            if (!EventoCambiosPolicy.PermiteCambiosDelegados(entity.Evento))
                throw new BadRequestException("La ventana de cambios está cerrada.");

            var result = await _inscripcionService.UpdateTripulacionAsync(
                inscripcionId,
                dto,
                clubScope: entity.IdClub,
                bypassDeadline: false,
                requireSameClub: true);

            entity.Usos += 1;
            await _context.SaveChangesAsync();
            return result;
        }

        private async Task<TokenAccesoCambios> LoadValidTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new BadRequestException("Token inválido.");

            var entity = await _context.TokensAccesoCambios
                .Include(t => t.Evento)
                .Include(t => t.Club)
                .FirstOrDefaultAsync(t => t.Token == token)
                ?? throw new NotFoundException("Link de cambios no válido.");

            if (entity.Revocado)
                throw new BadRequestException("Este link fue revocado.");
            if (entity.ExpiraEn < DateTime.UtcNow)
                throw new BadRequestException("Este link expiró.");

            return entity;
        }

        private async Task<IEnumerable<TokenAccesoCambiosDto>> MapTokensAsync(List<int> ids)
        {
            if (ids.Count == 0) return Array.Empty<TokenAccesoCambiosDto>();

            var list = await _context.TokensAccesoCambios
                .AsNoTracking()
                .Include(t => t.Evento)
                .Include(t => t.Club)
                .Where(t => ids.Contains(t.Id))
                .ToListAsync();

            return list
                .OrderByDescending(t => t.FechaCreacion)
                .Select(t => new TokenAccesoCambiosDto
                {
                    Id = t.Id,
                    Token = t.Token,
                    EventoId = t.IdEvento,
                    EventoNombre = t.Evento?.Nombre,
                    ClubId = t.IdClub,
                    ClubNombre = t.Club?.Nombre,
                    ExpiraEn = t.ExpiraEn,
                    Revocado = t.Revocado,
                    FechaCreacion = t.FechaCreacion,
                    Usos = t.Usos,
                    UrlRelativa = $"/cambios/{t.Token}"
                });
        }
    }
}
