using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Threading;
using Microsoft.Extensions.Options;
using Savvyio.Commands;
using Savvyio.Messaging;

namespace Savvyio.Extensions.SimpleQueueService.Commands
{
    /// <summary>
    /// Provides a default implementation of the <see cref="AmazonQueue{TRequest}"/> class tailored for messages holding an <see cref="ICommand"/> implementation.
    /// </summary>
    /// <seealso cref="AmazonQueue{TRequest}" />
    public class AmazonCommandQueue : AmazonQueue<ICommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonCommandQueue"/> class.
        /// </summary>
        /// <param name="serializerContext">The <see cref="ISerializerContext"/> that is used when converting <see cref="ICommand"/> implementations to messages.</param>
        /// <param name="options">The <see cref="AmazonCommandQueueOptions" /> which need to be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializerContext"/> cannot be null -or-
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public AmazonCommandQueue(ISerializerContext serializerContext, IOptions<AmazonCommandQueueOptions> options) : base(serializerContext, options.Value)
        {
        }

        /// <summary>
        /// Sends the specified <paramref name="command" /> asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async Task SendAsync(IMessage<ICommand> command, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            var sqs = new AmazonSQSClient(Options.Credentials, Options.Endpoint);
            var request = new SendMessageRequest
            {
                QueueUrl = Options.SourceQueue.OriginalString,
                MessageGroupId = UseFirstInFirstOut ? command.Source : null,
                MessageDeduplicationId = UseFirstInFirstOut ? command.Id : null,
                MessageBody = await SerializerContext.Serialize(command).ToEncodedStringAsync().ConfigureAwait(false),
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        MessageAttributeTypeKey, new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = command.Data.GetMemberType()
                        }
                    }
                }
            };
            await sqs.SendMessageAsync(request, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Receive one or more command(s) asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="setup">The <see cref="ReceiveAsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="ICommand"/>.</returns>
        public override Task<IEnumerable<IMessage<ICommand>>> ReceiveAsync(Action<ReceiveAsyncOptions> setup = null)
        {
            return RetrieveMessagesAsync(setup);
        }
    }
}
