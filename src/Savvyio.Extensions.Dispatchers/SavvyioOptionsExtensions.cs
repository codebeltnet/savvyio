using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Queries;

namespace Savvyio.Extensions.Dispatchers
{
    /// <summary>
    /// Extension methods for the <see cref="SavvyioOptions"/> class.
    /// </summary>
    public static class SavvyioOptionsExtensions
    {
        /// <summary>
        /// Adds an implementation of the <see cref="IMediator"/> interface.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="options">The <see cref="SavvyioOptions"/> to extend.</param>
        /// <returns>A reference to <paramref name="options"/> after the operation has completed.</returns>
        public static SavvyioOptions AddMediator<TImplementation>(this SavvyioOptions options) where TImplementation : class, IMediator
        {
            options.AddDispatcher<IMediator, TImplementation>();
            options.AddDispatcher<ICommandDispatcher, TImplementation>();
            options.AddDispatcher<IDomainEventDispatcher, TImplementation>();
            options.AddDispatcher<IIntegrationEventDispatcher, TImplementation>();
            options.AddDispatcher<IQueryDispatcher, TImplementation>();
            return options;
        }
    }
}
