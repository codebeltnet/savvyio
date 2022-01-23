using System;
using Microsoft.EntityFrameworkCore;
using Savvyio.Assets.Domain;
using Savvyio.Domain;
using Savvyio.Extensions.Storage;

namespace Savvyio.Assets.Storage
{
    public class ActiveRecordEfCoreDataStore<TMarker> : EfCoreDataStore<TMarker>
    {
        public ActiveRecordEfCoreDataStore(IDomainEventDispatcher dispatcher) : base(dispatcher, o =>
        {
            o.ContextConfigurator = b => b.UseInMemoryDatabase(typeof(TMarker).Name).EnableDetailedErrors().LogTo(Console.WriteLine);
            o.ModelConstructor = mb =>
            {
                mb.AddAccount();
            };
        })
        {
        }

        public IPersistentRepository<Account, long, TMarker> Account => new EfCoreRepository<Account, long, TMarker>(this);
    }
}
