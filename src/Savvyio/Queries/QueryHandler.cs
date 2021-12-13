namespace Savvyio.Queries
{
    public abstract class QueryHandler : IQueryHandler
    {
        private readonly ResponseHandlerManager<IQuery> _handlerManager = new();

        protected QueryHandler()
        {
            Initialize();
        }

        private void Initialize()
        {
            RegisterQueryHandlers(_handlerManager);
        }

        protected abstract void RegisterQueryHandlers(IResponseHandlerRegistry<IQuery> handler);

        public IResponseHandlerActivator<IQuery> Queries => _handlerManager;
    }
}
