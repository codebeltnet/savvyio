using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Dispatchers;

namespace Savvyio.Domain
{
    /// <summary>
    /// Defines a Domain Event dispatcher that uses Fire-and-Forget/In-Only MEP.
    /// </summary>
    /// <seealso cref="IDispatcher" />
    public interface IDomainEventDispatcher : IDispatcher
    {
        /// <summary>
        /// Invokes any domain event handlers that is assigned to the specified event.
        /// </summary>
        /// <param name="request">The <see cref="IDomainEvent"/> to raise.</param>
        void Raise(IDomainEvent request);
        
        /// <summary>
        /// Invokes any domain event handlers that is assigned to the specified event.
        /// </summary>
        /// <param name="request">The <see cref="IDomainEvent"/> to raise.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task RaiseAsync(IDomainEvent request, Action<AsyncOptions> setup = null);
    }
}
