using Savvyio.Data;

namespace Savvyio.Extensions.DependencyInjection.Data
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of writable data access objects (CrUd).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDataStore{T,TMarker}"/>
    /// <seealso cref="IWritableDataStore{T,TMarker}"/>
    public interface IWritableDataStore<in T, TMarker> : IDataStore<T, TMarker>, IWritableDataStore<T> where T : class
    {
    }
}
