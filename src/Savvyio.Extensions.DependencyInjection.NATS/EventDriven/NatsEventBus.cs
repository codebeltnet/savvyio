using System;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.NATS;
using Savvyio.Extensions.NATS.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.NATS.EventDriven
{
    /// <summary>
    /// Provides a default implementation of the <see cref="NatsMessage"/> class for messages holding an <see cref="IIntegrationEvent"/> implementation.
    /// </summary>
    /// <seealso cref="NatsEventBus"/>
    /// <seealso cref="IPublishSubscribeChannel{TRequest,TMarker}"/>
    public class NatsEventBus<TMarker> : NatsEventBus, IPublishSubscribeChannel<IIntegrationEvent, TMarker>
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NatsEventBus"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The options used to configure the NATS event bus.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null -or-
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public NatsEventBus(IMarshaller marshaller, NatsEventBusOptions<TMarker> options) : base(marshaller, options)
        {
        }
    }
}
