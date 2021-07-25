using System;

namespace Domain.Exceptions
{
    public class InvalidRowException : Exception
    {
        public InvalidRowException(string message)
            : base(message)
        {
        }
    }
}
