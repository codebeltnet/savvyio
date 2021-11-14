namespace Savvyio.Commands
{
    /// <summary>
    /// Provides a default implementation of the <see cref="ICommand"/> interface.
    /// </summary>
    /// <seealso cref="ICommand" />
    /// <seealso cref="Model"/>
    public abstract class Command : Model, ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        protected Command()
        {
        }
    }
}
