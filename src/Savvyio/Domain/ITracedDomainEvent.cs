namespace Savvyio.Domain
{
    public interface ITracedDomainEvent : IDomainEvent
    {
        long Version { get; set; }

        string Type { get; }
    }
}
