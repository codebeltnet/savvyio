namespace Savvyio.Queries
{
    /// <summary>
    /// Extension methods for the <see cref="SavvyioOptions"/> class.
    /// </summary>
    public static class SavvyioOptionsExtensions
    {
        /// <summary>
        /// Adds an implementation of the <see cref="IQueryHandler"/> interface to <see cref="SavvyioOptions.HandlerImplementationTypes"/> (if not already registered).
        /// </summary>
        /// <typeparam name="TImplementation">The type that implements the <see cref="IQueryHandler"/> interface.</typeparam>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> so that additional configuration calls can be chained.</returns>
        public static SavvyioOptions AddQueryHandler<TImplementation>(this SavvyioOptions options) where TImplementation : class, IQueryHandler
        {
            options.AddHandler<IQueryHandler, IQuery, TImplementation>();
            return options;
        }

        /// <summary>
        /// Adds a default implementation of the <see cref="IQueryDispatcher"/> interface.
        /// </summary>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> so that additional configuration calls can be chained.</returns>
        public static SavvyioOptions AddQueryDispatcher(this SavvyioOptions options)
        {
            return options.AddDispatcher<IQueryDispatcher, QueryDispatcher>();
        }
    }
}
