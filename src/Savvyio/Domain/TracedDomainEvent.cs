using System;
using Cuemon.Extensions.Reflection;

namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a default implementation of something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of.
    /// </summary>
    /// <seealso cref="DomainEvent" />
    /// <seealso cref="ITracedDomainEvent" />
    public abstract class TracedDomainEvent : DomainEvent, ITracedDomainEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ITracedDomainEvent"/> class.
        /// </summary>
        /// <param name="type">The optional type of the event.</param>
        protected TracedDomainEvent(Type type = null)
        {
            Type = (type ?? GetType()).ToFullNameIncludingAssemblyName();
        }

        /// <summary>
        /// Gets or sets the version of the event.
        /// </summary>
        /// <value>The version of the event.</value>
        public long Version { get; set; }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        public string Type { get; }
    }
}
