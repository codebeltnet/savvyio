namespace Savvyio.Domain
{
    /// <summary>
    /// Specifies a handler resposible for objects that implements the <see cref="IDomainEvent"/> interface.
    /// </summary>
    /// <seealso cref="IHandler{IDomainEvent}" />
    public interface IDomainEventHandler : IHandler<IDomainEvent>
    {
        /// <summary>
        /// Gets the domain events.
        /// </summary>
        /// <value>The domain events.</value>
        IHandlerActivator<IDomainEvent> DomainEvents { get; }
    }
}
