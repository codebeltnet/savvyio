using System;
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
        /// <param name="options">The <see cref="AmazonCommandQueueOptions{TMarker}"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public AmazonCommandQueue(IMarshaller marshaller, AmazonCommandQueueOptions<TMarker> options) : base(marshaller, options)
        {
        }
    }
}
