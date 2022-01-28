namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way of abstracting the actual I/O communication with a data store that is responsible of persisting data (CRUD).
    /// </summary>
    /// <seealso cref="IWritableDataStore" />
    /// <seealso cref="IReadableDataStore" />
    /// <seealso cref="IDeletableDataStore" />
    public interface IPersistentDataStore : IWritableDataStore, IReadableDataStore, IDeletableDataStore
    {
    }
}
