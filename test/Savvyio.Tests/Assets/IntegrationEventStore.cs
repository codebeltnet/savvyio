using Cuemon.Extensions.Xunit;
using Savvyio.Events;

namespace Savvyio.Assets
{
    public class IntegrationEventStore : InMemoryTestStore<IIntegrationEvent>
    {
        public IntegrationEventStore()
        {
        }
    }
}
