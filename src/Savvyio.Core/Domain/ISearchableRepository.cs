using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    /// <summary>
    /// Defines a generic way of abstracting searchable repositories (cRud).
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="IRepository{TEntity,TKey}"/>
    public interface ISearchableRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
        /// <summary>
        /// Finds all entities matching the specified <paramref name="predicate"/> asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate that matches the entities to retrieve.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching entities of the operation or an empty sequence if no match was found.</returns>
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null, Action<AsyncOptions> setup = null);
    }
}
