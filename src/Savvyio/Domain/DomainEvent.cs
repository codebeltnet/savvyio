using System;

namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a default implementation of something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of.
    /// </summary>
    /// <seealso cref="IDomainEvent" />
    public abstract class DomainEvent : IDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEvent" /> class.
        /// </summary>
        protected DomainEvent()
        {
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the time of occurrence of a particular event, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>The time of occurrence of a particular event.</value>
        public DateTime Timestamp { get; }
    }
}
