using Cuemon.Extensions.Xunit;
using Savvyio.Domain;

namespace Savvyio.Assets
{
    public class DomainEventStore : InMemoryTestStore<IDomainEvent>
    {
        public DomainEventStore()
        {
        }
    }
}
