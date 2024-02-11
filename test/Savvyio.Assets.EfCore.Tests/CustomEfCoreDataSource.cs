using System;
using Microsoft.EntityFrameworkCore;
using Savvyio.Assets.Domain;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;

namespace Savvyio.Assets
{
	public class CustomEfCoreDataSource : EfCoreAggregateDataSource<Account>
	{
		public CustomEfCoreDataSource(IDomainEventDispatcher dispatcher) : base(dispatcher, new EfCoreDataSourceOptions<Account>()
		{
			ContextConfigurator = b => b.UseInMemoryDatabase(nameof(Account)).EnableDetailedErrors().LogTo(Console.WriteLine),
			ModelConstructor = mb =>
			{
				mb.AddAccount();
			}
		})
		{
		}

		public IPersistentRepository<Account, long, Account> Account => new EfCoreAggregateRepository<Account, long, Account>(this);
	}
}
