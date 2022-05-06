namespace Savvyio.Domain
{
    /// <summary>
    /// Defines a generic way of abstracting persistent repositories (CRUD).
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="IWritableRepository{TEntity,TKey}"/>
    /// <seealso cref="IReadableRepository{TEntity,TKey}"/>
    /// <seealso cref="ISearchableRepository{TEntity,TKey}"/>
    /// <seealso cref="IDeletableRepository{TEntity,TKey}"/>
    public interface IPersistentRepository<TEntity, TKey> : IWritableRepository<TEntity, TKey>, IReadableRepository<TEntity, TKey>, ISearchableRepository<TEntity, TKey>, IDeletableRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
    }
}
