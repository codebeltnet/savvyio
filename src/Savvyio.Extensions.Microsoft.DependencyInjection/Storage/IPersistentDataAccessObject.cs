using Savvyio.Storage;

namespace Savvyio.Extensions.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of the actual I/O communication with a data store that is responsible of persisting data (CRUD).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IWritableDataAccessObject{T,TMarker}"/>
    /// <seealso cref="IReadableDataAccessObject{T,TMarker}"/>
    /// <seealso cref="IDeletableDataAccessObject{T,TMarker}"/>
    public interface IPersistentDataAccessObject<T, TMarker> : IPersistentDataAccessObject<T>, IWritableDataAccessObject<T, TMarker>, IReadableDataAccessObject<T, TMarker>, IDeletableDataAccessObject<T, TMarker> where T : class
    {
    }
}
