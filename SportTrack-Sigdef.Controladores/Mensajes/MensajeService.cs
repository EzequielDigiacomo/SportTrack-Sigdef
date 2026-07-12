using SportTrack_Sigdef.Controladores.Exceptions;
using SportTrack_Sigdef.Controladores.Mensajes.Dtos;
using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.Mensajes
{
    public class MensajeService : IMensajeService
    {
        private readonly IMensajeRepository _repository;

        public MensajeService(IMensajeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<HiloListItemDto>> GetHilosAsync(string username, string sistemaOrigen, int? campanaId = null)
        {
            var usuario = await RequireUsuarioAsync(username);
            var esSuperAdmin = EsSuperAdmin(usuario);
            var hilos = await _repository.GetHilosVisiblesAsync(usuario.IdUsuario, esSuperAdmin, sistemaOrigen, campanaId);

            return hilos
                .Select(h => MapHiloListItem(h, usuario.IdUsuario, esSuperAdmin))
                .Where(h => h != null)
                .Cast<HiloListItemDto>()
                .ToList();
        }

        public async Task<HiloDetalleDto> GetHiloDetalleAsync(int hiloId, string username, string sistemaOrigen)
        {
            var usuario = await RequireUsuarioAsync(username);
            var hilo = await RequireHiloDelSistemaAsync(hiloId, sistemaOrigen);
            await RequireAccesoHiloAsync(hilo, usuario);

            return MapHiloDetalle(hilo, usuario.IdUsuario, EsSuperAdmin(usuario));
        }

        public async Task<HiloDetalleDto> CrearHiloAsync(CrearHiloDto dto, string username, string sistemaOrigen)
        {
            var emisor = await RequireUsuarioAsync(username);
            var destinatario = await _repository.GetUsuarioByIdAsync(dto.DestinatarioId)
                ?? throw new NotFoundException("Destinatario no encontrado");

            ValidarNuevoMensaje(emisor, destinatario);
            ValidarContenido(dto.Asunto, dto.Cuerpo);

            var ahora = DateTime.UtcNow;
            var hilo = new Hilo
            {
                Asunto = dto.Asunto.Trim(),
                SistemaOrigen = sistemaOrigen,
                CreadoEn = ahora,
                UltimoMensajeEn = ahora
            };

            var mensaje = new Mensaje
            {
                Hilo = hilo,
                RemitenteId = emisor.IdUsuario,
                DestinatarioId = destinatario.IdUsuario,
                Cuerpo = dto.Cuerpo.Trim(),
                EnviadoEn = ahora
            };

            await _repository.AddHiloAsync(hilo);
            await _repository.AddMensajeAsync(mensaje);
            await _repository.SaveChangesAsync();

            var creado = await _repository.GetHiloConMensajesAsync(hilo.IdHilo)
                ?? throw new NotFoundException("No se pudo recuperar el hilo creado");

            return MapHiloDetalle(creado, emisor.IdUsuario, EsSuperAdmin(emisor));
        }

        public async Task<EnviarMasivoResultDto> EnviarMasivoAsync(EnviarMasivoDto dto, string username, string sistemaOrigen)
        {
            var emisor = await RequireUsuarioAsync(username);

            if (!EsSuperAdmin(emisor) && !EsAdmin(emisor))
                throw new UnauthorizedException("Solo SuperAdmin o Admin pueden enviar comunicados masivos");

            ValidarContenido(dto.Asunto, dto.Cuerpo);

            if (dto.DestinatarioIds == null || dto.DestinatarioIds.Count == 0)
                throw new BadRequestException("Debés seleccionar al menos un destinatario");

            var ids = dto.DestinatarioIds.Distinct().ToList();
            var destinatarios = await _repository.GetUsuariosByIdsAsync(ids);

            if (destinatarios.Count != ids.Count)
                throw new BadRequestException("Uno o más destinatarios no existen");

            foreach (var destinatario in destinatarios)
            {
                if (!PuedeEscribir(emisor, destinatario))
                    throw new UnauthorizedException($"No tenés permiso para enviar a {destinatario.Username}");
            }

            var ahora = DateTime.UtcNow;
            var tipo = EsSuperAdmin(emisor) ? "SuperAdminMasivo" : "AdminMasivo";
            var campana = new CampanaEnvio
            {
                RemitenteId = emisor.IdUsuario,
                Asunto = dto.Asunto.Trim(),
                Cuerpo = dto.Cuerpo.Trim(),
                EnviadoEn = ahora,
                CantidadDestinatarios = destinatarios.Count,
                TipoCampana = tipo,
                SistemaOrigen = sistemaOrigen
            };

            await _repository.AddCampanaAsync(campana);
            await _repository.SaveChangesAsync();

            var hilosPendientes = new List<(Hilo Hilo, Usuario Destinatario)>();

            foreach (var destinatario in destinatarios)
            {
                var hilo = new Hilo
                {
                    Asunto = campana.Asunto,
                    SistemaOrigen = sistemaOrigen,
                    IdCampana = campana.IdCampana,
                    CreadoEn = ahora,
                    UltimoMensajeEn = ahora
                };

                var mensaje = new Mensaje
                {
                    Hilo = hilo,
                    RemitenteId = emisor.IdUsuario,
                    DestinatarioId = destinatario.IdUsuario,
                    Cuerpo = campana.Cuerpo,
                    EnviadoEn = ahora
                };

                await _repository.AddHiloAsync(hilo);
                await _repository.AddMensajeAsync(mensaje);
                hilosPendientes.Add((hilo, destinatario));
            }

            await _repository.SaveChangesAsync();

            var hilosCreados = hilosPendientes.Select(x => new HiloCampanaItemDto
            {
                HiloId = x.Hilo.IdHilo,
                DestinatarioId = x.Destinatario.IdUsuario,
                DestinatarioNombre = NombreDisplay(x.Destinatario),
                DestinatarioUsername = x.Destinatario.Username,
                Leido = false,
                Respondido = false,
                UltimoMensajeEn = ahora
            }).ToList();

            return new EnviarMasivoResultDto
            {
                CampanaId = campana.IdCampana,
                CantidadHilos = hilosCreados.Count,
                Hilos = hilosCreados
            };
        }

        public async Task<List<CampanaListItemDto>> GetCampanasAsync(string username, string sistemaOrigen)
        {
            var usuario = await RequireUsuarioAsync(username);
            if (!EsSuperAdmin(usuario) && !EsAdmin(usuario))
                throw new UnauthorizedException("No tenés acceso a comunicados masivos");

            var campanas = await _repository.GetCampanasByRemitenteAsync(usuario.IdUsuario, sistemaOrigen);
            return campanas.Select(c => MapCampanaListItem(c, usuario.IdUsuario)).ToList();
        }

        public async Task<CampanaDetalleDto> GetCampanaDetalleAsync(int campanaId, string username, string sistemaOrigen)
        {
            var usuario = await RequireUsuarioAsync(username);
            if (!EsSuperAdmin(usuario) && !EsAdmin(usuario))
                throw new UnauthorizedException("No tenés acceso a comunicados masivos");

            var campana = await _repository.GetCampanaDetalleAsync(campanaId)
                ?? throw new NotFoundException("Campaña no encontrada");

            if (!string.Equals(campana.SistemaOrigen, sistemaOrigen, StringComparison.OrdinalIgnoreCase))
                throw new NotFoundException("Campaña no encontrada");

            if (campana.RemitenteId != usuario.IdUsuario && !EsSuperAdmin(usuario))
                throw new UnauthorizedException("No tenés acceso a esta campaña");

            return MapCampanaDetalle(campana, campana.RemitenteId);
        }

        public async Task<HiloDetalleDto> ResponderHiloAsync(int hiloId, ResponderHiloDto dto, string username, string sistemaOrigen)
        {
            var emisor = await RequireUsuarioAsync(username);
            var hilo = await RequireHiloDelSistemaAsync(hiloId, sistemaOrigen);
            await RequireAccesoHiloAsync(hilo, emisor);

            var destinatarioId = ObtenerContraparteId(hilo, emisor.IdUsuario);
            var destinatario = await _repository.GetUsuarioByIdAsync(destinatarioId)
                ?? throw new NotFoundException("Destinatario no encontrado");

            ValidarRespuesta(emisor, destinatario);
            ValidarCuerpo(dto.Cuerpo);

            var ahora = DateTime.UtcNow;
            var mensaje = new Mensaje
            {
                HiloId = hiloId,
                RemitenteId = emisor.IdUsuario,
                DestinatarioId = destinatarioId,
                Cuerpo = dto.Cuerpo.Trim(),
                EnviadoEn = ahora
            };

            hilo.UltimoMensajeEn = ahora;

            await _repository.AddMensajeAsync(mensaje);
            await _repository.SaveChangesAsync();

            var actualizado = await _repository.GetHiloConMensajesAsync(hiloId)
                ?? throw new NotFoundException("No se pudo recuperar el hilo actualizado");

            return MapHiloDetalle(actualizado, emisor.IdUsuario, EsSuperAdmin(emisor));
        }

        public async Task MarcarHiloLeidoAsync(int hiloId, string username, string sistemaOrigen)
        {
            var usuario = await RequireUsuarioAsync(username);
            var hilo = await RequireHiloDelSistemaAsync(hiloId, sistemaOrigen);
            await RequireAccesoHiloAsync(hilo, usuario);

            var ahora = DateTime.UtcNow;
            var huboCambios = false;

            foreach (var mensaje in hilo.Mensajes.Where(m =>
                         m.DestinatarioId == usuario.IdUsuario &&
                         m.LeidoEn == null &&
                         !m.EliminadoPorDestinatario))
            {
                mensaje.LeidoEn = ahora;
                huboCambios = true;
            }

            if (huboCambios)
            {
                await _repository.SaveChangesAsync();
            }
        }

        public async Task<int> GetNoLeidosCountAsync(string username, string sistemaOrigen)
        {
            var usuario = await RequireUsuarioAsync(username);
            return await _repository.CountNoLeidosAsync(usuario.IdUsuario, sistemaOrigen);
        }

        private async Task<Usuario> RequireUsuarioAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new UnauthorizedException("Usuario no autenticado");

            return await _repository.GetUsuarioByUsernameAsync(username)
                ?? throw new UnauthorizedException("Usuario no encontrado");
        }

        private async Task<Hilo> RequireHiloDelSistemaAsync(int hiloId, string sistemaOrigen)
        {
            var hilo = await _repository.GetHiloConMensajesAsync(hiloId)
                ?? throw new NotFoundException("Hilo no encontrado");

            if (!string.Equals(hilo.SistemaOrigen, sistemaOrigen, StringComparison.OrdinalIgnoreCase))
                throw new NotFoundException("Hilo no encontrado");

            return hilo;
        }

        private async Task RequireAccesoHiloAsync(Hilo hilo, Usuario usuario)
        {
            if (EsSuperAdmin(usuario))
                return;

            var participa = await _repository.UsuarioParticipaEnHiloAsync(hilo.IdHilo, usuario.IdUsuario);
            if (!participa)
                throw new UnauthorizedException("No tenés acceso a este hilo");
        }

        private static bool EsSuperAdmin(Usuario usuario) =>
            string.Equals(usuario.RolFederacion, "SuperAdmin", StringComparison.OrdinalIgnoreCase);

        private static bool EsAdmin(Usuario usuario) =>
            string.Equals(usuario.RolFederacion, "Admin", StringComparison.OrdinalIgnoreCase);

        private static bool EsClub(Usuario usuario) =>
            string.Equals(usuario.RolFederacion, "Club", StringComparison.OrdinalIgnoreCase);

        private static int? ResolverFederacionId(Usuario usuario)
        {
            if (usuario.IdFederacion.HasValue && usuario.IdFederacion.Value > 0)
                return usuario.IdFederacion;

            if (usuario.Club?.IdFederacion.HasValue == true && usuario.Club.IdFederacion.Value > 0)
                return usuario.Club.IdFederacion;

            return null;
        }

        private static bool MismaFederacion(Usuario a, Usuario b)
        {
            var fedA = ResolverFederacionId(a);
            var fedB = ResolverFederacionId(b);
            return fedA.HasValue && fedB.HasValue && fedA.Value == fedB.Value;
        }

        private static bool PuedeEscribir(Usuario emisor, Usuario destinatario)
        {
            if (EsSuperAdmin(emisor) && EsAdmin(destinatario))
                return true;

            if (EsAdmin(emisor) && EsSuperAdmin(destinatario))
                return true;

            if (EsAdmin(emisor) && EsClub(destinatario) && MismaFederacion(emisor, destinatario))
                return true;

            if (EsClub(emisor) && EsAdmin(destinatario) && MismaFederacion(emisor, destinatario))
                return true;

            return false;
        }

        private static void ValidarNuevoMensaje(Usuario emisor, Usuario destinatario)
        {
            if (!destinatario.EstaActivo)
                throw new BadRequestException("El destinatario no está activo");

            if (PuedeEscribir(emisor, destinatario))
                return;

            throw new UnauthorizedException("No tenés permiso para enviar mensajes a ese destinatario");
        }

        private static void ValidarRespuesta(Usuario emisor, Usuario destinatario)
        {
            if (!destinatario.EstaActivo)
                throw new BadRequestException("El destinatario no está activo");

            if (PuedeEscribir(emisor, destinatario))
                return;

            throw new UnauthorizedException("No tenés permiso para responder en este hilo");
        }

        private static void ValidarContenido(string asunto, string cuerpo)
        {
            if (string.IsNullOrWhiteSpace(asunto))
                throw new BadRequestException("El asunto es obligatorio");

            ValidarCuerpo(cuerpo);
        }

        private static void ValidarCuerpo(string cuerpo)
        {
            if (string.IsNullOrWhiteSpace(cuerpo))
                throw new BadRequestException("El cuerpo del mensaje es obligatorio");
        }

        private static int ObtenerContraparteId(Hilo hilo, int usuarioId)
        {
            var participantes = hilo.Mensajes
                .SelectMany(m => new[] { m.RemitenteId, m.DestinatarioId })
                .Distinct()
                .Where(id => id != usuarioId)
                .ToList();

            if (participantes.Count != 1)
                throw new BadRequestException("No se pudo determinar la contraparte del hilo");

            return participantes[0];
        }

        private static string NombreDisplay(Usuario usuario)
        {
            var full = $"{usuario.Nombre} {usuario.Apellido}".Trim();
            return string.IsNullOrWhiteSpace(full) ? usuario.Username : full;
        }

        private HiloListItemDto? MapHiloListItem(Hilo hilo, int usuarioId, bool esSuperAdmin)
        {
            var mensajesVisibles = hilo.Mensajes
                .Where(m => EsMensajeVisible(m, usuarioId, esSuperAdmin))
                .OrderByDescending(m => m.EnviadoEn)
                .ToList();

            if (mensajesVisibles.Count == 0)
                return null;

            var ultimo = mensajesVisibles[0];
            var contraparteUsuario = ultimo.RemitenteId == usuarioId
                ? ultimo.Destinatario
                : ultimo.Remitente;

            if (contraparteUsuario == null)
                return null;

            return new HiloListItemDto
            {
                IdHilo = hilo.IdHilo,
                Asunto = hilo.Asunto,
                UltimoMensajeEn = hilo.UltimoMensajeEn,
                Contraparte = MapUsuarioResumen(contraparteUsuario),
                UltimoMensajePreview = Truncar(ultimo.Cuerpo, 120),
                CantidadNoLeidos = mensajesVisibles.Count(m =>
                    m.DestinatarioId == usuarioId &&
                    m.LeidoEn == null &&
                    !m.EliminadoPorDestinatario)
            };
        }

        private static HiloDetalleDto MapHiloDetalle(Hilo hilo, int usuarioId, bool esSuperAdmin)
        {
            return new HiloDetalleDto
            {
                IdHilo = hilo.IdHilo,
                Asunto = hilo.Asunto,
                CreadoEn = hilo.CreadoEn,
                UltimoMensajeEn = hilo.UltimoMensajeEn,
                IdCampana = hilo.IdCampana,
                Mensajes = hilo.Mensajes
                    .Where(m => EsMensajeVisible(m, usuarioId, esSuperAdmin))
                    .OrderBy(m => m.EnviadoEn)
                    .Select(m => new MensajeItemDto
                    {
                        IdMensaje = m.IdMensaje,
                        RemitenteId = m.RemitenteId,
                        DestinatarioId = m.DestinatarioId,
                        Remitente = MapUsuarioResumen(m.Remitente!),
                        Cuerpo = m.Cuerpo,
                        EnviadoEn = m.EnviadoEn,
                        LeidoEn = m.LeidoEn,
                        EsPropio = m.RemitenteId == usuarioId
                    })
                    .ToList()
            };
        }

        private static CampanaListItemDto MapCampanaListItem(CampanaEnvio campana, int remitenteId)
        {
            var hilos = campana.Hilos?.ToList() ?? new List<Hilo>();
            var leidos = 0;
            var respondidos = 0;

            foreach (var hilo in hilos)
            {
                var mensajes = hilo.Mensajes?.ToList() ?? new List<Mensaje>();
                var primerMensaje = mensajes.OrderBy(m => m.EnviadoEn).FirstOrDefault(m => m.RemitenteId == remitenteId);
                if (primerMensaje?.LeidoEn != null) leidos++;
                if (mensajes.Any(m => m.RemitenteId != remitenteId)) respondidos++;
            }

            return new CampanaListItemDto
            {
                IdCampana = campana.IdCampana,
                Asunto = campana.Asunto,
                EnviadoEn = campana.EnviadoEn,
                CantidadDestinatarios = campana.CantidadDestinatarios,
                TipoCampana = campana.TipoCampana,
                CantidadLeidos = leidos,
                CantidadRespondidos = respondidos
            };
        }

        private static CampanaDetalleDto MapCampanaDetalle(CampanaEnvio campana, int remitenteId)
        {
            var hilosDto = new List<HiloCampanaItemDto>();

            foreach (var hilo in campana.Hilos ?? new List<Hilo>())
            {
                var mensajes = hilo.Mensajes?.OrderBy(m => m.EnviadoEn).ToList() ?? new List<Mensaje>();
                var primer = mensajes.FirstOrDefault(m => m.RemitenteId == remitenteId);
                var destinatario = primer?.Destinatario
                    ?? mensajes.Select(m => m.Destinatario).FirstOrDefault(d => d != null && d.IdUsuario != remitenteId);

                if (destinatario == null && primer != null)
                {
                    destinatario = new Usuario { IdUsuario = primer.DestinatarioId, Username = $"#{primer.DestinatarioId}" };
                }

                if (destinatario == null) continue;

                hilosDto.Add(new HiloCampanaItemDto
                {
                    HiloId = hilo.IdHilo,
                    DestinatarioId = destinatario.IdUsuario,
                    DestinatarioNombre = NombreDisplay(destinatario),
                    DestinatarioUsername = destinatario.Username,
                    Leido = primer?.LeidoEn != null,
                    Respondido = mensajes.Any(m => m.RemitenteId == destinatario.IdUsuario),
                    UltimoMensajeEn = hilo.UltimoMensajeEn
                });
            }

            return new CampanaDetalleDto
            {
                IdCampana = campana.IdCampana,
                Asunto = campana.Asunto,
                Cuerpo = campana.Cuerpo,
                EnviadoEn = campana.EnviadoEn,
                CantidadDestinatarios = campana.CantidadDestinatarios,
                TipoCampana = campana.TipoCampana,
                Hilos = hilosDto
            };
        }

        private static bool EsMensajeVisible(Mensaje mensaje, int usuarioId, bool esSuperAdmin)
        {
            if (esSuperAdmin)
                return true;

            if (mensaje.RemitenteId == usuarioId)
                return !mensaje.EliminadoPorRemitente;

            if (mensaje.DestinatarioId == usuarioId)
                return !mensaje.EliminadoPorDestinatario;

            return false;
        }

        private static UsuarioResumenDto MapUsuarioResumen(Usuario usuario) =>
            new()
            {
                Id = usuario.IdUsuario,
                Username = usuario.Username,
                RolFederacion = usuario.RolFederacion,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                FederacionId = usuario.IdFederacion
            };

        private static string Truncar(string texto, int max)
        {
            var limpio = texto.Trim();
            return limpio.Length <= max ? limpio : limpio[..max] + "...";
        }
    }
}
