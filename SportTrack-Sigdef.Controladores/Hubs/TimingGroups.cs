namespace SportTrack_Sigdef.Controladores.Hubs
{
    public static class TimingGroups
    {
        public const string Operators = "operators";

        public static string Race(int faseId) => $"race_{faseId}";
        public static string Race(string faseId) => $"race_{faseId}";
        public static string Event(int eventoId) => $"event_{eventoId}";
        public static string Event(string eventoId) => $"event_{eventoId}";

        public static string User(string username) => $"user_{NormalizeKey(username)}";

        public static string Federation(int federacionId) => $"fed_{federacionId}";

        private static string NormalizeKey(string value) =>
            (value ?? string.Empty).Trim().ToLowerInvariant();
    }
}
