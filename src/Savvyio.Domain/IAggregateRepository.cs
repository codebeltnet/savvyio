namespace Savvyio.Domain
{
    /// <summary>
    /// Defines a generic way of abstracting persistent repositories (CRUD) that is optimized for Domain Driven Design.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="IPersistentRepository{TEntity,TKey}"/>
    public interface IAggregateRepository<TEntity, TKey> : IPersistentRepository<TEntity, TKey> where TEntity : class, IAggregateRoot<IDomainEvent, TKey>
    {
    }
}
