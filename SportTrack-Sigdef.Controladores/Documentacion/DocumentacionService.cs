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
        Task<object> UploadAsync(IFormFile file, int personaId, int tipoDocumento);
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

        public async Task<object> UploadAsync(IFormFile file, int personaId, int tipoDocumento)
        {
            FileUploadRules.Validate(file);

            var persona = await _context.Participantes
                .AsNoTracking()
                .Include(p => p.Club!)
                    .ThenInclude(c => c.Federacion!)
                        .ThenInclude(f => f.PlanSaaS)
                .Include(p => p.Club!)
                    .ThenInclude(c => c.PlanSaaS)
                .FirstOrDefaultAsync(p => p.ParticipanteId == personaId)
                ?? throw new KeyNotFoundException($"No se encontró la persona con ID {personaId}.");

            var isImage = file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
            if (isImage)
            {
                var plan = persona.Club?.Federacion?.PlanSaaS ?? persona.Club?.PlanSaaS;
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
                var folder = $"sigdef/documentacion/{personaId}";

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
                FechaCarga = DateTime.UtcNow
            };

            _context.DocumentacionPersonas.Add(entity);
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
            success = true
        };
    }
}
