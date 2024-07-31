using Savvyio.Handlers;

namespace Savvyio.Queries
{
    /// <summary>
    /// Provides a generic and consistent way of handling Query objects that implements the <see cref="IQuery"/> interface. This is an abstract class.
    /// </summary>
    /// <seealso cref="IQueryHandler" />
    public abstract class QueryHandler : IQueryHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryHandler"/> class.
        /// </summary>
        protected QueryHandler()
        {
            Delegates = HandlerFactory.CreateRequestReply<IQuery>(RegisterDelegates);
        }

        /// <summary>
        /// Registers the delegates responsible of handling types that implements the <see cref="IQuery"/> interface.
        /// </summary>
        /// <param name="handlers">The registry that store the delegates of type <see cref="IQuery"/>.</param>
        protected abstract void RegisterDelegates(IRequestReplyRegistry<IQuery> handlers);

        /// <summary>
        /// Gets the activator responsible of invoking delegates that handles <see cref="IQuery" />.
        /// </summary>
        /// <value>The activator responsible of invoking delegates that handles <see cref="IQuery" />.</value>
        public IRequestReplyActivator<IQuery> Delegates { get; }
    }
}
