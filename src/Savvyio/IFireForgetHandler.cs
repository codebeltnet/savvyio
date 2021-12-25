namespace Savvyio
{
    /// <summary>
    /// Defines a generic Fire-and-Forget/In-Only MEP handler.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to handle.</typeparam>
    /// <seealso cref="IHandler{TRequest}" />
    public interface IFireForgetHandler<TRequest> : IHandler<TRequest>
    {
        /// <summary>
        /// Gets the <see cref="IFireForgetActivator{TRequest}"/> responsible of invoking delegates that handles the <typeparamref name="TRequest"/> model.
        /// </summary>
        /// <value>The <see cref="IFireForgetActivator{TRequest}"/> responsible of invoking delegates of type <typeparamref name="TRequest"/>.</value>
        public IFireForgetActivator<TRequest> Delegates { get; }
    }
}