using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SportTrack_Sigdef.Entidades.DTOs
{
    public class DocumentoUploadDto
    {
        public IFormFile File { get; set; }
        public int PersonaId { get; set; }
        public int TipoDocumento { get; set; }
    }
}
