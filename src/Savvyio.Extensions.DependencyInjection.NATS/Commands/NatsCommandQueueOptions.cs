using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.NATS.Commands;

namespace Savvyio.Extensions.DependencyInjection.NATS.Commands
{
    /// <summary>
    /// Configuration options for <see cref="NatsCommandQueue{TMarker}"/>.
    /// </summary>
    /// <seealso cref="NatsCommandQueueOptions"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public class NatsCommandQueueOptions<TMarker> : NatsCommandQueueOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
