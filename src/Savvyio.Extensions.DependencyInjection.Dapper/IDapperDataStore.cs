using Savvyio.Extensions.Dapper;

namespace Savvyio.Extensions.DependencyInjection.Dapper
{
    /// <summary>
    /// Defines a generic way to support multiple implementations that does the actual I/O communication towards a data store optimized for Dapper.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDapperDataStore" />
    /// <seealso cref="IDataStore{TMarker}"/>
    public interface IDapperDataStore<TMarker> : IDapperDataStore, IDataStore<TMarker>
    {
    }
}
