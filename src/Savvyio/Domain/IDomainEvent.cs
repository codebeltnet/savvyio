using System;

namespace Savvyio.Domain
{
    /// <summary>
    /// Specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the time of occurrence of the event.
        /// </summary>
        /// <value>The time of occurrence of the event.</value>
        DateTime Timestamp { get; }
    }
}
