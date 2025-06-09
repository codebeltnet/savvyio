using Savvyio.Extensions.RabbitMQ.EventDriven;
using System;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.RabbitMQ;

namespace Savvyio.Extensions.DependencyInjection.RabbitMQ.EventDriven
{
    /// <summary>
    /// Provides a default implementation of the <see cref="RabbitMqMessage"/> class for messages holding an <see cref="IIntegrationEvent"/> implementation.
    /// </summary>
    /// <seealso cref="RabbitMqEventBus"/>
    /// <seealso cref="IPointToPointChannel{TRequest,TMarker}"/>
    public class RabbitMqEventBus<TMarker> : RabbitMqEventBus, IPublishSubscribeChannel<IIntegrationEvent, TMarker>
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqEventBus"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The options used to configure the RabbitMQ event bus.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null -or-
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public RabbitMqEventBus(IMarshaller marshaller, RabbitMqEventBusOptions<TMarker> options) : base(marshaller, options)
        {
        }
    }
}
