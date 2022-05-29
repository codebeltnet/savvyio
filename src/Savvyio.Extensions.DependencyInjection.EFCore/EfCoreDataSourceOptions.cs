using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.EFCore;

namespace Savvyio.Extensions.DependencyInjection.EFCore
{
    /// <summary>
    /// Configuration options for <see cref="IEfCoreDataSource{TMarker}"/>.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that these options represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IEfCoreDataSourceOptions" />
    /// <seealso cref="IDependencyInjectionMarker{TMarker}" />
    public class EfCoreDataSourceOptions<TMarker> : IEfCoreDataSourceOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
