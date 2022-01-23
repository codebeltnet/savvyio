using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Dispatchers;

namespace Savvyio.Queries
{
    /// <summary>
    /// Defines a Query dispatcher that uses Request-Reply/In-Out MEP.
    /// </summary>
    /// <seealso cref="IDispatcher" />
    public interface IQueryDispatcher : IDispatcher
    {
        /// <summary>
        /// Queries the specified <paramref name="request" /> using Request-Reply/In-Out MEP.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to return.</typeparam>
        /// <param name="request">The <see cref="IQuery{TResult}" /> to request.</param>
        /// <returns>The outcome of the query operation.</returns>
        TResult Query<TResult>(IQuery<TResult> request);

        /// <summary>
        /// Queries the specified <paramref name="request"/> asynchronous using Request-Reply/In-Out MEP.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to return.</typeparam>
        /// <param name="request">The <see cref="IQuery{TResult}"/> to request.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the outcome of the query operation.</returns>
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> request, Action<AsyncOptions> setup = null);
    }
}
