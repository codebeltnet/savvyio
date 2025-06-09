using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.DependencyInjection.RabbitMQ.Commands;
using Savvyio.Extensions.DependencyInjection.RabbitMQ.EventDriven;
using System;
using Savvyio.Commands;
using Savvyio.EventDriven;
using Savvyio.Extensions.RabbitMQ.Commands;
using Savvyio.Extensions.RabbitMQ.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.RabbitMQ
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="RabbitMqCommandQueue" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="rabbitMqSetup">The <see cref="RabbitMqCommandQueueOptions" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddRabbitMqCommandQueue(this IServiceCollection services, Action<RabbitMqCommandQueueOptions> rabbitMqSetup, Action<ServiceOptions> serviceSetup = null)
        {
            return services
                    .AddMessageQueue<RabbitMqCommandQueue, ICommand>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(rabbitMqSetup);
        }

        /// <summary>
        /// Adds an <see cref="RabbitMqCommandQueue{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="rabbitMqSetup">The <see cref="RabbitMqCommandQueueOptions{TMarker}" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddRabbitMqCommandQueue<TMarker>(this IServiceCollection services, Action<RabbitMqCommandQueueOptions<TMarker>> rabbitMqSetup, Action<ServiceOptions> serviceSetup = null)
        {
            return services
                    .AddMessageQueue<RabbitMqCommandQueue<TMarker>, ICommand>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(rabbitMqSetup);
        }

        /// <summary>
        /// Adds an <see cref="RabbitMqEventBus" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="rabbitMqSetup">The <see cref="RabbitMqEventBusOptions" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services, Action<RabbitMqEventBusOptions> rabbitMqSetup, Action<ServiceOptions> serviceSetup = null)
        {
            return services
                    .AddMessageBus<RabbitMqEventBus, IIntegrationEvent>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(rabbitMqSetup);
        }

        /// <summary>
        /// Adds an <see cref="RabbitMqEventBus{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="rabbitMqSetup">The <see cref="RabbitMqEventBusOptions{TMarker}" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddRabbitMqEventBus<TMarker>(this IServiceCollection services, Action<RabbitMqEventBusOptions<TMarker>> rabbitMqSetup, Action<ServiceOptions> serviceSetup = null)
        {
            return services
                    .AddMessageBus<RabbitMqEventBus<TMarker>, IIntegrationEvent>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(rabbitMqSetup);
        }
    }
}
