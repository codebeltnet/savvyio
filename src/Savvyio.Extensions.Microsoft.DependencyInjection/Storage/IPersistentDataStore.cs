namespace Savvyio.Extensions.Microsoft.DependencyInjection.Storage
{
    /// <summary>
    /// Interface IPersistentDataStore
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IWritableDataStore{TMarker}" />
    /// <seealso cref="IReadableDataStore{TMarker}" />
    /// <seealso cref="IDeletableDataStore{TMarker}" />
    public interface IPersistentDataStore<TMarker> : IWritableDataStore<TMarker>, IReadableDataStore<TMarker>, IDeletableDataStore<TMarker>
    {
    }
}
