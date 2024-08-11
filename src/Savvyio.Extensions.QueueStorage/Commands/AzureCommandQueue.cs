using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Commands;
using Savvyio.Messaging;

namespace Savvyio.Extensions.QueueStorage.Commands
{
    /// <summary>
    /// Provides an Azure Storage Queue implementation of the <see cref="IPointToPointChannel{TRequest}"/>.
    /// </summary>
    public class AzureCommandQueue : AzureQueue<ICommand>, IPointToPointChannel<ICommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureCommandQueue"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The <see cref="AzureQueueOptions"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public AzureCommandQueue(IMarshaller marshaller, AzureQueueOptions options) : base(marshaller, options)
        {
        }

        /// <summary>
        /// Sends the specified messages to the Azure Storage Queue.
        /// </summary>
        /// <param name="messages">The messages to send.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messages"/> cannot be null.
        /// </exception>
        public Task SendAsync(IEnumerable<IMessage<ICommand>> messages, Action<AsyncOptions> setup = null)
        {
            return SendMessageAsync(messages, setup);
        }

        /// <summary>
        /// Receives messages from the Azure Storage Queue.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="ICommand"/>.</returns>
        public IAsyncEnumerable<IMessage<ICommand>> ReceiveAsync(Action<AsyncOptions> setup = null)
        {
            return ReceiveMessagesAsync(setup);
        }
    }
}
