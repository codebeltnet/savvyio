using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Messaging;

namespace Savvyio.Extensions.SimpleQueueService
{
    /// <summary>
    /// Represents the base class from which all implementations in need of bus capabilities should derive.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to handle.</typeparam>
    /// <seealso cref="AmazonMessage{TRequest}" />
    /// <seealso cref="IPublishSubscribeChannel{TRequest}" />
    public abstract class AmazonBus<TRequest> : AmazonMessage<TRequest>, IPublishSubscribeChannel<TRequest> where TRequest : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonBus{TRequest}"/> class.
        /// </summary>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting models to messages.</param>
        /// <param name="options">The <see cref="AmazonMessageOptions"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        protected AmazonBus(IMarshaller marshaller, AmazonMessageOptions options) : base(marshaller, options)
        {
        }

        /// <summary>
        /// Publishes the specified <paramref name="message" /> asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="message">The message to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public abstract Task PublishAsync(IMessage<TRequest> message, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Subscribe to one or more message(s) asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="asyncHandler">The function delegate that will handle the message.</param>
        /// <param name="setup">The <see cref="SubscribeAsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public abstract Task SubscribeAsync(Func<IMessage<TRequest>, CancellationToken, Task> asyncHandler, Action<SubscribeAsyncOptions> setup = null);
    }
}
