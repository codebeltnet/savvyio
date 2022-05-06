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
    /// <seealso cref="IPersistentDataAccessObject{T,TOptions}"/>
    /// <seealso cref="IWritableDataAccessObject{T,TOptions,TMarker}"/>
    /// <seealso cref="IReadableDataAccessObject{T,TOptions,TMarker}"/>
    /// <seealso cref="IDeletableDataAccessObject{T,TOptions,TMarker}"/>
    public interface IPersistentDataAccessObject<T, TOptions, TMarker> : IPersistentDataAccessObject<T, TOptions>, IWritableDataAccessObject<T, TOptions, TMarker>, IReadableDataAccessObject<T, TOptions, TMarker>, IDeletableDataAccessObject<T, TOptions, TMarker> 
        where T : class 
        where TOptions : AsyncOptions, new()
    {
    }
}
