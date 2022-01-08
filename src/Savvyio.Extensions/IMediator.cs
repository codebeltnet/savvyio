using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Queries;

namespace Savvyio.Extensions
{
    /// <summary>
    /// Defines a mediator to encapsulate requests (Fire-and-Forget/In-Only) and request/response (Request-Reply/In-Out) patterns.
    /// </summary>
    /// <seealso cref="ICommandDispatcher" />
    /// <seealso cref="IDomainEventDispatcher" />
    /// <seealso cref="IIntegrationEventDispatcher" />
    /// <seealso cref="IQueryDispatcher" />
    public interface IMediator : ICommandDispatcher, IDomainEventDispatcher, IIntegrationEventDispatcher, IQueryDispatcher
    {
    }
}
