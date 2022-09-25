using System;

namespace Savvyio.Domain.EventSourcing
{
    /// <summary>
    /// Provides a default implementation of something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of.
    /// </summary>
    /// <seealso cref="DomainEvent" />
    /// <seealso cref="ITracedDomainEvent" />
    public abstract record TracedDomainEvent : DomainEvent, ITracedDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ITracedDomainEvent" /> class.
        /// </summary>
        /// <param name="eventId">The optional identifier of the event. Default is an auto-generated UUID.</param>
        /// <param name="type">The optional type of the event. Default is the type of this instance.</param>
        /// <param name="metadata">The optional metadata to merge with this instance.</param>
        /// <remarks>
        /// The following table shows the initial metadata values for an instance of <see cref="TracedDomainEvent"/>.
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
        ///     <item>
        ///         <term><see cref="MetadataDictionary.MemberType"/></term>
        ///         <description><c>type ?? GetType()</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        protected TracedDomainEvent(string eventId = null, Type type = null, IMetadata metadata = null) : base(eventId, metadata)
        {
            this.SetMemberType(type ?? GetType());
        }
    }
}
