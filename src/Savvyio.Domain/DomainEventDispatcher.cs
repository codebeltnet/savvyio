using System;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;
using Savvyio.Dispatchers;

namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a default implementation of of the <see cref="IDomainEventDispatcher" /> interface.
    /// </summary>
    /// <seealso cref="FireForgetDispatcher" />
    /// <seealso cref="IDomainEventDispatcher" />
    public class DomainEventDispatcher : FireForgetDispatcher, IDomainEventDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventDispatcher"/> class.
        /// </summary>
        /// <param name="serviceLocator">The provider of service implementations.</param>
        public DomainEventDispatcher(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        /// <summary>
        /// Raises the specified <paramref name="request"/> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IDomainEvent" /> to raise.</param>
        public void Raise(IDomainEvent request)
        {
            Validator.ThrowIfNull(request, nameof(request));
            Dispatch<IDomainEvent, IDomainEventHandler>(request, handler => handler.Delegates);
        }

        /// <summary>
        /// Raises the specified <paramref name="request"/> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IDomainEvent" /> to raise.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public Task RaiseAsync(IDomainEvent request, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(request, nameof(request));
            return DispatchAsync<IDomainEvent, IDomainEventHandler>(request, handler => handler.Delegates, setup);
        }
    }
}
