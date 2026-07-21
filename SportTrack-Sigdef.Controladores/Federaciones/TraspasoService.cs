using Microsoft.EntityFrameworkCore;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Audit;
using SportTrack_Sigdef.Controladores.Exceptions;
using SportTrack_Sigdef.Controladores.Mensajes;
using SportTrack_Sigdef.Entidades.DTOs.Traspaso;
using SportTrack_Sigdef.Entidades.Entidades;
using SportTrack_Sigdef.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportTrack_Sigdef.Controladores.Federaciones
{
    public class TraspasoService : ITraspasoService
    {
        private static readonly EstadoSolicitudTraspaso[] EstadosActivos =
        {
            EstadoSolicitudTraspaso.PendienteOrigen,
            EstadoSolicitudTraspaso.PendienteFederacion
        };

        private readonly SportTrackDbContext _context;
        private readonly ITenantProvider _tenantProvider;
        private readonly IAuditService _auditService;
        private readonly ITraspasoNotificacionService _notificacionService;

        public TraspasoService(
            SportTrackDbContext context,
            ITenantProvider tenantProvider,
            IAuditService auditService,
            ITraspasoNotificacionService notificacionService)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _auditService = auditService;
            _notificacionService = notificacionService;
        }

        public async Task<IEnumerable<PeriodoTraspasoDto>> GetPeriodosAsync()
        {
            var fedId = RequireFederacionId();
            var now = DateTime.UtcNow;

            var list = await _context.PeriodosTraspaso
                .AsNoTracking()
                .Where(p => p.IdFederacion == fedId)
                .OrderByDescending(p => p.FechaInicio)
                .ToListAsync();

            return list.Select(p => MapPeriodo(p, now));
        }

        public async Task<PeriodoTraspasoDto?> GetPeriodoActivoAsync()
        {
            var fedId = ResolveFederacionId();
            if (!fedId.HasValue) return null;

            var now = DateTime.UtcNow;
            var periodo = await FindPeriodoActivoEntityAsync(fedId.Value, now);
            return periodo == null ? null : MapPeriodo(periodo, now);
        }

        public async Task<PeriodoTraspasoDto> CreatePeriodoAsync(PeriodoTraspasoCreateDto dto)
        {
            RequireFedAdmin();
            var fedId = RequireFederacionId();
            ValidateRangoFechas(dto.FechaInicio, dto.FechaFin);

            var entity = new PeriodoTraspaso
            {
                IdFederacion = fedId,
                FechaInicio = ToUtc(dto.FechaInicio),
                FechaFin = ToUtcEndOfDay(dto.FechaFin),
                Activo = dto.Activo,
                Observaciones = dto.Observaciones,
                CreadoPorUsuarioId = await GetCurrentUsuarioIdAsync(),
                FechaCreacion = DateTime.UtcNow
            };

            _context.PeriodosTraspaso.Add(entity);
            await _context.SaveChangesAsync();

            await _auditService.RegistrarAccionAsync(
                "CREATE_TRASPASO_PERIODO",
                $"Periodo traspaso {entity.IdPeriodoTraspaso} fed {fedId} ({entity.FechaInicio:dd/MM/yyyy}-{entity.FechaFin:dd/MM/yyyy})",
                null,
                "Traspasos");

            return MapPeriodo(entity, DateTime.UtcNow);
        }

        public async Task<PeriodoTraspasoDto> UpdatePeriodoAsync(int id, PeriodoTraspasoUpdateDto dto)
        {
            RequireFedAdmin();
            var fedId = RequireFederacionId();

            var entity = await _context.PeriodosTraspaso
                .FirstOrDefaultAsync(p => p.IdPeriodoTraspaso == id && p.IdFederacion == fedId)
                ?? throw new NotFoundException("Periodo de traspaso no encontrado.");

            if (dto.FechaInicio.HasValue) entity.FechaInicio = ToUtc(dto.FechaInicio.Value);
            if (dto.FechaFin.HasValue) entity.FechaFin = ToUtcEndOfDay(dto.FechaFin.Value);
            if (dto.Activo.HasValue) entity.Activo = dto.Activo.Value;
            if (dto.Observaciones != null) entity.Observaciones = dto.Observaciones;

            ValidateRangoFechas(entity.FechaInicio, entity.FechaFin);
            await _context.SaveChangesAsync();

            return MapPeriodo(entity, DateTime.UtcNow);
        }

        public async Task<IEnumerable<SolicitudTraspasoDto>> GetSolicitudesAsync(string? estado = null)
        {
            var query = ApplySolicitudScope(_context.SolicitudesTraspaso.AsNoTracking());

            if (!string.IsNullOrWhiteSpace(estado)
                && Enum.TryParse<EstadoSolicitudTraspaso>(estado, true, out var estadoEnum))
            {
                query = query.Where(s => s.Estado == estadoEnum);
            }

            var list = await query
                .Include(s => s.Participante)
                .Include(s => s.ClubOrigen)
                .Include(s => s.ClubDestino)
                .OrderByDescending(s => s.FechaSolicitud)
                .ToListAsync();

            return list.Select(MapSolicitud);
        }

        public async Task<SolicitudTraspasoDto> GetSolicitudByIdAsync(int id)
        {
            var solicitud = await LoadSolicitudScopedAsync(id);
            return MapSolicitud(solicitud);
        }

        public async Task<TraspasoValidacionDto> GetValidacionesAsync(int id)
        {
            var solicitud = await LoadSolicitudScopedAsync(id);
            return await BuildValidacionesAsync(solicitud);
        }

        public async Task<SolicitudTraspasoDto> CrearSolicitudAsync(SolicitudTraspasoCreateDto dto)
        {
            var clubDestinoId = RequireClubDestinoId(dto.IdClubDestino);
            var fedId = await RequireFederacionFromClubAsync(clubDestinoId);
            await EnsurePeriodoActivoAsync(fedId);

            var atleta = await _context.AtletasFederados
                .Include(a => a.Participante)
                .Include(a => a.Club)
                .FirstOrDefaultAsync(a => a.ParticipanteId == dto.ParticipanteId)
                ?? throw new NotFoundException("Atleta no encontrado.");

            if (!atleta.IdClub.HasValue)
                throw new BadRequestException("El atleta no pertenece a ningún club.");

            var idClubOrigen = atleta.IdClub.Value;
            if (idClubOrigen == clubDestinoId)
                throw new BadRequestException("El club destino debe ser distinto al club actual del atleta.");

            await EnsureMismaFederacionAsync(idClubOrigen, clubDestinoId, fedId);
            await EnsureSinSolicitudActivaAsync(dto.ParticipanteId);

            var solicitud = new SolicitudTraspaso
            {
                IdFederacion = fedId,
                ParticipanteId = dto.ParticipanteId,
                IdClubOrigen = idClubOrigen,
                IdClubDestino = clubDestinoId,
                Estado = EstadoSolicitudTraspaso.PendienteFederacion,
                MotivoSolicitud = dto.MotivoSolicitud,
                SolicitadoPorUsuarioId = await GetCurrentUsuarioIdAsync(),
                FechaSolicitud = DateTime.UtcNow
            };

            _context.SolicitudesTraspaso.Add(solicitud);
            await _context.SaveChangesAsync();

            await ReloadNavigationForDto(solicitud);
            await _auditService.RegistrarAccionAsync(
                "CREATE_TRASPASO_SOLICITUD",
                $"Solicitud traspaso #{solicitud.IdSolicitudTraspaso} atleta {dto.ParticipanteId} {idClubOrigen}→{clubDestinoId}",
                null,
                "Traspasos");

            await _notificacionService.NotificarAsync(solicitud, TraspasoNotificacionEvento.SolicitudCreada);

            return MapSolicitud(solicitud);
        }

        public async Task<SolicitudTraspasoDto> AceptarOrigenAsync(int id)
        {
            var solicitud = await LoadSolicitudForOrigenAsync(id);
            if (solicitud.Estado != EstadoSolicitudTraspaso.PendienteOrigen)
                throw new BadRequestException("La solicitud no está pendiente de aceptación del club origen.");

            if (!solicitud.FechaRespuestaFederacion.HasValue)
                throw new BadRequestException("La federación debe verificar la deuda antes de que el club origen responda.");

            solicitud.FechaRespuestaOrigen = DateTime.UtcNow;
            solicitud.MotivoRechazo = null;

            await EjecutarTraspasoAsync(solicitud, forzado: false);

            await ReloadNavigationForDto(solicitud);
            await _auditService.RegistrarAccionAsync(
                "ACEPTAR_TRASPASO_ORIGEN",
                $"Club origen aceptó y se ejecutó traspaso #{id}",
                null,
                "Traspasos");

            await _notificacionService.NotificarAsync(solicitud, TraspasoNotificacionEvento.OrigenAcepto);

            return MapSolicitud(solicitud);
        }

        public async Task<SolicitudTraspasoDto> RechazarOrigenAsync(int id, TraspasoMotivoDto dto)
        {
            var solicitud = await LoadSolicitudForOrigenAsync(id);
            if (solicitud.Estado != EstadoSolicitudTraspaso.PendienteOrigen)
                throw new BadRequestException("La solicitud no está pendiente de aceptación del club origen.");

            solicitud.Estado = EstadoSolicitudTraspaso.RechazadoOrigen;
            solicitud.FechaRespuestaOrigen = DateTime.UtcNow;
            solicitud.MotivoRechazo = dto.Motivo?.Trim();
            await _context.SaveChangesAsync();

            await ReloadNavigationForDto(solicitud);
            await _auditService.RegistrarAccionAsync(
                "RECHAZAR_TRASPASO_ORIGEN",
                $"Club origen rechazó solicitud #{id}",
                null,
                "Traspasos");
            await _notificacionService.NotificarAsync(solicitud, TraspasoNotificacionEvento.OrigenRechazo);

            return MapSolicitud(solicitud);
        }

        public async Task<SolicitudTraspasoDto> AprobarFederacionAsync(int id, bool forzar = false)
        {
            RequireFedAdmin();
            var solicitud = await LoadSolicitudForFederacionAsync(id);

            if (solicitud.Estado != EstadoSolicitudTraspaso.PendienteFederacion)
                throw new BadRequestException("La solicitud no está pendiente de verificación federativa.");

            var validaciones = await BuildValidacionesAsync(solicitud);
            if (!validaciones.PuedeAprobar && !forzar)
                throw new BadRequestException("No se puede habilitar: hay validaciones bloqueantes (deuda u otras).");

            if (forzar && !IsGlobalAdmin())
                throw new UnauthorizedException("Solo administradores globales pueden forzar la habilitación.");

            solicitud.Estado = EstadoSolicitudTraspaso.PendienteOrigen;
            solicitud.FechaRespuestaFederacion = DateTime.UtcNow;
            solicitud.AprobadoPorUsuarioId = await GetCurrentUsuarioIdAsync();
            solicitud.MotivoRechazo = null;
            await _context.SaveChangesAsync();

            await ReloadNavigationForDto(solicitud);
            await _auditService.RegistrarAccionAsync(
                "HABILITAR_TRASPASO_FEDERACION",
                $"Federación habilitó traspaso #{id} tras verificar deuda{(forzar ? " (forzado)" : string.Empty)}",
                null,
                "Traspasos");

            await _notificacionService.NotificarAsync(
                solicitud,
                forzar ? TraspasoNotificacionEvento.FederacionHabilitoForzado : TraspasoNotificacionEvento.FederacionHabilito);

            return MapSolicitud(solicitud);
        }

        public async Task<SolicitudTraspasoDto> RechazarFederacionAsync(int id, TraspasoMotivoDto dto)
        {
            RequireFedAdmin();
            var solicitud = await LoadSolicitudForFederacionAsync(id);

            if (solicitud.Estado != EstadoSolicitudTraspaso.PendienteFederacion)
                throw new BadRequestException("La solicitud no está pendiente de verificación federativa.");

            solicitud.Estado = EstadoSolicitudTraspaso.RechazadoFederacion;
            solicitud.FechaRespuestaFederacion = DateTime.UtcNow;
            solicitud.MotivoRechazo = dto.Motivo?.Trim();
            solicitud.AprobadoPorUsuarioId = await GetCurrentUsuarioIdAsync();
            await _context.SaveChangesAsync();

            await ReloadNavigationForDto(solicitud);
            await _auditService.RegistrarAccionAsync(
                "RECHAZAR_TRASPASO_FEDERACION",
                $"Federación rechazó solicitud #{id}",
                null,
                "Traspasos");
            await _notificacionService.NotificarAsync(solicitud, TraspasoNotificacionEvento.FederacionRechazo);

            return MapSolicitud(solicitud);
        }

        public async Task<SolicitudTraspasoDto> CancelarAsync(int id)
        {
            var clubDestinoId = _tenantProvider.GetClubId()
                ?? throw new UnauthorizedException("Solo el club destino puede cancelar la solicitud.");

            var solicitud = await _context.SolicitudesTraspaso
                .Include(s => s.Participante)
                .Include(s => s.ClubOrigen)
                .Include(s => s.ClubDestino)
                .FirstOrDefaultAsync(s => s.IdSolicitudTraspaso == id && s.IdClubDestino == clubDestinoId)
                ?? throw new NotFoundException("Solicitud no encontrada.");

            if (solicitud.Estado != EstadoSolicitudTraspaso.PendienteOrigen
                && solicitud.Estado != EstadoSolicitudTraspaso.PendienteFederacion)
                throw new BadRequestException("Solo se pueden cancelar solicitudes pendientes.");

            solicitud.Estado = EstadoSolicitudTraspaso.Cancelado;
            await _context.SaveChangesAsync();

            await _auditService.RegistrarAccionAsync(
                "CANCELAR_TRASPASO",
                $"Club destino canceló solicitud #{id}",
                null,
                "Traspasos");
            await _notificacionService.NotificarAsync(solicitud, TraspasoNotificacionEvento.Cancelado);

            return MapSolicitud(solicitud);
        }

        public async Task<IEnumerable<AtletaTraspasoBusquedaDto>> BuscarAtletasAsync(string term)
        {
            var clubDestinoId = _tenantProvider.GetClubId()
                ?? throw new UnauthorizedException("Solo un club puede buscar atletas para traspaso.");

            if (string.IsNullOrWhiteSpace(term) || term.Trim().Length < 2)
                throw new BadRequestException("Ingrese al menos 2 caracteres para buscar.");

            var fedId = await RequireFederacionFromClubAsync(clubDestinoId);
            var search = term.Trim().ToLower();

            var list = await _context.AtletasFederados
                .AsNoTracking()
                .Include(a => a.Participante)
                .Include(a => a.Club)
                .Where(a => a.IdFederacion == fedId
                    && a.IdClub.HasValue
                    && a.IdClub != clubDestinoId
                    && (a.Participante.Nombre.ToLower().Contains(search)
                        || a.Participante.Apellido.ToLower().Contains(search)
                        || (a.Participante.Documento != null && a.Participante.Documento.ToLower().Contains(search))))
                .OrderBy(a => a.Participante.Apellido)
                .ThenBy(a => a.Participante.Nombre)
                .Take(30)
                .ToListAsync();

            return list.Select(a => new AtletaTraspasoBusquedaDto
            {
                ParticipanteId = a.ParticipanteId,
                Nombre = $"{a.Participante.Nombre} {a.Participante.Apellido}".Trim(),
                Documento = a.Participante.Documento,
                IdClub = a.IdClub!.Value,
                ClubNombre = a.Club?.Nombre ?? string.Empty
            });
        }

        public async Task<IEnumerable<TraspasoAuditoriaDto>> GetAuditoriaAsync(int limit = 50)
        {
            RequireFedAdmin();
            limit = Math.Clamp(limit, 1, 200);

            var query = _context.Auditoria.AsNoTracking()
                .Where(a => a.Modulo == "Traspasos");

            return await query
                .OrderByDescending(a => a.Fecha)
                .Take(limit)
                .Select(a => new TraspasoAuditoriaDto
                {
                    Id = a.Id,
                    Fecha = a.Fecha,
                    Accion = a.Accion,
                    Detalle = a.Detalle,
                    Usuario = a.Usuario
                })
                .ToListAsync();
        }

        public async Task<byte[]> ExportSolicitudesCsvAsync(int? periodoId = null, string? estado = null)
        {
            RequireFedAdmin();

            var query = ApplySolicitudScope(_context.SolicitudesTraspaso.AsNoTracking())
                .Include(s => s.Participante)
                .Include(s => s.ClubOrigen)
                .Include(s => s.ClubDestino)
                .AsQueryable();

            if (periodoId.HasValue)
            {
                var periodo = await _context.PeriodosTraspaso.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.IdPeriodoTraspaso == periodoId.Value)
                    ?? throw new NotFoundException("Periodo no encontrado.");

                query = query.Where(s =>
                    s.FechaSolicitud >= periodo.FechaInicio &&
                    s.FechaSolicitud <= periodo.FechaFin);
            }

            if (!string.IsNullOrWhiteSpace(estado)
                && Enum.TryParse<EstadoSolicitudTraspaso>(estado, true, out var estadoEnum))
            {
                query = query.Where(s => s.Estado == estadoEnum);
            }

            var list = await query.OrderByDescending(s => s.FechaSolicitud).ToListAsync();

            static string Csv(string? value)
            {
                if (string.IsNullOrEmpty(value)) return string.Empty;
                var escaped = value.Replace("\"", "\"\"");
                return escaped.Contains(';') || escaped.Contains('"') || escaped.Contains('\n')
                    ? $"\"{escaped}\""
                    : escaped;
            }

            var sb = new StringBuilder();
            sb.AppendLine("Id;Atleta;Documento;ClubOrigen;ClubDestino;Estado;FechaSolicitud;FechaEjecucion;MotivoSolicitud;MotivoRechazo");

            foreach (var s in list)
            {
                var dto = MapSolicitud(s);
                sb.Append(Csv(dto.Id.ToString())).Append(';');
                sb.Append(Csv(dto.ParticipanteNombre)).Append(';');
                sb.Append(Csv(dto.ParticipanteDocumento)).Append(';');
                sb.Append(Csv(dto.ClubOrigenNombre)).Append(';');
                sb.Append(Csv(dto.ClubDestinoNombre)).Append(';');
                sb.Append(Csv(dto.Estado)).Append(';');
                sb.Append(Csv(dto.FechaSolicitud.ToString("yyyy-MM-dd HH:mm"))).Append(';');
                sb.Append(Csv(dto.FechaEjecucion?.ToString("yyyy-MM-dd HH:mm"))).Append(';');
                sb.Append(Csv(dto.MotivoSolicitud)).Append(';');
                sb.AppendLine(Csv(dto.MotivoRechazo));
            }

            return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
        }

        private async Task EjecutarTraspasoAsync(SolicitudTraspaso solicitud, bool forzado)
        {
            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var atleta = await _context.AtletasFederados
                    .FirstOrDefaultAsync(a => a.ParticipanteId == solicitud.ParticipanteId)
                    ?? throw new NotFoundException("Atleta federado no encontrado.");

                var participante = await _context.Participantes
                    .FirstOrDefaultAsync(p => p.ParticipanteId == solicitud.ParticipanteId)
                    ?? throw new NotFoundException("Participante no encontrado.");

                atleta.IdClub = solicitud.IdClubDestino;
                participante.IdClub = solicitud.IdClubDestino;
                participante.PagoAfiliacionAlDia = false;
                atleta.EstadoPago = EstadoPago.Pendiente;

                solicitud.Estado = EstadoSolicitudTraspaso.Aprobado;
                solicitud.FechaEjecucion = DateTime.UtcNow;
                if (!solicitud.FechaRespuestaFederacion.HasValue)
                    solicitud.FechaRespuestaFederacion = DateTime.UtcNow;
                if (!solicitud.AprobadoPorUsuarioId.HasValue)
                    solicitud.AprobadoPorUsuarioId = await GetCurrentUsuarioIdAsync();

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                var nota = forzado ? " (aprobación forzada)" : string.Empty;
                await _auditService.RegistrarAccionAsync(
                    "EJECUTAR_TRASPASO",
                    $"Traspaso ejecutado #{solicitud.IdSolicitudTraspaso} atleta {solicitud.ParticipanteId}{nota}",
                    null,
                    "Traspasos");
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        private async Task<TraspasoValidacionDto> BuildValidacionesAsync(SolicitudTraspaso solicitud)
        {
            var items = new List<TraspasoValidacionItemDto>();
            var now = DateTime.UtcNow;

            var periodoActivo = await FindPeriodoActivoEntityAsync(solicitud.IdFederacion, now);
            items.Add(new TraspasoValidacionItemDto
            {
                Codigo = "PERIODO",
                Descripcion = "Periodo de traspaso vigente",
                Ok = periodoActivo != null,
                Bloqueante = true,
                Detalle = periodoActivo == null ? "No hay periodo activo en la federación." : null
            });

            var clubOrigen = await _context.Clubes.AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdClub == solicitud.IdClubOrigen);
            var clubDestino = await _context.Clubes.AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdClub == solicitud.IdClubDestino);

            items.Add(BuildPagoClubItem("CLUB_ORIGEN", "Club origen al día", clubOrigen));
            items.Add(BuildPagoClubItem("CLUB_DESTINO", "Club destino al día", clubDestino));

            var atleta = await _context.AtletasFederados.AsNoTracking()
                .Include(a => a.Participante)
                .FirstOrDefaultAsync(a => a.ParticipanteId == solicitud.ParticipanteId);

            if (atleta?.Participante != null)
            {
                items.Add(new TraspasoValidacionItemDto
                {
                    Codigo = "ATLETA_AFILIACION",
                    Descripcion = "Afiliación del atleta al día",
                    Ok = atleta.Participante.PagoAfiliacionAlDia,
                    Bloqueante = true,
                    Detalle = atleta.Participante.PagoAfiliacionAlDia ? null : "El atleta tiene afiliación pendiente."
                });
            }

            if (atleta != null)
            {
                var estadoOk = atleta.EstadoPago is EstadoPago.Pagado or EstadoPago.Parcial;
                items.Add(new TraspasoValidacionItemDto
                {
                    Codigo = "ATLETA_ESTADO_PAGO",
                    Descripcion = "Estado de pago SIGDEF del atleta",
                    Ok = estadoOk,
                    Bloqueante = atleta.EstadoPago is EstadoPago.Vencido or EstadoPago.Pendiente,
                    Detalle = $"Estado actual: {atleta.EstadoPago}"
                });
            }

            var inscripcionesImpagas = await _context.Inscripciones
                .AsNoTracking()
                .CountAsync(i => i.IdParticipante == solicitud.ParticipanteId && !i.Pagado);

            items.Add(new TraspasoValidacionItemDto
            {
                Codigo = "INSCRIPCIONES_IMPAGAS",
                Descripcion = "Sin inscripciones impagas",
                Ok = inscripcionesImpagas == 0,
                Bloqueante = inscripcionesImpagas > 0,
                Detalle = inscripcionesImpagas > 0 ? $"{inscripcionesImpagas} inscripción(es) impaga(s)." : null
            });

            return new TraspasoValidacionDto
            {
                SolicitudId = solicitud.IdSolicitudTraspaso,
                PuedeAprobar = items.Where(i => i.Bloqueante).All(i => i.Ok),
                Items = items
            };
        }

        private static TraspasoValidacionItemDto BuildPagoClubItem(string codigo, string desc, SportTrack_Sigdef.Entidades.Entidades.Club? club)
        {
            var ok = club != null && club.PagoAfiliacionAlDia && !club.BloqueadoPorFaltaDePago;
            return new TraspasoValidacionItemDto
            {
                Codigo = codigo,
                Descripcion = desc,
                Ok = ok,
                Bloqueante = true,
                Detalle = ok ? null : (club?.Nombre ?? "Club no encontrado") + " con situación irregular."
            };
        }

        private IQueryable<SolicitudTraspaso> ApplySolicitudScope(IQueryable<SolicitudTraspaso> query)
        {
            var clubId = _tenantProvider.GetClubId();
            if (clubId.HasValue)
            {
                return query.Where(s => s.IdClubOrigen == clubId || s.IdClubDestino == clubId);
            }

            var fedId = _tenantProvider.GetFederacionId();
            if (fedId.HasValue)
            {
                return query.Where(s => s.IdFederacion == fedId);
            }

            if (IsGlobalAdmin()) return query;

            throw new UnauthorizedException("No tiene permisos para consultar traspasos.");
        }

        private async Task<SolicitudTraspaso> LoadSolicitudScopedAsync(int id)
        {
            var query = ApplySolicitudScope(_context.SolicitudesTraspaso);
            return await query
                .Include(s => s.Participante)
                .Include(s => s.ClubOrigen)
                .Include(s => s.ClubDestino)
                .FirstOrDefaultAsync(s => s.IdSolicitudTraspaso == id)
                ?? throw new NotFoundException("Solicitud de traspaso no encontrada.");
        }

        private async Task<SolicitudTraspaso> LoadSolicitudForOrigenAsync(int id)
        {
            var clubId = _tenantProvider.GetClubId()
                ?? throw new UnauthorizedException("Solo el club de origen puede responder esta solicitud.");

            return await _context.SolicitudesTraspaso
                .Include(s => s.Participante)
                .Include(s => s.ClubOrigen)
                .Include(s => s.ClubDestino)
                .FirstOrDefaultAsync(s => s.IdSolicitudTraspaso == id && s.IdClubOrigen == clubId)
                ?? throw new NotFoundException("Solicitud no encontrada para su club.");
        }

        private async Task<SolicitudTraspaso> LoadSolicitudForFederacionAsync(int id)
        {
            var fedId = RequireFederacionId();
            return await _context.SolicitudesTraspaso
                .Include(s => s.Participante)
                .Include(s => s.ClubOrigen)
                .Include(s => s.ClubDestino)
                .FirstOrDefaultAsync(s => s.IdSolicitudTraspaso == id && s.IdFederacion == fedId)
                ?? throw new NotFoundException("Solicitud no encontrada.");
        }

        private async Task EnsurePeriodoActivoAsync(int idFederacion)
        {
            var periodo = await FindPeriodoActivoEntityAsync(idFederacion, DateTime.UtcNow);
            if (periodo == null)
                throw new BadRequestException("No hay un periodo de traspaso activo en la federación.");
        }

        private async Task<PeriodoTraspaso?> FindPeriodoActivoEntityAsync(int idFederacion, DateTime utcNow)
        {
            return await _context.PeriodosTraspaso
                .AsNoTracking()
                .Where(p => p.IdFederacion == idFederacion
                    && p.Activo
                    && p.FechaInicio <= utcNow
                    && p.FechaFin >= utcNow)
                .OrderByDescending(p => p.FechaInicio)
                .FirstOrDefaultAsync();
        }

        private async Task EnsureSinSolicitudActivaAsync(int participanteId)
        {
            var exists = await _context.SolicitudesTraspaso.AnyAsync(s =>
                s.ParticipanteId == participanteId && EstadosActivos.Contains(s.Estado));
            if (exists)
                throw new BadRequestException("El atleta ya tiene una solicitud de traspaso activa.");
        }

        private async Task EnsureMismaFederacionAsync(int clubOrigen, int clubDestino, int fedId)
        {
            var clubs = await _context.Clubes.AsNoTracking()
                .Where(c => c.IdClub == clubOrigen || c.IdClub == clubDestino)
                .ToListAsync();

            if (clubs.Count != 2)
                throw new BadRequestException("Club origen o destino inválido.");

            if (clubs.Any(c => c.IdFederacion != fedId))
                throw new BadRequestException("Los clubes deben pertenecer a la misma federación.");
        }

        private int RequireClubDestinoId(int dtoClubDestino)
        {
            var tenantClub = _tenantProvider.GetClubId();
            if (tenantClub.HasValue)
            {
                if (tenantClub.Value != dtoClubDestino)
                    throw new UnauthorizedException("Solo puede solicitar traspasos hacia su propio club.");
                return tenantClub.Value;
            }

            if (IsFedAdmin())
                return dtoClubDestino;

            throw new UnauthorizedException("Solo un club puede iniciar solicitudes de traspaso.");
        }

        private async Task<int> RequireFederacionFromClubAsync(int clubId)
        {
            var club = await _context.Clubes.AsNoTracking().FirstOrDefaultAsync(c => c.IdClub == clubId)
                ?? throw new BadRequestException("Club destino no encontrado.");

            var tenantFedId = _tenantProvider.GetFederacionId();
            if (tenantFedId.HasValue && club.IdFederacion != tenantFedId.Value)
                throw new BadRequestException("El club no pertenece a su federación.");

            if (!club.IdFederacion.HasValue)
                throw new BadRequestException("El club no tiene federación asignada.");

            return club.IdFederacion.Value;
        }

        private int RequireFederacionId()
        {
            return ResolveFederacionId()
                ?? throw new UnauthorizedException("Debe operar en el contexto de una federación.");
        }

        private int? ResolveFederacionId()
        {
            return _tenantProvider.GetFederacionId();
        }

        private void RequireFedAdmin()
        {
            if (!IsFedAdmin())
                throw new UnauthorizedException("Solo administradores de federación pueden realizar esta acción.");
        }

        private bool IsFedAdmin()
        {
            var rol = _tenantProvider.GetRol();
            return string.Equals(rol, "Admin", StringComparison.OrdinalIgnoreCase)
                || IsGlobalAdmin();
        }

        private bool IsGlobalAdmin()
        {
            var rol = _tenantProvider.GetRol();
            return string.Equals(rol, "SuperAdmin", StringComparison.OrdinalIgnoreCase)
                || string.Equals(rol, "soporte_tecnico", StringComparison.OrdinalIgnoreCase);
        }

        private async Task<int?> GetCurrentUsuarioIdAsync()
        {
            var username = _tenantProvider.GetUser()?.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(username)) return null;

            return await _context.Usuarios.AsNoTracking()
                .Where(u => u.Username == username)
                .Select(u => (int?)u.IdUsuario)
                .FirstOrDefaultAsync();
        }

        private async Task ReloadNavigationForDto(SolicitudTraspaso solicitud)
        {
            await _context.Entry(solicitud).Reference(s => s.Participante).LoadAsync();
            await _context.Entry(solicitud).Reference(s => s.ClubOrigen).LoadAsync();
            await _context.Entry(solicitud).Reference(s => s.ClubDestino).LoadAsync();
        }

        private static void ValidateRangoFechas(DateTime inicio, DateTime fin)
        {
            if (fin < inicio)
                throw new BadRequestException("La fecha de fin debe ser posterior a la de inicio.");
        }

        private static DateTime ToUtc(DateTime value) =>
            DateTime.SpecifyKind(value, DateTimeKind.Utc);

        private static DateTime ToUtcEndOfDay(DateTime value)
        {
            var d = DateTime.SpecifyKind(value.Date, DateTimeKind.Utc);
            return d.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        private static PeriodoTraspasoDto MapPeriodo(PeriodoTraspaso p, DateTime now) => new()
        {
            Id = p.IdPeriodoTraspaso,
            IdFederacion = p.IdFederacion,
            FechaInicio = p.FechaInicio,
            FechaFin = p.FechaFin,
            Activo = p.Activo,
            Observaciones = p.Observaciones,
            EsVigente = p.Activo && p.FechaInicio <= now && p.FechaFin >= now
        };

        private static SolicitudTraspasoDto MapSolicitud(SolicitudTraspaso s) => new()
        {
            Id = s.IdSolicitudTraspaso,
            IdFederacion = s.IdFederacion,
            ParticipanteId = s.ParticipanteId,
            ParticipanteNombre = s.Participante != null
                ? $"{s.Participante.Nombre} {s.Participante.Apellido}".Trim()
                : string.Empty,
            ParticipanteDocumento = s.Participante?.Documento,
            IdClubOrigen = s.IdClubOrigen,
            ClubOrigenNombre = s.ClubOrigen?.Nombre ?? string.Empty,
            IdClubDestino = s.IdClubDestino,
            ClubDestinoNombre = s.ClubDestino?.Nombre ?? string.Empty,
            Estado = s.Estado.ToString(),
            MotivoSolicitud = s.MotivoSolicitud,
            MotivoRechazo = s.MotivoRechazo,
            FechaSolicitud = s.FechaSolicitud,
            FechaRespuestaOrigen = s.FechaRespuestaOrigen,
            FechaRespuestaFederacion = s.FechaRespuestaFederacion,
            FechaEjecucion = s.FechaEjecucion
        };
    }
}
