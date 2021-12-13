using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Queries;
using Savvyio.Queries;

namespace Savvyio.Assets
{
    public class AccountQueryHandler : QueryHandler
    {
        protected override void RegisterQueryHandlers(IResponseHandlerRegistry<IQuery> handler)
        {
            handler.RegisterAsync<GetAccount, string>(GetAccountAsync);
            handler.Register<GetAccount, string>(Handler);
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
