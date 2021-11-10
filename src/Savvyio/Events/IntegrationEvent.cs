using System;
using Cuemon.Extensions.Reflection;

namespace Savvyio.Events
{
    /// <summary>
    /// Provides a default implementation of something that happened when an Aggregate is successfully persisted and you want other subsystems (out-process/inter-application) to be aware of.
    /// </summary>
    /// <seealso cref="IIntegrationEvent" />
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEvent"/> class.
        /// </summary>
        /// <param name="type">The optional type of the event.</param>
        protected IntegrationEvent(Type type = null)
        {
            Timestamp = DateTime.UtcNow;
            Type = (type ?? GetType()).ToFullNameIncludingAssemblyName();
        }

        /// <summary>
        /// Gets the time of occurrence of the event, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>The time of occurrence of the event.</value>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        public string Type { get; }
    }
}
