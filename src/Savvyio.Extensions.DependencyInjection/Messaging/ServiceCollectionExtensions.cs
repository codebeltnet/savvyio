using System;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Messaging;

namespace Savvyio.Extensions.DependencyInjection.Messaging
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an implementation of <see cref="IPointToPointChannel{TRequest}" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="IPointToPointChannel{TRequest}"/> to add.</typeparam>
        /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TService"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IPointToPointChannel{TRequest}"/>
        /// <seealso cref="IPointToPointChannel{TRequest,TMarker}"/>
        public static IServiceCollection AddMessageQueue<TService, TRequest>(this IServiceCollection services, Action<ServiceOptions> setup = null) where TService : class, IPointToPointChannel<TRequest> where TRequest : IRequest
        {
            Validator.ThrowIfNull(services);
            var options = Patterns.Configure(setup, o => o.Lifetime = ServiceLifetime.Singleton);
            return services.Add<TService>(o =>
            {
                o.Lifetime = options.Lifetime;
                o.NestedTypePredicate = type => type.HasInterfaces(typeof(IPointToPointChannel<>), typeof(IReceiver<>), typeof(ISender<>));
            });
        }

        /// <summary>
        /// Adds an implementation of <see cref="IPublishSubscribeChannel{TRequest}" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="IPublishSubscribeChannel{TRequest}"/> to add.</typeparam>
        /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TService"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IPublishSubscribeChannel{TRequest}"/>
        /// <seealso cref="IPublishSubscribeChannel{TRequest,TMarker}"/>
        public static IServiceCollection AddMessageBus<TService, TRequest>(this IServiceCollection services, Action<ServiceOptions> setup = null) where TService : class, IPublishSubscribeChannel<TRequest> where TRequest : IRequest
        {
            Validator.ThrowIfNull(services);
            var options = Patterns.Configure(setup, o => o.Lifetime = ServiceLifetime.Singleton);
            return services.Add<TService>(o =>
            {
                o.Lifetime = options.Lifetime;
                o.NestedTypePredicate = type => type.HasInterfaces(typeof(IPublishSubscribeChannel<>), typeof(ISubscriber<>), typeof(IPublisher<>));
            });
        }
    }
}
