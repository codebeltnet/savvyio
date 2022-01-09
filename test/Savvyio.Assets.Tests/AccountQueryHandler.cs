using System.Threading.Tasks;
using Savvyio.Assets.Queries;
using Savvyio.Queries;

namespace Savvyio.Assets
{
    public class AccountQueryHandler : QueryHandler
    {
        protected override void RegisterDelegates(IRequestReplyRegistry<IQuery> handlers)
        {
            handlers.RegisterAsync<GetAccount, string>(GetAccountAsync);
        }

        private Task<string> GetAccountAsync(GetAccount arg1)
        {
            return Task.FromResult($"Result from: {arg1.Id}");
        }

    }
    
}
