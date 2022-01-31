using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Extensions.DependencyInjection.Storage;
using Savvyio.Storage;

namespace Savvyio.Extensions.Storage.Assets
{
    public class FakeRepository<TEntity, TKey> : IPersistentRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
        public FakeRepository(IDataStore ds)
        {
            
        }

        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(TKey id, Action<AsyncOptions> setup = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeRepository<TEntity, TKey, TMarker> : FakeRepository<TEntity, TKey>, IPersistentRepository<TEntity, TKey, TMarker> where TEntity : class, IIdentity<TKey>
    {
        public FakeRepository(IDataStore<TMarker> ds) : base(ds)
        {
            
        }
    }
}
