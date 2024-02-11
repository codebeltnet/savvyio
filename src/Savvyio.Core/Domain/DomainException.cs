using System;

namespace Savvyio.Domain
{
    /// <summary>
    /// The exception that is thrown when a domain model is not in a valid state.
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        public DomainException() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public DomainException(Exception innerException) : this(default, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the invalid state of the domain model.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public DomainException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}
