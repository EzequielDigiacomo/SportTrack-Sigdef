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

        public async Task<List<HiloListItemDto>> GetHilosAsync(string username)
        {
            var usuario = await RequireUsuarioAsync(username);
            var esSuperAdmin = EsSuperAdmin(usuario);
            var hilos = await _repository.GetHilosVisiblesAsync(usuario.IdUsuario, esSuperAdmin);

            return hilos
                .Select(h => MapHiloListItem(h, usuario.IdUsuario, esSuperAdmin))
                .Where(h => h != null)
                .Cast<HiloListItemDto>()
                .ToList();
        }

        public async Task<HiloDetalleDto> GetHiloDetalleAsync(int hiloId, string username)
        {
            var usuario = await RequireUsuarioAsync(username);
            await RequireAccesoHiloAsync(hiloId, usuario);

            var hilo = await _repository.GetHiloConMensajesAsync(hiloId)
                ?? throw new NotFoundException("Hilo no encontrado");

            return MapHiloDetalle(hilo, usuario.IdUsuario, EsSuperAdmin(usuario));
        }

        public async Task<HiloDetalleDto> CrearHiloAsync(CrearHiloDto dto, string username)
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

        public async Task<HiloDetalleDto> ResponderHiloAsync(int hiloId, ResponderHiloDto dto, string username)
        {
            var emisor = await RequireUsuarioAsync(username);
            await RequireAccesoHiloAsync(hiloId, emisor);

            var hilo = await _repository.GetHiloConMensajesAsync(hiloId)
                ?? throw new NotFoundException("Hilo no encontrado");

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

        public async Task MarcarHiloLeidoAsync(int hiloId, string username)
        {
            var usuario = await RequireUsuarioAsync(username);
            await RequireAccesoHiloAsync(hiloId, usuario);

            var hilo = await _repository.GetHiloConMensajesAsync(hiloId)
                ?? throw new NotFoundException("Hilo no encontrado");

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

        private async Task<Usuario> RequireUsuarioAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new UnauthorizedException("Usuario no autenticado");

            return await _repository.GetUsuarioByUsernameAsync(username)
                ?? throw new UnauthorizedException("Usuario no encontrado");
        }

        private async Task RequireAccesoHiloAsync(int hiloId, Usuario usuario)
        {
            if (EsSuperAdmin(usuario))
                return;

            var participa = await _repository.UsuarioParticipaEnHiloAsync(hiloId, usuario.IdUsuario);
            if (!participa)
                throw new UnauthorizedException("No tenés acceso a este hilo");
        }

        private static bool EsSuperAdmin(Usuario usuario) =>
            string.Equals(usuario.RolFederacion, "SuperAdmin", StringComparison.OrdinalIgnoreCase);

        private static bool EsAdmin(Usuario usuario) =>
            string.Equals(usuario.RolFederacion, "Admin", StringComparison.OrdinalIgnoreCase);

        private static void ValidarNuevoMensaje(Usuario emisor, Usuario destinatario)
        {
            if (!destinatario.EstaActivo)
                throw new BadRequestException("El destinatario no está activo");

            if (EsSuperAdmin(emisor) && EsAdmin(destinatario))
                return;

            if (EsAdmin(emisor) && EsSuperAdmin(destinatario))
                return;

            throw new UnauthorizedException("No tenés permiso para enviar mensajes a ese destinatario");
        }

        private static void ValidarRespuesta(Usuario emisor, Usuario destinatario)
        {
            if (!destinatario.EstaActivo)
                throw new BadRequestException("El destinatario no está activo");

            var superAdminAdmin =
                (EsSuperAdmin(emisor) && EsAdmin(destinatario)) ||
                (EsAdmin(emisor) && EsSuperAdmin(destinatario));

            if (!superAdminAdmin)
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
