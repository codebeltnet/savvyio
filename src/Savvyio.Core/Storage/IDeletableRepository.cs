using System.Collections.Generic;

namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way of abstracting deletable repositories (cruD).
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="IRepository{TEntity,TKey}"/>
    public interface IDeletableRepository<in TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
        /// <summary>
        /// Marks the specified <paramref name="entity"/> to be removed from the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Marks the specified <paramref name="entities"/> to be removed from the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
