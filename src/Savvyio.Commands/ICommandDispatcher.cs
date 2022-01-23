using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Dispatchers;

namespace Savvyio.Commands
{
    /// <summary>
    /// Defines a Command dispatcher that uses Fire-and-Forget/In-Only MEP.
    /// </summary>
    /// <seealso cref="IDispatcher" />
    public interface ICommandDispatcher : IDispatcher
    {
        /// <summary>
        /// Commits the specified <paramref name="request"/> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="ICommand"/> to commit.</param>
        void Commit(ICommand request);

        /// <summary>
        /// Commits the specified <paramref name="request"/> asynchronous using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="ICommand"/> to commit.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task CommitAsync(ICommand request, Action<AsyncOptions> setup = null);
    }
}
