using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Threading;
using NATS.Client.Core;
using Savvyio.EventDriven;
using Savvyio.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Savvyio.Extensions.NATS.EventDriven
{
    /// <summary>
    /// Provides a NATS implementation of the <see cref="IPublishSubscribeChannel{TRequest}"/> for integration event messages.
    /// </summary>
    /// <seealso cref="NatsMessage"/>
    /// <seealso cref="IPublishSubscribeChannel{TRequest}"/>
    public class NatsEventBus : NatsMessage, IPublishSubscribeChannel<IIntegrationEvent>
    {
        private readonly NatsEventBusOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="NatsEventBus"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The <see cref="NatsEventBusOptions"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public NatsEventBus(IMarshaller marshaller, NatsEventBusOptions options) : base(marshaller, options)
        {
            Validator.ThrowIfInvalidOptions(options);
            _options = options;
        }

        /// <summary>
        /// Publishes the specified integration event message asynchronously to the configured NATS subject.
        /// </summary>
        /// <param name="message">The message to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task PublishAsync(IMessage<IIntegrationEvent> message, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            await NatsClient.PublishAsync(_options.Subject, (await Marshaller.Serialize(message).ToByteArrayAsync().ConfigureAwait(false)).ToBase64String(), headers: new NatsHeaders()
                {
                    { "type", message.GetType().ToFullNameIncludingAssemblyName() }
                }, cancellationToken: options.CancellationToken);
        }

        /// <summary>
        /// Subscribes to integration event messages from the configured NATS subject and invokes the specified asynchronous handler for each received message.
        /// </summary>
        /// <param name="asyncHandler">The function delegate that will handle the message.</param>
        /// <param name="setup">The <see cref="SubscribeAsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task SubscribeAsync(Func<IMessage<IIntegrationEvent>, CancellationToken, Task> asyncHandler, Action<SubscribeAsyncOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            await foreach (var message in NatsClient.SubscribeAsync<string>(_options.Subject, opts: new NatsSubOpts()
                           {
                           }, cancellationToken: options.CancellationToken))
            {
                var messageType = Type.GetType(message.Headers["type"]);
                var deserialized = Marshaller.Deserialize(await message.Data.FromBase64().ToStreamAsync().ConfigureAwait(false), messageType) as IMessage<IIntegrationEvent>;
                await asyncHandler.Invoke(deserialized, options.CancellationToken).ConfigureAwait(false);
            }
        }
    }
}
