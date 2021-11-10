using System;

namespace Savvyio.Events
{
    /// <summary>
    /// Specifies something that happened when an Aggregate is successfully persisted and you want other subsystems (out-process/inter-application) to be aware of.
    /// </summary>
    public interface IIntegrationEvent
    {
        /// <summary>
        /// Gets the time of occurrence of the event.
        /// </summary>
        /// <value>The time of occurrence of the event.</value>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        string Type { get; }
    }
}
