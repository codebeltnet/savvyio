namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of the actual I/O communication with a data store that is responsible of deleting data (cruD).
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IDataStore{TMarker}" />
    /// <seealso cref="IDeletableDataStore" />
    public interface IDeletableDataStore<TMarker> : IDataStore<TMarker>, IDeletableDataStore
    {
    }
}
