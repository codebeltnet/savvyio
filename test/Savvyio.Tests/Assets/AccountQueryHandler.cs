using System;
using System.Threading;
using System.Threading.Tasks;
using Savvyio.Assets.Queries;
using Savvyio.Queries;

namespace Savvyio.Assets
{
    public class AccountQueryHandler : QueryHandler
    {
        protected override void RegisterQueryHandlers(IRequestReplyRegistry<IQuery> handler)
        {
            handler.RegisterAsync<GetAccount, string>(GetAccountAsync);
            handler.RegisterAsync<GetAccount, string>(HandlerAsync);
            handler.Register<GetAccount, string>(Handler);
        }

        private Task<string> HandlerAsync(GetAccount arg1, CancellationToken arg2)
        {
            throw new NotImplementedException();
        }

        private string Handler(GetAccount arg)
        {
            throw new NotImplementedException();
        }

        private Task<string> GetAccountAsync(GetAccount arg1)
        {
            return Task.FromResult($"Result from: {arg1.Id}");
        }
    }
}
