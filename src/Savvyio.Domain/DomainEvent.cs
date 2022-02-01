using System;

namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IDomainEvent"/> interface.
    /// </summary>
    /// <seealso cref="IDomainEvent" />
    /// <seealso cref="Request"/>
    public abstract class DomainEvent : Request, IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEvent" /> class.
        /// </summary>
        /// <param name="eventId">The optional identifier of the event. Default is an auto-generated UUID.</param>
        /// <param name="metadata">The optional metadata to merge with this instance.</param>
        /// <remarks>
        /// The following table shows the initial metadata values for an instance of <see cref="DomainEvent"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Key</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="MetadataDictionary.EventId"/></term>
        ///         <description><c>eventId ?? Guid.NewGuid().ToString("N")</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="MetadataDictionary.Timestamp"/></term>
        ///         <description><c>DateTime.UtcNow</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        protected DomainEvent(string eventId = null, IMetadata metadata = null)
        {
            this.SetEventId(eventId ?? Guid.NewGuid().ToString("N"));
            this.SetTimestamp();
            this.MergeMetadata(metadata);
        }
    }
}
