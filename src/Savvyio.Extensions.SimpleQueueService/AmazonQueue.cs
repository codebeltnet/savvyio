using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Messaging;

namespace Savvyio.Extensions.SimpleQueueService
{
    /// <summary>
    /// Represents the base class from which all implementations in need of queue capabilities should derive.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to handle.</typeparam>
    /// <seealso cref="AmazonMessage{TRequest}" />
    /// <seealso cref="IPointToPointChannel{TRequest}" />
    public abstract class AmazonQueue<TRequest> : AmazonMessage<TRequest>, IPointToPointChannel<TRequest> where TRequest : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonQueue{TRequest}"/> class.
        /// </summary>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting models to messages.</param>
        /// <param name="options">The configured <see cref="AmazonMessageOptions"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null -or-
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        protected AmazonQueue(IMarshaller marshaller, AmazonMessageOptions options) : base(marshaller, options)
        {
        }

        /// <summary>
        /// Sends the specified <paramref name="messages" /> asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="messages">The messages to send.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public abstract Task SendAsync(IEnumerable<IMessage<TRequest>> messages, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Receive one or more message(s) asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="IRequest"/>.</returns>
        public abstract IAsyncEnumerable<IMessage<TRequest>> ReceiveAsync(Action<AsyncOptions> setup = null);
    }
}
