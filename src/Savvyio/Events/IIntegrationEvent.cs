namespace Savvyio.Events
{
    /// <summary>
    /// Specifies something that happened when an Aggregate is successfully persisted and you want other subsystems (out-process/inter-application) to be aware of.
    /// </summary>
    public interface IIntegrationEvent : IMetadata
    {
    }
}
