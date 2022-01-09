using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Repositories
{
    /// <summary>
    /// Defines a repository complying to the Active Record concept.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the model.</typeparam>
    public interface IActiveRecordRepository<TModel, in TKey> : IRepository<TModel> where TModel : class, IIdentity<TKey>
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

        /// <summary>
        /// Deletes a model from the specified <paramref name="id"/> asynchronous.
        /// </summary>
        /// <param name="id">The identifier of the model.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task RemoveAsync(TKey id, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Persist the <paramref name="model"/> asynchronous.
        /// </summary>
        /// <param name="model">The model to persist.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SaveAsync(TModel model, Action<AsyncOptions> setup = null);
    }
}
