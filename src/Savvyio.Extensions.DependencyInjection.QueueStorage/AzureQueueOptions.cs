using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.QueueStorage;

namespace Savvyio.Extensions.DependencyInjection.QueueStorage
{
    /// <summary>
    /// Configuration options for <see cref="AzureQueueOptions{TMarker}"/>.
    /// </summary>
    /// <seealso cref="AzureQueueOptions"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public class AzureQueueOptions<TMarker> : AzureQueueOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
