using Cuemon.Threading;
using Savvyio.Data;

namespace Savvyio.Extensions.DependencyInjection.Data
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of searchable data access objects (cRud).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of the options associated with this DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDataStore{T,TMarker}"/>
    /// <seealso cref="ISearchableDataStore{T,TOptions}"/>
    public interface ISearchableDataStore<T, out TOptions, TMarker> : IDataStore<T, TMarker>, ISearchableDataStore<T, TOptions> 
        where T : class 
        where TOptions : AsyncOptions, new()
    {
    }
}
