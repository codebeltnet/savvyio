using System.Linq;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SQS;

namespace Savvyio.Extensions.SimpleQueueService
{
    /// <summary>
    /// Extension methods for the <see cref="ClientConfig"/> base class.
    /// </summary>
    public static class ClientConfigExtensions
    {
        /// <summary>
        /// Evaluates whether the specified <paramref name="configurations"/>> is valid, e.g. if it has a length of 2 and each element is of type <see cref="AmazonSQSConfig"/> or <see cref="AmazonSimpleNotificationServiceConfig"/>.
        /// </summary>
        /// <param name="configurations">The array of <see cref="ClientConfig"/> to extend.</param>
        /// <returns><c>true</c> if the specified configurations is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValid(this ClientConfig[] configurations)
        {
            return configurations != null &&
                   configurations.Length == 2 &&
                   configurations.All(config => config is AmazonSQSConfig || config is AmazonSimpleNotificationServiceConfig);
        }

        /// <summary>
        /// Resolves an instance of <see cref="AmazonSQSConfig"/> from the specified <paramref name="configurations"/>.
        /// </summary>
        /// <param name="configurations">The array of <see cref="ClientConfig"/> to extend.</param>
        /// <returns>A configured instance of <see cref="AmazonSQSConfig"/>.</returns>
        public static AmazonSQSConfig SimpleQueueService(this ClientConfig[] configurations)
        {
            return configurations.SingleOrDefault(config => config.GetType() == typeof(AmazonSQSConfig)) as AmazonSQSConfig;
        }

        /// <summary>
        /// Resolves an instance of <see cref="AmazonSimpleNotificationServiceConfig"/> from the specified <paramref name="configurations"/>.
        /// </summary>
        /// <param name="configurations">The array of <see cref="ClientConfig"/> to extend.</param>
        /// <returns>A configured instance of <see cref="AmazonSimpleNotificationServiceConfig"/>.</returns>
        public static AmazonSimpleNotificationServiceConfig SimpleNotificationService(this ClientConfig[] configurations)
        {
            return configurations.SingleOrDefault(config => config.GetType() == typeof(AmazonSimpleNotificationServiceConfig)) as AmazonSimpleNotificationServiceConfig;
        }
    }
}
