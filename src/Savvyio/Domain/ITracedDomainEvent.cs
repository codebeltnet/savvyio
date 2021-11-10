namespace Savvyio.Domain
{
    /// <summary>
    /// Specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of.
    /// </summary>
    public interface ITracedDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Gets or sets the version of the event.
        /// </summary>
        /// <value>The version of the event.</value>
        long Version { get; set; }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        string Type { get; }
    }
}
