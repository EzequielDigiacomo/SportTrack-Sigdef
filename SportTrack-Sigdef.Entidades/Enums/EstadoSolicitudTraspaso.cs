using System.ComponentModel.DataAnnotations;

namespace SportTrack_Sigdef.Entidades.Enums
{
    public enum EstadoSolicitudTraspaso
    {
        [Display(Name = "PendienteOrigen")]
        PendienteOrigen = 1,

        [Display(Name = "RechazadoOrigen")]
        RechazadoOrigen = 2,

        [Display(Name = "PendienteFederacion")]
        PendienteFederacion = 3,

        [Display(Name = "Aprobado")]
        Aprobado = 4,

        [Display(Name = "RechazadoFederacion")]
        RechazadoFederacion = 5,

        [Display(Name = "Cancelado")]
        Cancelado = 6,

        [Display(Name = "Vencido")]
        Vencido = 7
    }
}
