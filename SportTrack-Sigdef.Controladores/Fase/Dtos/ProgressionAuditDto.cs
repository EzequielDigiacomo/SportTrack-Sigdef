namespace SportTrack_Sigdef.Controladores.Fase.Dtos;

public class ProgressionAuditDto
{
    public string Atleta { get; set; } = string.Empty;
    public string? Eliminatoria { get; set; }
    public string? Semifinal { get; set; }
    public string? Final { get; set; }
    public string Plan { get; set; } = string.Empty;
}
