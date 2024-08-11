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
using Savvyio.Commands;
using Savvyio.Messaging;

namespace Savvyio.Extensions.QueueStorage.Commands
{
    /// <summary>
    /// Provides an Azure Storage Queue implementation of the <see cref="IPointToPointChannel{TRequest}"/>.
    /// </summary>
    public class AzureCommandQueue : IPointToPointChannel<ICommand>
    {
        private readonly IMarshaller _marshaller;
        private readonly AzureCommandQueueOptions _options;
        private readonly QueueClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureCommandQueue"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The <see cref="AzureCommandQueueOptions"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public AzureCommandQueue(IMarshaller marshaller, AzureCommandQueueOptions options)
        {
            Validator.ThrowIfNull(marshaller);
            Validator.ThrowIfInvalidOptions(options);

            _marshaller = marshaller;
            _options = options;

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
        public async Task SendAsync(IEnumerable<IMessage<ICommand>> messages, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(messages);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

            foreach (var message in messages)
            {
                var base64Type = message.GetType()
                    .ToFullNameIncludingAssemblyName()
                    .ToByteArray()
                    .ToBase64String();

                var base64Message = _marshaller.Serialize(message)
                    .ToByteArray()
                    .ToBase64String();

                await _client.SendMessageAsync($"{base64Type}.{base64Message}",
                    _options.SendContext.VisibilityTimeout,
                    _options.SendContext.TimeToLive,
                    options.CancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Receives messages from the Azure Storage Queue.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="ICommand"/>.</returns>
        public async IAsyncEnumerable<IMessage<ICommand>> ReceiveAsync(Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

            var rawMessages = await _client.ReceiveMessagesAsync(_options.ReceiveContext.NumberOfMessagesToTakePerRequest,
                _options.ReceiveContext.VisibilityTimeout,
                options.CancellationToken).ConfigureAwait(false);

            if (rawMessages.HasValue)
            {
                foreach (var rawMessage in rawMessages.Value)
                {
                    options.CancellationToken.ThrowIfCancellationRequested();
                    var base64Components = rawMessage.MessageText.Split('.');
                    var type = Type.GetType(base64Components[0].FromBase64().ToEncodedString());
                    var message = _marshaller.Deserialize(base64Components[1].FromBase64().ToStream(), type) as IMessage<ICommand>;

                    message!.Properties.Add(nameof(QueueMessage.MessageId), rawMessage.MessageId);
                    message.Properties.Add(nameof(QueueMessage.PopReceipt), rawMessage.PopReceipt);
                    message.Properties.Add(nameof(CancellationToken), options.CancellationToken);

                    message.Acknowledged += OnAcknowledgedAsync;

                    yield return message;
                }
            }
        }

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
