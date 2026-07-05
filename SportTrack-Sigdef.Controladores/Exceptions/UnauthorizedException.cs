using System;

namespace SportTrack_Sigdef.Controladores.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}
