using System;
using Microsoft.EntityFrameworkCore;
using Savvyio.Assets.Domain;
using Savvyio.Domain;
using Savvyio.Extensions.Storage;

namespace Savvyio.Assets
{
    public class CustomEfCoreDataStore : EfCoreDataStore<DbMarker>
    {
        public CustomEfCoreDataStore(IDomainEventDispatcher dispatcher) : base(dispatcher, o =>
        {
            o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(DbMarker)).EnableDetailedErrors().LogTo(Console.WriteLine);
            o.ModelConstructor = mb =>
            {
                mb.AddAccount();
            };
        })
        {
        }

        public IPersistentRepository<Account, long, DbMarker> Account => new EfCoreRepository<Account, long, DbMarker>(this);
    }
}
