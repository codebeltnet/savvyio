using Cuemon.Extensions.DependencyInjection;

namespace Savvyio.Extensions.DependencyInjection
{
    /// <summary>
    /// Defines a generic way to support multiple implementations that does the actual I/O communication towards a data store.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDataStore"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public interface IDataStore<TMarker> : IDataStore, IDependencyInjectionMarker<TMarker>
    {
    }
}
