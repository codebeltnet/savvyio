using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.SimpleQueueService.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven
{
    /// <summary>
    /// Configuration options for <see cref="AmazonEventBus{TMarker}"/>.
    /// </summary>
    /// <seealso cref="AmazonEventBusOptions"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public class AmazonEventBusOptions<TMarker> : AmazonEventBusOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
