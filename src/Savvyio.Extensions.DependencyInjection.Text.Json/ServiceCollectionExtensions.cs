using Cuemon.Extensions.DependencyInjection;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System;
using Cuemon;
using Savvyio.Extensions.Text.Json;

namespace Savvyio.Extensions.DependencyInjection.Text.Json
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="JsonMarshaller" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="jsonSetup">The <see cref="JsonFormatterOptions" /> which may be configured. Default is optimized for messaging.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured. Default is <see cref="ServiceLifetime.Singleton"/>.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddJsonMarshaller(this IServiceCollection services, Action<JsonFormatterOptions> jsonSetup = null, Action<ServiceOptions> serviceSetup = null)
        {
            Validator.ThrowIfNull(services);
            return services
                .AddMarshaller<JsonMarshaller>(serviceSetup)
                .AddConfiguredOptions(jsonSetup ?? (o => o.Settings.WriteIndented = false));
        }
    }
}
