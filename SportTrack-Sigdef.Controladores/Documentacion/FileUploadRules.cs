using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SportTrack_Sigdef.Controladores.Documentacion;

public static class FileUploadRules
{
    public const long MaxBytes = 6 * 1024 * 1024;

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif", ".pdf"
    };

    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp",
        "image/gif",
        "application/pdf"
    };

    /// <summary>Valida tamaño, extensión y Content-Type. Lanza ArgumentException si no cumple.</summary>
    public static void Validate(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("El archivo es obligatorio.");

        if (file.Length > MaxBytes)
            throw new ArgumentException($"El archivo supera el máximo de {MaxBytes / (1024 * 1024)} MB.");

        var ext = Path.GetExtension(file.FileName ?? string.Empty);
        if (string.IsNullOrWhiteSpace(ext) || !AllowedExtensions.Contains(ext))
            throw new ArgumentException(
                $"Extensión no permitida. Usá: {string.Join(", ", AllowedExtensions.OrderBy(x => x))}.");

        var contentType = (file.ContentType ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(contentType) || !AllowedContentTypes.Contains(contentType))
            throw new ArgumentException(
                $"Tipo MIME no permitido ('{contentType}'). Permitidos: imágenes (jpeg/png/webp/gif) o PDF.");
    }
}
