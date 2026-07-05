using System;

namespace SportTrack_Sigdef.Controladores.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
