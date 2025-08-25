using System;
using Savvyio.Commands;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.NATS;
using Savvyio.Extensions.NATS.Commands;

namespace Savvyio.Extensions.DependencyInjection.NATS.Commands
{
    /// <summary>
    /// Provides a default implementation of the <see cref="NatsMessage"/> class for messages holding an <see cref="ICommand"/> implementation.
    /// </summary>
    /// <seealso cref="NatsCommandQueue{TMarker}"/>
    /// <seealso cref="IPointToPointChannel{TRequest,TMarker}"/>
    public class NatsCommandQueue<TMarker> : NatsCommandQueue, IPointToPointChannel<ICommand, TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NatsCommandQueue{TMarker}"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The options used to configure the NATS command queue.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null -or-
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public NatsCommandQueue(IMarshaller marshaller, NatsCommandQueueOptions<TMarker> options) : base(marshaller, options)
        {
        }
    }
}
