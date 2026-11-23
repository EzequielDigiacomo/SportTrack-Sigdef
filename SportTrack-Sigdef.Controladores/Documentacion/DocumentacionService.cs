using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Entidades.Entidades;

namespace SportTrack_Sigdef.Controladores.Documentacion
{
    public interface IDocumentacionService
    {
        Task<object> UploadAsync(IFormFile file, int personaId, int tipoDocumento, DateTime? fechaVencimiento = null);
        Task<IEnumerable<object>> GetByPersonaAsync(int personaId);
        Task<bool> DeleteAsync(int id);
    }

    public class DocumentacionService : IDocumentacionService
    {
        private readonly SportTrackDbContext _context;
        private readonly Cloudinary? _cloudinary;
        private readonly bool _cloudinaryConfigured;

        public DocumentacionService(SportTrackDbContext context, IOptions<CloudinarySettings> options)
        {
            _context = context;
            var settings = options.Value;
            _cloudinaryConfigured =
                !string.IsNullOrWhiteSpace(settings.CloudName)
                && !string.IsNullOrWhiteSpace(settings.ApiKey)
                && !string.IsNullOrWhiteSpace(settings.ApiSecret);

            if (_cloudinaryConfigured)
            {
                _cloudinary = new Cloudinary(new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret));
                _cloudinary.Api.Secure = true;
            }
        }

        public async Task<object> UploadAsync(IFormFile file, int personaId, int tipoDocumento, DateTime? fechaVencimiento = null)
        {
            FileUploadRules.Validate(file);

            var persona = await _context.Participantes
                .AsNoTracking()
                .Include(p => p.Club!)
                    .ThenInclude(c => c.Federacion!)
                        .ThenInclude(f => f.PlanSaaS)
                .Include(p => p.Club!)
                    .ThenInclude(c => c.PlanSaaS)
                .Include(p => p.AtletaFederacion)
                .FirstOrDefaultAsync(p => p.ParticipanteId == personaId)
                ?? throw new KeyNotFoundException($"No se encontró la persona con ID {personaId}.");

            // Resolver club: preferir el de Participante; si falta, el de la ficha de atleta.
            SportTrack_Sigdef.Entidades.Entidades.Club? club = persona.Club;
            if (club == null)
            {
                var clubId = persona.AtletaFederacion?.IdClub ?? persona.IdClub;
                if (clubId.HasValue)
                {
                    club = await _context.Clubes
                        .AsNoTracking()
                        .Include(c => c.Federacion!)
                            .ThenInclude(f => f.PlanSaaS)
                        .Include(c => c.PlanSaaS)
                        .FirstOrDefaultAsync(c => c.IdClub == clubId.Value);
                }
            }

            var isImage = file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
            if (isImage)
            {
                var plan = club?.Federacion?.PlanSaaS ?? club?.PlanSaaS;
                if (plan != null && !plan.PermitirCargaImagenes)
                {
                    throw new ArgumentException(
                        $"El plan '{plan.Nombre}' no incluye carga de imágenes. Podés subir PDF, o actualizar a Ecosistema.");
                }
            }

            string urlArchivo;
            string? publicId;

            if (_cloudinaryConfigured && _cloudinary != null)
            {
                await using var stream = file.OpenReadStream();
                var publicIdValue = $"{tipoDocumento}_{DateTime.UtcNow:yyyyMMddHHmmss}";
                var folder = BuildCloudinaryFolder(persona, club);

                if (file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                {
                    var imageParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = folder,
                        PublicId = publicIdValue
                    };
                    var imageResult = await _cloudinary.UploadAsync(imageParams);
                    if (imageResult.Error != null)
                        throw new InvalidOperationException($"Cloudinary: {imageResult.Error.Message}");

                    urlArchivo = imageResult.SecureUrl?.ToString() ?? imageResult.Url?.ToString()
                        ?? throw new InvalidOperationException("Cloudinary no devolvió URL.");
                    publicId = imageResult.PublicId;
                }
                else
                {
                    var uploadParams = new RawUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = folder,
                        PublicId = publicIdValue
                    };
                    var rawResult = await _cloudinary.UploadAsync(uploadParams);
                    if (rawResult.Error != null)
                        throw new InvalidOperationException($"Cloudinary: {rawResult.Error.Message}");

                    urlArchivo = rawResult.SecureUrl?.ToString() ?? rawResult.Url?.ToString()
                        ?? throw new InvalidOperationException("Cloudinary no devolvió URL.");
                    publicId = rawResult.PublicId;
                }
            }
            else
            {
                // Fallback local (dev sin Cloudinary): data URL en BD
                await using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var base64 = Convert.ToBase64String(ms.ToArray());
                var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
                urlArchivo = $"data:{contentType};base64,{base64}";
                publicId = null;
            }

