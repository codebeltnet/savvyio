namespace Savvyio
{
    /// <summary>
    /// Defines a generic Request-Reply/In-Out MEP handler.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to handle.</typeparam>
    /// <seealso cref="IHandler{TRequest}" />
    public interface IRequestReplyHandler<TRequest> : IHandler<TRequest>
    {
        /// <summary>
        /// Gets the <see cref="IRequestReplyActivator{TRequest}"/> responsible of invoking delegates that handles the <typeparamref name="TRequest"/> model.
        /// </summary>
        /// <value>The <see cref="IRequestReplyActivator{TRequest}"/> responsible of invoking delegates of type <typeparamref name="TRequest"/>.</value>
        public IRequestReplyActivator<TRequest> Delegates { get; }
    }
}