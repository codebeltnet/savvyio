using System;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.QueueStorage.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.QueueStorage.EventDriven
{
    /// <summary>
    /// Provides a default implementation of the <see cref="AzureEventBus"/> class that is optimized for Dependency Injection.
    /// </summary>
    /// <seealso cref="AzureEventBus"/>
    /// <seealso cref="IPublishSubscribeChannel{TRequest,TMarker}"/>
    public class AzureEventBus<TMarker> : AzureEventBus, IPublishSubscribeChannel<IIntegrationEvent, TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureEventBus{TMarker}"/> class.
        /// </summary>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting <see cref="IIntegrationEvent"/> implementations to messages.</param>
        /// <param name="azureQueueOptions">The <see cref="AzureQueueOptions{TMarker}"/> used to configure this instance.</param>
        /// <param name="azureEventBusOptions">The <see cref="AzureEventBusOptions{TMarker}"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="azureEventBusOptions"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="azureEventBusOptions"/> are not in a valid state.
        /// </exception>
        public AzureEventBus(IMarshaller marshaller, AzureQueueOptions<TMarker> azureQueueOptions, AzureEventBusOptions<TMarker> azureEventBusOptions) : base(marshaller, azureQueueOptions, azureEventBusOptions)
        {
        }
    }
}
