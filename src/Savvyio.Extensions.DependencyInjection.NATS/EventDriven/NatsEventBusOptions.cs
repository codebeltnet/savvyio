using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.NATS.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.NATS.EventDriven
{
    /// <summary>
    /// Configuration options for <see cref="NatsEventBus"/>.
    /// </summary>
    /// <seealso cref="NatsEventBusOptions"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public class NatsEventBusOptions<TMarker> : NatsEventBusOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
