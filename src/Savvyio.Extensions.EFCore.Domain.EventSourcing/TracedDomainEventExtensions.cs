using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Savvyio.Domain.EventSourcing;

namespace Savvyio.Extensions.EFCore.Domain.EventSourcing
{
    /// <summary>
    /// Extension methods for the <see cref="ITracedDomainEvent"/> interface.
    /// </summary>
    public static class TracedDomainEventExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="domainEvent"/> into an array of bytes.
        /// </summary>
        /// <param name="domainEvent">The domain event to convert.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="domainEvent"/>.</returns>
        public static byte[] ToByteArray(this ITracedDomainEvent domainEvent)
        {
            var formatter = new NewtonsoftJsonFormatter();
            EfCoreTracedAggregateEntity.RemoveRedundantEntries(domainEvent.Metadata);
            var bytes = formatter.Serialize(domainEvent, typeof(ITracedDomainEvent)).ToByteArray();
            return bytes;
        }
    }
}
