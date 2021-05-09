using System;

namespace BusinessLogicLayer.Exceptions
{
    public class ContinentException : Exception
    {
        public ContinentException(string message) : base(message) { }
    }
}
