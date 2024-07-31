using System;
using Cuemon;
using Cuemon.Extensions;

namespace Savvyio.Extensions.SimpleQueueService.EventDriven
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="source"/> to its equivalent AWS ARN <see cref="Uri"/> representation.
        /// </summary>
        /// <param name="source">The <see cref="string"/> to extend.</param>
        /// <param name="setup">The <see cref="AmazonResourceNameOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Uri"/> that corresponds to <paramref name="source"/> and <paramref name="setup"/>.</returns>
        /// <remarks></remarks>
        public static Uri ToSnsUri(this string source, Action<AmazonResourceNameOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            var arn = $"arn:{options.Partition}:sns:{options.Region}:{options.AccountId}:{source}";
            return arn.ToUri();
        }
    }
}
