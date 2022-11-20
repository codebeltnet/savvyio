using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Savvyio.Messaging;

namespace Savvyio.EventDriven.Messaging
{
    /// <summary>
    /// Specifies the subscriber/receiver of a bus used by subsystems to subscribe to events and be made aware of that something happened.
    /// </summary>
    public interface IIntegrationEventSubscriber
    {
        /// <summary>
        /// Subscribe to one or more event(s) asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="setup">The <see cref="MessageBatchOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="IIntegrationEvent"/>.</returns>
        Task<IEnumerable<IMessage<IIntegrationEvent>>> SubscribeAsync(Action<MessageBatchOptions> setup = null);
    }
}
