using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Configuration;
using Cuemon.Extensions;
using Cuemon.Threading;
using Microsoft.Extensions.Options;

namespace Savvyio.Domain
{
    public class InMemoryActiveRecordStore<TAggregate, TKey> : Configurable<InMemoryActiveRecordStoreOptions<TAggregate, TKey>>, IActiveRecordStore<TAggregate, TKey> where TAggregate : class, IAggregateRoot<TKey>
    {
        private readonly List<TAggregate> _store = new();

        public InMemoryActiveRecordStore(IOptions<InMemoryActiveRecordStoreOptions<TAggregate, TKey>> setup) : base(setup.Value)
        {
        }

        //public Task<TAggregate> LoadAsync<TAggregate, TKey>(TKey id, Action<AsyncOptions> setup = null) where TAggregate : class, IAggregateRoot<TKey>
        //{
        //    return Task.FromResult(_store.SingleOrDefault(o => o.As<IAggregateRoot<TKey>>().Id.Equals(id)) as TAggregate);
        //}

        //public Task<IQueryable<TAggregate>> QueryAsync<TAggregate, TKey>(Expression<Func<TAggregate, bool>> predicate = null, Action<AsyncOptions> setup = null) where TAggregate : class, IAggregateRoot<TKey>
        //{
        //    return Task.FromResult(Condition.TernaryIf(predicate == null, () => _store.Cast<TAggregate>().AsQueryable(), () =>
        //    {
        //        var funcPredicate = predicate.Compile();
        //        return _store.Where(o => funcPredicate((TAggregate)o)).Cast<TAggregate>().AsQueryable();
        //    }));
        //}

        //public async Task SaveAsync<TAggregate, TKey>(TAggregate aggregate, Action<AsyncOptions> setup = null) where TAggregate : class, IAggregateRoot<TKey>
        //{
        //    var entity = await LoadAsync<TAggregate, TKey>(aggregate.Id);
        //    if (entity != null)
        //    {
        //        var index = _store.IndexOf(entity);
        //        if (index != -1)
        //        {
        //            _store[index] = entity;
        //        }
        //    }
        //    else
        //    {
        //        if (Options.HasIdentityProvider && (aggregate.Id == null || aggregate.Id.Equals(default(TKey))))
        //        {
        //            var property = aggregate.GetType().GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);
        //            if (property != null)
        //            {
        //                var id = Options.IdentityProvider(_store);
        //                property.SetValue(aggregate, id);
        //            }
        //        }
        //        _store.Add(aggregate);
        //    }
        //}

        //public async Task RemoveAsync<TAggregate, TKey>(TKey id, Action<AsyncOptions> setup = null) where TAggregate : class, IAggregateRoot<TKey>
        //{
        //    var entity = await LoadAsync<TAggregate, TKey>(id, setup);
        //    if (entity != null)
        //    {
        //        var index = _store.IndexOf(entity);
        //        if (index != -1)
        //        {
        //            _store.RemoveAt(index);
        //        }
        //    }
        //}

        public Task<TAggregate> LoadAsync(TKey id, Action<AsyncOptions> setup = null)
        {
            return Task.FromResult(_store.SingleOrDefault(o => o.As<IAggregateRoot<TKey>>().Id.Equals(id)) as TAggregate);
        }

        public Task<IQueryable<TAggregate>> QueryAsync(Expression<Func<TAggregate, bool>> predicate = null, Action<AsyncOptions> setup = null)
        {
            return Task.FromResult(Condition.TernaryIf(predicate == null, () => _store.Cast<TAggregate>().AsQueryable(), () =>
            {
                var funcPredicate = predicate.Compile();
                return _store.Where(o => funcPredicate((TAggregate)o)).Cast<TAggregate>().AsQueryable();
            }));
        }

        public async Task SaveAsync(TAggregate aggregate, Action<AsyncOptions> setup = null)
        {
            var entity = await LoadAsync(aggregate.Id);
            if (entity != null)
            {
                var index = _store.IndexOf(entity);
                if (index != -1)
                {
                    _store[index] = entity;
                }
            }
            else
            {
                if (Options.HasIdentityProvider && (aggregate.Id == null || aggregate.Id.Equals(default(TKey))))
                {
                    var property = aggregate.GetType().GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);
                    if (property != null)
                    {
                        var id = Options.IdentityProvider(_store);
                        property.SetValue(aggregate, id);
                    }
                }
                _store.Add(aggregate);
            }
        }

        public async Task RemoveAsync(TKey id, Action<AsyncOptions> setup = null)
        {
            var entity = await LoadAsync(id, setup);
            if (entity != null)
            {
                var index = _store.IndexOf(entity);
                if (index != -1)
                {
                    _store.RemoveAt(index);
                }
            }
        }
    }
}
