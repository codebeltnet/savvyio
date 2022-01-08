namespace Savvyio.EventDriven
{
    /// <summary>
    /// Extension methods for the <see cref="SavvyioOptions"/> class.
    /// </summary>
    public static class SavvyioOptionsExtensions
    {
        /// <summary>
        /// Adds an implementation of the <see cref="IIntegrationEventHandler"/> interface to <see cref="SavvyioOptions.HandlerImplementationTypes"/> (if not already registered).
        /// </summary>
        /// <typeparam name="T">The type that implements the <see cref="IIntegrationEventHandler"/> interface.</typeparam>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> after the operation has completed.</returns>
        public static SavvyioOptions AddIntegrationEventHandler<T>(this SavvyioOptions options) where T : class, IIntegrationEventHandler
        {
            options.AddHandler<IIntegrationEventHandler, IIntegrationEvent, T>();
            return options;
        }
    }
}
