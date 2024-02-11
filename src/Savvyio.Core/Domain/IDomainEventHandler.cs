using Savvyio.Handlers;

namespace Savvyio.Domain
{
    /// <summary>
    /// Specifies a handler responsible for objects that implements the <see cref="IDomainEvent"/> interface.
    /// </summary>
    /// <seealso cref="IFireForgetHandler{TRequest}" />
    public interface IDomainEventHandler : IFireForgetHandler<IDomainEvent>
    {
    }
}
