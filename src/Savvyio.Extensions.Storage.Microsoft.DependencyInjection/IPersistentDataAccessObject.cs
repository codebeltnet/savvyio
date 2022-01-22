namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of the actual I/O communication with a data store that is responsible of persisting data (CRUD).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    public interface IPersistentDataAccessObject<T, TMarker> : IWritableDataAccessObject<T, TMarker>, IReadableDataAccessObject<T, TMarker>, IDeletableDataAccessObject<T, TMarker> where T : class
    {
    }
}
