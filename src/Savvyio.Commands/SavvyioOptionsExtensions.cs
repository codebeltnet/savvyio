namespace Savvyio.Commands
{
    /// <summary>
    /// Extension methods for the <see cref="SavvyioOptions"/> class.
    /// </summary>
    public static class SavvyioOptionsExtensions
    {
        /// <summary>
        /// Adds an implementation of the <see cref="ICommandHandler"/> interface to <see cref="SavvyioOptions.HandlerImplementationTypes"/> (if not already registered).
        /// </summary>
        /// <typeparam name="TImplementation">The type that implements the <see cref="ICommandHandler"/> interface.</typeparam>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> so that additional configuration calls can be chained.</returns>
        public static SavvyioOptions AddCommandHandler<TImplementation>(this SavvyioOptions options) where TImplementation : class, ICommandHandler
        {
            options.AddHandler<ICommandHandler, ICommand, TImplementation>();
            return options;
        }

        /// <summary>
        /// Adds a default implementation of the <see cref="ICommandDispatcher"/> interface.
        /// </summary>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> so that additional configuration calls can be chained.</returns>
        public static SavvyioOptions AddCommandDispatcher(this SavvyioOptions options)
        {
            return options.AddDispatcher<ICommandDispatcher, CommandDispatcher>();
        }
    }
}
