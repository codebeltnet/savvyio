using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Storage;

namespace Savvyio.Extensions.Storage.Assets
{
    public class FakeDataStore : IDataStore
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
