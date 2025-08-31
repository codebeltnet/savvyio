using System;
using Cuemon;
using Cuemon.Extensions;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;
using Savvyio.Diagnostics;

namespace Savvyio.Extensions.RabbitMQ
{
    /// <summary>
    /// Provides a base class for RabbitMQ message operations, including connection and channel management, marshalling, and resource disposal. Ensures thread-safe initialization of RabbitMQ connectivity.
    /// </summary>
    public abstract class RabbitMqMessage : AsyncDisposable, IAsyncHealthCheckProvider<IConnection>
    {
        /// <summary>
        /// The header key used to indicate the message type in RabbitMQ properties.
        /// </summary>
        protected const string MessageType = "type";

        private readonly SemaphoreSlim _asyncLock = new(1, 1);
        private bool _initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqMessage"/> class with the specified marshaller and options.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The options used to configure the RabbitMQ connection.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null -or-
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        protected RabbitMqMessage(IMarshaller marshaller, RabbitMqMessageOptions options)
        {
            Validator.ThrowIfInvalidOptions(options);
            Validator.ThrowIfNull(marshaller);
            Marshaller = marshaller;
            RabbitMqFactory = new ConnectionFactory
            {
                Uri = options.AmqpUrl
            };
        }

        /// <summary>
        /// Gets the RabbitMQ connection factory used to create connections to the broker.
        /// </summary>
        protected IConnectionFactory RabbitMqFactory { get; private set; }

        /// <summary>
        /// Ensures that a connection and channel to the RabbitMQ broker are established and initialized.
        /// This method is thread-safe and will only initialize the connection and channel once.
        /// </summary>
        /// <param name="ct">
        /// A <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// If the connection and channel are already initialized, this method returns immediately.
        /// Otherwise, it acquires an asynchronous lock to ensure only one initialization occurs,
        /// then creates the RabbitMQ connection and channel.
        /// </remarks>
        protected async Task EnsureConnectivityAsync(CancellationToken ct)
        {
            if (_initialized) { return; }
            await _asyncLock.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                if (!_initialized)
                {
                    RabbitMqConnection = await RabbitMqFactory.CreateConnectionAsync(ct).ConfigureAwait(false);
                    RabbitMqChannel = await RabbitMqConnection.CreateChannelAsync(cancellationToken: ct).ConfigureAwait(false);
                    _initialized = true;
                }
            }
            finally
            {
                _asyncLock.Release();
            }
        }

        /// <summary>
        /// Gets the current RabbitMQ connection instance.
        /// </summary>
        protected IConnection RabbitMqConnection { get; private set; }

        /// <summary>
        /// Gets the current RabbitMQ channel instance.
        /// </summary>
        protected IChannel RabbitMqChannel { get; private set; }

        /// <summary>
        /// Gets the by constructor provided serializer context.
        /// </summary>
        /// <value>The by constructor provided serializer context.</value>
        protected IMarshaller Marshaller { get; }

        /// <summary>
        /// Called when this object is being disposed by <see cref="AsyncDisposable.DisposeAsync()"/>.
        /// Disposes the RabbitMQ channel and connection asynchronously if they have been initialized.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
        protected override async ValueTask OnDisposeManagedResourcesAsync()
        {
            if (RabbitMqChannel != null) { await RabbitMqChannel.DisposeAsync().ConfigureAwait(false); }
            if (RabbitMqConnection != null) { await RabbitMqConnection.DisposeAsync().ConfigureAwait(false); }
        }

        /// <summary>
        /// Asynchronously an <see cref="IConnection"/> instance used for probing the health status of the RabbitMQ broker.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="IConnection"/> instance representing the active connection to the RabbitMQ broker, or a newly created connection if not already initialized.</returns>
        public async Task<IConnection> GetHealthCheckTargetAsync(CancellationToken cancellationToken = default)
        {
            return RabbitMqConnection ?? await RabbitMqFactory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
