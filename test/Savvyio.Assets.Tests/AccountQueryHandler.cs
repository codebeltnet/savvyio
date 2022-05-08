using System.Threading.Tasks;
using Cuemon;
using Savvyio.Assets.Events;
using Savvyio.Assets.Queries;
using Savvyio.Data;
using Savvyio.Extensions.Dapper;
using Savvyio.Handlers;
using Savvyio.Queries;

namespace Savvyio.Assets
{
    public class AccountQueryHandler : QueryHandler
    {
        private readonly IReadableDataAccessObject<AccountCreated, DapperOptions> _accountDao;

        public AccountQueryHandler(IReadableDataAccessObject<AccountCreated, DapperOptions> accountDao = null)
        {
            _accountDao = accountDao;
        }

        protected override void RegisterDelegates(IRequestReplyRegistry<IQuery> handlers)
        {
            handlers.RegisterAsync<GetAccount, AccountCreated>(GetAccountAsync);
            handlers.RegisterAsync<GetFakeAccount, AccountCreated>(GetFakeAccountAsync);
        }

        private Task<AccountCreated> GetFakeAccountAsync(GetFakeAccount a)
        {
            return Task.FromResult(new AccountCreated(a.Id, $"{a.Id}___{Generate.RandomString(16)}", $"{a.Id}@no.where"));
        }

        private async Task<AccountCreated> GetAccountAsync(GetAccount arg1)
        {
            var dao = await _accountDao.ReadAsync(null, o =>
            {
                o.CommandText = "SELECT * FROM Account WHERE Id = @Id";
                o.Parameters = new { arg1.Id };
            });
            return dao;
        }

    }
}
