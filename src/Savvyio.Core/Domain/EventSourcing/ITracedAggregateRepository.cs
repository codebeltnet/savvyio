namespace Savvyio.Domain.EventSourcing
{
    /// <summary>
    /// Defines a generic way of abstracting traced read- and writable repositories (CRud) that is optimized for Domain Driven Design.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="IReadableRepository{TEntity,TKey}"/>
    /// <seealso cref="IWritableRepository{TEntity,TKey}"/>
    public interface ITracedAggregateRepository<TEntity, TKey> : IReadableRepository<TEntity, TKey>, IWritableRepository<TEntity, TKey> where TEntity : class, ITracedAggregateRoot<TKey>
    {
    }
}
