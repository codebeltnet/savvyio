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
        /// <typeparam name="TImplementation">The type that implements the <see cref="IIntegrationEventHandler"/> interface.</typeparam>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> so that additional configuration calls can be chained.</returns>
        public static SavvyioOptions AddIntegrationEventHandler<TImplementation>(this SavvyioOptions options) where TImplementation : class, IIntegrationEventHandler
        {
            options.AddHandler<IIntegrationEventHandler, IIntegrationEvent, TImplementation>();
            return options;
        }

        /// <summary>
        /// Adds a default implementation of the <see cref="IIntegrationEventDispatcher"/> interface.
        /// </summary>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> so that additional configuration calls can be chained.</returns>
        public static SavvyioOptions AddIntegrationEventDispatcher(this SavvyioOptions options)
        {
            return options.AddDispatcher<IIntegrationEventDispatcher, IntegrationEventDispatcher>();
        }
    }
}
