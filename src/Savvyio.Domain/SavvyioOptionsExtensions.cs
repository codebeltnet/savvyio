namespace Savvyio.Domain
{
    /// <summary>
    /// Extension methods for the <see cref="SavvyioOptions"/> class.
    /// </summary>
    public static class SavvyioOptionsExtensions
    {
        /// <summary>
        /// Adds an implementation of the <see cref="IDomainEventHandler"/> interface to <see cref="SavvyioOptions.HandlerImplementationTypes"/> (if not already registered).
        /// </summary>
        /// <typeparam name="TImplementation">The type that implements the <see cref="IDomainEventHandler"/> interface.</typeparam>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> so that additional configuration calls can be chained.</returns>
        public static SavvyioOptions AddDomainEventHandler<TImplementation>(this SavvyioOptions options) where TImplementation : class, IDomainEventHandler
        {
            return options.AddHandler<IDomainEventHandler, IDomainEvent, TImplementation>();
        }

        /// <summary>
        /// Adds a default implementation of the <see cref="IDomainEventDispatcher"/> interface.
        /// </summary>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> so that additional configuration calls can be chained.</returns>
        public static SavvyioOptions AddDomainEventDispatcher(this SavvyioOptions options)
        {
            return options.AddDispatcher<IDomainEventDispatcher, DomainEventDispatcher>();
        }
    }
}
