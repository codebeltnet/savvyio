namespace Savvyio.Domain
{
    /// <summary>
    /// A marker interface that specifies something that happened in the domain that you want other parts of the same domain (in-process/inner-application) to be made aware of.
    /// </summary>
    public interface IDomainEvent : IEvent
    {
    }
}
