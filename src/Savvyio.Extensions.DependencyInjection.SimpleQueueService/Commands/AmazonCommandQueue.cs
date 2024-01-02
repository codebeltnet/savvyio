using Microsoft.Extensions.Options;
using Savvyio.Commands;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.SimpleQueueService;
using Savvyio.Extensions.SimpleQueueService.Commands;

namespace Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands
{
    /// <summary>
    /// Provides a default implementation of the <see cref="AmazonQueue{TRequest}"/> class tailored for messages holding an <see cref="ICommand"/> implementation.
    /// </summary>
    /// <seealso cref="AmazonCommandQueue"/>
    /// <seealso cref="IPointToPointChannel{TRequest,TMarker}"/>
    public class AmazonCommandQueue<TMarker> : AmazonCommandQueue, IPointToPointChannel<ICommand, TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonCommandQueue{TMarker}"/> class.
        /// </summary>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting <see cref="ICommand"/> implementations to messages.</param>
        /// <param name="options">The <see cref="AmazonCommandQueueOptions" /> which need to be configured.</param>
        public AmazonCommandQueue(IMarshaller marshaller, IOptions<AmazonCommandQueueOptions<TMarker>> options) : base(marshaller, options)
        {
        }
    }
}
