namespace Savvyio.Domain.EventSourcing
{
    /// <summary>
    /// Specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be aware of.
    /// </summary>
    public interface ITracedDomainEvent : IDomainEvent
    {
    }
}
