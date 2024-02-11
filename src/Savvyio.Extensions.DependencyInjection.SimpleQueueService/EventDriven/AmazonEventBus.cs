using System;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.SimpleQueueService;
using Savvyio.Extensions.SimpleQueueService.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven
{
    /// <summary>
    /// Provides a default implementation of the <see cref="AmazonBus{TRequest}"/> class tailored for messages holding an <see cref="IIntegrationEvent"/> implementation.
    /// </summary>
    /// <seealso cref="AmazonEventBus"/>
    /// <seealso cref="IPublishSubscribeChannel{TRequest,TMarker}"/>
    public class AmazonEventBus<TMarker> : AmazonEventBus, IPublishSubscribeChannel<IIntegrationEvent, TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonEventBus{TMarker}"/> class.
        /// </summary>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting <see cref="IIntegrationEvent"/> implementations to messages.</param>
        /// <param name="options">The <see cref="AmazonEventBusOptions{TMarker}"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public AmazonEventBus(IMarshaller marshaller, AmazonEventBusOptions<TMarker> options) : base(marshaller, options)
        {
        }
    }
}
