using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Cuemon.Threading;
using Dapper;
using Savvyio.Assets.Queries;
using Savvyio.Extensions.Dapper;

namespace Savvyio.Assets
{
    public class AccountData : DapperDataStore<AccountProjection, DapperQueryOptions>
    {
        public AccountData(IDapperDataSource source) : base(source)
        {
        }

        public override Task CreateAsync(AccountProjection dto, Action<AsyncOptions> setup = null)
        {
            return Source.ExecuteAsync("INSERT INTO AccountProjection (Id, FullName, EmailAddress) VALUES (@Id, @FullName, @EmailAddress)", dto);
        }

        public override Task UpdateAsync(AccountProjection dto, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public override Task<AccountProjection> GetByIdAsync(object id, Action<AsyncOptions> setup)
        {
            return Source.QuerySingleOrDefaultAsync<AccountProjection>("SELECT * FROM AccountProjection WHERE Id = @Id", new { Id = id });
        }

        public override Task<IEnumerable<AccountProjection>> FindAllAsync(Action<DapperQueryOptions> setup = null)
        {
            return setup == null ? Source.QueryAsync<AccountProjection>("SELECT * FROM AccountProjection") : Source.QueryAsync<AccountProjection>(setup.Configure());
        }

        public override Task DeleteAsync(AccountProjection dto, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }
    }
}
