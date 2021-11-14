using Savvyio.Messaging;

namespace Savvyio.Domain
{
    /// <summary>
    /// Specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of.
    /// </summary>
    public interface ITracedDomainMessage : IMessage<ITracedDomainEvent>
    {
        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        string Type { get; }
    }
}
