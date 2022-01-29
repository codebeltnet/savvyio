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
        /// <typeparam name="T">The type that implements the <see cref="IDomainEventHandler"/> interface.</typeparam>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> after the operation has completed.</returns>
        public static SavvyioOptions AddDomainEventHandler<T>(this SavvyioOptions options) where T : class, IDomainEventHandler
        {
            return options.AddHandler<IDomainEventHandler, IDomainEvent, T>();
        }
    }
}
