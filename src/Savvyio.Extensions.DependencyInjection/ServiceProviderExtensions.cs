using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Savvyio.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceProvider"/> interface.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Writes the handler discoveries to <see cref="ILogger{TCategoryName}"/> with <see cref="LogLevel.Information"/>.
        /// </summary>
        /// <typeparam name="TCategoryName">The type used to categorize the log messages.</typeparam>
        /// <param name="provider">The service provider used to resolve services.</param>
        public static void WriteHandlerDiscoveriesToLog<TCategoryName>(this IServiceProvider provider)
        {
            var descriptor = provider.GetRequiredService<IHandlerServicesDescriptor>();
            var logger = provider.GetRequiredService<ILogger<TCategoryName>>();
            logger.LogInformation("{HandlerDiscoveries}", descriptor.ToString());
        }
    }
}
