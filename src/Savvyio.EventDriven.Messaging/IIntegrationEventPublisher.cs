using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Messaging;

namespace Savvyio.EventDriven.Messaging
{
    /// <summary>
    /// Specifies the publisher/sender of a bus used for interacting with other subsystems (out-process/inter-application) to be made aware of that something happened.
    /// </summary>
    public interface IIntegrationEventPublisher
    {
        /// <summary>
        /// Publishes the specified <paramref name="event"/> asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="event">The <see cref="IIntegrationEvent"/> to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task PublishAsync(IMessage<IIntegrationEvent> @event, Action<AsyncOptions> setup = null);
    }
}
