using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Messaging;
using System;
using Cuemon;
using Savvyio.Commands;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.NATS.Commands;
using Savvyio.Extensions.DependencyInjection.NATS.EventDriven;
using Savvyio.Extensions.NATS.Commands;
using Savvyio.Extensions.NATS.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.NATS
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="NatsCommandQueue" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="natsSetup">The <see cref="NatsCommandQueueOptions" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddNatsCommandQueue(this IServiceCollection services, Action<NatsCommandQueueOptions> natsSetup, Action<ServiceOptions> serviceSetup = null)
        {
            Validator.ThrowIfNull(services);
            return services
                    .AddMessageQueue<NatsCommandQueue, ICommand>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(natsSetup);
        }

        /// <summary>
        /// Adds an <see cref="NatsCommandQueue{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="natsSetup">The <see cref="NatsCommandQueueOptions{TMarker}" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddNatsCommandQueue<TMarker>(this IServiceCollection services, Action<NatsCommandQueueOptions<TMarker>> natsSetup, Action<ServiceOptions> serviceSetup = null)
        {
            Validator.ThrowIfNull(services);
            return services
                    .AddMessageQueue<NatsCommandQueue<TMarker>, ICommand>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(natsSetup);
        }

        /// <summary>
        /// Adds an <see cref="NatsEventBus" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="natsSetup">The <see cref="NatsEventBusOptions" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddNatsEventBus(this IServiceCollection services, Action<NatsEventBusOptions> natsSetup, Action<ServiceOptions> serviceSetup = null)
        {
            Validator.ThrowIfNull(services);
            return services
                    .AddMessageBus<NatsEventBus, IIntegrationEvent>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(natsSetup);
        }

        /// <summary>
        /// Adds an <see cref="NatsEventBus{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="natsSetup">The <see cref="NatsEventBusOptions{TMarker}" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddNatsEventBus<TMarker>(this IServiceCollection services, Action<NatsEventBusOptions<TMarker>> natsSetup, Action<ServiceOptions> serviceSetup = null)
        {
            Validator.ThrowIfNull(services);
            return services
                    .AddMessageBus<NatsEventBus<TMarker>, IIntegrationEvent>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(natsSetup);
        }
    }
}
