using System;
using Cuemon;

namespace Savvyio.Commands
{
    public class CommandError
    {
        public CommandError(string errorMessage, Exception exception = null)
        {
            Validator.ThrowIfNullOrWhitespace(errorMessage, nameof(errorMessage));
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public string ErrorMessage { get; }

        public Exception Exception { get; }
    }
}
