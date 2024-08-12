using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.QueueStorage.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.QueueStorage.EventDriven
{
    /// <summary>
    /// Configuration options for <see cref="AzureEventBus{TMarker}"/>.
    /// </summary>
    /// <seealso cref="AzureEventBusOptions"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public class AzureEventBusOptions<TMarker> : AzureEventBusOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
