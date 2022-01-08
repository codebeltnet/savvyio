using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace Savvyio.Queries
{
    /// <summary>
    /// Provides a default implementation of of the <see cref="IQueryDispatcher" /> interface.
    /// </summary>
    /// <seealso cref="RequestReplyDispatcher" />
    /// <seealso cref="IQueryDispatcher" />
    public class QueryDispatcher : RequestReplyDispatcher, IQueryDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryDispatcher"/> class.
        /// </summary>
        /// <param name="serviceFactory">The function delegate that provides the services.</param>
        public QueryDispatcher(Func<Type, IEnumerable<object>> serviceFactory) : base(serviceFactory)
        {
        }

        /// <summary>
        /// Queries the specified <paramref name="request" /> using Request-Reply/In-Out MEP.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to return.</typeparam>
        /// <param name="request">The <see cref="IQuery{TResult}" /> to request.</param>
        /// <returns>TResult.</returns>
        public TResult Query<TResult>(IQuery<TResult> request)
        {
            Validator.ThrowIfNull(request, nameof(request));
            return Dispatch<IQuery, IQueryHandler, TResult>(request, handler => handler.Delegates);
        }

        /// <summary>
        /// Queries the specified <paramref name="request" /> asynchronous using Request-Reply/In-Out MEP.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to return.</typeparam>
        /// <param name="request">The <see cref="IQuery{TResult}" /> to request.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result contains the outcome of the query operation.</returns>
        public Task<TResult> QueryAsync<TResult>(IQuery<TResult> request, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(request, nameof(request));
            return DispatchAsync<IQuery, IQueryHandler, TResult>(request, handler => handler.Delegates, setup);
        }
    }
}
