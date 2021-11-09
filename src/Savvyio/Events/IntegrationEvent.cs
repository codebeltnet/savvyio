using System;
using Cuemon.Extensions.Reflection;

namespace Savvyio.Events
{
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        protected IntegrationEvent(Type type = null)
        {
            Timestamp = DateTime.UtcNow;
            Type = (type ?? GetType()).ToFullNameIncludingAssemblyName();
        }

        /// <summary>
        /// Gets the time of occurrence of a particular event, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>The time of occurrence of a particular event.</value>
        public DateTime Timestamp { get; }

        public string Type { get; }
    }
}
