using System;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.DependencyInjection.QueueStorage.Commands;
using Savvyio.Extensions.DependencyInjection.QueueStorage.EventDriven;
using Savvyio.Extensions.QueueStorage;
using Savvyio.Extensions.QueueStorage.Commands;
using Savvyio.Extensions.QueueStorage.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.QueueStorage
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="AzureCommandQueue" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="azureQueueSetup">The <see cref="AzureQueueOptions" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddAzureCommandQueue(this IServiceCollection services, Action<AzureQueueOptions> azureQueueSetup, Action<ServiceOptions> serviceSetup = null)
        {
            return services
                    .AddMessageQueue<AzureCommandQueue, ICommand>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(azureQueueSetup);
        }

        /// <summary>
        /// Adds an <see cref="AzureCommandQueue{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="azureQueueSetup">The <see cref="AzureQueueOptions{TMarker}" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddAzureCommandQueue<TMarker>(this IServiceCollection services, Action<AzureQueueOptions<TMarker>> azureQueueSetup, Action<ServiceOptions> serviceSetup = null)
        {
            return services
                    .AddMessageQueue<AzureCommandQueue<TMarker>, ICommand>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(azureQueueSetup);
        }

        /// <summary>
        /// Adds an <see cref="AzureEventBus" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="azureQueueSetup">The <see cref="AzureQueueOptions" /> that needs to be configured.</param>
        /// <param name="azureEventBusSetup">The <see cref="AzureEventBusOptions" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddAzureEventBus(this IServiceCollection services, Action<AzureQueueOptions> azureQueueSetup, Action<AzureEventBusOptions> azureEventBusSetup, Action<ServiceOptions> serviceSetup = null)
        {
            return services
                    .AddMessageBus<AzureEventBus, IIntegrationEvent>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(azureQueueSetup)
                    .AddConfiguredOptions(azureEventBusSetup);
        }

        /// <summary>
        /// Adds an <see cref="AzureEventBus{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="azureQueueSetup">The <see cref="AzureQueueOptions{TMarker}" /> that needs to be configured.</param>
        /// <param name="azureEventBusSetup">The <see cref="AzureEventBusOptions{TMarker}" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddAzureEventBus<TMarker>(this IServiceCollection services, Action<AzureQueueOptions<TMarker>> azureQueueSetup, Action<AzureEventBusOptions<TMarker>> azureEventBusSetup, Action<ServiceOptions> serviceSetup = null)
        {
            return services
                    .AddMessageBus<AzureEventBus<TMarker>, IIntegrationEvent>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton))
                    .AddConfiguredOptions(azureQueueSetup)
                    .AddConfiguredOptions(azureEventBusSetup);
        }
    }
}
