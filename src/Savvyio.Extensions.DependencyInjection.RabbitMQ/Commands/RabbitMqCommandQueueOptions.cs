using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.RabbitMQ.Commands;

namespace Savvyio.Extensions.DependencyInjection.RabbitMQ.Commands
{
    /// <summary>
    /// Configuration options for <see cref="RabbitMqCommandQueue{TMarker}"/>.
    /// </summary>
    /// <seealso cref="RabbitMqCommandQueueOptions"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public class RabbitMqCommandQueueOptions<TMarker> : RabbitMqCommandQueueOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
