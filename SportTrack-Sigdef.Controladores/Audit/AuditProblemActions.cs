using System;

namespace SportTrack_Sigdef.Controladores.Audit
{
    /// <summary>
    /// Acciones que conviene conservar aunque el evento haya salido bien al final.
    /// </summary>
    public static class AuditProblemActions
    {
        private static readonly string[] IssuePrefixes = { "TIMING_", "RACE_START_" };
        private static readonly string[] KeepActions =
        {
            "ERROR_FATAL",
            "FRONTEND_CRASH",
            "TIMING_SUBMIT_FAILED",
            "TIMING_RETRY_FAILED",
            "TIMING_FLUSH_FAILED",
            "TIMING_QUEUED_OFFLINE",
            "RACE_START_QUEUED",
            "RACE_START_FAILED",
        };

        public static bool IsOperationalIssue(string? accion)
        {
            if (string.IsNullOrWhiteSpace(accion)) return false;
            foreach (var keep in KeepActions)
            {
                if (string.Equals(accion, keep, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return accion.EndsWith("_FAILED", StringComparison.OrdinalIgnoreCase);
        }

        public static bool ShouldKeepOnBulkCleanup(string? accion) => IsOperationalIssue(accion);
    }

}
