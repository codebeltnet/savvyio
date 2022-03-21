using System.Threading.Tasks;
using Savvyio.Handlers;

namespace Savvyio.Queries.Assets
{
    internal class DefaultQueryHandler : QueryHandler
    {
        public DefaultQueryHandler()
        {
        }


        protected override void RegisterDelegates(IRequestReplyRegistry<IQuery> handlers)
        {
            handlers.Register<DefaultQuery<string>, string>(_ => nameof(DefaultQuery<string>));
            handlers.RegisterAsync<DefaultQuery<string>, string>(_ => Task.FromResult(nameof(DefaultQuery<string>) + "Async"));
        }
    }
}
