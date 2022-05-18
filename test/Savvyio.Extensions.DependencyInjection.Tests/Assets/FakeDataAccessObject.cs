using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Data;
using Savvyio.Extensions.DependencyInjection.Data;

namespace Savvyio.Extensions.DependencyInjection.Assets
{
    public class FakeDataAccessObject<T> : IPersistentDataAccessObject<T, AsyncOptions> where T : class
    {
        public FakeDataAccessObject(IDataStore ds)
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

        public Task<IEnumerable<T>> ReadAllAsync(Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> ReadAsync(Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeDataAccessObject<T, TMarker> : FakeDataAccessObject<T>, IPersistentDataAccessObject<T, AsyncOptions, TMarker> where T : class
    {
        public FakeDataAccessObject(IDataStore<TMarker> ds) : base(ds)
        {
        }
    }
}
