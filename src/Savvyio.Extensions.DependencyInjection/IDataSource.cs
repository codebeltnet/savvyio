using Cuemon.Extensions.DependencyInjection;

namespace Savvyio.Extensions.DependencyInjection
{
    /// <summary>
    /// Defines a generic way to support multiple implementations that does the actual I/O communication with a source of data.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data source represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDataSource"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public interface IDataSource<TMarker> : IDataSource, IDependencyInjectionMarker<TMarker>
    {
    }
}
