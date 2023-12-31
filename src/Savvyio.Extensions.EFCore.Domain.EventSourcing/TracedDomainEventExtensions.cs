using Cuemon.Extensions.IO;
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
        /// <param name="serializerContext">The <see cref="ISerializerContext"/> that is used when converting <see cref="ITracedDomainEvent"/> into an array of bytes.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="domainEvent"/>.</returns>
        public static byte[] ToByteArray(this ITracedDomainEvent domainEvent, ISerializerContext serializerContext)
        {
            var bytes = serializerContext.Serialize(domainEvent, typeof(ITracedDomainEvent)).ToByteArray();
            return bytes;
        }
    }
}
