using System;
using Savvyio.Commands;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.QueueStorage.Commands;

namespace Savvyio.Extensions.DependencyInjection.QueueStorage.Commands
{
    /// <summary>
    /// Provides a default implementation of the <see cref="AzureCommandQueue"/> class that is optimized for Dependency Injection.
    /// </summary>
    /// <seealso cref="AzureCommandQueue"/>
    /// <seealso cref="IPointToPointChannel{TRequest,TMarker}"/>
    public class AzureCommandQueue<TMarker> : AzureCommandQueue, IPointToPointChannel<ICommand, TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureCommandQueue{TMarker}"/> class.
        /// </summary>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting <see cref="ICommand"/> implementations to messages.</param>
        /// <param name="options">The <see cref="AzureQueueOptions{TMarker}"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public AzureCommandQueue(IMarshaller marshaller, AzureQueueOptions<TMarker> options) : base(marshaller, options)
        {
        }
    }
}
