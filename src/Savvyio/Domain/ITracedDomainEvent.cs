namespace Savvyio.Domain
{
    /// <summary>
    /// Specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of.
    /// </summary>
    public interface ITracedDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Gets or sets the version of the associated <see cref="ITracedAggregateRoot{TKey}"/>.
        /// </summary>
        /// <value>The version of the event.</value>
        //long AggregateVersion { get; set; }
    }
}
