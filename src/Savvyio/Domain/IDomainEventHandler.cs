namespace Savvyio.Domain
{
    /// <summary>
    /// Specifies a handler resposible for objects that implements the <see cref="IDomainEvent"/> interface.
    /// </summary>
    /// <seealso cref="IHandler{IDomainEvent}" />
    public interface IDomainEventHandler : IHandler<IDomainEvent>
    {
        /// <summary>
        /// Gets the activator responsible of invoking delegates that handles <see cref="IDomainEvent"/>.
        /// </summary>
        /// <value>The activator responsible of invoking delegates that handles <see cref="IDomainEvent"/>.</value>
        IHandlerActivator<IDomainEvent> DomainEvents { get; }
    }
}
