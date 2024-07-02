using System;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven;
using Savvyio.Extensions.SimpleQueueService.Commands;
using Savvyio.Extensions.SimpleQueueService.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.SimpleQueueService
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="AmazonCommandQueue" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="awsSetup">The <see cref="AmazonCommandQueueOptions" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddAmazonCommandQueue(this IServiceCollection services, Action<AmazonCommandQueueOptions> awsSetup, Action<ServiceOptions> serviceSetup = null)
        {
            services.AddMessageQueue<AmazonCommandQueue, ICommand>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton));
            return services.ConfigureTriple(awsSetup);
        }

        /// <summary>
        /// Adds an <see cref="AmazonCommandQueue{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="awsSetup">The <see cref="AmazonCommandQueueOptions{TMarker}" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddAmazonCommandQueue<TMarker>(this IServiceCollection services, Action<AmazonCommandQueueOptions<TMarker>> awsSetup, Action<ServiceOptions> serviceSetup = null)
        {
            services.AddMessageQueue<AmazonCommandQueue<TMarker>, ICommand>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton));
            return services.ConfigureTriple(awsSetup);
        }

        /// <summary>
        /// Adds an <see cref="AmazonEventBus" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="awsSetup">The <see cref="AmazonEventBusOptions" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddAmazonEventBus(this IServiceCollection services, Action<AmazonEventBusOptions> awsSetup, Action<ServiceOptions> serviceSetup = null)
        {
            services.AddMessageBus<AmazonEventBus, IIntegrationEvent>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton));
            return services.ConfigureTriple(awsSetup);
        }

        /// <summary>
        /// Adds an <see cref="AmazonEventBus{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="awsSetup">The <see cref="AmazonEventBusOptions{TMarker}" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddAmazonEventBus<TMarker>(this IServiceCollection services, Action<AmazonEventBusOptions<TMarker>> awsSetup, Action<ServiceOptions> serviceSetup = null)
        {
            services.AddMessageBus<AmazonEventBus<TMarker>, IIntegrationEvent>(serviceSetup ?? (o => o.Lifetime = ServiceLifetime.Singleton));
            return services.ConfigureTriple(awsSetup);
        }
    }
}
