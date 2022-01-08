using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    /// <summary>
    /// Defines the read part of a Repository.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to persist.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this Aggregate.</typeparam>
    public interface IReadOnlyRepository<TModel, in TKey> where TModel : class, IIdentity<TKey>
    {
        /// <summary>
        /// Loads the data of a model asynchronous.
        /// </summary>
        /// <param name="id">The identifier of the model.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the loaded model.</returns>
        Task<TModel> LoadAsync(TKey id, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Queries data related to a model asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate that defines the from clause.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the model wrapped in an implementation of the <see cref="IQueryable{T}"/> interface.</returns>
        Task<IQueryable<TModel>> QueryAsync(Expression<Func<TModel, bool>> predicate = null, Action<AsyncOptions> setup = null);
    }
}
