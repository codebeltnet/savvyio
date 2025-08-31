using System;
using Cuemon.Extensions;
using NATS.Net;
using System.Threading.Tasks;
using Cuemon;
using NATS.Client.Core;
using Savvyio.Diagnostics;

namespace Savvyio.Extensions.NATS
{
    /// <summary>
    /// Provides a base class for NATS message operations, supporting asynchronous disposal and message serialization.
    /// </summary>
    public abstract class NatsMessage : AsyncDisposable, IHealthCheckProvider<INatsConnection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NatsMessage"/> class with the specified marshaller and options.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The configuration options for the NATS message.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        protected NatsMessage(IMarshaller marshaller, NatsMessageOptions options)
        {
            Validator.ThrowIfNull(marshaller);
            Validator.ThrowIfInvalidOptions(options);
            Marshaller = marshaller;
            NatsClient = new NatsClient(options.NatsUrl.OriginalString);
        }

        /// <summary>
        /// Gets the NATS client provided by the constructor for communication with the NATS server.
        /// </summary>
        protected NatsClient NatsClient { get; }

        /// <summary>
        /// Gets the marshaller provided by the constructor for serializing and deserializing messages.
        /// </summary>
        /// <value>The marshaller used for message serialization and deserialization.</value>
        protected IMarshaller Marshaller { get; }

        /// <summary>
        /// Releases the managed resources used by the <see cref="NatsMessage"/> asynchronously.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> that represents the asynchronous dispose operation.</returns>
        protected override async ValueTask OnDisposeManagedResourcesAsync()
        {
            if (NatsClient != null) { await NatsClient.DisposeAsync().ConfigureAwait(false); }
        }
        /// <summary>
        /// Gets the <see cref="INatsConnection"/> instance used for probing the health status of the NATS server.
        /// </summary>
        /// <returns><see cref="INatsConnection"/> instance representing the active connection to the NATS server.</returns>
        public INatsConnection GetHealthCheckTarget()
        {
            return NatsClient.Connection;
        }
    }
}
