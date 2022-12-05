using Microsoft.Extensions.Options;
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
        /// <param name="options">The <see cref="AmazonEventBusOptions" /> which need to be configured.</param>
        public AmazonEventBus(IOptions<AmazonEventBusOptions<TMarker>> options) : base(options)
        {
        }
    }
}
