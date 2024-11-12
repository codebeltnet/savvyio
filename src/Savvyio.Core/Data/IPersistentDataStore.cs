using Cuemon.Threading;

namespace Savvyio.Data
{
    /// <summary>
    /// Defines a generic way of abstracting persistent data access objects (CRUD).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of options associated with this DTO.</typeparam>
    /// <seealso cref="IWritableDataStore{T}"/>
    /// <seealso cref="IReadableDataStore{T}"/>
    /// <seealso cref="IDeletableDataStore{T}"/>
    /// <seealso cref="ISearchableDataStore{T,TOptions}"/>
    public interface IPersistentDataStore<T, out TOptions> : IWritableDataStore<T>, IReadableDataStore<T>, ISearchableDataStore<T, TOptions>, IDeletableDataStore<T>
        where T : class
        where TOptions : AsyncOptions, new()
    {
    }
}
