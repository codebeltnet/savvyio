using System.Threading.Tasks;
using Cuemon;
using Savvyio.Assets.EventDriven;
using Savvyio.Assets.Queries;
using Savvyio.Data;
using Savvyio.Handlers;
using Savvyio.Queries;

namespace Savvyio.Assets
{
    public class AccountQueryHandler : QueryHandler
    {
        private readonly IReadableDataStore<AccountProjection> _accountDao;

        public AccountQueryHandler(IReadableDataStore<AccountProjection> accountDao = null)
        {
            _accountDao = accountDao;
        }

        protected override void RegisterDelegates(IRequestReplyRegistry<IQuery> handlers)
        {
            handlers.RegisterAsync<GetAccount, AccountProjection>(GetAccountAsync);
            handlers.RegisterAsync<GetAccount, AccountProjection>(s => Task.FromResult(new AccountProjection(222, "A", "B")));
            handlers.RegisterAsync<GetFakeAccount, AccountCreated>(GetFakeAccountAsync);
            handlers.RegisterAsync<GetFakeAccount, AccountCreated>(_ => Task.FromResult(new AccountCreated(222, "A", "B")));
        }

        private Task<AccountCreated> GetFakeAccountAsync(GetFakeAccount a)
        {
            return Task.FromResult(new AccountCreated(a.Id, $"{a.Id}___{Generate.RandomString(16)}", $"{a.Id}@no.where"));
        }

        private async Task<AccountProjection> GetAccountAsync(GetAccount arg1)
        {
            var dao = await _accountDao.GetByIdAsync(arg1.Id);
            return dao;
        }

    }
}
