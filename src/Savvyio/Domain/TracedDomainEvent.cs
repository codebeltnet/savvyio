using System;
using Cuemon.Extensions.Reflection;

namespace Savvyio.Domain
{
    public abstract class TracedDomainEvent : DomainEvent, ITracedDomainEvent
    {
        protected TracedDomainEvent(Type type = null)
        {
            Type = (type ?? GetType()).ToFullNameIncludingAssemblyName();
        }

        public long Version { get; set; }

        public string Type { get; }
    }
}
