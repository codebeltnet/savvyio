using System;

namespace Savvyio
{
    public class OrphanedHandlerException : ArgumentException
    {
        public OrphanedHandlerException(string message, string paramName) : base(message, paramName)
        {
        }

        public OrphanedHandlerException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
        {   
        }
    }
}
