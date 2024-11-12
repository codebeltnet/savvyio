using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Dispatchers;

namespace Savvyio.EventDriven
{
    /// <summary>
    /// Defines an Integration Event dispatcher that uses Fire-and-Forget/In-Only MEP.
    /// </summary>
    /// <seealso cref="IDispatcher" />
    public interface IIntegrationEventDispatcher : IDispatcher
    {
        /// <summary>
        /// Publishes the specified <paramref name="request"/> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IIntegrationEvent"/> to publish.</param>
        void Publish(IIntegrationEvent request);

        /// <summary>
        /// Publishes the specified <paramref name="request"/> asynchronous using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IIntegrationEvent"/> to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task PublishAsync(IIntegrationEvent request, Action<AsyncOptions> setup = null);
    }
}
