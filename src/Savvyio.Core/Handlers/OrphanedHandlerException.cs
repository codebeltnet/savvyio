using System;

namespace Savvyio.Handlers
{
    /// <summary>
    /// The exception that is thrown when an <see cref="IHandler{TRequest}"/> implementation cannot be resolved.
    /// </summary>
    /// <seealso cref="ArgumentException" />
    public class OrphanedHandlerException : ArgumentException

    {
        /// <summary>
        /// Creates a new instance of <see cref="OrphanedHandlerException" /> using the provided generic type arguments.
        /// </summary>
        /// <typeparam name="TRequest">The type of the input model registered to a handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler to locate the input model.</typeparam>
        /// <param name="request">The model that is being handled by a registered delegate.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        /// <returns>A new instance of <see cref="OrphanedHandlerException" /> initialized from the provided generic type arguments.</returns>
        public static OrphanedHandlerException Create<TRequest, THandler>(TRequest request, string paramName)
            where TRequest : IRequest
            where THandler : IHandler<TRequest>
        {
            var requestType = request.GetType();
            return new OrphanedHandlerException($"Unable to retrieve an {typeof(THandler).Name} for the specified {requestType.Name}: {requestType.FullName}.", paramName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrphanedHandlerException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        public OrphanedHandlerException(string message, string paramName) : base(message, paramName)
        {
        }
    }
}
