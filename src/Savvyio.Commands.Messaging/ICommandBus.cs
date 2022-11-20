namespace Savvyio.Commands.Messaging
{
    /// <summary>
    /// Specifies an interface for a bus that is used for interacting with other subsystems (out-process/inter-application) to do something (e.g. change the state).
    /// </summary>
    /// <seealso cref="ICommandSender" />
    /// <seealso cref="ICommandReceiver" />
    public interface ICommandBus : ICommandSender, ICommandReceiver
    {
    }
}
