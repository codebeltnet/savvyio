using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Data;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Data;

namespace Savvyio.Extensions.Assets
{
    public class FakeDataAccessObject<T> : IPersistentDataAccessObject<T> where T : class
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

        public Task<IEnumerable<T>> ReadAllAsync(Expression<Func<T, bool>> predicate = null, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> ReadAsync(Expression<Func<T, bool>> predicate, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeDataAccessObject<T, TMarker> : FakeDataAccessObject<T>, IPersistentDataAccessObject<T, TMarker> where T : class
    {
        public FakeDataAccessObject(IDataStore<TMarker> ds) : base(ds)
        {
        }
    }
}
