using System;

namespace SportTrack_Sigdef.Controladores.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
