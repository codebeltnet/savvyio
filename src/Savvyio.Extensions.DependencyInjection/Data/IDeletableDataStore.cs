using Savvyio.Data;

namespace Savvyio.Extensions.DependencyInjection.Data
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of deletable data access objects (cruD).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDataStore{T,TMarker}"/>
    /// <seealso cref="IDeletableDataStore{T}"/>
    public interface IDeletableDataStore<in T, TMarker> : IDataStore<T, TMarker>, IDeletableDataStore<T> where T : class
    {
    }
}
