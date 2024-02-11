namespace Savvyio.EventDriven
{
    /// <summary>
    /// A marker interface that specifies something that happened when an object was successfully persisted and you want other subsystems (out-process/inter-application) to be made aware of.
    /// </summary>
    public interface IIntegrationEvent : IRequest, IMetadata
    {
    }
}
