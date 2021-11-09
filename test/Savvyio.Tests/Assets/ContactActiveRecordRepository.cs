using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Assets.Domain;
using Savvyio.Domain;

namespace Savvyio.Assets
{
    public class ContactActiveRecordRepository : IActiveRecordRepository<Account, long> 
    {
        public Task<Account> LoadAsync(long id, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Account>> QueryAsync(Expression<Func<Account, bool>> predicate = null, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Account aggregate, Action<AsyncOptions> setup = null)
        {
            
            throw new NotImplementedException();
        }

        public Task RemoveAsync(long id, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }
    }
}
