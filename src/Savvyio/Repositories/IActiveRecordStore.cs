using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Repositories
{
    /// <summary>
    /// Defines a data store that does the actual I/O communication optimized for Active Record.
    /// </summary>
    /// <seealso cref="IStore" />
    public interface IActiveRecordStore : IStore
    {
        /// <summary>
        /// Loads the model from a data store asynchronous.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the model.</typeparam>
        /// <param name="id">The identifier of the model.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the loaded model.</returns>
        Task<TModel> LoadAsync<TModel, TKey>(TKey id, Action<AsyncOptions> setup = null) where TModel : class, IIdentity<TKey>;

        /// <summary>
        /// Queries the model from a data store asynchronous.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the model.</typeparam>
        /// <param name="predicate">The predicate that defines the from clause.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the model wrapped in an implementation of the <see cref="IQueryable{T}"/> interface.</returns>
        Task<IQueryable<TModel>> QueryAsync<TModel, TKey>(Expression<Func<TModel, bool>> predicate = null, Action<AsyncOptions> setup = null) where TModel : class, IIdentity<TKey>;

        /// <summary>
        /// Removes the model from a data store asynchronous.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the model.</typeparam>
        /// <param name="id">The identifier of the model.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task RemoveAsync<TModel, TKey>(TKey id, Action<AsyncOptions> setup = null) where TModel : class, IIdentity<TKey>;

        /// <summary>
        /// Saves the model to a data store asynchronous.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the model.</typeparam>
        /// <param name="model">The model to save.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SaveAsync<TModel, TKey>(TModel model, Action<AsyncOptions> setup = null) where TModel : class, IIdentity<TKey>;
    }
}
