using System;

namespace BusinessLogicLayer.Exceptions
{
    public class CityException : Exception
    {
        public CityException(string message) : base(message) { }
    }
}
