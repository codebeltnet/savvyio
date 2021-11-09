using System;

namespace Savvyio.Events
{
    /// <summary>
    /// Specifies something that happened when an aggregate is successfully persisted and you want other subsystems (out-process/inter-application) to be aware of.
    /// </summary>
    public interface IIntegrationEvent
    {
        /// <summary>
        /// Gets the time of occurrence of a particular event.
        /// </summary>
        /// <value>The time of occurrence of a particular event.</value>
        DateTime Timestamp { get; }

        string Type { get; }
    }
}
