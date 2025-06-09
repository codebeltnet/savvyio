using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.RabbitMQ.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.RabbitMQ.EventDriven
{
    /// <summary>
    /// Configuration options for <see cref="RabbitMqEventBus"/>.
    /// </summary>
    /// <seealso cref="RabbitMqEventBusOptions"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public class RabbitMqEventBusOptions<TMarker> : RabbitMqEventBusOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
