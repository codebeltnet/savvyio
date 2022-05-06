using System.Collections.Generic;
using Savvyio.Storage;

namespace Savvyio.Domain
{
    /// <summary>
    /// Defines a generic way of abstracting writable repositories (CrUd).
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="IRepository{TEntity,TKey}"/>
    public interface IWritableRepository<in TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
        /// <summary>
        /// Marks the specified <paramref name="entity"/> to be added in the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Marks the specified <paramref name="entities"/> to be added in the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        void AddRange(IEnumerable<TEntity> entities);
    }
}
