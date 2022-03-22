using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Extensions.DependencyInjection.Storage;
using Savvyio.Storage;

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