            var entity = new DocumentacionFederacionPersona
            {
                PersonaId = personaId,
                TipoDocumento = tipoDocumento,
                UrlArchivo = urlArchivo,
                PublicId = publicId,
                FechaCarga = DateTime.UtcNow,
                FechaVencimiento = fechaVencimiento.HasValue
                    ? DateTime.SpecifyKind(fechaVencimiento.Value, DateTimeKind.Utc)
                    : null,
                EstadoRevision = SportTrack_Sigdef.Entidades.Enums.EstadoRevisionDocumento.Pendiente
            };

            _context.DocumentacionPersonas.Add(entity);

            // Si es apto médico con vencimiento, sincronizar ficha del atleta
            if (tipoDocumento == (int)SportTrack_Sigdef.Entidades.Enums.TipoDocumentoFederacion.AptoMedico
                && entity.FechaVencimiento.HasValue)
            {
                var atleta = await _context.AtletasFederados.FindAsync(personaId);
                if (atleta != null)
                {
                    atleta.PresentoAptoMedico = true;
                    atleta.FechaAptoMedico ??= DateTime.UtcNow;
                    atleta.FechaVencimientoApto = entity.FechaVencimiento;
                }
            }

            await _context.SaveChangesAsync();

            return MapDoc(entity);
        }

        public async Task<IEnumerable<object>> GetByPersonaAsync(int personaId)
        {
            var docs = await _context.DocumentacionPersonas
                .AsNoTracking()
                .Where(d => d.PersonaId == personaId)
                .OrderByDescending(d => d.FechaCarga)
                .ToListAsync();

            return docs.Select(MapDoc);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var documento = await _context.DocumentacionPersonas.FirstOrDefaultAsync(d => d.Id == id);
            if (documento == null) return false;

            if (_cloudinaryConfigured && _cloudinary != null && !string.IsNullOrEmpty(documento.PublicId))
            {
                try
                {
                    await _cloudinary.DestroyAsync(new DeletionParams(documento.PublicId));
                }
                catch
                {
                    // Continuar con borrado en BD aunque falle Cloudinary
                }
            }

            _context.DocumentacionPersonas.Remove(documento);
            await _context.SaveChangesAsync();
            return true;
        }

        private static object MapDoc(DocumentacionFederacionPersona d) => new
        {
            id = d.Id,
            personaId = d.PersonaId,
            tipoDocumento = d.TipoDocumento,
            urlArchivo = d.UrlArchivo,
            publicId = d.PublicId,
            fechaCarga = d.FechaCarga,
            fechaVencimiento = d.FechaVencimiento,
            estadoRevision = d.EstadoRevision,
            observacion = d.Observacion,
            success = true
        };

        /// <summary>
        /// Carpeta Cloudinary: sigdef/{federacion}/{club}/{persona}
        /// Ej: sigdef/fed-7-fac/club-12-rcn/persona-341-perez-juan
        /// </summary>
        private static string BuildCloudinaryFolder(
            SportTrack_Sigdef.Entidades.Entidades.Participante persona,
            SportTrack_Sigdef.Entidades.Entidades.Club? club)
        {
            var fed = club?.Federacion;

            var fedSegment = fed != null
                ? $"fed-{fed.IdFederacion}-{Slug(fed.Sigla ?? fed.Nombre)}"
                : "sin-federacion";

            var clubSegment = club != null
                ? $"club-{club.IdClub}-{Slug(club.Siglas ?? club.Nombre)}"
                : "sin-club";

            var personaLabel = $"{persona.Apellido}-{persona.Nombre}".Trim('-');
            var personaSegment = $"persona-{persona.ParticipanteId}-{Slug(personaLabel)}";

            return $"sigdef/{fedSegment}/{clubSegment}/{personaSegment}";
        }

        private static string Slug(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "sin-nombre";

            var normalized = value.Trim().ToLowerInvariant()
                .Replace('á', 'a').Replace('à', 'a').Replace('ä', 'a').Replace('â', 'a')
                .Replace('é', 'e').Replace('è', 'e').Replace('ë', 'e').Replace('ê', 'e')
                .Replace('í', 'i').Replace('ì', 'i').Replace('ï', 'i').Replace('î', 'i')
                .Replace('ó', 'o').Replace('ò', 'o').Replace('ö', 'o').Replace('ô', 'o')
                .Replace('ú', 'u').Replace('ù', 'u').Replace('ü', 'u').Replace('û', 'u')
                .Replace('ñ', 'n').Replace('ç', 'c');

            var sb = new System.Text.StringBuilder(normalized.Length);
            var lastWasDash = false;

            foreach (var ch in normalized)
            {
                if ((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9'))
                {
                    sb.Append(ch);
                    lastWasDash = false;
                }
                else if (ch is ' ' or '_' or '-' or '.')
                {
                    if (!lastWasDash && sb.Length > 0)
                    {
                        sb.Append('-');
                        lastWasDash = true;
                    }
                }
            }

            var slug = sb.ToString().Trim('-');
            if (string.IsNullOrEmpty(slug)) return "sin-nombre";
            return slug.Length <= 40 ? slug : slug[..40].TrimEnd('-');
        }
    }
}
