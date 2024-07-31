using Savvyio.Data;

namespace Savvyio.Extensions.DependencyInjection.Data
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of readable data access objects (cRud).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDataStore{T,TMarker}"/>
    /// <seealso cref="IReadableDataStore{T}"/>
    public interface IReadableDataStore<T, TMarker> : IDataStore<T, TMarker>, IReadableDataStore<T> where T : class
    {
    }
}
