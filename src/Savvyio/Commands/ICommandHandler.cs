namespace Savvyio.Commands
{
    public interface ICommandHandler : IHandler<ICommand>
    {
        IHandlerActivator<ICommand> Commands { get; }
    }
}
