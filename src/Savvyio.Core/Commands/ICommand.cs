namespace Savvyio.Commands
{
    /// <summary>
    /// A marker interface that specifies an intention to do something (e.g. change the state).
    /// </summary>
    public interface ICommand : IRequest, IMetadata
    {
    }
}
