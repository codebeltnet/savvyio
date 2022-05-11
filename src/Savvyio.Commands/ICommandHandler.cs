using Savvyio.Handlers;

namespace Savvyio.Commands
{
    /// <summary>
    /// Specifies a handler responsible for objects that implements the <see cref="ICommand"/> interface.
    /// </summary>
    /// <seealso cref="IFireForgetHandler{TRequest}" />
    public interface ICommandHandler : IFireForgetHandler<ICommand>
    {
    }
}
