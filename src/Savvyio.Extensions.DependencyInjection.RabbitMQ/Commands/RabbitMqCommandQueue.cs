using System;
using Savvyio.Commands;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.RabbitMQ;
using Savvyio.Extensions.RabbitMQ.Commands;

namespace Savvyio.Extensions.DependencyInjection.RabbitMQ.Commands
{
    /// <summary>
    /// Provides a default implementation of the <see cref="RabbitMqMessage"/> class for messages holding an <see cref="ICommand"/> implementation.
    /// </summary>
    /// <seealso cref="RabbitMqCommandQueue"/>
    /// <seealso cref="IPointToPointChannel{TRequest,TMarker}"/>
    public class RabbitMqCommandQueue<TMarker> : RabbitMqCommandQueue, IPointToPointChannel<ICommand, TMarker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqCommandQueue{TMarker}"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The options used to configure the RabbitMQ command queue.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null -or-
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public RabbitMqCommandQueue(IMarshaller marshaller, RabbitMqCommandQueueOptions<TMarker> options) : base(marshaller, options)
        {
        }
    }
}
