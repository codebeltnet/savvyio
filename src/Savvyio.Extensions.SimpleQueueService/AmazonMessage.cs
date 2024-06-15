﻿using System.Collections.Generic;
using Cuemon.Configuration;
using Cuemon;
using Savvyio.Messaging;
using Amazon.SQS.Model;
using Amazon.SQS;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Runtime.CompilerServices;
using Cuemon.Extensions.IO;

namespace Savvyio.Extensions.SimpleQueueService
{
    /// <summary>
    /// Represents the base class from which all implementations of AWS SQS should derive.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to handle.</typeparam>
    /// <seealso cref="IConfigurable{TOptions}" />
    public abstract class AmazonMessage<TRequest> : IConfigurable<AmazonMessageOptions> where TRequest : IRequest
    {
        /// <summary>
        /// The key for the message type attribute.
        /// </summary>
        protected const string MessageTypeAttributeKey = "type";

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonMessage{TRequest}"/> class.
        /// </summary>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting models to messages.</param>
        /// <param name="options">The <see cref="AmazonMessageOptions"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null -or-
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        protected AmazonMessage(IMarshaller marshaller, AmazonMessageOptions options)
        {
            Validator.ThrowIfNull(marshaller);
            Validator.ThrowIfInvalidOptions(options);
            Marshaller = marshaller;
            Options = options;
            UseFirstInFirstOut = options.SourceQueue.OriginalString.Contains(".fifo", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets a value indicating whether AWS SQS is configured for FIFO.
        /// </summary>
        /// <value><c>true</c> if AWS SQS is configured for FIFO; otherwise, <c>false</c>.</value>
        protected bool UseFirstInFirstOut { get; }

        /// <summary>
        /// Gets the by constructor provided serializer context.
        /// </summary>
        /// <value>The by constructor provided serializer context.</value>
        protected IMarshaller Marshaller { get; }

        /// <summary>
        /// Gets the configured <see cref="AmazonMessageOptions"/> of this instance.
        /// </summary>
        /// <value>The configured <see cref="AmazonMessageOptions"/> of this instance.</value>
        public AmazonMessageOptions Options { get; }

        /// <summary>
        /// Receive one or more message(s) asynchronous from AWS SQS.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token of an asynchronous operations.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="IRequest"/>.</returns>
        protected virtual async IAsyncEnumerable<IMessage<TRequest>> RetrieveMessagesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var sqs = Options.ClientConfigurations.IsValid()
                ? new AmazonSQSClient(Options.Credentials, Options.ClientConfigurations.SimpleQueueService())
                : new AmazonSQSClient(Options.Credentials, Options.Endpoint);

            if (Options.ReceiveContext.UseApproximateNumberOfMessages)
            {
                var approximateNumberOfMessages = await FetchApproximateNumberOfMessagesAsync(sqs, cancellationToken).ConfigureAwait(false); // cost 1 request
                var currentMessagesCount = approximateNumberOfMessages;
                if (approximateNumberOfMessages == 0) { yield break; }
                while (approximateNumberOfMessages > 0 && currentMessagesCount > 0)
                {
                    var messagesCount = 0;
                    await foreach (var message in ReceiveMessagesAsync(sqs, cancellationToken).ConfigureAwait(false))
                    {
                        messagesCount++;
                        yield return message;
                    }
                    currentMessagesCount = messagesCount;
                    approximateNumberOfMessages -= currentMessagesCount;
                }
            }
            else
            {
                await foreach (var message in ReceiveMessagesAsync(sqs, cancellationToken).ConfigureAwait(false))
                {
                    yield return message;
                }
            }
        }

        private async Task<int> FetchApproximateNumberOfMessagesAsync(IAmazonSQS sqs, CancellationToken cancellationToken)
        {
            var attributes = await sqs.GetQueueAttributesAsync(new GetQueueAttributesRequest
            {
                QueueUrl = Options.SourceQueue.OriginalString,
                AttributeNames = new List<string>()
                {
                    "ApproximateNumberOfMessages"
                }
            }, cancellationToken).ConfigureAwait(false);
            return attributes.ApproximateNumberOfMessages;
        }

        private async IAsyncEnumerable<IMessage<TRequest>> ReceiveMessagesAsync(IAmazonSQS sqs, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var request = new ReceiveMessageRequest
            {
                MaxNumberOfMessages = Options.ReceiveContext.NumberOfMessagesToTakePerRequest,
                WaitTimeSeconds = (int)Options.ReceiveContext.PollingTimeout.TotalSeconds,
                QueueUrl = Options.SourceQueue.OriginalString,
                MessageAttributeNames = new List<string>()
                {
                    MessageTypeAttributeKey
                }
            };

            var response = await sqs.ReceiveMessageAsync(request, cancellationToken).ConfigureAwait(false); // cost 1 request with fetching of up to 10 messages
            var deserializedMessages = new List<IMessage<TRequest>>();
            foreach (var message in response.Messages)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var messageType = Type.GetType(message.MessageAttributes[MessageTypeAttributeKey].StringValue);
                var deserialized = Marshaller.Deserialize(message.Body.ToStream(), messageType) as IMessage<TRequest>;
                deserialized!.Properties.Add(nameof(DeleteMessageBatchRequestEntry.Id), message.MessageId);
                deserialized.Properties.Add(nameof(DeleteMessageBatchRequestEntry.ReceiptHandle), message.ReceiptHandle);
                deserializedMessages.Add(deserialized);
            }

            await foreach (var message in new MessageAsyncEnumerable<TRequest>(deserializedMessages, o =>
                           {
                               o.MessageCallback += message =>
                               {
                                   if (Options.ReceiveContext.AssumeMessageProcessed)
                                   {
                                       message.Acknowledge();
                                   }
                               };
                               o.AcknowledgedPropertiesCallback = async properties =>
                               {
                                   if (Options.ReceiveContext.RemoveProcessedMessages)
                                   {
                                       await RemoveProcessedMessagesAsync(sqs, properties, cancellationToken).ConfigureAwait(false);
                                   }
                               };
                           }).ConfigureAwait(false))
            {
                yield return message;
            }
        }

        private async Task RemoveProcessedMessagesAsync(IAmazonSQS sqs, IEnumerable<IDictionary<string, object>> properties, CancellationToken cancellationToken)
        {
            var batchRequest = new DeleteMessageBatchRequest
            {
                QueueUrl = Options.SourceQueue.OriginalString,
                Entries = new(properties.Select(property => new DeleteMessageBatchRequestEntry
                {
                    Id = (string)property[nameof(DeleteMessageBatchRequestEntry.Id)],
                    ReceiptHandle = (string)property[nameof(DeleteMessageBatchRequestEntry.ReceiptHandle)]
                }))
            };
            await sqs.DeleteMessageBatchAsync(batchRequest, cancellationToken).ConfigureAwait(false); // cost 1 request with deletion of up to 10 messages
        }
    }
}
