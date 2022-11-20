using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Messaging;

namespace Savvyio.Commands.Messaging
{
    /// <summary>
    /// Specifies the producer/sender of a bus used for interacting with other subsystems (out-process/inter-application) to do something (e.g. change the state).
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        /// Sends the specified <paramref name="command"/> asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="command">The enclosed <see cref="ICommand"/> to send.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SendAsync(IMessage<ICommand> command, Action<AsyncOptions> setup = null);
    }
}
