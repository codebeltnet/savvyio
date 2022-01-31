using Cuemon.Extensions;

namespace Savvyio.Domain.EventSourcing
{
    /// <summary>
    /// Extension methods for the <see cref="ITracedDomainEvent"/> interface.
    /// </summary>
    public static class TracedDomainEventExtensions
    {
        /// <summary>
        /// Assigns a new <paramref name="version"/> to the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="ITracedDomainEvent"/> interface.</typeparam>
        /// <param name="request">The <see cref="ITracedDomainEvent"/> to extend.</param>
        /// <param name="version">The aggregate version of the model.</param>
        /// <returns>A reference to <paramref name="request"/> after the operation has completed.</returns>
        public static T SetAggregateVersion<T>(this T request, long version) where T : ITracedDomainEvent
        {
            return MetadataFactory.Set(request, MetadataDictionary.AggregateVersion, version);
        }

        /// <summary>
        /// Gets the aggregate version from the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="ITracedDomainEvent"/> interface.</typeparam>
        /// <param name="request">The <see cref="ITracedDomainEvent"/> to extend.</param>
        /// <returns>The version of the associated <see cref="ITracedAggregateRoot{TKey}"/>.</returns>
        public static long GetAggregateVersion<T>(this T request) where T : ITracedDomainEvent
        {
            return MetadataFactory.Get(request, MetadataDictionary.AggregateVersion).As<long>();
        }

        /// <summary>
        /// Gets the string representation of the type from the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="ITracedDomainEvent"/> interface.</typeparam>
        /// <param name="request">The <see cref="ITracedDomainEvent"/> to extend.</param>
        /// <returns>The string representation of the type from the <paramref name="request"/>.</returns>
        public static string GetMemberType<T>(this T request) where T : ITracedDomainEvent
        {
            return MetadataFactory.Get(request, MetadataDictionary.MemberType).As<string>();
        }
    }
}
