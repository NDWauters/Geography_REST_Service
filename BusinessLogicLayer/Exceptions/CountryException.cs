using System;

namespace BusinessLogicLayer.Exceptions
{
    public class CountryException : Exception
    {
        public CountryException(string message) : base(message) { }
    }
}
