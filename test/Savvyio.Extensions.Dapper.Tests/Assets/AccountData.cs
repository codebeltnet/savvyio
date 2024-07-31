using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Cuemon.Threading;
using Dapper;
using Savvyio.Assets.Domain;

namespace Savvyio.Extensions.Dapper.Assets
{
    public class AccountData : DapperDataStore<Account, DapperQueryOptions>
    {
        public AccountData(IDapperDataSource source) : base(source)
        {
        }

        public override Task CreateAsync(Account dto, Action<AsyncOptions> setup = null)
        {
            return Source.ExecuteAsync("INSERT INTO AccountProjection (FullName, EmailAddress) VALUES (@FullName, @EmailAddress)", dto);
        }

        public override Task UpdateAsync(Account dto, Action<AsyncOptions> setup = null)
        {
            return Source.ExecuteAsync("UPDATE AccountProjection SET FullName = @FullName WHERE Id = @Id", dto);
        }

        public override Task<Account> GetByIdAsync(object id, Action<AsyncOptions> setup)
        {
            return Source.QuerySingleOrDefaultAsync<Account>("SELECT * FROM AccountProjection WHERE Id = @Id", id);
        }

        public override async Task<IEnumerable<Account>> FindAllAsync(Action<DapperQueryOptions> setup = null)
        {
            var cd = setup.Configure();
            return setup == null ? await Source.QueryAsync<Account>("SELECT * FROM AccountProjection").ConfigureAwait(false) : await Source.QueryAsync<Account>(cd).ConfigureAwait(false);
        }

        public override Task DeleteAsync(Account dto, Action<AsyncOptions> setup = null)
        {
            return Source.ExecuteAsync("DELETE FROM AccountProjection WHERE Id = @Id", dto);
        }
    }
}
