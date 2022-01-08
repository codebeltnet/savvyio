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
        /// <typeparam name="T">The type of the <see cref="IQueryHandler"/> implementation.</typeparam>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> after the operation has completed.</returns>
        public static SavvyioOptions AddQueryHandler<T>(this SavvyioOptions options) where T : class, IQueryHandler
        {
            options.AddHandler<IQueryHandler, IQuery, T>();
            return options;
        }
    }
}
