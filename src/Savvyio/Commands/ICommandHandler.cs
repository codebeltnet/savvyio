namespace Savvyio.Commands
{
    /// <summary>
    /// Specifies a handler resposible for objects that implements the <see cref="ICommand"/> interface.
    /// </summary>
    /// <seealso cref="IHandler{ICommand}" />
    public interface ICommandHandler : IHandler<ICommand>
    {
        /// <summary>
        /// Gets the activator responsible of invoking delegates that handles <see cref="ICommand"/>.
        /// </summary>
        /// <value>The activator responsible of invoking delegates that handles <see cref="ICommand"/>.</value>
        IHandlerActivator<ICommand> Commands { get; }
    }
}
