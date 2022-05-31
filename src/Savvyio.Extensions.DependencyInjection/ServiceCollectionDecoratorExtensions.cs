using System;
using System.Linq;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Savvyio.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface hidden by the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    /// <remarks>This API supports the product infrastructure and is not intended to be used directly from your code.</remarks>
    public static class ServiceCollectionDecoratorExtensions
    {
        /// <summary>
        /// Adds an implementation of <typeparamref name="TService"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{IServiceCollection}" /> to extend.</param>
        /// <param name="predicate">The function delegate to test each element for a condition based on a <see cref="Type"/>.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to the enclosed <see cref="IServiceCollection"/> of the specified <paramref name="decorator"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TService"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly. This API supports the product infrastructure and is not intended to be used directly from your code.</remarks>
        public static IServiceCollection AddWithNestedTypeForwarding<TService>(this IDecorator<IServiceCollection> decorator, Func<Type, bool> predicate, Action<ServiceOptions> setup = null) where TService : class
        {
            var options = setup.Configure();
            decorator.Inner.TryAdd<TService, TService>(options.Lifetime);
            var hasMarkerType = typeof(TService).TryGetDependencyInjectionMarker(out _);
            var groupTypes = typeof(TService).GetInterfaces().Where(predicate).GroupBy(type => type.ToFriendlyName(o => o.ExcludeGenericArguments = true));
            foreach (var groupType in groupTypes)
            {
                decorator.Inner.TryAdd(hasMarkerType ? groupType.Last() : groupType.First(), p => p.GetRequiredService<TService>(), options.Lifetime);
            }
            return decorator.Inner;
        }
    }
}
