using Cuemon.Extensions.Xunit;
using Savvyio.EventDriven;

namespace Savvyio.Assets
{
    public class IntegrationEventStore : InMemoryTestStore<IIntegrationEvent>
    {
        public IntegrationEventStore()
        {
        }
    }
}
