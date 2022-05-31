using Cuemon.Threading;
using Savvyio.Data;

namespace Savvyio.Extensions.DependencyInjection.Data
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of the actual I/O communication with a data store that is responsible of persisting data (CRUD).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of the options associated with this DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IPersistentDataStore{T,TOptions}"/>
    /// <seealso cref="IWritableDataStore{T,TMarker}"/>
    /// <seealso cref="IReadableDataStore{T,TMarker}"/>
    /// <seealso cref="ISearchableDataStore{T,TOptions,TMarker}"/>
    /// <seealso cref="IDeletableDataStore{T,TMarker}"/>
    public interface IPersistentDataStore<T, out TOptions, TMarker> : IPersistentDataStore<T, TOptions>, IWritableDataStore<T, TMarker>, IReadableDataStore<T, TMarker>, ISearchableDataStore<T, TOptions, TMarker>, IDeletableDataStore<T, TMarker> 
        where T : class 
        where TOptions : AsyncOptions, new()
    {
    }
}
