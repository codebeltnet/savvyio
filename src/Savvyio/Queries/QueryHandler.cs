namespace Savvyio.Queries
{
    public abstract class QueryHandler : IQueryHandler
    {
        private readonly RequestReplyManager<IQuery> _handlerManager = new();

        protected QueryHandler()
        {
            Initialize();
        }

        private void Initialize()
        {
            RegisterQueryHandlers(_handlerManager);
        }

        protected abstract void RegisterQueryHandlers(IRequestReplyRegistry<IQuery> handler);

        public IRequestReplyActivator<IQuery> Queries => _handlerManager;
    }
}
