using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Threading;
using Savvyio.Messaging;

namespace Savvyio.Extensions.QueueStorage
{
    /// <summary>
    /// Represents the base class from which all implementations of Azure Storage Queue should derive.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    public abstract class AzureQueue<TRequest> where TRequest : IRequest
    {
        private readonly IMarshaller _marshaller;
        private readonly AzureQueueOptions _options;
        private readonly QueueClient _client;

        private readonly Func<IMessage<TRequest>, IMarshaller, string> _sendMessageFormatter = (message, marshaller) =>
        {
            var base64Type = message.GetType()
                .ToFullNameIncludingAssemblyName()
                .ToByteArray()
                .ToBase64String();

            var base64Message = marshaller.Serialize(message)
                .ToByteArray()
                .ToBase64String();

            return $"{base64Type}.{base64Message}";
        };

        private readonly Func<QueueMessage, IMarshaller, IMessage<TRequest>> _receiveMessageFormatter = (rawMessage, marshaller) =>
        {
            var base64Components = rawMessage.MessageText.Split('.');
            var type = Type.GetType(base64Components[0].FromBase64().ToEncodedString());
            return marshaller.Deserialize(base64Components[1].FromBase64().ToStream(), type) as IMessage<TRequest>;
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueue{TRequest}"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The <see cref="AzureQueueOptions"/> used to configure this instance.</param>
        /// <param name="sendMessageFormatter">The function delegate to format messages for sending. If null, a default formatter is used.</param>
        /// <param name="receiveMessageFormatter">The function delegate to format messages for receiving. If null, a default formatter is used.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        /// <remarks>
        /// The following table shows the initial field values for an instance of <see cref="AzureQueue{TRequest}"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Field</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="_sendMessageFormatter"/></term>
        ///         <description>
        ///             <code>(message, marshaller) =&gt;
        /// {
        ///     var base64Type = message.GetType()
        ///         .ToFullNameIncludingAssemblyName()
        ///         .ToByteArray()
        ///         .ToBase64String();
        ///     var base64Message = marshaller.Serialize(message)
        ///         .ToByteArray()
        ///         .ToBase64String();
        ///     return $"{base64Type}.{base64Message}";
        /// };</code>
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="_receiveMessageFormatter"/></term>
        ///         <description>
        ///             <code>(rawMessage, marshaller) =&gt;
        /// {
        ///     var base64Components = rawMessage.MessageText.Split('.');
        ///     var type = Type.GetType(base64Components[0].FromBase64().ToEncodedString());
        ///     return marshaller.Deserialize(base64Components[1].FromBase64().ToStream(), type) as IMessage&lt;TRequest&gt;;
        /// };</code>
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        protected AzureQueue(IMarshaller marshaller, AzureQueueOptions options, Func<IMessage<TRequest>, IMarshaller, string> sendMessageFormatter = null, Func<QueueMessage, IMarshaller, IMessage<TRequest>> receiveMessageFormatter = null)
        {
            Validator.ThrowIfNull(marshaller);
            Validator.ThrowIfInvalidOptions(options);

            _marshaller = marshaller;
            _options = options;
            _sendMessageFormatter = sendMessageFormatter ?? _sendMessageFormatter;
            _receiveMessageFormatter = receiveMessageFormatter ?? _receiveMessageFormatter;

            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                var queueUri = $"https://{options.StorageAccountName}.queue.core.windows.net/{options.QueueName}".ToUri();
                if (options.Credential != null)
                {
                    _client = new QueueClient(queueUri, options.Credential, options.Settings);
                }
                else if (options.SasCredential != null)
                {
                    _client = new QueueClient(queueUri, options.SasCredential, options.Settings);
                }
                else if (options.StorageKeyCredential != null)
                {
                    _client = new QueueClient(queueUri, options.StorageKeyCredential, options.Settings);
                }
            }
            else
            {
                _client = new QueueClient(options.ConnectionString, options.QueueName, options.Settings);
            }

            options.SetConfiguredClient(_client);
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
        protected async Task SendMessageAsync(IEnumerable<IMessage<TRequest>> messages, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(messages);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

            foreach (var message in messages)
            {
                await _client.SendMessageAsync(_sendMessageFormatter(message, _marshaller),
                    _options.SendContext.VisibilityTimeout,
                    _options.SendContext.TimeToLive,
                    options.CancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Receives messages from the Azure Storage Queue.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="TRequest"/>.</returns>
        protected async IAsyncEnumerable<IMessage<TRequest>> ReceiveMessagesAsync(Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

            var rawMessages = await _client.ReceiveMessagesAsync(_options.ReceiveContext.NumberOfMessagesToTakePerRequest,
                _options.ReceiveContext.VisibilityTimeout,
                options.CancellationToken).ConfigureAwait(false);

            if (rawMessages.HasValue && rawMessages.Value.Length > 0)
            {
                foreach (var rawMessage in rawMessages.Value)
                {
                    options.CancellationToken.ThrowIfCancellationRequested();

                    var message = _receiveMessageFormatter(rawMessage, _marshaller);
                    message!.Properties.Add(nameof(QueueMessage.MessageId), rawMessage.MessageId);
                    message.Properties.Add(nameof(QueueMessage.PopReceipt), rawMessage.PopReceipt);
                    message.Properties.Add(nameof(CancellationToken), options.CancellationToken);
                    message.Acknowledged += OnAcknowledgedAsync;
                    yield return message;
                }
            }
        }

        /// <summary>
        /// Handles the acknowledgment of a message by deleting it from the queue.
        /// </summary>
        /// <param name="sender">The message that was acknowledged.</param>
        /// <param name="args">The event arguments containing properties of the acknowledged message.</param>
        private async Task OnAcknowledgedAsync(object sender, AcknowledgedEventArgs args)
        {
            var messageId = (string)args.Properties[nameof(QueueMessage.MessageId)];
            var popReceipt = (string)args.Properties[nameof(QueueMessage.PopReceipt)];
            var cancellationToken = (CancellationToken)args.Properties[nameof(CancellationToken)];

            await _client.DeleteMessageAsync(messageId, popReceipt, cancellationToken).ConfigureAwait(false);

            if (sender is IAcknowledgeable message) { message.Acknowledged -= OnAcknowledgedAsync; }
        }
    }
}
