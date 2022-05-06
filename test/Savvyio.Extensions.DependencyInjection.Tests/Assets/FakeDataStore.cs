using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Data;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Data;

namespace Savvyio.Extensions.Assets
{
    public class FakeDataStore : IDataStore, IUnitOfWork
    {
        public Task SaveChangesAsync(Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeDataStore<TMarker> : FakeDataStore, IDataStore<TMarker>
    {
    }
}
