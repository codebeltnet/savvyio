using System;
using Microsoft.EntityFrameworkCore;
using Savvyio.Assets.Domain;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;

namespace Savvyio.Assets
{
    public class CustomEfCoreDataStore : EfCoreAggregateDataStore<Account>
    {
        public CustomEfCoreDataStore(IDomainEventDispatcher dispatcher) : base(dispatcher, o =>
        {
            o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(Account)).EnableDetailedErrors().LogTo(Console.WriteLine);
            o.ModelConstructor = mb =>
            {
                mb.AddAccount();
            };
        })
        {
        }

        public IPersistentRepository<Account, long, Account> Account => new EfCoreAggregateRepository<Account, long, Account>(this);
    }
}
