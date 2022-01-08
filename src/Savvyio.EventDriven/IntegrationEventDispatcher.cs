using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace Savvyio.EventDriven
{
    /// <summary>
    /// Provides a default implementation of of the <see cref="IIntegrationEventDispatcher" /> interface.
    /// </summary>
    /// <seealso cref="FireForgetDispatcher" />
    /// <seealso cref="IIntegrationEventDispatcher" />
    public class IntegrationEventDispatcher : FireForgetDispatcher, IIntegrationEventDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventDispatcher"/> class.
        /// </summary>
        /// <param name="serviceFactory">The function delegate that provides the services.</param>
        public IntegrationEventDispatcher(Func<Type, IEnumerable<object>> serviceFactory) : base(serviceFactory)
        {
        }

        /// <summary>
        /// Publishes the specified <paramref name="request"/> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IIntegrationEvent"/> to publish.</param>
        public void Publish(IIntegrationEvent request)
        {
            Validator.ThrowIfNull(request, nameof(request));
            Dispatch<IIntegrationEvent, IIntegrationEventHandler>(request, handler => handler.Delegates);
        }

        /// <summary>
        /// Publishes the specified <paramref name="request"/> asynchronous using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IIntegrationEvent"/> to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task PublishAsync(IIntegrationEvent request, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(request, nameof(request));
            return DispatchAsync<IIntegrationEvent, IIntegrationEventHandler>(request, handler => handler.Delegates, setup);
        }
    }
}
