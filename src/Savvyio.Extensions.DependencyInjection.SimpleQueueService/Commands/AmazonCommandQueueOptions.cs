using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.SimpleQueueService.Commands;

namespace Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands
{
    /// <summary>
    /// Configuration options for <see cref="AmazonCommandQueue{TMarker}"/>.
    /// </summary>
    /// <seealso cref="AmazonCommandQueueOptions"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public class AmazonCommandQueueOptions<TMarker> : AmazonCommandQueueOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
