using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Data;
using Savvyio.Extensions.DependencyInjection.Data;

namespace Savvyio.Extensions.DependencyInjection.Assets
{
    public class FakeDataStore<T> : IPersistentDataStore<T, AsyncOptions> where T : class
    {
        public FakeDataStore(IDataSource ds)
        {

        }

        public Task CreateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T dto, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> FindAllAsync(Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindAsync(Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(object id, Action<AsyncOptions> setup)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeDataStore<T, TMarker> : FakeDataStore<T>, IPersistentDataStore<T, AsyncOptions, TMarker> where T : class
    {
        public FakeDataStore(IDataSource<TMarker> ds) : base(ds)
        {
        }
    }
}
