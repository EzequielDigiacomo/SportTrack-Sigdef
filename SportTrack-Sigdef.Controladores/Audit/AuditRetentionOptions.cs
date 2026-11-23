using System;
using System.Collections.Generic;

namespace SportTrack_Sigdef.Controladores.Audit
{
    public class AuditRetentionOptions
    {
        /// <summary>Retención global (todas las federaciones / logs sin scope).</summary>
        public bool Enabled { get; set; } = false;
        public int Days { get; set; } = 90;

        /// <summary>Retención / preferencias independientes por federación.</summary>
        public List<FederacionAuditRetentionOptions> Federaciones { get; set; } = new();
    }

    public class FederacionAuditRetentionOptions
    {
        public int FederacionId { get; set; }
        public string? Nombre { get; set; }
        public bool Enabled { get; set; } = false;
        /// <summary>Días a conservar en purga automática de esta federación.</summary>
        public int Days { get; set; } = 90;
    }

    public class FederacionAuditPurgeRequest
    {
        public int FederacionId { get; set; }
        /// <summary>Si true, borra toda la auditoría de la federación (ignora fechas).</summary>
        public bool DeleteAll { get; set; }
        /// <summary>Inicio inclusive del rango a borrar (fecha local/UTC según cliente).</summary>
        public DateTime? From { get; set; }
        /// <summary>Fin inclusive del rango a borrar.</summary>
        public DateTime? To { get; set; }
    }
}
