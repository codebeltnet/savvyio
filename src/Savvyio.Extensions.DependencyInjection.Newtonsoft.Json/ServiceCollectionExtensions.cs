using System;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Savvyio.Extensions.Newtonsoft.Json;

namespace Savvyio.Extensions.DependencyInjection.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="NewtonsoftJsonMarshaller" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="jsonSetup">The <see cref="NewtonsoftJsonFormatterOptions" /> which may be configured. Default is optimized for messaging.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured. Default is <see cref="ServiceLifetime.Singleton"/>.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddNewtonsoftJsonMarshaller(this IServiceCollection services, Action<NewtonsoftJsonFormatterOptions> jsonSetup = null, Action<ServiceOptions> serviceSetup = null)
        {
            Validator.ThrowIfNull(services);
            return services
                .AddMarshaller<NewtonsoftJsonMarshaller>(serviceSetup)
                .AddConfiguredOptions(jsonSetup ?? (o => o.Settings.Formatting = Formatting.None));
        }
    }
}
